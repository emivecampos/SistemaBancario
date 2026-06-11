using SistemaBancario.ConsoleApp.Interfaces;
using SistemaBancario.ConsoleApp.Models;

namespace SistemaBancario.ConsoleApp.Repositories;

public sealed class ContaRepositoryMemoria : IContaRepository
{
    private readonly Dictionary<string, ContaBancaria> _contas;

    public ContaRepositoryMemoria()
    {
        _contas = new Dictionary<string, ContaBancaria>(
            StringComparer.OrdinalIgnoreCase);
    }

    public void Adicionar(ContaBancaria conta)
    {
        if (conta is null)
        {
            throw new ArgumentNullException(nameof(conta));
        }

        if (_contas.ContainsKey(conta.Numero))
        {
            throw new InvalidOperationException(
                "Já existe uma conta com esse número.");
        }

        _contas.Add(conta.Numero, conta);
    }

    public ContaBancaria? ObterPorNumero(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
        {
            return null;
        }

        _contas.TryGetValue(numero.Trim(), out ContaBancaria? conta);

        return conta;
    }

    public IReadOnlyCollection<ContaBancaria> Listar()
    {
        return _contas.Values
            .OrderBy(conta => conta.Numero)
            .ToList();
    }
}