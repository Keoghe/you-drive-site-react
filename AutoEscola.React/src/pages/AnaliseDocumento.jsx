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
      const documentoAtivos = 0;
      const response = await fetch(
        `${API_BASE_URL}/Documento/buscar-documentos-analise/${id}/${documentoAtivos}`, // ajuste sua rota
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();

      setDocumentos(data);
      todosAprovados()
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }

  async function aprovar(id) {
    await fetch(`${API_BASE_URL}/documento`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${usuario.token}`,
      },
      body: JSON.stringify({
        id: id,
        status: 1,
        descricaoAnalise: `Documento Aprovado por: id ${usuario.usuarioId} - ${usuario.nome}`,
      }),
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

  
async function aprovarInstrutor() {
  const result = await Swal.fire({
    title: "Confirmação",
    text: `O usuário ${usuario.nome}: Tem certeza da aprovação do instrutor?`,
    icon: "question",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
    confirmButtonColor: "#00c853",
    cancelButtonColor: "#d33",
  });

  if (!result.isConfirmed) return;

  // ✅ aqui chama sua API
  console.log("Aprovando instrutor...");
}


async function reprovarInstrutor() {
  const result = await Swal.fire({
    title: "Confirmação",
    text: `O usuário ${usuario.nome}: Tem certeza da reprovação do instrutor?`,
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "Sim",
    cancelButtonText: "Não",
    confirmButtonColor: "#d32f2f",
    cancelButtonColor: "#999",
  });

  if (!result.isConfirmed) return;

  // ✅ aqui chama sua API
  console.log("Reprovando instrutor...");
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

  function getFileUrl(base64, nomeOriginal) {
    if (!base64) return "";

    const nome = nomeOriginal.toLowerCase();

    let contentType = "application/pdf";

    const blob = base64ToBlob(base64, contentType);
    return URL.createObjectURL(blob);
  }

  function base64ToBlob(base64, contentType) {
    // ✅ remove prefixo se existir
    const cleanedBase64 = base64.includes(",") ? base64.split(",")[1] : base64;

    const byteCharacters = atob(cleanedBase64);
    const byteNumbers = new Array(byteCharacters.length);

    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }

    const byteArray = new Uint8Array(byteNumbers);

    return new Blob([byteArray], { type: contentType });
  }

  function todosAprovados() {
    return documentos.length > 0 && documentos.every((doc) => doc.status === 1);
  }

  return (
    <div className="home-container">
      <h2 className="home-title">Análise de Documentos</h2>

      {loading ? (
        <div className="loading-overlay">
          <div className="spinner"></div>
          <p>Carregando documentos...</p>
        </div>
      ) : documentos.length === 0 ? (
        <p className="empty-message">
          <h3>Nenhum documento encaminhado</h3>
        </p>
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

      {todosAprovados() && (
        <div className="actions-container">
          <button className="btn-aprovar"  onClick={aprovarInstrutor}>Aprovar Instrutor</button>

          <button className="btn-reprovar" onClick={reprovarInstrutor}>Reprovar Instrutor</button>
        </div>
      )}

      {modalOpen && documentoSelecionado && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h2>{documentoSelecionado.nomeOriginal}</h2>

            {documentoSelecionado?.base64 ? (
              <iframe
                src={getFileUrl(
                  documentoSelecionado.base64,
                  documentoSelecionado.nomeOriginal,
                )}
                width="100%"
                height="500px"
                style={{ border: "none" }}
              />
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
