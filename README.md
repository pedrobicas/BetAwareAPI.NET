# 🎯 BetAware API.NET

## 📋 Sobre o Projeto

API RESTful para sistema de apostas esportivas desenvolvida em .NET 9, implementando arquitetura em camadas com foco em boas práticas de desenvolvimento, autenticação JWT e integração com banco de dados SQL Server.

---

## 🏗️ Arquitetura do Projeto

A solução está organizada em múltiplos projetos seguindo o padrão de arquitetura em camadas:

```
BetAware/
├── 📁 BetAware.Api/          # Controllers e configuração da API
├── 📁 BetAware.Business/     # Lógica de negócio e serviços
├── 📁 BetAware.Data/         # Entity Framework e acesso a dados
└── 📁 BetAware.Model/        # Modelos e DTOs
```

### 📦 Responsabilidades das Camadas

- **BetAware.Api** – Exposição dos endpoints REST e configuração da aplicação
- **BetAware.Business** – Implementação das regras de negócio e serviços
- **BetAware.Data** – Contexto do Entity Framework e configuração do banco
- **BetAware.Model** – Entidades de domínio e objetos de transferência de dados

---

## 🛠️ Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server** - Banco de dados principal
- **JWT Bearer** - Autenticação e autorização
- **Swagger/OpenAPI** - Documentação da API
- **BCrypt** - Hash de senhas

---

## 🚀 Como Executar

### ⚙️ Pré-requisitos

- .NET 9 SDK
- SQL Server (LocalDB ou instância completa)
- Git

### 🏃‍♂️ Executando o Projeto

```bash
# Clone o repositório
git clone https://github.com/pedrobicas/BetAwareAPI.NET.git

# Acesse a pasta do projeto
cd BetAwareAPI.NET/BetAware

# Restaure as dependências
dotnet restore

# Configure a connection string no appsettings.json se necessário

# Execute as migrations
dotnet ef migrations add InitialCreate --project BetAware.Data --startup-project BetAware.Api
dotnet ef database update --project BetAware.Data --startup-project BetAware.Api

# Execute a aplicação
dotnet run --project BetAware.Api
```

A API estará disponível em: `https://localhost:7000` e `http://localhost:5000`

---

## 📚 Endpoints Disponíveis

### 🔐 Autenticação
- **POST** `/v1/auth/register` - Cadastro de usuário
- **POST** `/v1/auth/login` - Login e obtenção do token JWT

### 🎰 Apostas
- **GET** `/v1/apostas` - Listar todas as apostas do usuário
- **GET** `/v1/apostas/{id}` - Obter aposta específica
- **POST** `/v1/apostas` - Criar nova aposta
- **PUT** `/v1/apostas/{id}` - Atualizar aposta
- **DELETE** `/v1/apostas/{id}` - Excluir aposta

### 💊 Health Check
- **GET** `/v1/health` - Verificar status da API

### 📖 Documentação
- **Swagger UI**: `/swagger`

---

## 📊 Modelos de Dados

### Usuario
```json
{
  "id": "int",
  "nome": "string",
  "email": "string",
  "senha": "string (hash)",
  "dataCriacao": "DateTime"
}
```

### Aposta
```json
{
  "id": "int",
  "usuarioId": "int",
  "evento": "string",
  "valorApostado": "decimal",
  "odds": "decimal",
  "status": "string",
  "dataAposta": "DateTime"
}
```

---

## 🔧 Configuração

### Connection String
Edite o arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BetAwareDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### JWT Settings
```json
{
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-aqui-muito-longa-e-segura",
    "Issuer": "BetAware",
    "Audience": "BetAware-Users",
    "ExpiryMinutes": 60
  }
}
```

---

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

---

## 👨‍💻 Autor

**Pedro Bicas**
- GitHub: [@pedrobicas](https://github.com/pedrobicas)