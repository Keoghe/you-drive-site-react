import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
// import L from "leaflet";
import "leaflet-routing-machine";
import "leaflet-routing-machine/dist/leaflet-routing-machine.css";

import L from "leaflet";
import { useEffect, useState, useRef } from "react";
import API_BASE_URL from "../config/api";
import { FaPlus, FaPencilAlt, FaSearch, FaTrash } from "react-icons/fa";

// ================== ICONE ==================
const icon = new L.Icon({
  iconUrl: "https://cdn-icons-png.flaticon.com/512/744/744465.png",
  iconSize: [35, 35],
  iconAnchor: [17, 35],
  popupAnchor: [0, -30],
});

function RoutingMachine({ origem, destino, aluno }) {
  const map = useMap();

  useEffect(() => {
    if (!origem || !destino) return;

    const routingControl = L.Routing.control({
      waypoints: [
        L.latLng(origem[0], origem[1]),
        L.latLng(destino[0], destino[1]),
      ],
      routeWhileDragging: false,
      addWaypoints: false,
      draggableWaypoints: false,
      fitSelectedRoutes: true,
      show: false,
      lineOptions: {
        styles: [
          {
            color: "#0066ff",
            weight: 6,
          },
        ],
      },
      createMarker: (i, wp) => {
        const marker = L.marker(wp.latLng);
        // Apenas o marcador de destino
        if (i === 1 && aluno) {
          marker.bindPopup(`
          <div>
          <h4>${aluno.nome}</h4>
          </div>
          `);
        } else {
          marker.bindPopup(`
          <div>
          <h4>Você está aqui!</h4>
          </div>
          `);
        }
        return marker;
      },
    }).addTo(map);

    return () => {
      map.removeControl(routingControl);
    };
  }, [map, origem, destino, aluno]);

  return null;
}

