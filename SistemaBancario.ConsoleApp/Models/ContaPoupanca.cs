namespace SistemaBancario.ConsoleApp.Models;

public sealed class ContaPoupanca : ContaBancaria
{
    public override string TipoConta => "Conta poupança";

    public ContaPoupanca(
        string numero,
        string titular)
        : base(numero, titular)
    {
    }

    public void AplicarRendimento(decimal percentual)
    {
        if (percentual <= 0)
        {
            throw new ArgumentException(
                "O percentual deve ser maior que zero.");
        }

        if (Saldo <= 0)
        {
            throw new InvalidOperationException(
                "A conta não possui saldo para receber rendimento.");
        }

        decimal rendimento = Saldo * percentual / 100;

        Saldo += rendimento;

        RegistrarTransacao(
            TipoTransacao.Rendimento,
            rendimento,
            $"Rendimento de {percentual:N2}% aplicado");
    }
}