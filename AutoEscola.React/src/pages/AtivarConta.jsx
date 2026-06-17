import { useState } from "react";

export default function AtivarConta() {
  const [documentos, setDocumentos] = useState({});
  const [files, setFiles] = useState({});
  const tipos = [
    { id: 1, nome: "CNH" },
    { id: 2, nome: "Credencial ou Certificado de Instrutor Autônomo" },
    { id: 3, nome: "Comprovante de Endereço" },
    { id: 4, nome: "Certidão de Antecedentes Criminais" },
  ];

  function handleFileChange(e, tipoId) {
    const file = e.target.files[0];

    if (!file) return;

    setFiles((prev) => ({
      ...prev,
      [tipoId]: file.name,
    }));

    const reader = new FileReader();

    reader.onload = () => {
      const base64 = reader.result.split(",")[1]; // remove prefixo

      setDocumentos((prev) => ({
        ...prev,
        [tipoId]: base64, // ✅ pronto pra API
      }));
    };

    reader.readAsDataURL(file);
  }

  function enviarDocumentos() {
    console.log(documentos);

    // ✅ depois você envia para API
    // fetch...
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
              {files[tipo.id] && <p className="file-name">{files[tipo.id]}</p>}
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
