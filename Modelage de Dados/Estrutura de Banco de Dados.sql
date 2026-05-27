PERMISSOES → USUARIOS → ENDERECOS
                      ↘
                       FINANCEIRO_USUARIO
                       CARTOES
                       CONTATOS

USUARIOS → INSTRUTORES → VEICULOS

USUARIOS → AULAS → INSTRUTORES
                  → VALORES_AULA
                  → PROMOCOES

-- =========================================
-- PERMISSOES
-- =========================================
CREATE TABLE permissoes (
    id INT IDENTITY PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    descricao VARCHAR(255),
    excluido BIT DEFAULT 0
);

-- =========================================
-- ENDERECOS
-- =========================================
CREATE TABLE enderecos (
    id INT IDENTITY PRIMARY KEY,
    cep VARCHAR(10),
    logradouro VARCHAR(150),
    numero VARCHAR(20),
    complemento VARCHAR(100),
    bairro VARCHAR(100),
    cidade VARCHAR(100),
    estado VARCHAR(2),
    excluido BIT DEFAULT 0
);

-- =========================================
-- USUARIOS
-- =========================================
CREATE TABLE usuarios (
    id INT IDENTITY PRIMARY KEY,
    permissao_id INT,
    endereco_id INT,

    nome VARCHAR(100) NOT NULL,
    cpf VARCHAR(14) UNIQUE NOT NULL,
    cnh VARCHAR(20),
    data_nascimento DATE NOT NULL,

    email VARCHAR(150) UNIQUE NOT NULL,
    senha VARCHAR(255) NOT NULL,

    saldo DECIMAL(10,2) DEFAULT 0,
    data_cadastro DATETIME DEFAULT GETDATE(),

    excluido BIT DEFAULT 0,

    FOREIGN KEY (permissao_id) REFERENCES permissoes(id),
    FOREIGN KEY (endereco_id) REFERENCES enderecos(id)
);

-- =========================================
-- INSTRUTORES
-- =========================================
CREATE TABLE instrutores (
    id INT IDENTITY PRIMARY KEY,
    usuario_id INT,

    avaliacao DECIMAL(2,1) DEFAULT 0,
    valor_hora DECIMAL(10,2),

    latitude DECIMAL(10,6),
    longitude DECIMAL(10,6),

    ativo BIT DEFAULT 1,
    excluido BIT DEFAULT 0,

    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);

-- =========================================
-- VEICULOS
-- =========================================
CREATE TABLE veiculos (
    id INT IDENTITY PRIMARY KEY,
    instrutor_id INT,

    modelo VARCHAR(100),
    cor VARCHAR(50),
    placa VARCHAR(10),

    excluido BIT DEFAULT 0,

    FOREIGN KEY (instrutor_id) REFERENCES instrutores(id)
);

-- =========================================
-- VALORES AULA
-- =========================================
CREATE TABLE valores_aula (
    id INT IDENTITY PRIMARY KEY,
    descricao VARCHAR(100),
    valor DECIMAL(10,2),
    duracao_minutos INT,
    excluido BIT DEFAULT 0
);

-- =========================================
-- PROMOCOES
-- =========================================
CREATE TABLE promocoes (
    id INT IDENTITY PRIMARY KEY,

    descricao VARCHAR(150),
    percentual_desconto DECIMAL(5,2),
    valor_desconto DECIMAL(10,2),

    data_inicio DATETIME,
    data_fim DATETIME,

    ativa BIT DEFAULT 1,
    excluido BIT DEFAULT 0
);

-- =========================================
-- AULAS
-- =========================================
CREATE TABLE aulas (
    id INT IDENTITY PRIMARY KEY,

    usuario_id INT,
    instrutor_id INT,
    valor_aula_id INT,
    promocao_id INT NULL,

    data_aula DATE,
    hora_inicio TIME,
    hora_fim TIME,

    status VARCHAR(20),
    valor_final DECIMAL(10,2),

    excluido BIT DEFAULT 0,

    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
    FOREIGN KEY (instrutor_id) REFERENCES instrutores(id),
    FOREIGN KEY (valor_aula_id) REFERENCES valores_aula(id),
    FOREIGN KEY (promocao_id) REFERENCES promocoes(id)
);

-- =========================================
-- CARTOES
-- =========================================
CREATE TABLE cartoes (
    id INT IDENTITY PRIMARY KEY,
    usuario_id INT,

    bandeira VARCHAR(50),
    numero VARCHAR(20),
    final CHAR(4),
    nome_titular VARCHAR(100),

    excluido BIT DEFAULT 0,

    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);

-- =========================================
-- FINANCEIRO USUARIO
-- =========================================
CREATE TABLE financeiro_usuario (
    id INT IDENTITY PRIMARY KEY,

    usuario_id INT,

    tipo VARCHAR(20), -- CREDITO, DEBITO, PLATAFORMA
    descricao VARCHAR(255),

    valor DECIMAL(10,2),

    data_movimento DATETIME DEFAULT GETDATE(),

    excluido BIT DEFAULT 0,

    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);

-- =========================================
-- CONTATOS
-- =========================================
CREATE TABLE contatos (
    id INT IDENTITY PRIMARY KEY,

    usuario_id INT NULL,
    email VARCHAR(150),
    mensagem TEXT,

    data_envio DATETIME DEFAULT GETDATE(),

    excluido BIT DEFAULT 0,

    FOREIGN KEY (usuario_id) REFERENCES usuarios(id)
);