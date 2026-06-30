import { useEffect, useState } from "react";
import API_BASE_URL from "../config/api";
import Swal from "sweetalert2";
import { useNavigate } from "react-router-dom";

export default function AnaliseDocumento() {
  const [descricao, setDescricao] = useState("");
  const [instrutores, setInstrutores] = useState([]);
  const usuario = JSON.parse(localStorage.getItem("usuario"));
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  useEffect(() => {
    carregarInstrutores();
  }, []);

  const mockInstrutores = [
    {
      id: 1,
      nome: "João Silva",
      cpf: "123.456.789-00",
      cnh: "999999999",
      dataNascimento: "1990-01-01",
      email: "joao@email.com",
      login: "joao",
      saldo: 150.5,
      tipoUsuario: 2,
      ativo: 1,
    },
    {
      id: 2,
      nome: "Maria Souza",
      cpf: "987.654.321-00",
      cnh: "888888888",
      dataNascimento: "1985-05-10",
      email: "maria@email.com",
      login: "maria",
      saldo: 300.0,
      tipoUsuario: 2,
      ativo: 0,
    },
  ];

  async function carregarInstrutores(pagina = 1, quantidade = 10) {
    try {
      setLoading(true);

      const response = await fetch(
        `${API_BASE_URL}/usuarios/buscar-instrutores/${pagina}/${quantidade}`, // ajuste sua rota
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();
 
      setInstrutores(data.dados || data.content || data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    } 
  }

  function getStatus(status) {
    switch (status) {
      case 0:
        return "PENDENTE APROVAÇÃO";
      case 1:
        return "INATIVO";
      case 2:
        return "PENDENTE";
      case 3:
        return "APROVADO";
      case 4:
        return "REPROVADO";
      default:
        return "Desconhecido";
    }
  }

  function getStatusCor(ativo) {
    switch (ativo) {
      case 0:
        return "#d50ae7";
      case 1:
        return "#df1954";
      case 2:
        return "#1ab3ce";
      case 3:
        return "#00c853";
      case 4:
        return "#d32f2f";
      default:
        return "#482385";
    }
  }

  return (
    <div className="home-container">
      <h2 className="home-title">Instrutores</h2>

      {loading ? (
        <div className="loading-overlay">
          <div className="spinner"></div>
          <p>Carregando documentos...</p>
        </div>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>Id</th>
              <th>Nome</th>
              <th>CPF</th>
              <th>Email</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>

          <tbody>
            {instrutores.map((inst) => (
              <tr key={inst.id}>
                <td>{inst.id}</td>
                <td>{inst.nome}</td>
                <td>{inst.cpf}</td>
                <td>{inst.email}</td>
                <td style={{ color: getStatusCor(inst.ativo) }}>
                  {getStatus(inst.ativo)}
                </td>
                <td>
                  <button
                    className="btn-action"
                    onClick={() => navigate(`/analise-documento/${inst.id}`)}
                  >
                    Analisar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
