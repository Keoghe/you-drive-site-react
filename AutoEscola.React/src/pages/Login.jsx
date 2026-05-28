import { useState } from "react";
import { Link } from "react-router-dom";

export default function Login() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [erro, setErro] = useState("");

  async function handleLogin(e) {
    e.preventDefault();

    setErro("");

    try {
      const response = await fetch("http://localhost:3000/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email,
          senha,
        }),
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || "Erro ao fazer login");
      }

      // ✅ salvar token
      localStorage.setItem("token", data.token);

      // ✅ redirecionar (futuro)
      alert("Login realizado com sucesso!");
    } catch (error) {
      setErro(error.message);
    }
  }

  return (
    <div className="login-container">
      <div className="login-box">
        <img src="/images/logo.png" alt="Logo" className="login-logo" />

        <h2>Entrar na sua conta</h2>

        <form className="login-form" onSubmit={handleLogin}>
          <label>Email</label>
          <input
            type="email"
            placeholder="Digite seu e-mail"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <label>Senha</label>
          <input
            type="password"
            placeholder="Digite sua senha"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
          />

          {erro && <p className="login-error">{erro}</p>}

          <button type="submit">Entrar</button>

          {/* ✅ NOVO LINK */}
          <p className="login-link">
            Não tem conta? <Link to="/cadastro">Cadastre-se</Link>
          </p>
        </form>
      </div>
    </div>
  );
}
