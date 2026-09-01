# AgendaConsultas

Aplicação Web desenvolvida em **C# utilizando ASP.NET Core MVC**, criada como projeto da disciplina de **Desenvolvimento Back-end**.

O sistema permite o cadastro e autenticação de usuários e o gerenciamento de consultas. Cada usuário autenticado possui acesso exclusivamente às suas próprias consultas.

## Funcionalidades

* Cadastro de usuários
* Validação dos dados informados
* Armazenamento seguro da senha utilizando hash
* Login e autenticação por Cookies
* Logout
* Proteção de rotas com `[Authorize]`
* Cadastro de consultas
* Listagem das consultas
* Edição de consultas
* Exclusão de consultas
* Relacionamento entre usuários e consultas
* Persistência dos dados em SQL Server

## Tecnologias utilizadas

* C#
* ASP.NET Core MVC
* Entity Framework Core
* SQL Server / LocalDB
* Razor Views
* Bootstrap
* Cookie Authentication
* Data Annotations
* Visual Studio

## Arquitetura

O projeto utiliza o padrão **MVC (Model-View-Controller)**, separando as responsabilidades da aplicação.

### Models

Representam as entidades e regras de validação da aplicação.

Principais entidades:

* `Usuario`
* `Consulta`

O relacionamento entre as entidades é de **um para muitos (1:N)**, em que um usuário pode possuir várias consultas.

### Views

Responsáveis pela interface apresentada ao usuário, utilizando Razor Views e Bootstrap.

### Controllers

Responsáveis por receber as requisições, executar a lógica necessária e realizar a comunicação entre Views e Models.

Principais Controllers:

* `AccountController`
* `ConsultasController`
* `HomeController`

### Data

A pasta `Data` contém o `AppDbContext`, responsável pela integração da aplicação com o Entity Framework Core e o banco de dados.

## Banco de Dados

O projeto utiliza **Entity Framework Core com abordagem Code First**.

O banco utilizado durante o desenvolvimento é SQL Server LocalDB.

A Connection String pode ser configurada no arquivo:

`appsettings.json`

Exemplo:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AgendaConsultasDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

## Configuração do projeto

Após clonar o repositório, abra a solução no Visual Studio.

Confira a Connection String presente no arquivo `appsettings.json`.

Abra o **Package Manager Console** através do menu:

`Tools → NuGet Package Manager → Package Manager Console`

Para aplicar as migrations e criar/atualizar o banco de dados, execute:

```powershell
Update-Database
```

Caso seja necessário criar uma nova migration após alterações nos Models:

```powershell
Add-Migration NomeDaMigration
```

Em seguida:

```powershell
Update-Database
```

## Autenticação e Segurança

A aplicação utiliza autenticação baseada em **Cookies**.

Após realizar o login, são criadas Claims contendo informações do usuário autenticado.

As páginas responsáveis pelo gerenciamento das consultas são protegidas utilizando:

```csharp
[Authorize]
```

O pipeline da aplicação também configura os middlewares na ordem:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

As senhas dos usuários não são armazenadas diretamente em texto puro. Antes da persistência, é utilizado `PasswordHasher` para gerar o hash da senha.

## Controle de acesso às consultas

Cada consulta possui um `UsuarioId`.

Ao acessar, editar ou excluir uma consulta, o sistema verifica o identificador do usuário autenticado.

Dessa forma, um usuário não pode visualizar ou manipular as consultas pertencentes a outro usuário.

## Validação

Os Models utilizam Data Annotations para validação dos dados, incluindo atributos como:

```csharp
[Required]
[EmailAddress]
[StringLength]
```

As validações são verificadas no servidor através do `ModelState`.

## Executando o projeto

1. Clone ou baixe o repositório.
2. Abra a solução no Visual Studio.
3. Configure a Connection String, se necessário.
4. Execute `Update-Database` no Package Manager Console.
5. Compile o projeto.
6. Execute a aplicação.
7. Cadastre um novo usuário.
8. Realize o login.
9. Acesse **Minhas Consultas** para utilizar o CRUD.

## Fluxo principal

```text
Cadastro
   ↓
Login
   ↓
Autenticação
   ↓
Minhas Consultas
   ↓
Cadastrar / Visualizar / Editar / Excluir
   ↓
Logout
```

## Vídeo demonstrativo

O vídeo demonstrativo apresenta o funcionamento do cadastro de usuário, login, autenticação e gerenciamento das consultas.

**Link do vídeo:**
- Postar o vídeo no YouTube!!

## Integrantes

Inserir os integrantes do grupo em ordem alfabética:

Leonardo Nobre Fagnoni

## Disciplina

**Desenvolvimento Back-end**

Projeto acadêmico desenvolvido utilizando ASP.NET Core MVC, Entity Framework Core e SQL Server.
