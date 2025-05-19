# WalletAPI – API de Carteiras Digitais

API REST desenvolvida em C# utilizando .NET 8, seguindo Clean Architecture e DDD, com autenticação JWT e banco de dados relacional (PostgreSQL com fallback para SQLite).

## ✅ Funcionalidades

- Autenticação JWT
- Criação de usuário
- Consulta de saldo (próprio ou de outro usuário)
- Adição de saldo à carteira
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

Utiliza JWT Bearer. Após o login, utilize o token no cabeçalho da requisição.

**Usuário admin:**  
Email: `admin@admin.com`  
Senha: `admin@123`

## 🧪 Usuários pré-cadastrados (Seed)

| Email             | Senha     | Saldo Inicial |
|-------------------|-----------|----------------|
| user1@email.com   | 123456    | R$ 100         |
| user2@email.com   | 123456    | R$ 200         |
| user3@email.com   | 123456    | R$ 300         |
| admin@admin.com   | admin@123 | R$ 100         |

## ⚙️ Configuração

1. Edite o arquivo `appsettings.json` com suas strings de conexão:

```json
"ConnectionStrings": {
  "PostgreSQL": "Host=localhost;Port=5432;Database=walletapi;Username=postgres;Password=senhabanco",
  "SQLite": "Data Source=walletapi.db"
}
```

⚠️ **Observação**  
- Caso o PostgreSQL não tenha o banco de dados conforme o `ConnectionStrings`,  
- ou o banco esteja offline, ou o usuário e/ou senha estejam inválidos,  
- a aplicação usará automaticamente o **SQLite** como fallback.

## ▶️ Funcionamento

### Rodando via terminal:

```bash
dotnet build
dotnet run --project WalletAPI.Api
```

### Rodando via Visual Studio:

- Abra a solução;
- Escolha o projeto **WalletAPI.Api** ou **Docker** como projeto de inicialização;
- Pressione **F5** para executar.

### ⚠️ Usando Docker:

- Certifique-se de que o **Docker Desktop** está instalado e em execução;
- Caso contrário, a aplicação em Docker não funcionará corretamente;
- Para mais detalhes, consulte a documentação oficial:  
  [https://docs.docker.com/](https://docs.docker.com/)

## 📘 Swagger

Acesse a documentação da API através do navegador:

[https://localhost:44389/index.html](https://localhost:44389/index.html) – Projeto **WalletAPI.Api**
