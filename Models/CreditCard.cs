namespace DocIntelligence.Models;

internal class CreditCard
{
    public string CardNumber { get; set; }

    public string IssuingBank { get; set; }

    public string PaymentNetwork { get; set; }

    public string CardHolderName { get; set; }

    public string CardHolderCompanyName { get; set; }

    public DateOnly ValidDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public string CardVerificationValue { get; set; }
}