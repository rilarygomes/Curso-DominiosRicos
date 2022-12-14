using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Payment : Entity
    {
        public Payment(
            DateTime paidDate,
            DateTime expireDate,
            decimal total,
            decimal totalPaid,
            Document document,
            Adress adress,
            Email email
            )
        {
            Number = Guid.NewGuid().ToString().Replace("-", "").Substring(0,10).ToUpper();
            PaidDate = paidDate;
            ExpireDate = expireDate;
            Total = total;
            TotalPaid = totalPaid;
            Document = document;
            Adress = adress;
            Email = email;

            AddNotifications(new Contract<Payment>()
                .Requires()
                .IsLowerThan(0, Total, "Payment.Total", "O total não pode ser menor que zero.")
                .IsGreaterOrEqualsThan(Total, TotalPaid, "Payment.TotalPaid", "O valor pago é menor que o valor do pagamento.")
            );
        }

        public string Number { get; private set; }
        public DateTime PaidDate{ get; private set; }
        public DateTime ExpireDate { get; private set; }
        public decimal Total { get; private set; }
        public decimal TotalPaid { get; private set; }
        public string Payer { get; private set; }
        public Document Document { get; private set; }
        public Adress Adress { get; private set; }
        public Email Email { get; private set; }
    }
}