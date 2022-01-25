using CheckoutService.Commands;
using DatabaseAccessor.Repositories.Interfaces;
using MediatR;

namespace CheckoutService.Handlers
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand>, IDisposable
    {
        private readonly IProductRepository _productRepository;

        public CheckOutCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<Unit> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            
        }

        public void Dispose()
        {
            _productRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
