import { Link } from "react-router-dom";
import { FaSignInAlt, FaSignOutAlt } from "react-icons/fa";
import { usuarioLogado } from "../services/auth";
import { useState, useEffect } from "react";
import { logout } from "../services/auth";
import { useNavigate } from "react-router-dom";
import API_BASE_URL from "../config/api";

export default function Header() {
  const [usuario, setUsuario] = useState(null);
  const navigate = useNavigate();
  const [menu, setMenu] = useState([]);

  useEffect(() => {
    const updateUsuario = () => {
      const usuarioSistema = usuarioLogado();
      setUsuario(usuarioSistema);
      carregarMenu(usuarioSistema);
    };

    // ✅ executa na inicialização
    updateUsuario();

    // ✅ escuta mudanças
    window.addEventListener("storage", updateUsuario);

    return () => {
      window.removeEventListener("storage", updateUsuario);
    };
  }, []);

  function handleLogout() {
    logout((status) => {
      if (status) navigate("/login"); // ✅ aqui sim pode usar
    });
  }

  async function carregarMenu(usuario) {
    try {
      debugger;
      const response = await fetch(
        `${API_BASE_URL}/ConfiguracaoAcesso/usuarioId/${usuario.usuarioId}`,
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();

      setMenu(data);
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <header className="header">
      <div className="header-container">
        {/* Logo + Nome */}
        <div className="logo-container">
          <img src="/images/logo.png" alt="Logo" className="logo-img" />
          <h1 className="logo-text">Auto Escola</h1>
        </div>

        {/* Menu */}
        <nav className="nav-menu">
          {/* <Link to="/">Home</Link>
          <Link to="/agendadas">Aulas Agendadas</Link>
          <Link to="/agendaInstrutor">Agenda Instrutor</Link>
          <Link to="/agendarAula">Agendar Aula</Link>
          <Link to="/conta">Minha Conta</Link>
          <Link to="/contato">Fale Conosco</Link>
          <Link to="/Instrutores">Instrutores</Link>
          {/* <Link to="/AnaliseDocumento">Analise Documento</Link> */}
          {/*<Link to="/cadastro">Cadastre-se</Link>
          <Link to="/ativar-conta">Ativar Conta</Link> */}
          {menu.map((item) => (
            <Link key={item.id} to={item.rota}>
              {item.titulo}
            </Link>
          ))}
          {usuario ? (
            <div className="user-area">
              <span className="user-name">Olá, {usuario.nome}</span>

              <button className="logout-btn" onClick={handleLogout}>
                <FaSignOutAlt />
              </button>
            </div>
          ) : (
            <Link to="/login">Login</Link>
          )}
        </nav>

        {/* ✅ LOGIN MOBILE */}
        <div className="login-mobile">
          {usuario ? (
            <button onClick={handleLogout}>
              <FaSignOutAlt />
            </button>
          ) : (
            <Link to="/login">
              <FaSignInAlt />
            </Link>
          )}
        </div>
      </div>
    </header>
  );
}
