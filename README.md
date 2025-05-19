# WalletAPI – API de Carteiras Digitais

API REST desenvolvida em C# utilizando .NET 8, seguindo Clean Architecture e DDD, com autenticação JWT e banco de dados relacional (PostgreSQL com fallback para SQLite).

## ✅ Funcionalidades

- Autenticação JWT
- Criação de usuário
- Consultar saldo (próprio ou de outro usuário)
- Adicionar saldo à carteira
- Transferência entre usuários autenticados
- Transferência administrativa entre quaisquer usuários
- Listagem de transferências realizadas (com filtros de data e e-mail)
- Seed automático de dados fictícios

## 🚀 Tecnologias

- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL / SQLite
- Swagger (Swashbuckle)
- BCrypt para hash de senhas

## 🏗️ Arquitetura

- **API**: Controllers e Middlewares
- **Application**: Regras de negócio
- **Domain**: Entidades e contratos
- **Infrastructure**: Banco de dados, repositórios e autenticação

## 🔐 Autenticação

Utiliza JWT Bearer. Após o login, use o token no header
usuario admin: admin@admin.com
senha: admin@123

## 🧪 Usuários pré-cadastrados (seed)

| Email            | Senha    | Saldo Inicial |
|------------------|----------|---------------|
| user1@email.com  | 123456   | R$ 100        |
| user2@email.com  | 123456   | R$ 200        |
| user3@email.com  | 123456   | R$ 300        |
| admin@admin.com  | admin@123| R$ 100        |
|------------------|----------|---------------|

## 🐳 Execução local

1. Configure o `appsettings.json` com suas strings de conexão:

```json
"ConnectionStrings": {
  "PostgreSQL": "Host=localhost;Port=5432;Database=walletapi;Username=postgres;Password=senhabanco",
  "SQLite": "Data Source=walletapi.db"
} 

```markdown

⚠️ Observação
    Caso o PostgreSQL nao tenha o banco de dados conforme o ConnectionStrings, 
	o banco esteja offline ou com usuario é senha inválido, a aplicação usará SQLite automaticamente.

## Funcionamento
Rode o projeto:
dotnet build
dotnet run --project WalletAPI.Api

Acesse o Swagger:
https://localhost:44389/index.html
