import { useState, useEffect, useRef } from "react";
import API_BASE_URL from "../config/api";
import Swal from "sweetalert2";

export default function AtivarConta() {
  const [documentos, setDocumentos] = useState({});
  const [tipos, setTipos] = useState([]);
  const usuario = JSON.parse(localStorage.getItem("usuario"));
  const [documentosUsuario, setDocumentosUsuario] = useState([]);
  const [loading, setLoading] = useState(true);
  const [mostrarMotivo, setMostrarMotivo] = useState(null);

  const [form, setForm] = useState({
    logradouro: "",
    numero: "",
    complemento: "",
    bairro: "",
    cidade: "",
    estado: "",
    cep: "",

    modelo: "",
    cor: "",
    placa: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: name === "cep" ? formatarCep(value) : value,
    }));
  };

  function handleFileChange(e, tipoId) {
    const file = e.target.files[0];

    const nome = file.name.toLowerCase();

    const extensoesPermitidas = [".pdf", ".jpg", ".jpeg", ".png"];

    const valido = extensoesPermitidas.some((ext) => nome.endsWith(ext));

    if (!valido) {
      Swal.fire({
        title: "Arquivo inválido",
        text: "Envie apenas arquivos PDF, JPG, JPEG ou PNG",
        icon: "warning",
      });

      e.target.value = ""; // ✅ limpa input
      return;
    }

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
      setLoading(true);

      const lista = Object.keys(documentos).map((tipoId) => ({
        TipoDocumentoId: documentos[tipoId]?.tipoId,
        usuarioId: usuario.usuarioId, // ✅ ajuste conforme seu objeto
        nomeOriginal: documentos[tipoId]?.nomeOriginal || "arquivo",
        base64: documentos[tipoId].base64,
      }));
 
      const dadosCadastrais = {
        endereco: {
          cep: form.cep,
          logradouro: form.rua,
          numero: form.numero,
          complemento: form.complemento,
          bairro: form.bairro,
          cidade: form.cidade,
          estado: form.estado,
          usuarioId: usuario.usuarioId
        },

        veiculo: {
          modelo: form.modelo,
          cor: form.cor,
          placa: form.placa,
          insrtutorId: usuario.usuarioId
        },

        documentos: lista,
      }; 

      // const dadosInstrutor = {
      //   dadosCadastrais = dadosCadastrais,
      //   documentos = lista
      // }

      const response = await fetch(
        `${API_BASE_URL}/documento/upload/ativar/conta/instrutor`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${usuario.token}`,
          },
          body: JSON.stringify(dadosCadastrais),
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
      });
      console.log("Recarregar Status");
    } catch (error) {
      Swal.fire({
        title: "Erro!",
        text: error.message,
        icon: "error",
        confirmButtonColor: "#ff5252",
      });
    } finally {
      setLoading(false);
      await carregarTipos();
    }
  }

  function validaTipoDeDocumento() {}

 async function buscarEndereco() {
    try { 
      const response = await fetch(
        `${API_BASE_URL}/endereco/${usuario.usuarioId}`,
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();       
      console.log('ENDEREÇO - ' + JSON.stringify(data));
      setForm((prev) => ({
            ...prev,
            rua: data.logradouro ?? "",
            numero: data.numero ?? "",
            complemento: data.complemento ?? "",
            bairro: data.bairro ?? "",
            cidade: data.cidade ?? "",
            estado: data.estado ?? "",
            cep: data.cep ?? "",
          }));


    } catch (error) {
      console.error(error);
    }
  }

   async function buscarVeiculo() {
    try { 
      const response = await fetch(
        `${API_BASE_URL}/veiculo/${usuario.usuarioId}`,
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();        
      setForm((prev) => ({
            ...prev,
            modelo: data.modelo ?? "",
            cor: data.cor ?? "",
            placa: data.placa ?? ""             
          }));


    } catch (error) {
      console.error(error);
    }
  }
 
  async function carregarDocumentos() {
    try {
       
      const response = await fetch(
        `${API_BASE_URL}/Documento/${usuario.usuarioId}/0`,
        {
          headers: {
            Authorization: `Bearer ${usuario.token}`,
          },
        },
      );

      const data = await response.json();
      
      setDocumentosUsuario(data);
    } catch (error) {
      console.error(error);
    }
  }

  function getStatus(tipoDocumentoId) {
    const doc = documentosUsuario.find(
      (d) => d.tipoDocumentoId === tipoDocumentoId,
    );

    return doc != null ? doc.status : null;
  }

  function getStatusDescricao(status) {
    switch (status) {
      case 0:
        return "Em Análise";
      case 1:
        return "Aprovado";
      case 2:
        return "Reprovado";
      default:
        return "Não enviado";
    }
  }

  function getStatusCor(status) {
    switch (status) {
      case 0:
        return "#ffa726"; // laranja
      case 1:
        return "#00c853"; // verde
      case 2:
        return "#d32f2f"; // vermelho
      default:
        return "#999";
    }
  }

  function getDocumento(tipoDocumentoId) {
    return documentosUsuario.find(
      (d) => d.tipoDocumentoId === tipoDocumentoId,
    );
  }

  const formatarCep = (valor) => {
    return valor
      .replace(/\D/g, "")
      .replace(/^(\d{5})(\d)/, "$1-$2")
      .slice(0, 9);
  };

  const buscarCep = async () => {
    try {
      setLoading(true);
      const cep = form.cep.replace(/\D/g, "");

      if (cep.length !== 8) {
        Swal.fire({
          icon: "warning",
          title: "CEP inválido",
          text: "Informe um CEP válido.",
        });
        return;
      }

      const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);

      const data = await response.json();

      if (data.erro) {
        Swal.fire({
          icon: "error",
          title: "CEP não encontrado",
          text: "Verifique o CEP informado.",
        });
        return;
      }

      setForm((prev) => ({
        ...prev,
        rua: data.logradouro || "",
        bairro: data.bairro || "",
        cidade: data.localidade || "",
        estado: data.uf || "",
      }));
    } catch (error) {
      console.error(error);

      Swal.fire({
        icon: "error",
        title: "Erro",
        text: "Não foi possível consultar o CEP.",
      });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    buscarEndereco();
    buscarVeiculo();
    carregarTipos();
  }, []);

  async function carregarTipos() {
    try {
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

      await carregarDocumentos();
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false); // ✅ aqui termina tudo
    }
  }

  return (
    <div className="home-container">
      <section className="home-section">
        <h2 className="home-title">Ativar Conta</h2>

        {loading ? (
          <div className="loading-overlay">
            <div className="spinner"></div>
            <p>Carregando documentos...</p>
          </div>
        ) : (
          <div className="plans">
            <div className="dados-container">
              <div className="card-form">
                <h3>Dados do Endereço</h3>

                <div className="form-grid">
                  <input
                    type="text"
                    name="cep"
                    placeholder="Cep"
                    value={form.cep}
                    onChange={handleChange}
                    className="campo-inteiro"
                    onBlur={buscarCep}
                  />
                  <input
                    type="text"
                    name="rua"
                    placeholder="Rua"
                    value={form.rua}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="numero"
                    placeholder="Número"
                    value={form.numero}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="complemento"
                    placeholder="Complemento"
                    value={form.complemento}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="bairro"
                    placeholder="Bairro"
                    value={form.bairro}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="cidade"
                    placeholder="Cidade"
                    value={form.cidade}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="estado"
                    placeholder="Estado"
                    value={form.estado}
                    onChange={handleChange}
                  />
                </div>
              </div>
              <div className="card-form">
                <h3>Dados do Veículo</h3>

                <div className="form-grid">
                  <input
                    type="text"
                    name="modelo"
                    placeholder="Modelo"
                    value={form.modelo}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="cor"
                    placeholder="Cor"
                    value={form.cor}
                    onChange={handleChange}
                  />

                  <input
                    type="text"
                    name="placa"
                    placeholder="Placa"
                    value={form.placa}
                    onChange={handleChange}
                  />
                </div>
              </div>
            </div>
            <div className="documentos-container">
              {tipos.map((tipo) => {
                const status = getStatus(tipo.id); 
                return (
                  <div key={tipo.id} className="plan-card">
                    <h3>{tipo.nome}</h3>

                    <p
                      style={{
                        color: getStatusCor(status),
                        display: "flex",
                        alignItems: "center",
                        gap: "6px",
                      }}
                    >
                      {getStatusDescricao(status)}

                      {status === 2 && (
                        <span
                          className="icon-circle"
                          title={
                            getDocumento(tipo.id)?.descricaoAnalise ||
                            "Motivo da recusa"
                          }
                          onClick={(e) => {
                            e.stopPropagation();
                            setMostrarMotivo(
                              mostrarMotivo === tipo.id ? null : tipo.id,
                            );
                          }}
                        >
                          i
                        </span>
                      )}
                    </p>

                    {status === 2 &&
                      mostrarMotivo === tipo.id &&
                      getDocumento(tipo.id)?.descricaoAnalise && (
                        <div className="motivo-reprovacao">
                          {getDocumento(tipo.id).descricaoAnalise}
                        </div>
                      )}

                    <label
                      className={`upload-btn ${status === 0 || status === 1 ? "disabled" : ""}`}
                    >
                      {status === 1
                        ? "Aprovado"
                        : status === 2
                          ? "Reenviar documento"
                          : status === null
                            ? "Selecionar Arquivo"
                            : "Aguardando análise"}

                      <input
                        type="file"
                        disabled={status === 0 || status === 1}
                        onChange={(e) => handleFileChange(e, tipo.id)}
                      />
                    </label>

                    {documentos[tipo.id] && (
                      <p className="file-name">
                        {documentos[tipo.id].nomeOriginal}
                      </p>
                    )}
                  </div>
                );
              })}
            </div>
          </div>
        )}

        <button onClick={enviarDocumentos} className="btn-enviar">
          Enviar Dados
        </button>
      </section>
    </div>
  );
}
