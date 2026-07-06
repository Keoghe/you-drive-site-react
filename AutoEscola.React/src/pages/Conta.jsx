import { useEffect, useState } from "react";
import API_BASE_URL from "../config/api";
import Swal from "sweetalert2";
import { RxFontRoman } from "react-icons/rx";
import { FaPlus, FaPencilAlt } from "react-icons/fa";

export default function Conta() {
  const [usuario, setUsuario] = useState(null);
  const [loading, setLoading] = useState(true);
  const usuarioLogado = JSON.parse(localStorage.getItem("usuario"));
  const [mostrarModalEndereco, setMostrarModalEndereco] = useState(false);
  const [form, setForm] = useState({
    logradouro: "",
    numero: "",
    complemento: "",
    bairro: "",
    cidade: "",
    estado: "",
    cep: "",
  });

  const salvarEndereco = async () => {
    setLoading(true);
    try {
      const endereco = {
        cep: form.cep,
        logradouro: form.rua,
        numero: form.numero,
        complemento: form.complemento,
        bairro: form.bairro,
        cidade: form.cidade,
        estado: form.estado,
        usuarioId: usuarioLogado.usuarioId,
      };

      const response = await fetch(`${API_BASE_URL}/endereco`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${usuarioLogado.token}`,
        },
        body: JSON.stringify(endereco),
      });

      if (!response.ok) {
        const errorData = await response.text(); // ✅ pega mensagem da API
        throw new Error(errorData);
      }
      const data = await response.json();

      console.log("Resposta API:", data);
      Swal.fire({
        title: "Sucesso!",
        text: "Endereço Atualizado com Sucesso",
        icon: "success",
        confirmButtonColor: "#00c853",
      });
      buscarEndereco();
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
      setMostrarModalEndereco(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      [name]: name === "cep" ? formatarCep(value) : value,
    }));
  };

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
    carregarDadosUsuario();
    buscarEndereco();
  }, []);

  async function carregarDadosUsuario() {
    try {
      const response = await fetch(
        `${API_BASE_URL}/usuarios/buscar-dados/minha-conta/${usuarioLogado.usuarioId}`, // ajuste sua rota
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();

      setUsuario(data);
    } catch (error) {
      console.error(error);
    } finally {
    }
  }

  async function buscarEndereco() {
    try {
      const response = await fetch(
        `${API_BASE_URL}/endereco/${usuarioLogado.usuarioId}`,
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();
      console.log("ENDEREÇO - " + JSON.stringify(data));
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

  if (!usuario) return <p>Carregando...</p>;

  return (
    <div className="conta-container">
      {/* SALDO */}
      <div className="saldo-card">
        <h3>Saldo atual</h3>
        <h1>R$ {usuario.saldo.toFixed(2)}</h1>
      </div>

      {/* GRID */}
      <div className="conta-grid">
        {/* DADOS USUÁRIO */}
        <div className="card">
          <h2>Dados Cadastrais</h2>

          <p>
            <strong>Nome:</strong> {usuario.nome}
          </p>
          <p>
            <strong>CPF:</strong> {usuario.cpf}
          </p>
          <p>
            <strong>Endereço:</strong> {form.rua}, {form.numero}...
            <button
              type="button"
              className="btn-endereco"
              onClick={() => setMostrarModalEndereco(true)}
            >
              {form.rua ? <FaPencilAlt /> : <FaPlus />}

              {form.rua ? "Editar" : "Cadastrar Endereço"}
            </button>
          </p>
          <p>
            <strong>Email:</strong> {usuario.email}
          </p>
        </div>

        {/* CARTÕES */}
        <div className="card">
          <h2>Cartões de Crédito</h2>

          {usuario.cartoes?.length > 0 ? (
            usuario.cartoes.map((cartao) => (
              <div key={cartao.id} className="cartao">
                <p>
                  <strong>Bandeira:</strong> {cartao.bandeira}
                </p>
                <p>
                  <strong>Final:</strong> **** {cartao.final}
                </p>
              </div>
            ))
          ) : (
            <p>Nenhum cartão cadastrado</p>
          )}

          {mostrarModalEndereco && (
            <div className="modal-overlay">
              <div className="modal">
                <h3>Endereço</h3>

                <input
                  type="text"
                  name="cep"
                  placeholder="CEP"
                  value={form.cep}
                  onChange={handleChange}
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

                <div className="modal-actions">
                  <button
                    className="btn-cancelar"
                    type="button"
                    onClick={() => setMostrarModalEndereco(false)}
                  >
                    Cancelar
                  </button>

                  <button
                    className="btn-salvar"
                    type="button"
                    onClick={() => salvarEndereco()}
                  >
                    Salvar
                  </button>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
