import { useState, useEffect } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { FaPlus, FaPencilAlt } from "react-icons/fa";
import API_BASE_URL from "../config/api";

export default function AulasAgendadas() {
  const StatusAula = {
    PENDENTE: 0,
    REALIZADA: 1,
    CANCELADA: 2,
    INICIADA: 3,
  };
  const StatusAulaDescricao = {
    0: "Pendente",
    1: "Realizada",
    2: "Cancelada",
    3: "Iniciada",
  };

  const StatusAulaInfo = {
    0: { descricao: "Pendente", cor: "#f39c12" },
    1: { descricao: "Realizada", cor: "#27ae60" },
    2: { descricao: "Cancelada", cor: "#e74c3c" },
    3: { descricao: "Iniciada", cor: "#3498db" },
  };
  const [dataSelecionada, setDataSelecionada] = useState(new Date());
  const [mostrarModalAtivacao, setMostrarModalAtivacao] = useState(false);
  const [mostrarModalDesativacao, setMostrarModalDesativacao] = useState(false);
  const [statusDisponivel, setStatusDisponivel] = useState(false);
  const usuarioLogado = JSON.parse(localStorage.getItem("usuario"));
  const [aulas, setAulas] = useState([]);
  const [mesSelecionado, setMesSelecionado] = useState(
    new Date().getMonth() + 1,
  );
  const [loading, setLoading] = useState(true);
  const obterStatusAula = (status) => {
    switch (Number(status)) {
      case StatusAula.PENDENTE:
        return "Pendente";

      case StatusAula.REALIZADA:
        return "Realizada";

      case StatusAula.CANCELADA:
        return "Cancelada";

      case StatusAula.INICIADA:
        return "Iniciada";

      default:
        return "Desconhecido";
    }
  };

  async function carregarAulas(mes) {
    try {
      setLoading(true);

      const response = await fetch(
        `${API_BASE_URL}/aulas/usuarioId/${usuarioLogado.usuarioId}/mes/${mes}`, // ajuste sua rota
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();

      setAulas(data.dados || data.content || data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }

  function formatarData(date) {
    return date.toISOString().split("T")[0];
  }

  const aulasDoDia = aulas.filter(
    (aula) => aula.dataAula === formatarData(dataSelecionada),
  );
  const consultarStatus = async () => {
    try {
      const response = await fetch(
        `${API_BASE_URL}/instrutorDisponivel/${usuarioLogado.usuarioId}`,
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();
      debugger;
      // A API retorna true ou false
      setStatusDisponivel(data.status === 0);
    } catch (error) {
      console.error("Erro ao consultar status:", error);
      setStatusDisponivel(false);
    }
  };

  const alterarStatusInstrutor = async (status) => {
    try {
      const instrutor = {
        usuarioId: usuarioLogado.usuarioId,
        dataAula: new Date().toISOString(),
        status: status, // 0 = DISPONIVEL | 1 = INDISPONIVEL
      };

      const response = await fetch(
        `${API_BASE_URL}/instrutorDisponivel/atualizar/status`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
          body: JSON.stringify(instrutor),
        },
      );

      if (!response.ok) {
        throw new Error("Erro ao alterar status");
      }

      const data = await response.json();

      console.log("Status atualizado:", data);

      // Atualiza o indicador na tela
      setStatusDisponivel(status === 0);
    } catch (error) {
      console.error(error);
    } finally {
      setMostrarModalAtivacao(false);
      setMostrarModalDesativacao(false);
    }
  };

  useEffect(() => {
    consultarStatus();
    debugger;
    carregarAulas(new Date().getMonth() + 1);
  }, []);

  return (
    <div className="agenda-container">
      <div className="top-actions">
        {statusDisponivel ? (
          <button
            type="button"
            className="btn-cartao indisponivel"
            onClick={() => setMostrarModalDesativacao(true)}
          >
            <FaPlus />
            Ficar Indisponível
          </button>
        ) : (
          <button
            type="button"
            className="btn-cartao "
            onClick={() => setMostrarModalAtivacao(true)}
          >
            <FaPlus />
            Ficar Disponível
          </button>
        )}

        <div className="status-container">
          <span
            className={`status-circle ${statusDisponivel ? "green" : "red"}`}
          />
          <span>{statusDisponivel ? "Disponível" : "Indisponível"}</span>
        </div>
      </div>

      <h2>Aulas Agendadas</h2>

      <Calendar
        onChange={setDataSelecionada}
        value={dataSelecionada}
        tileContent={({ date }) => {
          const dataCalendario = formatarData(date);

          const aulasDoDia = aulas.filter((a) => a.dataAula === dataCalendario);

          if (aulasDoDia.length > 0) {
            return (
              <div className="aula-indicador">
                {/* <span className="qtd-aulas">{aulasDoDia.length}</span> */}
                <div className="dot green"></div>
              </div>
            );
          }
        }}
        onActiveStartDateChange={({ activeStartDate }) => {
          const mes = activeStartDate.getMonth() + 1;
          setMesSelecionado(mes);
          carregarAulas(mes);
        }}
      />

      <div className="aulas-dia">
        <h3>Aulas do dia</h3>

        {aulasDoDia.length === 0 ? (
          <p>Nenhuma aula neste dia</p>
        ) : (
          aulasDoDia.map((aula) => (
            <div className="aula-card" key={aula.id}>
              <p>
                <strong>Data:</strong> {aula.dataAula}
              </p>

              <p>
                <strong>Início:</strong> {aula.horaInicio}
              </p>

              <p>
                <strong>Fim:</strong> {aula.horaFim}
              </p>

              <p>
                <strong>Valor:</strong> R$ {aula.valorFinal}
              </p>

              <p>
                <strong>Status:</strong>
                <span style={{ color: StatusAulaInfo[aula.status]?.cor }}>
                  {" "}
                  {StatusAulaInfo[aula.status]?.descricao}
                </span>
              </p>
            </div>
          ))
        )}
      </div>

      {mostrarModalAtivacao && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Deseja ficar disponível para dar aulas?</h3>

            <div className="modal-actions">
              <button
                className="btn-cancelar"
                type="button"
                onClick={() => setMostrarModalAtivacao(false)}
              >
                Cancelar
              </button>

              <button
                className="btn-salvar"
                type="button"
                onClick={() => alterarStatusInstrutor(0)}
              >
                Sim
              </button>
            </div>
          </div>
        </div>
      )}

      {mostrarModalDesativacao && (
        <div className="modal-overlay">
          <div className="modal">
            <h3>Deseja ficar indisponível para dar aulas?</h3>

            <div className="modal-actions">
              <button
                className="btn-cancelar"
                type="button"
                onClick={() => setMostrarModalDesativacao(false)}
              >
                Cancelar
              </button>

              <button
                className="btn-salvar"
                type="button"
                onClick={() => alterarStatusInstrutor(1)}
              >
                Sim
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
