using MediatR;
using Shared.Models;
using System;

namespace ReportService.Commands
{
    public class CreateReportCommand : IRequest<CommandResponse<int>>
    {
        public Guid ReporterId { get; set; }

        public int InvoiceId { get; set; }
    }
}
