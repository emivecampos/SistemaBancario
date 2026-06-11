namespace SistemaBancario.ConsoleApp.Models;

public sealed class Transacao
{
    public Guid Id { get; }
    public DateTime DataHora { get; }
    public TipoTransacao Tipo { get; }
    public decimal Valor { get; }
    public decimal SaldoAposTransacao { get; }
    public string Descricao { get; }


    public Transacao(
        TipoTransacao tipo,
        decimal valor,
        decimal saldoAposTransacao,
        string descricao)


    {
        Id = Guid.NewGuid();
        DataHora = DateTime.Now;
        Tipo = tipo;
        Valor = valor;
        SaldoAposTransacao = saldoAposTransacao;
        Descricao = descricao;
    }

}