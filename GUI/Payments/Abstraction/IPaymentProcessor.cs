using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Payments.Abstraction
{
    public interface IPaymentProcessor
    {
        Task<PaymentResponse> ExecuteAsync(PaymentRequest paymentRequest);
    }
}
