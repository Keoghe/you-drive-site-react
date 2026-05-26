import { useState } from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";

const aulasMock = [
  {
    data: "2026-05-26",
    status: "pendente",
    instrutor: "Carlos Silva",
    tempo: "1h",
    custo: "R$ 120"
  },
  {
    data: "2026-05-25",
    status: "realizada",
    instrutor: "Ana Souza",
    tempo: "2h",
    custo: "R$ 200"
  }
];

export default function AulasAgendadas() {
  const [dataSelecionada, setDataSelecionada] = useState(new Date());

  function formatarData(date) {
    return date.toISOString().split("T")[0];
  }

  const aulasDoDia = aulasMock.filter(
    aula => aula.data === formatarData(dataSelecionada)
  );

  return (
    <div className="agenda-container">
      
      <h2>Aulas Agendadas</h2>

      <Calendar
        onChange={setDataSelecionada}
        value={dataSelecionada}
        tileContent={({ date }) => {
          const data = formatarData(date);

          const aula = aulasMock.find(a => a.data === data);

          if (aula) {
            return (
              <div
                className={
                  aula.status === "pendente"
                    ? "dot red"
                    : "dot green"
                }
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
              <p><strong>Instrutor:</strong> {aula.instrutor}</p>
              <p><strong>Tempo:</strong> {aula.tempo}</p>
              <p><strong>Custo:</strong> {aula.custo}</p>
              <p>
                <strong>Status:</strong>{" "}
                {aula.status === "pendente"
                  ? "Pendente"
                  : "Realizada"}
              </p>
            </div>
          ))
        )}
      </div>

    </div>
  );
}
