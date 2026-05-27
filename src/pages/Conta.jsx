import { useEffect, useState } from "react";

export default function Conta() {
  const [usuario, setUsuario] = useState(null);

  // ✅ simulando API
  useEffect(() => {
    const userMock = {
      nome: "Joelmir Moura",
      cpf: "123.456.789-00",
      endereco: "Rua Exemplo, 123 - SP",
      email: "joelmir@email.com",
      saldo: 350.5,
      cartoes: [
        {
          id: 1,
          bandeira: "Visa",
          final: "1234"
        },
        {
          id: 2,
          bandeira: "Mastercard",
          final: "5678"
        }
      ]
    };

    setUsuario(userMock);
  }, []);

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

          <p><strong>Nome:</strong> {usuario.nome}</p>
          <p><strong>CPF:</strong> {usuario.cpf}</p>
          <p><strong>Endereço:</strong> {usuario.endereco}</p>
          <p><strong>Email:</strong> {usuario.email}</p>
        </div>

        {/* CARTÕES */}
        <div className="card">
          <h2>Cartões de Crédito</h2>

          {usuario.cartoes.map((cartao) => (
            <div key={cartao.id} className="cartao">
              <p><strong>Bandeira:</strong> {cartao.bandeira}</p>
              <p><strong>Final:</strong> **** {cartao.final}</p>
            </div>
          ))}
        </div>

      </div>

    </div>
  );
}
