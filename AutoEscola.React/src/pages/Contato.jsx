import { useState } from "react";
import { FaWhatsapp, FaFacebook, FaInstagram } from "react-icons/fa";
import { SiTiktok } from "react-icons/si";
import { MdEmail, MdPhone } from "react-icons/md";

export default function Contato() {
  const [email, setEmail] = useState("");
  const [mensagem, setMensagem] = useState("");

  function handleSubmit(e) {
    e.preventDefault();

    // ✅ aqui vai integração futura com API
    console.log({ email, mensagem });

    alert("Mensagem enviada com sucesso!");

    setEmail("");
    setMensagem("");
  }

  return (
    <div className="contato-container">

      <h2>Fale Conosco</h2>

      <div className="contato-grid">

        {/* FORMULÁRIO */}
        <div className="card">
          <h3>Envie sua mensagem</h3>

          <form onSubmit={handleSubmit} className="form-contato">

            <label>Email</label>
            <input
              type="email"
              placeholder="Seu e-mail"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />

            <label>Mensagem</label>
            <textarea
              placeholder="Digite sua dúvida ou solicitação"
              value={mensagem}
              onChange={(e) => setMensagem(e.target.value)}
              required
            />

            <button type="submit">Enviar</button>

          </form>
        </div>

        {/* CONTATOS */}
        <div className="card">
          <h3>Informações de Contato</h3>

          <p>
            <MdPhone /> (11) 99999-9999
          </p>

          <p>
            <MdEmail /> contato@autoescola.com
          </p>

          <p>
            <FaWhatsapp /> 
            <a href="https://wa.me/5511999999999" target="_App">11 99999-9999
            </a>
          </p>

          <p>
            <FaFacebook /> 
            <a href="https://facebook.com" target="_ook">FACEBOOK
            </a>
          </p>

          <p>
            <FaInstagram /> 
            <a href="https://instagram.com" target="_gram">INSTAGRAN
            </a>
          </p>

          <p>
            <SiTiktok /> 
            <a href="https://tiktok.com" target="_k">TIKTOK
            </a>
          </p>

        </div>

      </div>

    </div>
  );
}
