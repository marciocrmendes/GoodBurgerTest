# GoodBurger API

API REST para gerenciar as operações de uma hamburgueria: cardápio, combos, pedidos e autenticação de usuários.

## Stack

- .NET 10 / ASP.NET Core
- Entity Framework Core + PostgreSQL
- JWT para autenticação

## Como rodar

### Docker Compose (recomendado)

```bash
docker-compose up
```

Sobe a API e o banco de dados automaticamente.

- API: http://localhost:8080
- Documentação interativa (Scalar): http://localhost:8080/scalar

### .NET CLI

Pré-requisitos: [.NET 10 SDK](https://dotnet.microsoft.com/download) e PostgreSQL rodando localmente.

```bash
cd src/GoodBurger.Api
dotnet run
```

A string de conexão pode ser ajustada em `src/GoodBurger.Api/appsettings.json`. As migrações são aplicadas automaticamente na inicialização.

## Testes

```bash
dotnet test tests/GoodBurger.Tests
```

## Credenciais de desenvolvimento

O seed popula automaticamente um usuário padrão (apenas em ambientes não-produção):

| Campo | Valor |
|-------|-------|
| username | `admin` |
| password | `admin123` |

Use essas credenciais no endpoint `POST /auth/login` para obter o JWT e autenticar as demais requisições.

## Arquitetura

O projeto segue **Vertical Slice Architecture**: cada feature (Auth, MenuItems, Combos, Orders) é autocontida com seus próprios handlers, validadores e DTOs, em vez de separar por camadas técnicas horizontais.

Dentro de cada slice:
- **Result pattern** para retorno explícito de erros, sem uso de exceções para fluxo de controle
- **FluentValidation** para validação de entrada
- **Repository pattern** para abstração do acesso a dados

## Possíveis melhorias

- **Value Objects** — campos como `Price`, `Discount` e `Email` poderiam ser Value Objects para garantir invariantes no domínio (ex: preço não negativo, formato de e-mail válido)
- **Micro SaaS multi-tenant** — a estrutura já está bem organizada para evoluir para um modelo SaaS: adicionar um `TenantId` nas entidades e um middleware de resolução de tenant permitiria que múltiplas hamburguerias usassem a mesma instância
- **Outbox pattern** — para operações críticas como criação de pedidos, garantiria consistência entre a persistência e eventuais notificações/eventos
- **Refresh tokens** — a autenticação atual usa JWT de longa duração (8h); refresh tokens melhorariam a segurança sem prejudicar a experiência
- **Domain Events** - para desacoplar ações que ocorrem após eventos importantes (ex: enviar e-mail após criação de pedido) e permitir uma arquitetura mais reativa

## Estrutura

```
src/
  GoodBurger.Api/      # API REST (domínio, features, infraestrutura)
tests/
  GoodBurger.Tests/    # Testes unitários
```
