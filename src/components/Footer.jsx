import { FaFacebook, FaInstagram, FaWhatsapp } from "react-icons/fa";

export default function Footer() {
  return (
    <footer className="footer">
      <div className="footer-container">

        <p className="footer-text">
          © 2026 Auto Escola MAZZI
        </p>

        <div className="footer-social">
          <a href="#">
            <FaFacebook />
          </a>

          <a href="#">
            <FaInstagram />
          </a>

          <a href="#">
            <FaWhatsapp />
          </a>
        </div>

      </div>
    </footer>
  );
}
``