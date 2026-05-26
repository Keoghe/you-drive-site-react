import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";
import L from "leaflet";
import { useState } from "react";

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

// ================== COMPONENTE ==================
export default function MapaInstrutores() {
  const [cep, setCep] = useState("");
  const [posicaoMapa, setPosicaoMapa] = useState([-23.545, -46.63]);

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
        `https://nominatim.openstreetmap.org/search?format=json&q=${endereco}`
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
  const instrutoresProximos = instrutores.filter((instrutor) => {
    const [lat, lon] = instrutor.posicao;
    return distancia(lat, lon, posicaoMapa[0], posicaoMapa[1]) < 5;
  });

  // ================== RENDER ==================

  
const iconUsuario = new L.Icon({
  iconUrl: "https://cdn-icons-png.flaticon.com/512/149/149059.png", // marcador azul
  iconSize: [35, 35],
  iconAnchor: [17, 35],
  popupAnchor: [0, -30],
});

  return (
    <div className="mapa-container">
      <h2>Instrutores Disponíveis</h2>

      {/* BUSCA CEP */}
      <div className="busca-cep">
        <input
          type="text"
          placeholder="Digite o CEP"
          value={cep}
          onChange={(e) => setCep(e.target.value)}
        />
        <button onClick={buscarCep}>Buscar</button>
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

        {instrutoresProximos.map((instrutor) => (
          <Marker
            key={instrutor.id}
            position={instrutor.posicao}
            icon={icon}
          >
            <Popup>
              <div className="card-instrutor">
                <img src={instrutor.foto} alt="instrutor" />

                <h4>{instrutor.nome}</h4>

                <p><strong>Placa:</strong> {instrutor.placa}</p>
                <p><strong>Carro:</strong> {instrutor.carro} ({instrutor.cor})</p>

                <p>
                  <strong>Nota:</strong>{" "}
                  {"⭐".repeat(instrutor.nota)}
                </p>

                <p><strong>Valor:</strong> {instrutor.valor}</p>
              </div>
            </Popup>
          </Marker>
        ))}
      </MapContainer>
    </div>
  );
}