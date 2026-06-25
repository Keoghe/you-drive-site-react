import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import API_BASE_URL from "../config/api";

export default function Login() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  const [form, setForm] = useState({
    login: "",
    senha: "",
  });

  function handleChange(e) {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  }

  async function handleLogin(e) {
    setLoading(true);
    e.preventDefault();
    
    if (loading) return;
    try {
      const response = await fetch(`${API_BASE_URL}/usuarios/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          login: form.login,
          senha: form.senha,
        }),
      });

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Login inválido");
      }

      const data = await response.json();
 
      // ✅ SALVAR USUÁRIO
      localStorage.setItem("usuario", JSON.stringify(data));

      // ✅ força atualização do Header
      window.dispatchEvent(new Event("storage"));

      // ✅ ALERT SUCESSO
      Swal.fire({
        title: "Sucesso!",
        text: "Login realizado com sucesso",
        icon: "success",
        confirmButtonColor: "#00c853",
      }).then(() => {
        navigate("/agendadas"); // ✅ redireciona
      });
    } catch (error) {
      Swal.fire({
        title: "Erro!",
        text: error.message,
        icon: "error",
        confirmButtonColor: "#ff5252",
      });
    } finally {
      setLoading(false); // ✅ libera botão
    }
  }

  return (
    <div className="login-container">
      <div className="login-box">
        <h2>Entrar</h2>

        <form onSubmit={handleLogin} className="login-form">
          <label>Login</label>
          <input
            type="text"
            name="login"
            value={form.login}
            onChange={handleChange}
            required
          />

          <label>Senha</label>
          <input
            type="password"
            name="senha"
            value={form.senha}
            onChange={handleChange}
            required
          />

          <button type="submit">Entrar</button>
        </form>
      </div>
    </div>
  );
}
``;
