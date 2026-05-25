import Carousel from "../components/Carousel";

export default function Home() {
  return (
    <div className="home-container">
      
      <Carousel />

      <section className="home-section">
        <h2 className="home-title">Tipos de contratação</h2>

        <div className="plans">
          <div className="plan-card">
            <h3>Pacote Básico</h3>
            <p>Ideal para quem está iniciando</p>
          </div>

          <div className="plan-card">
            <h3>Pacote Completo</h3>
            <p>Treinamento completo até a aprovação</p>
          </div>

          <div className="plan-card">
            <h3>Aulas Avulsas</h3>
            <p>Perfeito para reforço de direção</p>
          </div>
        </div>

      </section>

    </div>
  );
}



// =======================
// Outras páginas
// =======================

export function AulasAgendadas() {
  return <h2>Aulas Agendadas</h2>;
}

export function AulasRealizadas() {
  return <h2>Aulas Realizadas</h2>;
}

export function Conta() {
  return <h2>Dados da Conta</h2>;
}

export function Contato() {
  return <h2>Fale Conosco</h2>;
}
