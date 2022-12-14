using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Domain.Entities
{
    public class CreditCardPayment : Payment
    {
        public CreditCardPayment(
            string cardHolderName,
            string cardNumber,
            string lastTransactionNumber,
            DateTime paidDate,
            DateTime expireDate,
            decimal total, 
            decimal totalPaid, 
            Document document,
            string payer,
            Adress adress, 
            Email email
            ) : base(paidDate, expireDate, total,  totalPaid, document, payer, adress, email)
        {
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            LastTransactionNumber = lastTransactionNumber;
        }

        public string CardHolderName { get; private set; }
        public string CardNumber{  get; private set; }
        public string LastTransactionNumber { get; private set; }
    }
}