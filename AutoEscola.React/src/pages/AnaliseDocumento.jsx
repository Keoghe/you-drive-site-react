import { useEffect, useState } from "react";
import API_BASE_URL from "../config/api";
import Swal from "sweetalert2";
import { useParams } from "react-router-dom";

export default function AnaliseDocumento() {
  const [documentos, setDocumentos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [documentoSelecionado, setDocumentoSelecionado] = useState(null);
  const [modalReprovar, setModalReprovar] = useState(false);
  const [descricao, setDescricao] = useState("");
  const { id } = useParams();
  const usuario = JSON.parse(localStorage.getItem("usuario"));

  useEffect(() => {
    carregarDocumentos();
  }, []);

  async function carregarDocumentos() {
    try {
      setLoading(true);

      const response = await fetch(
        `${API_BASE_URL}/Documento/${id}/0`, // ajuste sua rota
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();

      setDocumentos(data);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }

  async function aprovar(id) {
    await fetch(`${API_BASE_URL}/documento/aprovar/${id}`, {
      method: "PUT",
      headers: {
        Authorization: `Bearer ${usuario.token}`,
      },
    });

    setModalOpen(false);
    carregarDocumentos();
  }
  async function reprovar(id, descricaoAnalise) {
    await fetch(`${API_BASE_URL}/documento`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${usuario.token}`,
      },
      body: JSON.stringify({
        id: id,
        status: 2,
        descricaoAnalise: descricaoAnalise,
      }),
    });

    setModalOpen(false);
    carregarDocumentos();
  }

  function getStatusDescricao(status) {
    switch (status) {
      case 0:
        return "Em análise";
      case 1:
        return "Aprovado";
      case 2:
        return "Reprovado";
      default:
        return "Desconhecido";
    }
  }

  function getStatusCor(status) {
    switch (status) {
      case 0:
        return "#ffa726";
      case 1:
        return "#00c853";
      case 2:
        return "#d32f2f";
      default:
        return "#999";
    }
  }

  function abrirModal(doc) {
    setDocumentoSelecionado(doc);
    setModalOpen(true);
  }

  function fecharModal() {
    setModalOpen(false);
    setDocumentoSelecionado(null);
  }

  function getPdfUrl(base64) {
    const blob = base64ToBlob(base64);
    return URL.createObjectURL(blob);
  }

  function base64ToBlob(base64, contentType = "application/pdf") {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: contentType });
  }

  return (
    <div className="home-container">
      <h2 className="home-title">Análise de Documentos</h2>

      {loading ? (
        <div className="loading-overlay">
          <div className="spinner"></div>
          <p>Carregando documentos...</p>
        </div>
      ) : (
        <div className="plans">
          {documentos.map((doc) => (
            <div key={doc.id} className="plan-card">
              <h3>{doc.nomeOriginal}</h3>

              <p style={{ color: getStatusCor(doc.status) }}>
                {getStatusDescricao(doc.status)}
              </p>

              {/* ✅ Botões só quando em análise */}
              {doc.status === 0 && (
                <div style={{ marginTop: "10px" }}>
                  <button
                    className="btn-visualizar"
                    onClick={() => abrirModal(doc)}
                  >
                    Visualizar
                  </button>
                </div>
              )}
            </div>
          ))}
        </div>
      )}

      {modalOpen && documentoSelecionado && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h2>{documentoSelecionado.nomeOriginal}</h2>

            {documentoSelecionado?.base64 ? (
              documentoSelecionado.nomeOriginal
                .toLowerCase()
                .endsWith(".pdf") ? (
                <iframe
                  src={getPdfUrl(documentoSelecionado.base64)}
                  width="100%"
                  height="500px"
                  style={{ border: "none" }}
                />
              ) : (
                <img
                  src={getSrc(documentoSelecionado)}
                  style={{ width: "100%" }}
                />
              )
            ) : (
              <p>Arquivo não disponível</p>
            )}

            <div className="modal-actions">
              {documentoSelecionado.status === 0 && (
                <>
                  <button
                    className="btn-aprovar"
                    onClick={() => aprovar(documentoSelecionado.id)}
                  >
                    Aprovar
                  </button>

                  <button
                    className="btn-reprovar"
                    onClick={() => setModalReprovar(true)}
                  >
                    Reprovar
                  </button>
                </>
              )}

              <button className="btn-fechar" onClick={fecharModal}>
                Fechar
              </button>
            </div>
          </div>
        </div>
      )}

      {modalReprovar && (
        <div className="modal-overlay">
          <div className="modal-content small">
            <h3>Confirmar reprovação</h3>

            <p>Tem certeza que deseja reprovar este documento?</p>

            <textarea
              placeholder="Informe o motivo da reprovação (obrigatório)"
              maxLength={500}
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
              style={{
                width: "100%",
                height: "100px",
                marginTop: "10px",
              }}
            />

            <p>{descricao.length}/500</p>

            <div className="modal-actions">
              <button
                className="btn-reprovar"
                disabled={!descricao.trim()}
                onClick={async () => {
                  await reprovar(documentoSelecionado.id, descricao);
                  setDescricao("");
                  setModalReprovar(false);
                }}
              >
                Confirmar Reprovação
              </button>

              <button
                className="btn-fechar"
                onClick={() => {
                  setModalReprovar(false);
                  setDescricao("");
                }}
              >
                Cancelar
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
