namespace SistemaBancario.ConsoleApp.Models;

public abstract class ContaBancaria
{
    private readonly List<Transacao> _historico;

    public Guid Id { get; }
    public string Numero { get; }
    public string Titular { get; private set; }
    public decimal Saldo { get; protected set; }

    public IReadOnlyCollection<Transacao> Historico =>
        _historico.AsReadOnly();

    public abstract string TipoConta { get; }

    protected ContaBancaria(string numero, string titular)
    {
        if (string.IsNullOrWhiteSpace(numero))
        {
            throw new ArgumentException(
                "O número da conta é obrigatório.",
                nameof(numero));
        }

        if (string.IsNullOrWhiteSpace(titular))
        {
            throw new ArgumentException(
                "O nome do titular é obrigatório.",
                nameof(titular));
        }

        Id = Guid.NewGuid();
        Numero = numero;
        Titular = titular.Trim();
        Saldo = 0;
        _historico = new List<Transacao>();
    }

    public void Depositar(decimal valor)
    {
        ValidarValor(valor);

        Saldo += valor;

        RegistrarTransacao(
            TipoTransacao.Deposito,
            valor,
            "Depósito realizado");
    }

    public virtual void Sacar(decimal valor)
    {
        ValidarValor(valor);

        if (valor > CalcularSaldoDisponivel())
        {
            throw new InvalidOperationException(
                "Saldo insuficiente para realizar o saque.");
        }

        Saldo -= valor;

        RegistrarTransacao(
            TipoTransacao.Saque,
            valor,
            "Saque realizado");
    }

    public void TransferirPara(
        ContaBancaria contaDestino,
        decimal valor)
    {
        if (contaDestino is null)
        {
            throw new ArgumentNullException(nameof(contaDestino));
        }

        if (contaDestino.Id == Id)
        {
            throw new InvalidOperationException(
                "Não é possível transferir para a mesma conta.");
        }

        ValidarValor(valor);

        if (valor > CalcularSaldoDisponivel())
        {
            throw new InvalidOperationException(
                "Saldo insuficiente para realizar a transferência.");
        }

        Saldo -= valor;
        contaDestino.Saldo += valor;

        RegistrarTransacao(
            TipoTransacao.TransferenciaEnviada,
            valor,
            $"Transferência enviada para a conta {contaDestino.Numero}");

        contaDestino.RegistrarTransacao(
            TipoTransacao.TransferenciaRecebida,
            valor,
            $"Transferência recebida da conta {Numero}");
    }

    public decimal ConsultarSaldoDisponivel()
    {
        return CalcularSaldoDisponivel();
    }

    public void AlterarTitular(string novoTitular)
    {
        if (string.IsNullOrWhiteSpace(novoTitular))
        {
            throw new ArgumentException(
                "O nome do titular não pode ficar vazio.");
        }

        Titular = novoTitular.Trim();
    }

    protected virtual decimal CalcularSaldoDisponivel()
    {
        return Saldo;
    }

    protected void RegistrarTransacao(
        TipoTransacao tipo,
        decimal valor,
        string descricao)
    {
        var transacao = new Transacao(
            tipo,
            valor,
            Saldo,
            descricao);

        _historico.Add(transacao);
    }

    private static void ValidarValor(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException(
                "O valor da operação deve ser maior que zero.");
        }
    }
}