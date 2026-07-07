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
  const [mostrarModalCartao, setMostrarModalCartao] = useState(false);
  const [form, setForm] = useState({
    endereco: {
      logradouro: "",
      numero: "",
      complemento: "",
      bairro: "",
      cidade: "",
      estado: "",
      cep: "",
    },
    cartoes: [],
    cartao: {
      bandeira: "",
      numero: "",
      codigo: "",
      nomeTitular: "",
      dataVigencia: "",
      cpfCnpj: "",
    },
  });

  const salvarEndereco = async () => {
    setLoading(true);
    try {
      const endereco = {
        cep: form.endereco.cep,
        logradouro: form.endereco.logradouro,
        numero: form.endereco.numero,
        complemento: form.endereco.complemento,
        bairro: form.endereco.bairro,
        cidade: form.endereco.cidade,
        estado: form.endereco.estado,
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

  const salvarCartao = async () => {
    setLoading(true);
    try {
      const cartao = {
        usuarioId: usuarioLogado.usuarioId,
        cpfCnpj: form.cartao.cpfCnpj.replace(/\D/g, ""),
        bandeira: form.cartao.bandeira,
        numero: form.cartao.numero.replace(/\s/g, ""),
        codigo: form.cartao.codigo,
        nomeTitular: form.cartao.nomeTitular,
        dataVigencia: form.cartao.dataVigencia,
      };

      const response = await fetch(`${API_BASE_URL}/cartao`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${usuarioLogado.token}`,
        },
        body: JSON.stringify(cartao),
      });

      if (!response.ok) {
        const errorData = await response.text(); // ✅ pega mensagem da API
        throw new Error(errorData);
      }
      const data = await response.json();

      console.log("Resposta API:", data);
      Swal.fire({
        title: "Sucesso!",
        text: "Cartão Atualizado com Sucesso",
        icon: "success",
        confirmButtonColor: "#00c853",
      });
      buscarCartao();
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
      setMostrarModalCartao(false);
      buscarCartao();
    }
  };
  const formatarCpfCnpj = (valor) => {
    valor = valor.replace(/\D/g, "");

    if (valor.length <= 11) {
      // CPF
      return valor
        .replace(/(\d{3})(\d)/, "$1.$2")
        .replace(/(\d{3})(\d)/, "$1.$2")
        .replace(/(\d{3})(\d{1,2})$/, "$1-$2")
        .substring(0, 14);
    }

    // CNPJ
    return valor
      .replace(/^(\d{2})(\d)/, "$1.$2")
      .replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")
      .replace(/\.(\d{3})(\d)/, ".$1/$2")
      .replace(/(\d{4})(\d)/, "$1-$2")
      .substring(0, 18);
  };

  const formatarDataVigencia = (valor) => {
    valor = valor.replace(/\D/g, "");

    return valor.replace(/^(\d{2})(\d)/, "$1/$2").substring(0, 5);
  };

  const formatarCartao = (valor) => {
    return valor
      .replace(/\D/g, "")
      .replace(/(\d{4})(?=\d)/g, "$1 ")
      .substring(0, 19);
  };

  const handleEnderecoChange = (e) => {
    const { name, value } = e.target;

    setForm((prev) => ({
      ...prev,
      endereco: {
        ...prev.endereco,
        [name]: name === "cep" ? formatarCep(value) : value,
      },
    }));
  };

  const handleCartaoChange = (e) => {
    const { name, value } = e.target;

    let novoValor = value;

    if (name === "cpfCnpj") {
      novoValor = formatarCpfCnpj(value);
    } else if (name === "dataVigencia") {
      novoValor = formatarDataVigencia(value);
    } else if (name === "numero") {
      novoValor = formatarCartao(value);
    } else if (name === "codigo") {
      novoValor = value.replace(/\D/g, "").substring(0, 4);
    }

    setForm((prev) => ({
      ...prev,
      cartao: {
        ...prev.cartao,
        [name]: novoValor,
      },
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
      const cep = form.endereco.cep.replace(/\D/g, "");

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
        endereco: {
          ...prev.endereco,
          logradouro: data.logradouro || "",
          bairro: data.bairro || "",
          cidade: data.localidade || "",
          estado: data.uf || "",
        },
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
    buscarCartao();
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
        endereco: {
          logradouro: data.logradouro ?? "",
          numero: data.numero ?? "",
          complemento: data.complemento ?? "",
          bairro: data.bairro ?? "",
          cidade: data.cidade ?? "",
          estado: data.estado ?? "",
          cep: data.cep ?? "",
        },
      }));
    } catch (error) {
      console.error(error);
    }
  }

  async function buscarCartao() {
    try {
      const response = await fetch(
        `${API_BASE_URL}/cartao/${usuarioLogado.usuarioId}`,
        {
          headers: {
            Authorization: `Bearer ${usuarioLogado.token}`,
          },
        },
      );

      const data = await response.json();
      console.log("CARTAO - " + JSON.stringify(data));
      setForm((prev) => ({
        ...prev,
        cartoes: data || [],
        // cartao: {
        //   bandeira: data.bairro ?? "",
        //   numero: data.numero ?? "",
        //   final: data.final ?? "",
        //   nome: data.nome ?? "",
        //   dataVigencia: data.dataVigencia ?? "",
        //   cpfCnpj: data.cpfCnpj ?? "",
        // },
      }));
    } catch (error) {
      console.error(error);
    }
  }
 
const editarCartao = (cartaoSelecionado) => {
  setForm((prev) => ({
    ...prev,
    cartao: {
      id: cartaoSelecionado.id,
      bandeira: cartaoSelecionado.bandeira ?? "",
      numero: cartaoSelecionado.numero ?? "", 
      nomeTitular: cartaoSelecionado.nomeTitular ?? "",
      dataVigencia: cartaoSelecionado.dataVigencia ?? "",
      cpfCnpj: formatarCpfCnpj(
        cartaoSelecionado.cpfCnpj?.toString() ?? ""
      ),
    },
  }));

  setMostrarModalCartao(true);
};


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
            <strong>Endereço:</strong> {form.endereco.logradouro},{" "}
            {form.endereco.numero}...
            <button
              type="button"
              className="btn-endereco"
              onClick={() => setMostrarModalEndereco(true)}
            >
              {form.endereco.logradouro ? <FaPencilAlt /> : <FaPlus />}

              {form.endereco.logradouro ? "Editar" : "Cadastrar Endereço"}
            </button>
          </p>
          <p>
            <strong>Email:</strong> {usuario.email}
          </p>
        </div>

        {/* CARTÕES */}
        <div className="card">
          <h2>Cartões de Crédito</h2>

          {form.cartoes.length > 0 ? (
            form.cartoes.map((cartao) => (
              <div key={cartao.id} className="cartao">
                <p>
                  <strong>Nome:</strong> {cartao.nomeTitular}
                </p>
                <p>
                  <strong>Bandeira:</strong> {cartao.bandeira}
                </p>
                <p>
                  <strong>Vencimento:</strong> {cartao.dataVigencia}
                </p>
                <button
                  type="button"
                  className="btn-cartao"
                  onClick={() => editarCartao(cartao)}
                >
                  {cartao.usuarioId ? <FaPencilAlt /> : <FaPlus />}

                  {cartao.usuarioId ? "Editar" : "Cadastrar"}
                </button>
              </div>
            ))
          ) : (
            <p>
              Nenhum cartão cadastrado
              <button
                type="button"
                className="btn-cartao"
                onClick={() => setMostrarModalCartao(true)}
              >
                {form.cartao.usuarioId ? <FaPencilAlt /> : <FaPlus />}

                {form.cartao.usuarioId ? "Editar" : "Cartão"}
              </button>
            </p>
          )}

          {mostrarModalEndereco && (
            <div className="modal-overlay">
              <div className="modal">
                <h3>Endereço</h3>

                <input
                  type="text"
                  name="cep"
                  placeholder="CEP"
                  value={form.endereco.cep}
                  onChange={handleEnderecoChange}
                  onBlur={buscarCep}
                />

                <input
                  type="text"
                  name="rua"
                  placeholder="Rua"
                  value={form.endereco.logradouro}
                  onChange={handleEnderecoChange}
                />

                <input
                  type="text"
                  name="numero"
                  placeholder="Número"
                  value={form.endereco.numero}
                  onChange={handleEnderecoChange}
                />

                <input
                  type="text"
                  name="complemento"
                  placeholder="Complemento"
                  value={form.endereco.complemento}
                  onChange={handleEnderecoChange}
                />

                <input
                  type="text"
                  name="bairro"
                  placeholder="Bairro"
                  value={form.endereco.bairro}
                  onChange={handleEnderecoChange}
                />

                <input
                  type="text"
                  name="cidade"
                  placeholder="Cidade"
                  value={form.endereco.cidade}
                  onChange={handleEnderecoChange}
                />

                <input
                  type="text"
                  name="estado"
                  placeholder="Estado"
                  value={form.endereco.estado}
                  onChange={handleEnderecoChange}
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
                    onClick={salvarEndereco}
                  >
                    Salvar
                  </button>
                </div>
              </div>
            </div>
          )}

          {mostrarModalCartao && (
            <div className="modal-overlay">
              <div className="modal">
                <h3>Cartao</h3>

                <input
                  type="text"
                  name="nomeTitular"
                  placeholder="Nome Titular"
                  value={form.cartao.nomeTitular}
                  onChange={handleCartaoChange}
                />
                <input
                  type="text"
                  name="cpfCnpj"
                  placeholder="Cpf/Cnpj"
                  value={form.cartao.cpfCnpj}
                  onChange={handleCartaoChange}
                />
                <select
                  name="bandeira"
                  value={form.cartao.bandeira}
                  onChange={handleCartaoChange}
                  className="form-control"
                >
                  <option value="">Selecione a bandeira</option>
                  <option value="Visa">Visa</option>
                  <option value="Mastercard">Mastercard</option>
                  <option value="Elo">Elo</option>
                  <option value="American Express">American Express</option>
                  <option value="Hipercard">Hipercard</option>
                </select>
                <input
                  type="text"
                  name="numero"
                  placeholder="Número"
                  value={form.cartao.numero}
                  onChange={handleCartaoChange}
                />
                <input
                  type="text"
                  name="codigo"
                  placeholder="Códigor"
                  value={form.cartao.codigo}
                  onChange={handleCartaoChange}
                />
                <input
                  type="text"
                  name="dataVigencia"
                  placeholder="Data Vigência"
                  value={form.cartao.dataVigencia}
                  onChange={handleCartaoChange}
                />
                <div className="modal-actions">
                  <button
                    className="btn-cancelar"
                    type="button"
                    onClick={() => setMostrarModalCartao(false)}
                  >
                    Cancelar
                  </button>

                  <button
                    className="btn-salvar"
                    type="button"
                    onClick={salvarCartao}
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
