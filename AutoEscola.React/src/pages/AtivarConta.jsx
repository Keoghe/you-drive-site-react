import { useState, useEffect, useRef } from "react";
import API_BASE_URL from "../config/api";
import Swal from "sweetalert2";

let carregadoGlobal = false;
export default function AtivarConta() {
  const [documentos, setDocumentos] = useState({});
  const [tipos, setTipos] = useState([]);
  const usuario = JSON.parse(localStorage.getItem("usuario"));

  function handleFileChange(e, tipoId) {
    const file = e.target.files[0];

    if (!file) return;
    const reader = new FileReader();

    reader.onload = () => {
      const base64 = reader.result.split(",")[1]; // remove prefixo

      setDocumentos((prev) => ({
        ...prev,
        [tipoId]: {
          tipoId: tipoId,
          nomeOriginal: file.name,
          base64: base64,
        },
      }));
    };

    reader.readAsDataURL(file);
  }

  async function enviarDocumentos() {
    try {
      debugger;

      const lista = Object.keys(documentos).map((tipoId) => ({
        TipoDocumentoId: documentos[tipoId]?.tipoId,
        usuarioId: usuario.usuarioId, // ✅ ajuste conforme seu objeto
        nomeOriginal: documentos[tipoId]?.nomeOriginal || "arquivo",
        base64: documentos[tipoId].base64,
      }));

      console.log(lista); // ✅ validar antes de enviar

      const response = await fetch(
        `${API_BASE_URL}/documento/upload/ativar/conta/instrutor`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuario.token}`,
          },
          body: JSON.stringify(lista),
        },
      );

      if (!response.ok) {
        const errorData = await response.text(); // ✅ pega mensagem da API
        throw new Error(errorData);
      }

      const data = await response.json();

      console.log("Resposta API:", data);
      Swal.fire({
        title: "Sucesso!",
        text: "Documentos Enviados com Sucesso",
        icon: "success",
        confirmButtonColor: "#00c853",
      }).then(() => {
        navigate("/agendadas"); // ✅ redireciona
      });
    } catch (error) {
      Swal.fire({
        title: "Erro!",
        text: error.message,
        icon: "error",
        confirmButtonColor: "#ff5252",
      });
    }
  }

  useEffect(() => {
    if (!carregadoGlobal) {
      carregarTipos();
      carregadoGlobal = true;
    }
  }, []);

  async function carregarTipos() {
    try {
      debugger;
      const tipoUsuario = 2;
      const response = await fetch(
        `${API_BASE_URL}/tiposdocumento/${tipoUsuario}`,
        {
          method: "GET",
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      if (!response.ok) {
        throw new Error("Erro ao carregar tipos de documentos");
      }

      const data = await response.json();

      setTipos(data);
    } catch (error) {
      console.error(error);
    }
  }

  return (
    <div className="home-container">
      <section className="home-section">
        <h2 className="home-title">Ativar Conta</h2>

        <div className="plans">
          {tipos.map((tipo) => (
            <div key={tipo.id} className="plan-card">
              <h3>{tipo.nome}</h3>

              <label className="upload-btn">
                Escolher arquivo
                <input
                  type="file"
                  onChange={(e) => handleFileChange(e, tipo.id)}
                />
              </label>
              {documentos[tipo.id] && (
                <p className="file-name">{documentos[tipo.id].nomeOriginal}</p>
              )}
            </div>
          ))}
        </div>

        <button onClick={enviarDocumentos} className="btn-enviar">
          Enviar Documentos
        </button>
      </section>
    </div>
  );
}
