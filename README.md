# Documento de Arquitetura Health & Med

## Introdução

Este documento descreve a arquitetura de um sistema de agendamento de consultas médicas online desenvolvido em .NET com C#, utilizando MongoDB como banco de dados e Angular como framework front-end. A aplicação tem como objetivo fornecer uma plataforma eficiente e segura para que os pacientes consigam se cadastrar e fazer seus agendamentos de consultas médicas totalmente online e listagem dos médicos disponíveis. O sistema foi projetado seguindo os princípios da Clean Architecture, garantindo uma separação clara de responsabilidades e promovendo a manutenção, escalabilidade e testabilidade do código. A arquitetura é dividida em várias camadas, incluindo API, APPLICATION, DOMAIN, INFRA.DATA, INFRA.IOC, e TESTS, cada uma responsável por um aspecto específico da funcionalidade e fluxo de dados da aplicação.

## Objetivo do Sistema

O sistema de agendamento de consultas médicas online foi concebido para atender às seguintes necessidades:

- **Consulta de disponibilidade dos médicos:** Permitir que os pacientes consigam verificar quais médicos estão disponíveis.
- **Agendamento de consultas:** Facilitar o agendamento de consultas com os médicos disponíveis na data e horário que seja mais adequado ao paciente.
- **Autenticação de médicos e pacientes:** Garantir a segurança dos dados dos usuários através de um mecanismo robusto de autenticação baseado em tokens JWT (JSON Web Tokens).
- **Escalabilidade e Performance:** Suportar um grande volume de dados e requisições simultâneas, utilizando MongoDB como banco de dados para garantir alta performance e escalabilidade.
- **Facilidade de Manutenção e Evolução:** Utilizar uma arquitetura modular e desacoplada para permitir a fácil adição de novas funcionalidades e manutenção do sistema.

## Justificativa para a Arquitetura e Tecnologias Escolhidas

A escolha do MongoDB como banco de dados foi motivada por sua flexibilidade no armazenamento de documentos JSON, alta performance em operações de escrita e capacidade de escalabilidade horizontal. Essas características o tornam ideal para uma aplicação que lida com dados financeiros complexos e variáveis, como o gerenciamento de ativos e transações.

O uso do C# e do .NET no backend fornece uma base sólida para o desenvolvimento, graças à robustez da linguagem, suporte para práticas de desenvolvimento orientadas a objetos, alta performance, e uma ampla gama de bibliotecas e ferramentas para segurança e comunicação com bancos de dados. Além disso, o framework .NET Core é multiplataforma, permitindo fácil implantação em diferentes ambientes, incluindo containers Docker.

Angular é conhecido por sua forte estrutura modular, facilitando a organização do código em módulos de funcionalidades bem definidos e oferecendo as ferramentas necessárias para o desenvolvimento da aplicação. Componentes, serviços e módulos podem ser estruturados de acordo com os princípios da arquitetura limpa.

## Estrutura do Documento

Este documento de arquitetura está organizado nas seguintes seções:

1. **Descrição Geral da Arquitetura:** Uma visão geral da arquitetura do sistema, detalhando cada camada e suas responsabilidades.
2. **Modelo de Dados:** Definição das entidades principais, incluindo suas propriedades e relacionamentos.
3. **Fluxo de Dados e Casos de Uso:** Descrição dos principais fluxos de dados e casos de uso da aplicação, incluindo autenticação, criação de portfólios e registro de transações.
4. **Definição dos Endpoints da API:** Detalhamento dos endpoints disponíveis na API RESTful, incluindo métodos, parâmetros e exemplos de requisições e respostas.
5. **Camadas da Aplicação:** Explicação detalhada das responsabilidades de cada camada da arquitetura (API, Application, Domain, Infra.Data, Infra.IOC, Tests).
6. **Casos de Teste:** Definição dos casos de teste para garantir a qualidade e a robustez da aplicação.

Este documento é destinado a desenvolvedores, arquitetos de software, engenheiros de DevOps e outros profissionais interessados em entender a estrutura e os fundamentos técnicos do sistema de agendamento de consultas. Ele serve como guia para o desenvolvimento, manutenção e evolução contínua da aplicação.

## Estrutura da Aplicação

O projeto será dividido nas seguintes camadas:

- **API:** Responsável por expor os endpoints da aplicação via HTTP usando controllers.
- **APPLICATION:** Contém os casos de uso, serviços e DTOs (Data Transfer Objects). Esta camada orquestra o fluxo de dados entre a API e a camada DOMAIN.
- **DOMAIN:** Contém as entidades, interfaces de repositório e regras de negócio.
- **INFRA.DATA:** Implementação dos repositórios e contextos de banco de dados, bem como qualquer lógica de persistência específica.
- **INFRA.IOC:** Configuração da Injeção de Dependência (Dependency Injection).
- **TESTS:** Contém os testes unitários e de integração.

## Entidades e Regras de Negócio

### 1. Usuário
- **GUID:** Identificador único
- **Nome:** String
- **CPF:** String
- **Email:** String
- **Senha:** String
- **TipoCadastro:** Char(1)

### 2. Médico
- **GUID:** Identificador único
- **UsuarioGUID:** String
- **CRM:** String

### 3. Agendamento
- **GUID:** Identificador único
- **Data:** Datetime
- **MedicoGUID:** String
- **UsuarioGUID:** String

### 4. Disponibilidade
- **GUID:** Identificador único
- **MedicoGUID:** String
- **DiaSemana:** Integer (0 é domingo, 6 é sábado)
- **HorarioInicio:** Time
- **HorarioFim:** Time
- **DataEspecifica:** Date NULLABLE (Pode ser nulo, para casos de disponibilidade apenas 1 dia da semana)
- **Repeticao:** String
- **Observacao:** String

