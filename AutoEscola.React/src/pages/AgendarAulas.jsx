import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
// import L from "leaflet";
import "leaflet-routing-machine";
import "leaflet-routing-machine/dist/leaflet-routing-machine.css";

import L from "leaflet";
import { useEffect, useState } from "react";
import API_BASE_URL from "../config/api";
import { FaPlus, FaPencilAlt, FaSearch, FaTrash } from "react-icons/fa";

// ================== ICONE ==================
const icon = new L.Icon({
  iconUrl: "https://cdn-icons-png.flaticon.com/512/744/744465.png",
  iconSize: [35, 35],
  iconAnchor: [17, 35],
  popupAnchor: [0, -30],
});

function RoutingMachine({ origem, destino }) {
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
    }).addTo(map);

    return () => {
      map.removeControl(routingControl);
    };
  }, [map, origem, destino]);

  return null;
}

// ================== COMPONENTE ==================
export default function MapaInstrutores() {
  const [cep, setCep] = useState("");
  const usuarioLogado = JSON.parse(localStorage.getItem("usuario"));
  const [posicaoMapa, setPosicaoMapa] = useState([-23.545, -46.63]);
  const [localizacaoUsuario, setLocalizacaoUsuario] = useState(null);
  const [alunoAvulso, setAlunoAvulso] = useState([]);
  const [cidadeAluno, setCidadeAluno] = useState("");
  const [instrutorSelecionado, setInstrutorSelecionado] = useState(null);
  const [instrutores, setInstrutores] = useState(null);
  const [paginacaoInstrutores, setPaginacaoInstrutores] = useState([]);
  const [aulaCadastrada, setAulaCadastrada] = useState([]);

  const [loading, setLoading] = useState(true);

  const [mostrarModalSolicitacao, setMostrarModalSolicitacao] = useState(false);
  const [cancelarSolicitacaoAula, setCancelarSolicitacaoAula] = useState(false);
  const [mostrarBotaoSolicitarAula, setMostrarBotaoSolicitarAula] =
    useState(false);
  const [mostrarBotaoCancelar, setMostrarBotaoCancelar] = useState(false);
  const [mostrarModalCancelamentoAula, setMostrarModalCancelamentoAula] =
    useState(false);
  const [solicitacaoAulaCancelada, setSolicitacaoAulaCancelada] =
    useState(false);
  const [notificaoAulaId, setNotificaoAulaId] = useState("");

  const StatusNotificacaoAula = Object.freeze({
    Pendente: 1,
    Aceita: 2,
    Recusada: 3,
    Excluida: 4,
    Cancelado: 5,
  });

  const [mensagemCancelamento, setMensagemCancelamento] = useState("");
  let mensagemCancelamentoPadrao = `Ao cancelar a aula após a confirmação do instrutor, será cobrada uma
            taxa correspondente a 10% do valor da aula. Essa cobrança ocorre
            porque o instrutor já aceitou realizar a aula e pode estar em
            deslocamento até o local combinado, gerando custos e despesas
            relacionados ao deslocamento.`;

  let mensagemCancelamentoAluna = `O instrutor já aceitou a aula, o cancelamento gerará um custo de 10% do valor da aula. <p>Tem certeza que deseja cancelar?</>`;
  const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

  async function obterLocalizacaoAtual() {
    if (!navigator.geolocation) {
      alert("Geolocalização não suportada pelo navegador.");
      return;
    }

    navigator.geolocation.getCurrentPosition(
      async (position) => {
        const lat = position.coords.latitude;
        const lng = position.coords.longitude;

        setLocalizacaoUsuario([lat, lng]);
        setPosicaoMapa([lat, lng]); // centraliza o mapa

        const endereco = await buscarDadosEndereco(lat, lng);
        setCidadeAluno(endereco);
        debugger;
        carregarInstrutoresDisponiveis(endereco.cidade);
      },
      (error) => {
        console.error("Erro ao obter localização:", error);
        alert("Não foi possível obter sua localização.");
      },
      {
        enableHighAccuracy: true,
        timeout: 10000,
        maximumAge: 0,
      },
    );
  }

  async function carregarInstrutoresDisponiveis(
    cidade,
    pagina = 1,
    quantidade = 10,
  ) {
    try {
      setLoading(true);

      const response = await fetch(
        `${API_BASE_URL}/instrutorDisponivel/cidade/${cidade}/pagina/${pagina}/quantidade/${quantidade}`,
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

      setInstrutores(lista);
      setPaginacaoInstrutores(data);
      // if (lista.length > 0) {
      //   setInstrutorSelecionado(lista[0]);
      // }
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }

  async function CriarSolicitacaoAula(instrutorId) {
    try {
      debugger;
      if (CriarSolicitacaoAula == undefined) {
        console.log("Nenhum instrutor disponível para solicitar aula.");
        return;
      }
      const response = await fetch(
        `${API_BASE_URL}/notificacaoAula/adicionar`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
          body: JSON.stringify({
            AlunoId: usuarioLogado.usuarioId,
            LatitudeAluno: cidadeAluno.latitude,
            LongitudeAluno: cidadeAluno.longitude,
            InstrutorId: instrutorId,
            Descricao: `Solicitação de aula do aluno ${usuarioLogado.nome} no bairro ${cidadeAluno.bairro}, cidade ${cidadeAluno.cidade}, estado ${cidadeAluno.estado}.`,
          }),
        },
      );

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Login inválido");
      }

      const data = await response.json();
      console.log(data);
      setAulaCadastrada(data);
      console.log("aulaCadastrada" + aulaCadastrada);
      await buscarNotificacaoAulaSolicitada(data.id);
    } catch (error) {
      console.error("Erro ao criar solicitação de aula:", error);
    } finally {
      // setLoading(false);
    }
  }

  async function CancelarSolicitacaoAula() {
    try {
      debugger;
      const response = await fetch(
        `${API_BASE_URL}/notificacaoAula/aluno/cancelar`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
          body: JSON.stringify({
            Id: aulaCadastrada.id,
            AlunoId: usuarioLogado.usuarioId,
          }),
        },
      );

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Login inválido");
      }

      const data = await response.json();
      console.log(data);

      setMensagemCancelamento(data.mensagem);
      setSolicitacaoAulaCancelada(true);
    } catch (error) {
      console.error("Erro ao criar solicitação de aula:", error);
    } finally {
      setLoading(false);
    }
  }

  async function solicitarAula(pagina = 1, quantidade = 10) {
    debugger;
    await carregarInstrutoresDisponiveis(
      cidadeAluno.cidade,
      pagina,
      quantidade,
    );

    console.log(instrutorSelecionado);
    setMostrarModalSolicitacao(false);
    setLoading(true);
    instrutores.forEach(async (instrutor) => {
      const aula = await CriarSolicitacaoAula(instrutor.usuarioId);
      debugger;
    });
  }

  async function buscarNotificacaoAulaSolicitada(notificacaoId) {
    debugger;
    while (true) {
      const response = await fetch(
        `${API_BASE_URL}/notificacaoAula/${notificacaoId}`,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();

      console.log("AULA SOLICITADA >>>", data);

      if (data && data.status !== StatusNotificacaoAula.Pendente) {
        const proximidade = distancia(
          data.latitudeAluno,
          data.longitudeAluno,
          data.latitudeInstrutor,
          data.longitudeInstrutor,
        );

        setLoading(false);
        setMostrarBotaoSolicitarAula(false);

        setInstrutorSelecionado([
          data.latitudeInstrutor,
          data.longitudeInstrutor,
        ]);

        if (proximidade <= 0.1) {
          alert("Você chegou ao local do aluno!");
          setMostrarBotaoCancelar(true);

          break;
        }
      }
      await delay(5000);
    }
  }

  async function buscarAulaSolicitada() {
    const response = await fetch(
      `${API_BASE_URL}/notificacaoAula/aluno/${usuarioLogado.usuarioId}`,
      {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${usuarioLogado.token}`,
        },
      },
    );
    const data = await response.json();
    debugger;
    if (
      data &&
      data.length > 0 &&
      data[0].status === StatusNotificacaoAula.Pendente
    ) {
      setPosicaoMapa([data[0].latitudeAluno, data[0].longitudeAluno]);
      setInstrutorSelecionado([
        data[0].latitudeInstrutor,
        data[0].longitudeInstrutor,
      ]);
      setMostrarBotaoCancelar(true);
      setMostrarBotaoSolicitarAula(false);
      notificaoAulaId = data[0].id;
      await delay(10000);
      buscarNotificacaoAulaSolicitada(data[0].id);
    } else {
      setMostrarBotaoCancelar(false);
      setMostrarBotaoSolicitarAula(true);
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

  // ================== FILTRO ==================
  const instrutoresProximos = [];

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

  useEffect(() => {
    obterLocalizacaoAtual();
    buscarAulaSolicitada();
  }, []);

  return (
    <div className="mapa-container">
      <h2>Aula Avulsa</h2>
      <p>
        {mostrarBotaoSolicitarAula && (
          <button
            type="button"
            className="btn-cartao"
            onClick={() => {
              setMostrarModalSolicitacao(true);
            }}
          >
            <FaSearch /> Solicitar Aula
          </button>
        )}
      </p>

      <div className="container-botao">
        {mostrarBotaoCancelar && (
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
        )}
      </div>

      {/* MAPA */}
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

        {instrutorSelecionado && (
          <RoutingMachine origem={posicaoMapa} destino={instrutorSelecionado} />
        )}
      </MapContainer>

      {mostrarModalSolicitacao && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Aviso Importante</h3>
            Ao solicitar uma aula, sua solicitação será encaminhada ao instrutor
            para análise. Após o aceite da solicitação pelo instrutor, a aula
            será considerada confirmada e o valor correspondente será debitado
            da sua conta, independentemente da realização de qualquer outra ação
            posterior.
            <p>
              Antes de solicitar uma aula, certifique-se de que possui saldo
              suficiente e de que concorda com as condições de cobrança.
            </p>
            <div className="modal-actions">
              <button
                className="btn-cancelar"
                type="button"
                onClick={() => setMostrarModalSolicitacao(false)}
              >
                Cancelar
              </button>

              <button
                className="btn-salvar"
                type="button"
                onClick={() => solicitarAula()}
              >
                Solicitar Aula
              </button>
            </div>
          </div>
        </div>
      )}

      {cancelarSolicitacaoAula && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Atenção: cancelamento da aula</h3>
            {mensagemCancelamento}
            <div className="modal-actions">
              <button
                className="btn-cancelar"
                type="button"
                onClick={() => {
                  setCancelarSolicitacaoAula(false);
                  setLoading(true);
                }}
              >
                Fechar
              </button>

              <button
                className="btn-salvar"
                type="button"
                onClick={() => {
                  console.log("Aula cancelada");
                  setCancelarSolicitacaoAula(false);
                  CancelarSolicitacaoAula();
                }}
              >
                Cancelar Aula
              </button>
            </div>
          </div>
        </div>
      )}

      {solicitacaoAulaCancelada && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Atenção: cancelamento da aula</h3>
            {mensagemCancelamento}
            <div className="modal-actions">
              <button
                className="btn-salvar"
                type="button"
                onClick={() => {
                  setSolicitacaoAulaCancelada(false);
                }}
              >
                Fechar
              </button>
            </div>
          </div>
        </div>
      )}

      {loading ? (
        <div className="loading-overlay">
          <button
            className="close-button"
            onClick={() => {
              setLoading(false);
              setCancelarSolicitacaoAula(true);
              setMensagemCancelamento(mensagemCancelamentoPadrao);
            }}
          >
            ✕
          </button>
          <div className="spinner"></div>
          <p>Buscando Instrutor...</p>
        </div>
      ) : (
        <div></div>
      )}

      {mostrarModalCancelamentoAula && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Cancelar Aula</h3>

            <p className="modal-texto">
              Tem certeza que deseja cancelar esta aula?
            </p>

            <p className="modal-alerta">
              <strong>Atenção:</strong> ao cancelar a aula, você poderá ter o
              custo de 10% do valor da aula, pois o instrutor já aceitou a
              solicitação e pode estar em deslocamento até o local combinado.
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
                  setMostrarModalCancelamentoAula(false);
                  setMostrarBotaoCancelar(false);
                  setMostrarBotaoSolicitarAula(true);
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
