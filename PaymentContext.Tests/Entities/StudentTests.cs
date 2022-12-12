using PaymentContext.Domain.Entities;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Domain.Enums;

namespace PaymentContext.Tests;

[TestClass] 
public class StudentTests 
{
    private readonly Name _name;
    private readonly Adress _adress;
    private readonly Email _email;
    private readonly Document _document;    
    private readonly Student _student;
    private readonly Subscription _subscription;

    public StudentTests()
    {
        _name = new Name("Bruce", "Wayne");
        _document = new Document("86136314525", EDocumentType.CPF);
        _adress = new Adress("Rua 1", "1234", "Bairro maneiro", "Gotham", "SP", "BR", "13400000");
        _email = new Email("batman@dc.com");
        _student = new Student(_name, _document, _email);
        _subscription = new Subscription(null);
                   
    }

    [TestMethod]
    public void ShouldReturnErrorWhenHadActiveSubscription()
    {
        var payment = new PaypalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, _adress, _email);
        _subscription.AddPayment(payment);  
        _student.AddSubscription(_subscription);
        _student.AddSubscription(_subscription);
            
        Assert.IsFalse(_student.IsValid);
    }

    [TestMethod]
    public void ShouldReturnErrorWhenHadSubscriptionHasNoPayment()
    { 
        _student.AddSubscription(_subscription);            
        Assert.IsFalse(_student.IsValid);
    }

    [TestMethod]
    public void ShouldReturnSucessWhenAddSubscription()
    {
        var payment = new PaypalPayment("12345678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, _adress, _email);
        _subscription.AddPayment(payment);  
        _student.AddSubscription(_subscription);
            
        Assert.IsTrue(_student.IsValid);
          
    }
}