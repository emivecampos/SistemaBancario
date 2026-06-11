namespace SistemaBancario.ConsoleApp.Models;

public sealed class ContaCorrente : ContaBancaria
{
    public decimal Limite { get; private set; }

    public override string TipoConta => "Conta corrente";

    public ContaCorrente(
        string numero,
        string titular,
        decimal limite)
        : base(numero, titular)
    {
        if (limite < 0)
        {
            throw new ArgumentException(
                "O limite não pode ser negativo.",
                nameof(limite));
        }

        Limite = limite;
    }

    protected override decimal CalcularSaldoDisponivel()
    {
        return Saldo + Limite;
    }

    public void AlterarLimite(decimal novoLimite)
    {
        if (novoLimite < 0)
        {
            throw new ArgumentException(
                "O limite não pode ser negativo.");
        }

        Limite = novoLimite;
    }
}