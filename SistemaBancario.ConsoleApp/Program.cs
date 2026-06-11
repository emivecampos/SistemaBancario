using System.Globalization;
using SistemaBancario.ConsoleApp.Interfaces;
using SistemaBancario.ConsoleApp.Models;
using SistemaBancario.ConsoleApp.Repositories;
using SistemaBancario.ConsoleApp.Services;

namespace SistemaBancario.ConsoleApp;

internal static class Program
{
    private static readonly IContaRepository ContaRepository =
        new ContaRepositoryMemoria();

    private static readonly BancoService BancoService =
        new BancoService(ContaRepository);

    private static void Main()
    {
        CultureInfo.DefaultThreadCurrentCulture =
            new CultureInfo("pt-BR");

        while (true)
        {
            Console.Clear();
            ExibirMenu();

            Console.Write("Escolha uma opção: ");
            string? opcao = Console.ReadLine();

            Console.Clear();

            if (opcao == "0")
            {
                Console.WriteLine("Sistema encerrado.");
                break;
            }

            try
            {
                ExecutarOpcao(opcao);
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro: {exception.Message}");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }

    private static void ExibirMenu()
    {
        Console.WriteLine("====================================");
        Console.WriteLine("       SISTEMA BANCÁRIO");
        Console.WriteLine("====================================");
        Console.WriteLine("1 - Criar conta");
        Console.WriteLine("2 - Depositar");
        Console.WriteLine("3 - Sacar");
        Console.WriteLine("4 - Transferir");
        Console.WriteLine("5 - Consultar saldo");
        Console.WriteLine("6 - Consultar histórico");
        Console.WriteLine("7 - Listar contas");
        Console.WriteLine("0 - Sair");
        Console.WriteLine("====================================");
    }

    private static void ExecutarOpcao(string? opcao)
    {
        switch (opcao)
        {
            case "1":
                CriarConta();
                break;

            case "2":
                Depositar();
                break;

            case "3":
                Sacar();
                break;

            case "4":
                Transferir();
                break;

            case "5":
                ConsultarSaldo();
                break;

            case "6":
                ConsultarHistorico();
                break;

            case "7":
                ListarContas();
                break;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    private static void CriarConta()
    {
        Console.WriteLine("CRIAÇÃO DE CONTA");
        Console.WriteLine();

        string titular = LerTextoObrigatorio(
            "Nome do titular: ");

        Console.WriteLine();
        Console.WriteLine("1 - Conta corrente");
        Console.WriteLine("2 - Conta poupança");
        Console.Write("Tipo de conta: ");

        string? tipoConta = Console.ReadLine();

        ContaBancaria conta;

        if (tipoConta == "1")
        {
            decimal limite = LerDecimalNaoNegativo(
                "Limite da conta: ");

            conta = BancoService.CriarContaCorrente(
                titular,
                limite);
        }
        else if (tipoConta == "2")
        {
            conta = BancoService.CriarContaPoupanca(
                titular);
        }
        else
        {
            throw new ArgumentException(
                "Tipo de conta inválido.");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine("Conta criada com sucesso!");
        Console.WriteLine($"Número: {conta.Numero}");
        Console.WriteLine($"Titular: {conta.Titular}");
        Console.WriteLine($"Tipo: {conta.TipoConta}");
        Console.ResetColor();
    }

    private static void Depositar()
    {
        Console.WriteLine("DEPÓSITO");
        Console.WriteLine();

        string numero = LerTextoObrigatorio(
            "Número da conta: ");

        decimal valor = LerDecimalPositivo(
            "Valor do depósito: ");

        BancoService.Depositar(numero, valor);

        ContaBancaria conta = BancoService.ObterConta(numero);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine("Depósito realizado com sucesso.");
        Console.WriteLine($"Novo saldo: {conta.Saldo:C}");
        Console.ResetColor();
    }

    private static void Sacar()
    {
        Console.WriteLine("SAQUE");
        Console.WriteLine();

        string numero = LerTextoObrigatorio(
            "Número da conta: ");

        decimal valor = LerDecimalPositivo(
            "Valor do saque: ");

        BancoService.Sacar(numero, valor);

        ContaBancaria conta = BancoService.ObterConta(numero);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine("Saque realizado com sucesso.");
        Console.WriteLine($"Novo saldo: {conta.Saldo:C}");
        Console.ResetColor();
    }

    private static void Transferir()
    {
        Console.WriteLine("TRANSFERÊNCIA");
        Console.WriteLine();

        string origem = LerTextoObrigatorio(
            "Conta de origem: ");

        string destino = LerTextoObrigatorio(
            "Conta de destino: ");

        decimal valor = LerDecimalPositivo(
            "Valor da transferência: ");

        BancoService.Transferir(
            origem,
            destino,
            valor);

        ContaBancaria contaOrigem =
            BancoService.ObterConta(origem);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine("Transferência realizada com sucesso.");
        Console.WriteLine(
            $"Saldo da conta de origem: {contaOrigem.Saldo:C}");
        Console.ResetColor();
    }

    private static void ConsultarSaldo()
    {
        Console.WriteLine("CONSULTA DE SALDO");
        Console.WriteLine();

        string numero = LerTextoObrigatorio(
            "Número da conta: ");

        ContaBancaria conta =
            BancoService.ObterConta(numero);

        Console.WriteLine();
        Console.WriteLine($"Conta: {conta.Numero}");
        Console.WriteLine($"Titular: {conta.Titular}");
        Console.WriteLine($"Tipo: {conta.TipoConta}");
        Console.WriteLine($"Saldo atual: {conta.Saldo:C}");
        Console.WriteLine(
            $"Saldo disponível: {conta.ConsultarSaldoDisponivel():C}");

        if (conta is ContaCorrente contaCorrente)
        {
            Console.WriteLine(
                $"Limite: {contaCorrente.Limite:C}");
        }
    }

    private static void ConsultarHistorico()
    {
        Console.WriteLine("HISTÓRICO DE TRANSAÇÕES");
        Console.WriteLine();

        string numero = LerTextoObrigatorio(
            "Número da conta: ");

        ContaBancaria conta =
            BancoService.ObterConta(numero);

        Console.WriteLine();
        Console.WriteLine($"Conta: {conta.Numero}");
        Console.WriteLine($"Titular: {conta.Titular}");
        Console.WriteLine();

        if (conta.Historico.Count == 0)
        {
            Console.WriteLine(
                "A conta ainda não possui transações.");

            return;
        }

        foreach (Transacao transacao in conta.Historico
                     .OrderByDescending(item => item.DataHora))
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine(
                $"Data: {transacao.DataHora:dd/MM/yyyy HH:mm:ss}");

            Console.WriteLine($"Tipo: {transacao.Tipo}");
            Console.WriteLine($"Valor: {transacao.Valor:C}");

            Console.WriteLine(
                $"Saldo após operação: " +
                $"{transacao.SaldoAposTransacao:C}");

            Console.WriteLine(
                $"Descrição: {transacao.Descricao}");
        }
    }

    private static void ListarContas()
    {
        Console.WriteLine("CONTAS CADASTRADAS");
        Console.WriteLine();

        IReadOnlyCollection<ContaBancaria> contas =
            BancoService.ListarContas();

        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta foi cadastrada.");
            return;
        }

        foreach (ContaBancaria conta in contas)
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"Número: {conta.Numero}");
            Console.WriteLine($"Titular: {conta.Titular}");
            Console.WriteLine($"Tipo: {conta.TipoConta}");
            Console.WriteLine($"Saldo: {conta.Saldo:C}");
        }
    }

    private static string LerTextoObrigatorio(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? texto = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                return texto.Trim();
            }

            Console.WriteLine(
                "O valor informado não pode ficar vazio.");
        }
    }

    private static decimal LerDecimalPositivo(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            bool valorValido = decimal.TryParse(
                entrada,
                out decimal valor);

            if (valorValido && valor > 0)
            {
                return valor;
            }

            Console.WriteLine(
                "Informe um valor numérico maior que zero.");
        }
    }

    private static decimal LerDecimalNaoNegativo(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string? entrada = Console.ReadLine();

            bool valorValido = decimal.TryParse(
                entrada,
                out decimal valor);

            if (valorValido && valor >= 0)
            {
                return valor;
            }

            Console.WriteLine(
                "Informe um valor numérico igual ou maior que zero.");
        }
    }
}