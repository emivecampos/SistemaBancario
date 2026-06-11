using SistemaBancario.ConsoleApp.Models;

namespace SistemaBancario.ConsoleApp.Interfaces;

public interface IContaRepository
{
    void Adicionar(ContaBancaria conta);

    ContaBancaria? ObterPorNumero(string numero);

    IReadOnlyCollection<ContaBancaria> Listar();
}