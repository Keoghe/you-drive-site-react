import Swal from "sweetalert2";
import { useNavigate } from "react-router-dom";

export function usuarioLogado() {
  const usuario = localStorage.getItem("usuario");
  return usuario ? JSON.parse(usuario) : null;
}


export function logout(callback) {
  Swal.fire({
    title: "Você deseja deslogar?",
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim, sair",
    cancelButtonText: "Cancelar",
  }).then((result) => {
    if (result.isConfirmed) {
      localStorage.removeItem("token");
      localStorage.removeItem("usuario");

      window.dispatchEvent(new Event("storage"));

      callback(true); // ✅ quem chamou decide o que fazer
    }
  });
}

