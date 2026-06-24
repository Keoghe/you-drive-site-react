# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Visão geral

Frontend **You Drive** — aplicação React (SPA) para uma autoescola. Consome uma API .NET (backend separado) por `fetch`. Idioma do domínio e da UI: português do Brasil.

## Comandos

```bash
npm run dev      # servidor de desenvolvimento Vite (HMR)
npm run build    # tsc -b && vite build (build de produção)
npm run lint     # ESLint em todo o projeto
npm run preview  # serve o build de produção localmente
```

Não há suíte de testes configurada no momento.

> ⚠️ O script `build` executa `tsc -b`, mas o repositório não tem `tsconfig*.json` e o código-fonte é `.jsx`/`.js` (não `.tsx`/`.ts`), apesar de o `package.json` listar `typescript` e `typescript-eslint`. Para rodar build, é preciso ou adicionar os `tsconfig` ou ajustar o script.

## Stack

- **React 19** com **React Compiler** habilitado (via `@rolldown/plugin-babel` + `reactCompilerPreset` em [vite.config.ts](vite.config.ts)).
- **Vite 8** como bundler/dev server.
- **react-router-dom 7** para roteamento.
- **sweetalert2** (`Swal`) para todos os diálogos/alertas (sucesso, erro, confirmação).
- **react-leaflet** / **leaflet** para mapas; **react-calendar** para datas; **react-icons** para ícones.

## Arquitetura

- **Ponto de entrada**: [src/main.jsx](src/main.jsx) monta `<App />` e importa os estilos globais (`styles/theme.css` + CSS do leaflet).
- **Roteamento**: [src/App.jsx](src/App.jsx) define todas as rotas. [src/layout/Layout.jsx](src/layout/Layout.jsx) é a **rota-pai** (`path="/"`) que renderiza `Header`, `Footer`, `MobileMenu` e um `<Outlet />`; cada página em [src/pages/](src/pages/) é uma rota-filha.
- **Páginas** (`src/pages/`): uma por rota — `Home`, `Login`, `Cadastro`, `Conta`, `Contato`, `AgendarAulas`, `AulasAgendadas`, `AtivarConta`.
- **Componentes compartilhados** (`src/components/`): `Header`, `Footer`, `MobileMenu`, `Carousel`.

### Autenticação (baseada em localStorage)

Não há context/provider de auth. O estado de login vive em `localStorage`:

- Após login bem-sucedido ([src/pages/Login.jsx](src/pages/Login.jsx)), o objeto do usuário (que **inclui o `token`**) é salvo em `localStorage.setItem("usuario", ...)`.
- Helpers em [src/services/auth.js](src/services/auth.js): `usuarioLogado()` lê/parseia o usuário; `logout(callback)` confirma via `Swal`, limpa o storage e dispara `callback`.
- **Sincronização entre componentes**: após login/logout, dispara-se manualmente `window.dispatchEvent(new Event("storage"))`. O `Header` escuta `window.addEventListener("storage", ...)` para reagir. Ao mexer no estado de auth, **mantenha esse padrão** (dispatch após escrever no localStorage), senão o Header não atualiza.

### Chamadas de API

- A base da API fica em [src/config/api.js](src/config/api.js): `API_BASE_URL` está **hardcoded** como `https://localhost:7095/api` (sem variável de ambiente).
- Padrão: `fetch` cru com `${API_BASE_URL}/...`. Requisições autenticadas enviam `Authorization: Bearer ${usuario.token}` (token vindo de `localStorage`).
- Tratamento de erro convencional: `if (!response.ok) throw new Error(await response.text())` e exibe a mensagem via `Swal.fire({ icon: "error" })`.

### Convenção de status de documentos

Em fluxos de documentos (ex.: [src/pages/AtivarConta.jsx](src/pages/AtivarConta.jsx)), o `status` numérico segue: `0 = Em Análise`, `1 = Aprovado`, `2 = Reprovado`, `null = Não enviado`. Uploads são enviados como **base64** (lidos via `FileReader.readAsDataURL`, removendo o prefixo `data:...,`).

## Convenções

- Comentários, mensagens de UI e termos de negócio em **pt-BR** (Usuário, Documento, Instrutor, Aula, Conta).
- Componentes funcionais com hooks; estado de formulário tipicamente um único objeto `form` com um `handleChange` genérico (`[e.target.name]: e.target.value`).
- Remova `debugger;` e `console.log` de depuração antes de finalizar — há vários espalhados no código atual.
