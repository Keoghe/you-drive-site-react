import { Link } from "react-router-dom";
import { FaHome, FaCalendarAlt, FaPlus, FaUser, FaPhone } from "react-icons/fa";

export default function MobileMenu() {
  return (
    <div className="mobile-menu">
      <Link to="/"><FaHome /></Link>
      <Link to="/agendadas"><FaCalendarAlt /></Link>
      <Link to="/agendarAula"><FaPlus /></Link>
      <Link to="/conta"><FaUser /></Link>
      <Link to="/contato"><FaPhone /></Link>
    </div>
  );
}
