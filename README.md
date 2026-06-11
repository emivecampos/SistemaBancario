Sistema Bancário em C#

Projeto desenvolvido para praticar os principais conceitos de orientação a objetos com C#/.NET.

Funcionalidades atuais

A primeira versão do sistema roda no console e possui as seguintes funcionalidades:

Criar conta corrente
Criar conta poupança
Depositar dinheiro
Sacar dinheiro
Transferir entre contas
Consultar saldo
Consultar saldo disponível
Consultar histórico de transações
Listar contas cadastradas

Estrutura do projeto
SistemaBancario.ConsoleApp
│
├── Models
│   ├── ContaBancaria.cs
│   ├── ContaCorrente.cs
│   ├── ContaPoupanca.cs
│   ├── Transacao.cs
│   └── TipoTransacao.cs
│
├── Interfaces
│   └── IContaRepository.cs
│
├── Repositories
│   └── ContaRepositoryMemoria.cs
│
├── Services
│   └── BancoService.cs
│
└── Program.cs

Explicação da estrutura
Models

A pasta Models contém as principais classes do sistema.

Ela representa as entidades do domínio bancário, como:

Conta bancária
Conta corrente
Conta poupança
Transação
Tipo de transação

Essas classes concentram as regras principais do negócio.

Interfaces

A pasta Interfaces contém os contratos do sistema.

O principal contrato atual é:

IContaRepository

Ele define o que um repositório de contas precisa fazer, independentemente de os dados estarem em memória, em SQLite, SQL Server ou outro banco.

Repositories

A pasta Repositories contém a implementação responsável por armazenar e recuperar contas.

Na versão atual, o projeto usa armazenamento em memória:

ContaRepositoryMemoria

Isso significa que os dados existem apenas enquanto o programa está rodando. Ao fechar o sistema, as contas são perdidas.

Services

A pasta Services contém a camada de serviço da aplicação.

O principal serviço é:

BancoService

Ele coordena as operações do sistema, como:

Criar conta
Depositar
Sacar
Transferir
Buscar conta
Listar contas

Essa camada evita que o Program.cs fique cheio de regras de negócio.



