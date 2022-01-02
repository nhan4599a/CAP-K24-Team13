using MediatR;
using Shared;
using System;

namespace ShopProductService.Commands.Category
{
    public class ActivateCategoryCommand : IRequest<CommandResponse<bool>>
    {
        private bool _shouldBeCascade;

        public int Id { get; set; }

        public bool IsActivateCommand { get; set; }

        public bool ShouldBeCascade
        {
            get => IsActivateCommand && _shouldBeCascade;
            set
            {
                if (!IsActivateCommand && !value)
                    throw new NotSupportedException("Cascade option for an deactivate request is always required");
                _shouldBeCascade = value;
            }
        }
    }
}
