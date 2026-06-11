using SistemaBancario.ConsoleApp.Interfaces;
using SistemaBancario.ConsoleApp.Models;

namespace SistemaBancario.ConsoleApp.Services;

public sealed class BancoService
{
    private readonly IContaRepository _contaRepository;
    private int _proximoNumeroConta;

    public BancoService(IContaRepository contaRepository)
    {
        _contaRepository = contaRepository
            ?? throw new ArgumentNullException(nameof(contaRepository));

        _proximoNumeroConta = 1;
    }

    public ContaCorrente CriarContaCorrente(
        string titular,
        decimal limite)
    {
        string numero = GerarNumeroConta();

        var conta = new ContaCorrente(
            numero,
            titular,
            limite);

        _contaRepository.Adicionar(conta);

        return conta;
    }

    public ContaPoupanca CriarContaPoupanca(string titular)
    {
        string numero = GerarNumeroConta();

        var conta = new ContaPoupanca(
            numero,
            titular);

        _contaRepository.Adicionar(conta);

        return conta;
    }

    public void Depositar(string numeroConta, decimal valor)
    {
        ContaBancaria conta = ObterConta(numeroConta);

        conta.Depositar(valor);
    }

    public void Sacar(string numeroConta, decimal valor)
    {
        ContaBancaria conta = ObterConta(numeroConta);

        conta.Sacar(valor);
    }

    public void Transferir(
        string numeroContaOrigem,
        string numeroContaDestino,
        decimal valor)
    {
        ContaBancaria origem = ObterConta(numeroContaOrigem);
        ContaBancaria destino = ObterConta(numeroContaDestino);

        origem.TransferirPara(destino, valor);
    }

    public ContaBancaria ObterConta(string numeroConta)
    {
        ContaBancaria? conta =
            _contaRepository.ObterPorNumero(numeroConta);

        if (conta is null)
        {
            throw new KeyNotFoundException(
                $"A conta {numeroConta} não foi encontrada.");
        }

        return conta;
    }

    public IReadOnlyCollection<ContaBancaria> ListarContas()
    {
        return _contaRepository.Listar();
    }

    private string GerarNumeroConta()
    {
        string numero;

        do
        {
            numero = _proximoNumeroConta.ToString("D4");
            _proximoNumeroConta++;
        }
        while (_contaRepository.ObterPorNumero(numero) is not null);

        return numero;
    }
}