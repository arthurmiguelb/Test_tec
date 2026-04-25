# EVIDENCE — Teste Técnico Analista de Qualidade

> Projeto de testes desenvolvido como parte do processo seletivo para **Analista de Qualidade de Software (C# / Playwright)**.  
> O sistema testado é um **controle de gastos residenciais** com backend em .NET e frontend em React/TypeScript.

---

## Índice

- [Visão Geral](#visão-geral)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Pré-requisitos](#pré-requisitos)
- [Configuração do Ambiente](#configuração-do-ambiente)
- [Como Rodar os Testes](#como-rodar-os-testes)
  - [E2E com Cypress](#1-testes-e2e--cypress)
  - [Integração API + Front (Cypress)](#2-testes-de-integração-api--front--cypress)
  - [Integração com C# / xUnit](#3-testes-de-integração--c--xunit)
  - [Unitários com C# / xUnit](#4-testes-unitários--c--xunit)
- [Regras de Negócio Testadas](#regras-de-negócio-testadas)
- [Bugs Encontrados](#bugs-encontrados)

---

## Visão Geral

Este repositório contém **exclusivamente os testes** do sistema. O código da aplicação principal está em um repositório separado e precisa estar em execução antes de rodar qualquer teste.

| Camada        | Tecnologia              | Localização no projeto              |
|---------------|-------------------------|-------------------------------------|
| E2E           | Cypress                 | `tests/Automation/cypress/e2e/front_geral`     |
| Integração API + Front | Cypress          | `tests/Automation/cypress/integration/` |
| Integração    | C# / xUnit              | `tests/integration/MinhasFinancas.IntegrationTests/` |
| Unitário      | C# / xUnit              | `tests/unit/MinhasFinancas.Tests/`  |

---

## Estrutura do Projeto

```
EVIDENCE/
├── bugs/
│   ├── Bug-Global/         # Bugs que afetam todo o sistema
│   ├── Categorias/         # Bugs específicos Módulo categorias
│   ├── Front/              # Bugs encontrados no frontend
│   ├── Pessoas/            # Bugs Módulo de pessoas
│   └── Trasacoes/          # Bugs no cadastro de transações
│
└── tests/
    ├── Automation/                         # Testes com Cypress
    │   ├── cypress/
    │   │   ├── e2e/                         
    |   |   ├── front_geral/                # Testes E2E (fluxo completo do usuário)
    │   │   ├── fixtures/                   # Dados de teste (JSON)
    │   │   ├── integration/                # Testes de integração API + Front
    │   │   └── support/                    # Comandos customizados e configurações
    │   ├── cypress.config.js
    │   ├── package.json
    │   └── package-lock.json
    │
    ├── integration/
    │   └── MinhasFinancas.IntegrationTests/
    │       ├── Regra1/                     # Menor de idade não pode ter receitas
    │       ├── Regra2/                     # Categoria só pode ser usada conforme sua finalidade
    │       ├── Regra3/                     # Exclusão em cascata ao excluir pessoa
    │       ├── CustomWebApplicationFactory.cs
    │       ├── TestDataBuilder.cs
    │       └── MinhasFinancas.IntegrationTests.csproj
    │
    └── unit/
        └── MinhasFinancas.Tests/           # Testes unitários
            ├── CategoriaValidationTests.cs
            ├── MinhasFinancas.Application.Tests.csproj
            └── MinhasFinancas.Tests.csproj
```

---

## Pré-requisitos

Antes de tudo, certifique-se de ter instalado:

| Ferramenta    | Versão mínima | Para quê                        |
|---------------|---------------|---------------------------------|
| Node.js       | 18.x ou superior | Rodar o Cypress               |
| .NET SDK      | 9.0 ou superior  | Rodar os testes C# + API             |
| Bun           | qualquer         | Rodar Front         |

---

## Configuração do Ambiente

### Passo 1 — Subir o projeto principal (obrigatório)

> ⚠️ **Todos os testes dependem do projeto principal rodando.** Clone e suba o repositório da aplicação antes de qualquer coisa.

O projeto principal deve estar disponível nos seguintes endereços:

| Serviço  | URL                    |
|----------|------------------------|
| API      | `http://localhost:5000` |
| Frontend | `http://localhost:5173` |

Caso sua API não fique na porta `localhost:5000` rode o seguinte comando:
```bash
dotnet run --urls http://localhost:5000
```


Consulte o README do repositório principal para instruções de como subir a aplicação.

### Passo 2 — Configurar o caminho da API nos projetos C#

Os projetos de testes unitários e de integração referenciam o código-fonte da aplicação principal. Antes de rodar os testes C#, você precisa informar o caminho da pasta raiz da API no arquivo `.csproj` correspondente.

Abra os arquivos abaixo e substitua o valor de `API_PATH` pelo caminho absoluto da pasta do projeto principal na sua máquina:

**`tests/integration/MinhasFinancas.IntegrationTests/MinhasFinancas.IntegrationTests.csproj`**
**`tests/unit/MinhasFinancas.Tests/MinhasFinancas.Tests.csproj`**
**`tests/unit/MinhasFinancas.Tests/MinhasFinancas.Application.Tests.csproj`**

```xml
<ApiPath>C:\caminho\para\o\projeto-principal\src</ApiPath>
```

> ⚠️ O caminho deve apontar para a pasta que contém o diretório `MinhasFinancas.Application`.

---

### Passo 3 — Clonar este repositório

```bash
git clone <url-deste-repositorio>
cd EVIDENCE
```

---

### Passo 4 — Instalar dependências do Cypress

```bash
cd tests/Automation
npm install
```

---

## Como Rodar os Testes

### 1. Testes E2E — Cypress

Testam o fluxo completo do usuário no navegador, do cadastro à exclusão.

**Com interface gráfica (recomendado para visualizar os testes rodando):**

```bash
cd tests/Automation
npx cypress open
```

Na interface do Cypress, selecione `E2E Testing` e escolha qualquer arquivo da pasta `cypress/e2e/`.

**Em modo headless (linha de comando, ideal para CI):**

```bash
cd tests/Automation
npx cypress run --spec "cypress/e2e/**/*.cy.js"
```

---

### 2. Testes de Integração API + Front — Cypress

Testam a comunicação entre o frontend e a API, validando que os dados trafegam e são exibidos corretamente na tela.

**Com interface gráfica:**

```bash
cd tests/Automation
npx cypress open
```

Na interface, selecione `E2E Testing` e escolha os arquivos dentro de `cypress/e2e/integration/`.

**Em modo headless:**

```bash
cd tests/Automation
npx cypress run --spec "cypress/integration/**/*.cy.js"
```

---

### 3. Testes de Integração — C# / xUnit


Testam as regras de negócio fazendo requisições HTTP reais para a API em `http://localhost:5000`. **A aplicação principal precisa estar rodando antes de executar estes testes.**

**Rodar todos os testes de integração:**

```bash
cd tests/integration/MinhasFinancas.IntegrationTests
dotnet test
```

**Rodar apenas uma regra específica:**

```bash
# Apenas a Regra 1 (menor de idade não pode ter receitas)
dotnet test --filter "FullyQualifiedName~Regra1"

# Apenas a Regra 2 (categoria conforme finalidade)
dotnet test --filter "FullyQualifiedName~Regra2"

# Apenas a Regra 3 (exclusão em cascata)
dotnet test --filter "FullyQualifiedName~Regra3"
```

**Rodar com output detalhado (ver nome de cada teste):**

```bash
dotnet test --logger "console;verbosity=detailed"
```

---

### 4. Testes Unitários — C# / xUnit

Testam a lógica de negócio de forma isolada, sem banco de dados e sem chamadas HTTP. São os testes mais rápidos da suíte.

```bash
cd tests/unit/MinhasFinancas.Tests
dotnet test .\MinhasFinancas.Tests.csproj
```
```bash
cd tests/unit/MinhasFinancas.Tests
dotnet test .\MinhasFinacas.Application.Tests.csproj
```

**Com output detalhado:**

```bash
dotnet test --logger "console;verbosity=detailed"
```

---

### Rodar tudo de uma vez (C# apenas)

A partir da raiz do repositório:

```bash
# Restaura pacotes de todos os projetos .NET
dotnet restore

# Roda unitários + integração em sequência
dotnet test tests/unit/MinhasFinancas.Tests/MinhasFinancas.Tests.csproj
dotnet test tests/integration/MinhasFinancas.IntegrationTests/MinhasFinancas.IntegrationTests.csproj
```

---

## Regras de Negócio Testadas


### Regra 1 — Menor de idade não pode ter receitas
> 
**ATENÇÃO — Módulo de Transações com falha crítica**
>
> Todas as rotas do módulo de transações estão retornando **HTTP 500 Internal Server Error** na API.
> Por conta disso, os testes de integração relacionados às regras de negócio de transações **não puderam ser validados de forma funcional** — apenas a estrutura e a lógica do código de teste foram implementadas.
>
> Ao rodar os testes de integração, os cenários que envolvem transações irão falhar com erro `500`, refletindo o comportamento atual da API. Isso foi documentado como bug e **nenhuma alteração foi feita no código da aplicação**.

| Cenário | Entrada | Resultado esperado |
|---|---|---|
| Menor + receita | Pessoa com 17 anos, tipo `receita` | `422 Unprocessable Entity` |
| Menor + despesa | Pessoa com 17 anos, tipo `despesa` | `201 Created` |
| Maior + receita | Pessoa com 30 anos, tipo `receita` | `201 Created` |
| Exatamente 18 anos + receita | Pessoa que faz 18 anos hoje | `201 Created` |
| 17 anos e 364 dias + receita | Um dia antes de completar 18 | `422 Unprocessable Entity` |

### Regra 2 — Categoria só pode ser usada conforme sua finalidade

| Cenário | Entrada | Resultado esperado |
|---|---|---|
| Categoria `Receita` em transação `despesa` | incompatível | `422 Unprocessable Entity` |
| Categoria `Despesa` em transação `receita` | incompatível | `422 Unprocessable Entity` |
| Categoria `Ambas` em transação `receita` | compatível | `201 Created` |
| Categoria `Ambas` em transação `despesa` | compatível | `201 Created` |

### Regra 3 — Exclusão em cascata ao excluir pessoa

| Cenário | Ação | Resultado esperado |
|---|---|---|
| Deletar pessoa com transações | `DELETE /pessoas/{id}` | `204` + transações removidas |
| Verificar transações após exclusão | `GET /transacoes?pessoaId={id}` | lista vazia |
| Verificar pessoa deletada | `GET /pessoas/{id}` | `404 Not Found` |
| Categorias não afetadas | `GET /categorias` | categorias intactas |

---

## Bugs Encontrados

Os bugs identificados durante a execução dos testes estão documentados na pasta `bugs/`, organizados por módulo.

```
bugs/
├── Bug-Global/    # Problemas transversais ao sistema
├── Categorias/    # Comportamentos incorretos no módulo de categorias
├── Front/         # Falhas de exibição ou interação no frontend
├── Pessoas/       # Erros no Módulo de pessoas
└── Trasacoes/     # Regras de negócio não aplicadas em transações
```

> Conforme instruções do teste técnico, **nenhuma alteração foi feita no código da aplicação**. Todos os problemas encontrados foram apenas documentados.

---

## Observações Finais

- Os testes de integração C# fazem requisições HTTP reais para `http://localhost:5000`, portanto a API precisa estar rodando antes de executá-los.
- Os testes E2E e de integração Cypress dependem da aplicação principal rodando nas portas `5000` (API) e `5173` (Front).
- Os testes unitários C# são os únicos completamente independentes — não precisam de nenhum serviço rodando.
- Qualquer falha nos testes representa uma divergência entre o comportamento implementado e a regra de negócio esperada — o resultado foi documentado como bug, não corrigido.