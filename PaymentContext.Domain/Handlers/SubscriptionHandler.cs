using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Command;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler : Notifiable<Notification>,
    IHandler<CreateBoletoSubscriptionCommand>,
    IHandler<CreatePayPalSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //Fail fast validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua assinatura.");
            }

            //Verificar se documento está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotifications(command);
                return new CommandResult(false, "Este CPF já está em uso.");
            }

            //Verificar se email já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotifications(command);
                return new CommandResult(false, "Este email já está em uso.");
            }

            //Gerar os OVs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var adress = new Adress(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            var email = new Email(command.Email);

            //Gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(command.BarCode, command.BoletoNumber, command.PaidDate, command.ExpireDate, command.Total,
            command.TotalPaid, new Document(command.PayerDocument, command.PayerDocumentType), adress, email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar validações
            AddNotifications(name, document, email, adress, student, subscription, payment);

            //Salvar as informações
            _repository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Adress, "Bem vindo ao balta.io", "Sua assinatura foi criada.");

            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso.");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
             //Verificar se documento está cadastrado
            if (_repository.DocumentExists(command.Document))
            {
                AddNotifications(command);
                return new CommandResult(false, "Este CPF já está em uso.");
            }

            //Verificar se email já está cadastrado
            if (_repository.EmailExists(command.Email))
            {
                AddNotifications(command);
                return new CommandResult(false, "Este email já está em uso.");
            }

            //Gerar os OVs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var adress = new Adress(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            var email = new Email(command.Email);

            //Gerar entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(command.TransactionCode, command.PaidDate, command.ExpireDate, command.Total,
            command.TotalPaid, new Document(command.PayerDocument, command.PayerDocumentType), command.Payer, adress, email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar validações
            AddNotifications(name, document, email, adress, student, subscription, payment);

            //Salvar as informações
            _repository.CreateSubscription(student);

            //Enviar email de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Adress, "Bem vindo ao balta.io", "Sua assinatura foi criada.");

            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso.");

        }
    }
}