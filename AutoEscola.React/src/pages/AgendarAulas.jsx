import { MapContainer, TileLayer, Marker, Popup, useMap } from "react-leaflet";
// import L from "leaflet";
import "leaflet-routing-machine";
import "leaflet-routing-machine/dist/leaflet-routing-machine.css";

import L from "leaflet";
import { useEffect, useState } from "react";
import API_BASE_URL from "../config/api";
import { FaPlus, FaPencilAlt, FaSearch } from "react-icons/fa";

// ================== DADOS ==================
const instrutores = [
  {
    id: 1,
    nome: "Carlos Silva",
    foto: "/images/instrutores/1.jpg", // ✅ corrigido
    placa: "ABC-1234",
    carro: "Onix",
    cor: "Branco",
    nota: 5,
    valor: "R$ 120/h",
    posicao: [-23.544, -46.629],
  },
  {
    id: 2,
    nome: "Ana Souza",
    foto: "/images/instrutores/2.jpg", // ✅ corrigido
    placa: "XYZ-5678",
    carro: "HB20",
    cor: "Prata",
    nota: 4,
    valor: "R$ 110/h",
    posicao: [-23.548, -46.633],
  },
];

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
  const [instrutores, setInstrutores] = useState([]);
  const [cidadeAluno, setCidadeAluno] = useState("");
  const [instrutorSelecionado, setInstrutorSelecionado] = useState(null);
  const [paginacaoInstrutores, setPaginacaoInstrutores] = useState([]);
  const [loading, setLoading] = useState(true);

  const [mostrarModalSolicitacao, setMostrarModalSolicitacao] = useState(false);

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
      if (lista.length > 0) {
        setInstrutorSelecionado(lista[0]);
      }
    } catch (error) {
      console.error(error);
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

    console.log(instrutores);
    setMostrarModalSolicitacao(false);
    setLoading(true);
    enviarSolicitacaoAula();
  }

  async function enviarSolicitacaoAula() {
    debugger; 
    console.log('OBTER LOCALIZACAO USUÁRIO'); 
    for (const instrutor of paginacaoInstrutores.dados) {
      const notificacao = {
        AlunoId: usuarioLogado.usuarioId,
        InstrutorId: instrutor.usuarioId,
        Latitude: cidadeAluno.latitude,
        Longitude: cidadeAluno.longitude,
        Descricao: `Solicitação de Aulas Avulsa no bairro ${cidadeAluno.bairro} - ${cidadeAluno.cidade}`,
      };
    console.log('ADICIONAR SOLICITAÇÃO DE NOTIFICAÇÃO');

      const response = await fetch(`${API_BASE_URL}/NotificacaoAula/adicionar`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${usuarioLogado.token}`,
        },
        body: JSON.stringify(notificacao),
      });

      if (!response.ok) {
        const errorData = await response.text(); // ✅ pega mensagem da API
        throw new Error(errorData);
      }
      const data = await response.json();

      console.log("Resposta API:", data);
    };

    // Swal.fire({
    //   title: "Sucesso!",
    //   text: "Endereço Atualizado com Sucesso",
    //   icon: "success",
    //   confirmButtonColor: "#00c853",
    // });
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
  // instrutores.filter((instrutor) => {
  //   const [lat, lon] = instrutor.posicao;
  //   return distancia(lat, lon, posicaoMapa[0], posicaoMapa[1]) < 5;
  // });

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

  useEffect(() => {
    obterLocalizacaoAtual();
  }, []);

  return (
    <div className="mapa-container">
      <h2>Instrutores Disponíveis</h2>
      <p>
        <button
          type="button"
          className="btn-cartao"
          onClick={() => {
            setMostrarModalSolicitacao(true);
          }}
        >
          <FaSearch /> Solicitar Aula
        </button>
      </p>

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
          <RoutingMachine
            origem={posicaoMapa}
            destino={instrutorSelecionado.posicao}
          />
        )}

        {instrutores.map((instrutor) => (
          <Marker
            key={instrutor.usuarioId}
            position={instrutor.posicao}
            icon={icon}
            eventHandlers={{
              click: () => {
                setInstrutorSelecionado(instrutor);
              },
            }}
          >
            <Popup>
              <div className="card-instrutor">
                <img src={obterSrcImagem(instrutor.foto)} alt="instrutor" />

                <h4>{instrutor.nome}</h4>

                <p>
                  <strong>Placa:</strong> {instrutor.placa}
                </p>
                <p>
                  <strong>Carro:</strong> {instrutor.carro} ({instrutor.cor})
                </p>

                <p>
                  <strong>Nota:</strong> {"⭐".repeat(instrutor.nota)}
                </p>

                <p>
                  <strong>Valor:</strong> {instrutor.valor}
                </p>
              </div>
            </Popup>
          </Marker>
        ))}
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

      {loading ? (
        <div className="loading-overlay">
          <div className="spinner"></div>
          <p>Buscando Instrutor...</p>
        </div>
      ) : (
        <div>achou</div>
      )}
    </div>
  );
}
