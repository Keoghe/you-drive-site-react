import { useState, useEffect } from "react";
import Swal from "sweetalert2";

export default function Cadastro() {
  const [form, setForm] = useState({
    nome: "",
    cpf: "",
    cnh: "",
    dataNascimento: "",
    email: "",
    login: "",
    senha: "",
    confirmarSenha: "",
  });

  const [erro, setErro] = useState("");

  useEffect(() => {
    if (form.Senha && form.senha !== form.confirmarSenha) {
      setErro("As senhas não coincidem");
    } else {
      setErro("");
    }
  }, [form.senha, form.confirmarSenha]);

  function handleChange(e) {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  }

  // ✅ MÁSCARA CPF
  function mascaraCpf(e) {
    let value = e.target.value.replace(/\D/g, "");

    value = value.replace(/(\d{3})(\d)/, "$1.$2");
    value = value.replace(/(\d{3})(\d)/, "$1.$2");
    value = value.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

    setForm({
      ...form,
      cpf: value,
    });
  }

  async function cadastrarUsuario(e) {
    e.preventDefault();
    if (form.senha !== form.confirmarSenha) {
      setErro("* As senhas não coincidem");
      return;
    }
    try {
      // ✅ VALIDAÇÃO

      const response = await fetch("https://localhost:7095/api/usuarios", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          nome: form.nome,
          cpf: form.cpf,
          cnh: form.cnh,
          dataNascimento: form.dataNascimento,
          email: form.email,
          login: form.login,
          senha: form.senha,
        }),
      });

      if (!response.ok) {
        const erro = await response.text();
        throw new Error(erro || "Erro ao cadastrar");
      }

      Swal.fire({
        title: "Sucesso!",
        text: "Usuário cadastrado com sucesso",
        icon: "success",
        confirmButtonColor: "#00c853",
      }).then(() => {
        window.location.href = "/login";
      });
    } catch (error) { 
      Swal.fire({
        title: "Alerta",
        text: error.message,
        icon: "info",
        confirmButtonColor: "#00c853",
      }).then(() => {
        
      });
    }
  }

  return (
    <div className="login-container">
      <div className="login-box">
        <img src="/images/logo.png" className="login-logo" />

        <h2>Criar conta</h2>

        <form onSubmit={cadastrarUsuario} className="login-form">
          <label className="cadastro-usuario">Nome completo</label>
          <input
            type="text"
            name="nome"
            value={form.nome}
            onChange={handleChange}
            required
          />

          <label className="cadastro-usuario">CPF</label>
          <input
            type="text"
            name="cpf"
            value={form.cpf}
            onChange={mascaraCpf}
            required
            maxLength={14}
          />

          <label className="cadastro-usuario">CNH</label>
          <input
            type="text"
            name="cnh"
            value={form.cnh}
            onChange={handleChange}
          />

          <label className="cadastro-usuario">Data de nascimento</label>
          <input
            type="date"
            name="dataNascimento"
            value={form.dataNascimento}
            onChange={handleChange}
            required
          />

          <label className="cadastro-usuario">Email</label>
          <input
            type="email"
            name="email"
            value={form.email}
            onChange={handleChange}
            required
          />
          <label className="cadastro-usuario">Login</label>
          <input
            type="login"
            name="login"
            value={form.login}
            onChange={handleChange}
            required
          />
          <label className="cadastro-usuario ">Senha</label>
          <input
            className={erro ? "input-error" : ""}
            type="password"
            name="senha"
            value={form.senha}
            onChange={handleChange}
            required
            className={erro ? "input-error" : ""}
          />
          <label className="cadastro-usuario">Confirmação Senha</label>
          <input
            type="password"
            name="confirmarSenha"
            value={form.confirmarSenha}
            onChange={handleChange}
            required
            className={erro ? "input-error" : ""}
          />
          {erro && <p className="erro">{erro}</p>}

          <button type="submit">Cadastrar</button>
        </form>
      </div>
    </div>
  );
}