// ================== COMPONENTE ==================
export default function MapaInstrutores() {
  const [cep, setCep] = useState("");
  const usuarioLogado = JSON.parse(localStorage.getItem("usuario"));
  const [posicaoMapa, setPosicaoMapa] = useState([-23.545, -46.63]);
  const [localizacaoUsuario, setLocalizacaoUsuario] = useState(null);
  const [instrutores, setInstrutores] = useState([]);
  const [alunoAvulso, setAlunoAvulso] = useState([]);
  const [cidadeAluno, setCidadeAluno] = useState("");
  const [notificaoAula, setNotificaoAula] = useState("");
  const [notificaoAulaId, setNotificaoAulaId] = useState("");
  const [alunoSelecionado, setAlunoSelecionado] = useState([]);
  const [mostrarModalSolicitacao, setMostrarModalSolicitacao] = useState(false);
  const [mostrarModalCancelamentoAula, setMostrarModalCancelamentoAula] =
    useState(false);
  const [loading, setLoading] = useState(true);
  const statusDeslocamentoRef = useRef(true);
  const StatusNotificacaoAula = Object.freeze({
    Pendente: 1,
    Aceita: 2,
    Recusada: 3,
    Excluida: 4,
    Cancelado: 5,
  });
  const timeoutRef = useRef(null);
  const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

  function obterLocalizacaoAtual() {
    return new Promise((resolve, reject) => {
      if (!navigator.geolocation) {
        reject("Geolocalização não suportada.");
        return;
      }

      navigator.geolocation.getCurrentPosition(
        (position) => {
          const lat = position.coords.latitude;
          const lng = position.coords.longitude;

          const novaPosicao = [lat, lng];

          setLocalizacaoUsuario(novaPosicao);
          setPosicaoMapa(novaPosicao);

          resolve(novaPosicao);
        },
        (error) => {
          console.error("Erro ao obter localização:", error);
          reject(error);
        },
        {
          enableHighAccuracy: true,
          timeout: 10000,
          maximumAge: 0,
        },
      );
    });
  }

  async function aceitarAula(cadastrarAula = false) {
    try {
      debugger;
      const response = await fetch(
        `${API_BASE_URL}/notificacaoAula/atualizar`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
          body: JSON.stringify({
            NotificacaoId: notificaoAulaId,
            Status: StatusNotificacaoAula.Aceita,
          }),
        },
      );

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Ocorrreu um erro ao aceitar a aula.");
      }

      const data = await response.json();
    } catch (error) {
      console.error(error);
    } finally {
      alunoSelecionado.posicao = [
        Number(alunoAvulso.latitudeAluno),
        Number(alunoAvulso.longitudeAluno),
      ];

      setAlunoSelecionado(alunoSelecionado);
      setMostrarModalSolicitacao(false);
      const dadoAula = {
        usuarioId: alunoAvulso.alunoId,
        InstrutorId: usuarioLogado.usuarioId,
        PromocaoId: 1,
        ValorAulaId: 2,
      };

      if (cadastrarAula) gravarAgendaAula(dadoAula);
    }
  }

  async function atualizarLocalizacaoInstrutor(
    posicaoAtual = localizacaoUsuario,
  ) {
    try {
      debugger;
      const response = await fetch(
        `${API_BASE_URL}/notificacaoAula/atualizar/localizacao`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
          body: JSON.stringify({
            NotificacaoId: notificaoAulaId,
            LatitudeInstrutor: posicaoAtual[0],
            LongitudeInstrutor: posicaoAtual[1],
          }),
        },
      );

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Ocorrreu um erro ao aceitar a aula.");
      }

      const data = await response.json();
    } catch (error) {
      console.error(error);
    } finally {
    }
  }

  async function acompanharDeslocamento() {
    while (statusDeslocamentoRef.current) {
      debugger;
      const posicaoAtual = await obterLocalizacaoAtual();

      const proximidade = distancia(
        posicaoAtual[0],
        posicaoAtual[1],
        alunoSelecionado.posicao[0],
        alunoSelecionado.posicao[1],
      );

      console.log("Distância até o aluno:", proximidade, "km");
      await atualizarLocalizacaoInstrutor(posicaoAtual);
      if (proximidade <= 0.1) {
        alert("Você chegou ao local do aluno!");
        break;
      }

      await delay(5000);

      if (!statusDeslocamentoRef.current) {
        break;
      }
    }
  }

  async function gravarAgendaAula(dadoAula) {
    try {
      let horaAtual = new Date().toLocaleTimeString("pt-BR", {
        hour: "2-digit",
        minute: "2-digit",
        hour12: false,
      });
      const response = await fetch(`${API_BASE_URL}/aulas/cadastrar`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${usuarioLogado.token}`,
        },
        body: JSON.stringify({
          UsuarioId: dadoAula.usuarioId,
          InstrutorId: dadoAula.InstrutorId,
          DataAula: new Date().toISOString().split("T")[0],
          PromocaoId: dadoAula.PromocaoId,
          HoraInicio: horaAtual,
          HoraFim: horaAtual,
          ValorAulaId: dadoAula.ValorAulaId,
        }),
      });

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Ocorrreu um erro ao aceitar a aula.");
      }

      const data = await response.json();
    } catch (error) {
      console.error(error);
    } finally {
    }
  }

  async function buscarDadosEndereco(lat, lng) {
    const response = await fetch(
      `https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`,
    );
    const data = await response.json();
    const endereco = {
      latitude: lat,
      longitude: lng,
      bairro: data.address.suburb,
      cidade: data.address.city,
      estado: data.address.state,
      rua: data.address.road,
      cep: data.address.postcode,
    };

    return endereco;
  }

  // ================== BUSCAR CEP ==================
  async function buscarCep() {
    try {
      const res = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
      const data = await res.json();

      if (data.erro) {
        alert("CEP inválido");
        return;
      }

      const endereco = `${data.logradouro}, ${data.localidade}, ${data.uf}`;

      const geo = await fetch(
        `https://nominatim.openstreetmap.org/search?format=json&q=${endereco}`,
      );

      const geoData = await geo.json();

      if (geoData.length > 0) {
        const lat = parseFloat(geoData[0].lat);
        const lon = parseFloat(geoData[0].lon);

        setPosicaoMapa([lat, lon]);
      }
    } catch (error) {
      console.error(error);
    }
  }

  // ================== DISTÂNCIA ==================
  //Essa função calcula a distância entre dois pontos geográficos (latitude e longitude) usando a Fórmula de Haversine, que considera a curvatura da Terra.
  //Ela retorna a distância em quilômetros.
  function distancia(lat1, lon1, lat2, lon2) {
    const R = 6371;
    const dLat = ((lat2 - lat1) * Math.PI) / 180;
    const dLon = ((lon2 - lon1) * Math.PI) / 180;

    const a =
      Math.sin(dLat / 2) * Math.sin(dLat / 2) +
      Math.cos((lat1 * Math.PI) / 180) *
        Math.cos((lat2 * Math.PI) / 180) *
        Math.sin(dLon / 2) *
        Math.sin(dLon / 2);

    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    return R * c;
  }

  // ================== RENDER ==================

  const iconUsuario = new L.Icon({
    iconUrl: "https://cdn-icons-png.flaticon.com/512/149/149059.png", // marcador azul
    iconSize: [35, 35],
    iconAnchor: [17, 35],
    popupAnchor: [0, -30],
  });

  function obterSrcImagem(base64) {
    if (!base64) return "";

    let mimeType = "image/jpeg";

    // PNG
    if (base64.startsWith("iVBOR")) {
      mimeType = "image/png";
    }
    // JPG/JPEG
    else if (base64.startsWith("/9j/")) {
      mimeType = "image/jpeg";
    }
    // GIF
    else if (base64.startsWith("R0lGOD")) {
      mimeType = "image/gif";
    }
    // WEBP
    else if (base64.startsWith("UklGR")) {
      mimeType = "image/webp";
    }

    return `data:${mimeType};base64,${base64}`;
  }

  async function carregarNotificacaoAula() {
    try {
      setLoading(true);

      const response = await fetch(
        `${API_BASE_URL}/NotificacaoAula/instrutor/${usuarioLogado.usuarioId}/pendente`,
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();

      const lista = (data.dados || data.content || data).map((instrutor) => ({
        ...instrutor,
        posicao: [Number(instrutor.latitude), Number(instrutor.longitude)],
      }));
      console.log(lista);

      if (lista.length > 0) {
        for (const item of lista) {
          const dataSolicitacao = new Date(item.dataSolicitacao);
          const agora = new Date();

          // Verifica se é do mesmo dia
          const ehHoje =
            dataSolicitacao.getDate() === agora.getDate() &&
            dataSolicitacao.getMonth() === agora.getMonth() &&
            dataSolicitacao.getFullYear() === agora.getFullYear();

          // Diferença em minutos
          const diferencaMinutos = agora - dataSolicitacao > 5 * 60 * 1000;

          if (ehHoje && diferencaMinutos < 5) {
            setNotificaoAula(item.descricao);
            setNotificaoAulaId(item.id);
            setMostrarModalSolicitacao(true);
            buscarDadosAluno(item.alunoId);
            setLoading(false);
            setAlunoAvulso(item);
            break;
          } else {
            alterarStatusNotificacao(item.id, StatusNotificacaoAula.Excluida);
          }
        }
      } else {
        setTimeout(() => {
          carregarNotificacaoAula();
        }, 5000);
      }
    } catch (error) {
      console.error(error);
    } finally {
    }
  }

  async function alterarStatusNotificacao(
    notificacaoId,
    status = StatusNotificacaoAula.Excluida,
  ) {
    debugger;
    try {
      const response = await fetch(
        `${API_BASE_URL}/notificacaoAula/atualizar`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
          body: JSON.stringify({
            NotificacaoId: notificacaoId,
            Status: status,
          }),
        },
      );

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Login inválido");
      }

      const data = await response.json();
    } catch (error) {
    } finally {
      // setLoading(false);
    }
  }

  async function buscarDadosAluno(alunoId) {
    try {
      const response = await fetch(
        `${API_BASE_URL}/Usuarios/buscar-dados/minha-conta/${alunoId}`,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Nenhum Aluno foi encontrado");
      }

      const data = await response.json();
      alunoSelecionado.dados = data;
    } catch (error) {
    } finally {
      // setLoading(false);
    }
  }

  async function iniciarDeslocamento() {
    await aceitarAula(true);
    statusDeslocamentoRef.current = true;
    await acompanharDeslocamento();
  }

  useEffect(() => {
    obterLocalizacaoAtual();
    carregarNotificacaoAula();
  }, []);

  return (
    <div className="mapa-container">
      <h2>Aula Avulsa</h2>

      {/* MAPA */}
      <div className="container-botao">
        <button
          type="button"
          className="btn-solicitar-aula"
          onClick={() => {
            setMostrarModalCancelamentoAula(true);
          }}
        >
          <FaTrash />
          <span>Cancelar Aula</span>
        </button>
      </div>

      {loading ? (
        <div className="loading-overlay">
          <div className="spinner"></div>
          <p>Buscando Aula...</p>
        </div>
      ) : (
        <MapContainer
          center={posicaoMapa}
          zoom={14}
          className="map"
          key={posicaoMapa.toString()}
        >
          <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />

          {/* SUA LOCALIZAÇÃO */}
          <Marker position={posicaoMapa} icon={iconUsuario}>
            <Popup>
              <strong>Você está aqui</strong>
            </Popup>
          </Marker>

          {alunoSelecionado && (
            <RoutingMachine
              origem={posicaoMapa}
              destino={alunoSelecionado.posicao}
              aluno={alunoSelecionado.dados}
            />
          )}
 
        </MapContainer>
      )}
      {mostrarModalSolicitacao && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Solicitação de Aula Avulsa</h3>
            {notificaoAula}
            <div className="modal-actions">
              <button
                className="btn-cancelar"
                type="button"
                onClick={() => {
                  setMostrarModalSolicitacao(false);
                  alterarStatusNotificacao(
                    notificaoAulaId,
                    StatusNotificacaoAula.Recusada,
                  );
                  carregarNotificacaoAula();
                }}
              >
                Cancelar
              </button>

              <button
                className="btn-salvar"
                type="button"
                onClick={iniciarDeslocamento}
              >
                Aceitar Aula
              </button>
            </div>
          </div>
        </div>
      )}

      {mostrarModalCancelamentoAula && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Cancelar Aula</h3>

            <p className="modal-texto">
              Tem certeza que deseja cancelar esta aula?
            </p>

            <p className="modal-alerta">
              <strong>Atenção:</strong> ao cancelar a aula, você não receberá
              qualquer valor referente ao deslocamento realizado até o local de
              encontro do aluno.
            </p>

            <div className="modal-actions">
              <button
                className="btn-cancelar"
                type="button"
                onClick={() => {
                  setMostrarModalCancelamentoAula(false);
                }}
              >
                Não, Voltar
              </button>

              <button
                className="btn-salvar"
                type="button"
                onClick={() => {
                  alterarStatusNotificacao(
                    notificaoAulaId,
                    StatusNotificacaoAula.Cancelado,
                  );
                  statusDeslocamentoRef.current = false;
                  setMostrarModalCancelamentoAula(false);
                  carregarNotificacaoAula();
                }}
              >
                Sim, Cancelar Aula
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
