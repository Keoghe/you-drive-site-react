import { useState, useEffect } from "react";

const slides = [
  {
    image: "/images/carro.jpg",
    text: "Aulas práticas de carro com instrutores qualificados"
  },
  {
    image: "/images/moto.png",
    text: "Treinamento completo para moto"
  },
  {
    image: "/images/autoescola.jpg",
    text: "Prepare-se para conquistar sua CNH"
  }
];

export default function Carousel() {
  const [index, setIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      next();
    }, 4000);

    return () => clearInterval(interval);
  }, [index]);

  function next() {
    setIndex((prev) => (prev + 1) % slides.length);
  }

  function prev() {
    setIndex((prev) => (prev - 1 + slides.length) % slides.length);
  }

  return (
    <div className="carousel-container">
      <div
        className="carousel-track"
        style={{ transform: `translateX(-${index * 100}%)` }}
      >
        {slides.map((slide, i) => (
          <div className="carousel-slide" key={i}>
            <img src={slide.image} alt="slide" />
            <div className="carousel-text">
              <p>{slide.text}</p>
            </div>
          </div>
        ))}
      </div>

      <button className="btn prev" onClick={prev}>◀</button>
      <button className="btn next" onClick={next}>▶</button>
    </div>
  );
}