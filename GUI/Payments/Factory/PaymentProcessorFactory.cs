using GUI.Payments.Abstraction;
using GUI.Payments.Momo.Processor;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models;
using System;

namespace GUI.Payments.Factory
{
    public class PaymentProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentProcessor Create(PaymentMethod paymentMethod)
        {
            return paymentMethod switch
            {
                PaymentMethod.MoMo => _serviceProvider.GetRequiredService<MomoWalletProcessor>(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
