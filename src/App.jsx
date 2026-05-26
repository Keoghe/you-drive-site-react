import { BrowserRouter, Routes, Route } from "react-router-dom";
import Layout from "./layout/Layout";
import Home from "./pages/Home";
import AulasAgendadas from "./pages/AulasAgendadas";
import AulasRealizadas from "./pages/AulasRealizadas";
import Conta from "./pages/Conta";
import Contato from "./pages/Contato";
import Login from "./pages/Login";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Layout virou rota pai */}
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="agendadas" element={<AulasAgendadas />} />
          <Route path="realizadas" element={<AulasRealizadas />} />
          <Route path="conta" element={<Conta />} />
          <Route path="contato" element={<Contato />} />
          <Route path="login" element={<Login />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
