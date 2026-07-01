import { useEffect, useState } from "react";
import API_BASE_URL from "../config/api";
import Swal from "sweetalert2";

export default function Conta() {
  const [usuario, setUsuario] = useState(null);
  const usuarioLogado = JSON.parse(localStorage.getItem("usuario"));

  useEffect(() => {
    carregarDadosUsuario();
  }, []);

  async function carregarDadosUsuario() {
    try {
      const response = await fetch(
        `${API_BASE_URL}/usuarios/buscar-dados/minha-conta/${usuarioLogado.usuarioId}`, // ajuste sua rota
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();

      setUsuario(data);
    } catch (error) {
      console.error(error);
    } finally {
    }
  }

  if (!usuario) return <p>Carregando...</p>;

  return (
    <div className="conta-container">
      {/* SALDO */}
      <div className="saldo-card">
        <h3>Saldo atual</h3>
        <h1>R$ {usuario.saldo.toFixed(2)}</h1>
      </div>

      {/* GRID */}
      <div className="conta-grid">
        {/* DADOS USUÁRIO */}
        <div className="card">
          <h2>Dados Cadastrais</h2>

          <p>
            <strong>Nome:</strong> {usuario.nome}
          </p>
          <p>
            <strong>CPF:</strong> {usuario.cpf}
          </p>
          <p>
            <strong>Endereço:</strong> {usuario.endereco}
          </p>
          <p>
            <strong>Email:</strong> {usuario.email}
          </p>
        </div>

        {/* CARTÕES */}
        <div className="card">
          <h2>Cartões de Crédito</h2>

          {usuario.cartoes?.length > 0 ? (
            usuario.cartoes.map((cartao) => (
              <div key={cartao.id} className="cartao">
                <p>
                  <strong>Bandeira:</strong> {cartao.bandeira}
                </p>
                <p>
                  <strong>Final:</strong> **** {cartao.final}
                </p>
              </div>
            ))
          ) : (
            <p>Nenhum cartão cadastrado</p>
          )}
        </div>
      </div>
    </div>
  );
}
