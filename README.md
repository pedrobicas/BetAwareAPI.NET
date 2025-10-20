# 🎯 BetAware API.NET

## 📋 Sobre o Projeto

API RESTful completa para sistema de apostas esportivas desenvolvida em .NET 9, implementando arquitetura em camadas com foco em boas práticas de desenvolvimento, autenticação JWT, CRUD completo, pesquisas LINQ avançadas e integração com APIs externas.

---

## 🏗️ Arquitetura do Projeto

A solução está organizada em múltiplos projetos seguindo o padrão de arquitetura em camadas:

```
BetAware/
├── 📁 BetAware.Api/          # Controllers e configuração da API
├── 📁 BetAware.Business/     # Lógica de negócio e serviços
├── 📁 BetAware.Data/         # Entity Framework e acesso a dados
├── 📁 BetAware.Model/        # Modelos e DTOs
└── 📁 docs/                  # Documentação e diagramas
```

### 📦 Responsabilidades das Camadas

- **BetAware.Api** – Exposição dos endpoints REST, configuração da aplicação e documentação Swagger
- **BetAware.Business** – Implementação das regras de negócio, serviços e interfaces
- **BetAware.Data** – Contexto do Entity Framework, configuração do banco e migrações
- **BetAware.Model** – Entidades de domínio, DTOs e validações

### 🖼️ Diagramas de Arquitetura

- Arquitetura em Camadas

  ![Arquitetura em Camadas](docs/arquitetura-camada.png)

- Visão Geral da Arquitetura

  ![Visão Geral da Arquitetura](docs/driagrama-arquitetura.png)

- Fluxo de Autenticação

  ![Fluxo de Autenticação](docs/fluxo-autenticacao.png)

---

## 🛠️ Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **ASP.NET Core Web API** - Framework web
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados principal
- **JWT Bearer** - Autenticação e autorização
- **Swagger/OpenAPI** - Documentação interativa da API
- **BCrypt** - Hash seguro de senhas
- **LINQ** - Consultas avançadas
- **HttpClient** - Integração com APIs externas

---

## 🔧 Como Executar

### ⚙️ Pré-requisitos

- .NET 9 SDK
- SQL Server (LocalDB ou instância completa)
- Visual Studio 2022 ou VS Code
- Git

### 🏃‍♂️ Executando o Projeto

1. **Clone o repositório**
```bash
git clone https://github.com/pedrobicas/BetAwareAPI.NET.git
cd BetAwareAPI.NET
```

2. **Restaure as dependências**
```bash
dotnet restore
```

3. **Configure a string de conexão**
Edite o arquivo `appsettings.json` em `BetAware.Api`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BetAwareDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

4. **Execute as migrações do banco**
```bash
dotnet ef database update --project BetAware.Data --startup-project BetAware.Api
```

5. **Execute a aplicação**
```bash
dotnet run --project BetAware.Api
```

6. **Acesse a documentação**
- Swagger UI: `https://localhost:7000/swagger`
- API Base: `https://localhost:7000/`

---

## 📚 Guia de Uso da API

### 🔐 Autenticação

A API utiliza JWT Bearer Token para autenticação. Siga os passos:

#### 1. Registrar um novo usuário
```http
POST /v1/auth/register
Content-Type: application/json

{
  "nome": "João Silva",
  "username": "joao",
  "email": "joao@email.com",
  "senha": "MinhaSenh@123",
  "perfil": "USER"
}
```

#### 2. Fazer login
```http
POST /v1/auth/login
Content-Type: application/json

{
  "username": "joao",
  "senha": "MinhaSenh@123"
}
```

#### 3. Usar o token nas requisições
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 🎯 Principais Endpoints

#### 🎲 Apostas
- `GET /v1/apostas` - Listar apostas do usuário logado
- `POST /v1/apostas` - Criar nova aposta
- `GET /v1/apostas/{id}` - Obter aposta específica
- `PUT /v1/apostas/{id}` - Atualizar aposta
- `DELETE /v1/apostas/{id}` - Deletar aposta
- `GET /v1/apostas/pesquisar` - Pesquisa avançada com filtros

#### 🌐 APIs Externas
- `GET /v1/external/cep/{cep}` - Consultar endereço por CEP
- `GET /v1/external/cotacao` - Obter cotação de moedas
- `GET /v1/external/jogos` - Listar jogos esportivos disponíveis
- `GET /v1/external/tempo/{cidade}` - Previsão do tempo
- `GET /v1/external/dashboard/{cep}` - Dashboard com dados integrados

#### 👥 Administração (Requer perfil ADMIN)
- `GET /v1/usuarios` - Listar todos os usuários
- `GET /v1/usuarios/{id}` - Obter usuário específico
- `PUT /v1/usuarios/{id}` - Atualizar usuário
- `DELETE /v1/usuarios/{id}` - Deletar usuário
- `GET /v1/usuarios/pesquisar` - Pesquisar usuários

### 📝 Exemplos de Uso

#### Criar uma aposta
```json
{
  "categoria": "FUTEBOL",
  "jogo": "Flamengo vs Palmeiras",
  "valor": 50.00,
  "resultado": "PENDENTE",
  "data": "2024-12-20T20:00:00"
}
```

#### Pesquisar apostas com filtros
```http
GET /v1/apostas/pesquisar?categoria=FUTEBOL&valorMinimo=10&valorMaximo=100&page=1&pageSize=10&orderBy=Data&ascending=false
```

#### Consultar CEP
```http
GET /v1/external/cep/01310-100
```

---

## 🔒 Segurança Implementada

- **Autenticação JWT** com tokens seguros e expiração configurável
- **Autorização baseada em roles** (USER/ADMIN)
- **Validação rigorosa de dados** com Data Annotations
- **Proteção contra SQL Injection** via Entity Framework
- **CORS configurado** para ambientes específicos
- **Senhas hasheadas** com BCrypt para máxima segurança
- **Validação de entrada** em todos os endpoints

---

## 🧪 Testes e Validação

### Testando com Swagger UI

1. Acesse `https://localhost:7000/swagger`
2. Registre um usuário via endpoint de registro
3. Faça login para obter o token JWT
4. Clique em "Authorize" e insira: `Bearer {seu_token}`
5. Teste todos os endpoints protegidos

### Cenários de Teste Recomendados

- ✅ Registro e login de usuários
- ✅ CRUD completo de apostas
- ✅ Pesquisas com diferentes filtros
- ✅ Integração com APIs externas
- ✅ Autorização de endpoints administrativos

---

## 📊 Estrutura do Banco de Dados

### Entidades Principais

#### Usuario
- Id, Nome, Username, Email, Senha, Perfil, DataCriacao

#### Aposta
- Id, UsuarioId, Categoria, Jogo, Valor, Resultado, Data

### Relacionamentos
- Um usuário pode ter muitas apostas (1:N)
- Chaves estrangeiras configuradas com Entity Framework

---

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/NovaFuncionalidade`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova funcionalidade'`)
4. Push para a branch (`git push origin feature/NovaFuncionalidade`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👥 Integrantes

Felipe Terra (RM: 99405)
Pedro Bicas (RM: 99534)
Gabriel Doms (RM: 98630)
Lucas Vassão (RM: 98607)
Bryan Willian (RM: 551305)

---