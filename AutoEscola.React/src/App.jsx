import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./layout/Layout";
import Home from "./pages/Home";
import AulasAgendadas from "./pages/AulasAgendadas";
import AgendarAulas from "./pages/AgendarAulas";
import AgendaInstrutor from "./pages/AgendaInstrutor";
import Conta from "./pages/Conta";
import Contato from "./pages/Contato";
import Login from "./pages/Login";
import Cadastro from "./pages/Cadastro";
import AtivarConta from "./pages/AtivarConta";
import AnaliseDocumento from "./pages/AnaliseDocumento";
import Instrutores from "./pages/Instrutores";
import MapaInstrutor from "./pages/MapaInstrutor";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Layout virou rota pai */}
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="agendadas" element={<AulasAgendadas />} />
          <Route path="agendaInstrutor" element={<AgendaInstrutor />} />
          <Route path="agendarAula" element={<AgendarAulas />} /> 
          <Route path="/analise-documento/:id" element={<AnaliseDocumento />} />
          <Route path="instrutores" element={<Instrutores />} />
          <Route path="conta" element={<Conta />} />
          <Route path="contato" element={<Contato />} />
          <Route path="login" element={<Login />} />
          <Route path="cadastro" element={<Cadastro />} />
          <Route path="ativar-conta" element={<AtivarConta />} />
          <Route path="mapa-instrutor" element={<MapaInstrutor />} />
          
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
