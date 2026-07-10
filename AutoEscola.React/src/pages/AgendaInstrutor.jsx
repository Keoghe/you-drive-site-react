import { useState, useEffect } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { FaPlus, FaPencilAlt } from "react-icons/fa";
import API_BASE_URL from "../config/api";

const aulasMock = [
  {
    data: "2026-05-26",
    status: "pendente",
    instrutor: "Carlos Silva",
    tempo: "1h",
    custo: "R$ 120",
  },
  {
    data: "2026-05-25",
    status: "realizada",
    instrutor: "Ana Souza",
    tempo: "2h",
    custo: "R$ 200",
  },
];

export default function AulasAgendadas() {
  const [dataSelecionada, setDataSelecionada] = useState(new Date());
  const [mostrarModalAtivacao, setMostrarModalAtivacao] = useState(false);
  const [mostrarModalDesativacao, setMostrarModalDesativacao] = useState(false);

  const [statusDisponivel, setStatusDisponivel] = useState(false);
  const usuarioLogado = JSON.parse(localStorage.getItem("usuario"));

  function formatarData(date) {
    return date.toISOString().split("T")[0];
  }

  const aulasDoDia = aulasMock.filter(
    (aula) => aula.data === formatarData(dataSelecionada),
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
          const data = formatarData(date);

          const aula = aulasMock.find((a) => a.data === data);

          if (aula) {
            return (
              <div
                className={aula.status === "pendente" ? "dot red" : "dot green"}
              ></div>
            );
          }
        }}
      />

      <div className="aulas-dia">
        <h3>Aulas do dia</h3>

        {aulasDoDia.length === 0 ? (
          <p>Nenhuma aula neste dia</p>
        ) : (
          aulasDoDia.map((aula, i) => (
            <div className="aula-card" key={i}>
              <p>
                <strong>Instrutor:</strong> {aula.instrutor}
              </p>
              <p>
                <strong>Tempo:</strong> {aula.tempo}
              </p>
              <p>
                <strong>Custo:</strong> {aula.custo}
              </p>
              <p>
                <strong>Status:</strong>{" "}
                {aula.status === "pendente" ? "Pendente" : "Realizada"}
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