### 5. Notificações
- **GUID:** Identificador único
- **CorpoEmail:** String
- **Assunto:** String
- **Destinatario:** String
- **UsuarioGUID:** String

## Endpoints

### Cadastra período de atendimento
**Endpoint para cadastra período de atendimento do médico**  
**POST:** `/Medico/cadastrar-periodo-atendimento`  
**Requisição:**
```json
{
  "idMedico": "string",
  "diaDaSemana": int,
  "inicio": "string",
  "fim": "string"
}
```
**Resposta:**
Status: 200

### Listar período de atendimento
**Endpoint para lista todos os períodos de atendimento do médicos**  
**GET:** `/Medico/listar-periodo-atendimento/{id}`  
**Resposta:**
```json
[
  {
    "idMedico": "string",
    "diaDaSemana": int,
    "inicio": "string",
    "fim": "string",
    "id": "string"
  }
]
```

### Liberar agenda
**Endpoint para cadastrar as agendas disponíveis do médico**  
**POST:** `/Medico/liberar-agenda`  
**Requisição:**
```json
{
  "idMedico": "string",
  "dataLiberar": "string"
}
```
**Resposta:**
Status: 200

### Listar agenda
**Endpoint para listar as agendas do médico**  
**GET:** `/Medico/listar-agenda/{id}`  
**Resposta:**
```json
[
  {
    "idMedico": "string",
    "idPaciente": "string",
    "dataConsulta": "string",
    "disponivel": bool,
    "isLocked": bool,
    "id": "string"
  }
]
```

### Listar
**Endpoint para listar todos os médicos**  
**GET:** `/Medico/listar`  
**Resposta:**
```json
[  
  {
    "nome": "string",
    "cpf": "string",
    "crm": "string",
    "tempoDeConsulta": int,
    "tipo": "string",
    "email": "string",
    "senha": "string",
    "periodoAtendimento": [],
    "agendaDiaria": [],
    "id": "string"
  }
]
```

### Marcar Consulta
**Endpoint para marcar consulta**  
**POST:** `/Paciente/marcar-consulta`  
**Requisição:**
```json
{      
  "dataConsulta": "string",
  "idMedico": "string",
  "idPaciente": "string"
}
```
**Resposta:**
Status: 200

### Consultar agenda
**Endpoint consultar agenda**  
**GET:** `/Paciente/listar-consultas-agendadas/{GUID}`  
**Requisição:**
```json
{ 
  "UsuarioId": "{usuarioId}" 
}
```
**Resposta:**
```json
[  
  { 
    "data": "20/10/2024", 
    "horario": "15:00", 
    "medico": "Dr. João" 
  } 
]
```

### Autenticação
**Endpoint para autenticação de usuários**  
**POST:** `/Usuario/autenticar`  
**Requisição:**
```json
{ 
  "email": "usuario@exemplo.com", 
  "senha": "senha123"  
}
```
**Resposta:**
```json
{ 
  "token": "JWT_TOKEN"   
}
```

### Cadastrar Médico
**Endpoint para cadastrar um novo médico**  
**POST:** `/Usuario/cadastrar-medico`  
**Requisição:**
```json
{  
  "nome": "João da Silva",  
  "email": "joao@exemplo.com",  
  "cpf": "123456789", 
  "senha": "senha123", 
  "crm": "123455678", 
  "tipoCadastro": "M" 
}
```
**Resposta:**
Status: 200

### Cadastrar Paciente
**Endpoint para cadastrar um novo usuário**  
**POST:** `/Usuario/cadastrar-paciente`  
**Requisição:**
```json
{  
  "nome": "string",  
  "email": "string",  
  "cpf": int, 
  "senha": "string", 
  "tipoCadastro": "string"
}  
```
**Resposta:**
Status: 200

## Casos de Uso

### Caso de Uso: Cadastro de Usuário

- **Ator:** Usuário  
- **Descrição:** O usuário realiza seu cadastro na plataforma, preenchendo as informações necessárias. Após o preenchimento e validação dos dados, o sistema registra o usuário no banco de dados e retorna um token de autenticação.

### Caso de Uso: Agendamento de Consulta

- **Ator:** Usuário  
- **Descrição:** O usuário faz o login na plataforma e seleciona um médico, verifica a disponibilidade e agenda uma consulta. O sistema armazena a consulta no banco de dados e notifica o usuário por e-mail.

# Instalação de Projeto C# com .NET e MongoDB

## Pré-requisitos

Antes de começar, certifique-se de que você possui as seguintes ferramentas instaladas:

1. **.NET SDK**: Baixe e instale o SDK do .NET em [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).
2. **MongoDB**: Você pode instalar o MongoDB localmente ou usar um serviço de MongoDB na nuvem, como o MongoDB Atlas.
3. **Git**: Instale o Git em [git-scm.com](https://git-scm.com/).

## Clonando o Repositório

1. **Abra o terminal ou prompt de comando.**
2. **Navegue até o diretório onde deseja clonar o projeto.**
   ```bash
   cd /caminho/para/seu/diretorio
3. **Clone o repositório.**
   ```bash
   git clone https://github.com/parissenti/HackatonFiap.git
4. **Navegue até o diretório do projeto clonado.**

## Configurando o MongoDB

1. **Crie um banco de dados no MongoDB.** Se estiver usando o MongoDB localmente, inicie o serviço MongoDB. Se estiver usando o MongoDB Atlas, crie um cluster e obtenha a string de conexão.

2. **Adicione a string de conexão ao seu projeto.** Normalmente, você pode encontrar um arquivo de configuração (como `appsettings.json` ou `appsettings.Development.json`) onde você deve adicionar ou atualizar a string de conexão do MongoDB.
