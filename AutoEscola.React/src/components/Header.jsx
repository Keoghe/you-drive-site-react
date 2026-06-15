import { Link } from "react-router-dom";
import { FaSignInAlt } from "react-icons/fa";

export default function Header() {
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
          <Link to="/">Home</Link>
          <Link to="/agendadas">Aulas Agendadas</Link>
          <Link to="/agendarAula">Agendar Aula</Link>
          <Link to="/conta">Minha Conta</Link>
          <Link to="/contato">Fale Conosco</Link>
          <Link to="/login">Login</Link>
          <Link to="/cadastro">Cadastre-se</Link>
        </nav> 

        {/* ✅ LOGIN MOBILE */}
        <div className="login-mobile">
          <Link to="/login">
            <FaSignInAlt />
          </Link>
        </div> 
      </div>
    </header>
  );
}
