# 📚 Library API

API REST para gerenciamento de uma biblioteca. Desenvolvida em **.NET 8**, utiliza banco de dados **PostgreSQL**, **EF Core**, testes unitários com **xUnit** e testes de integração com **Testcontainers**.

## 🧰 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker](https://www.docker.com/)
- [xUnit](https://xunit.net/)
- [Testcontainers](https://dotnet.testcontainers.org/)
- [Swagger](https://swagger.io/)

## ⚙️ Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- Editor de código como [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

## 🚀 Como rodar o projeto localmente

**1. Clone o repositório**

**2. Suba o banco de dados PostgreSQL com Docker**: Rode o comando na raiz do projeto
```bash
docker compose up -d
```
**3. Aplique as migrations**:

1. Abra o Package Manager Console (Ferramentas > Gerenciador de Pacotes NuGet > Console do Gerenciador de Pacotes)
2. Selecione o projeto Library.Infrastructure como "Projeto Padrão"
3. Execute o comando:
```bash
Update-Database
```
**4. Execute a aplicação**

## 🧪 Como rodar os testes
1. Abra a solução no Visual Studio
2. Vá até a aba Test > Test Explorer
3. Aguarde os testes carregarem
4. Clique com o botão direito em Library.IntegrationTests, Library.UnitTests ou clique em Run All para executar todos os testes

## 🧑‍💻 Autor
Desenvolvido com ❤️ por [Dinis Simoes](https://www.linkedin.com/in/dinis-f-simoes/)
