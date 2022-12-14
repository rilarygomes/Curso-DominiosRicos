using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class PaypalPayment : Payment
    {
        public PaypalPayment(
            string transactionCode,
             DateTime paidDate,
             DateTime expireDate,
             decimal total,
             decimal totalPaid,
            Document document,
            string payer,
            Adress adress,
            Email email
            ) : base(paidDate, expireDate, total, totalPaid, document, payer, adress, email)
        {
            TransactionCode = transactionCode;
        }

        public string TransactionCode { get; private set; }
    }
}