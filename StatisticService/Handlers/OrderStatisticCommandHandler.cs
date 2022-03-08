using DatabaseAccessor.Converters;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using StatisticService.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StatisticService.Handlers
{
    public class OrderStatisticCommandHandler : IRequestHandler<OrderStatisticCommand, StatisticResult<Invoice>>, IDisposable
    {
        public readonly IInvoiceRepository _repository;

        public OrderStatisticCommandHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public Task<StatisticResult<Invoice>> Handle(OrderStatisticCommand request, CancellationToken cancellationToken)
        {
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var groupingResult = _repository.DbContext.Invoices
                .Where(invoice => invoice.CreatedAt.Year == currentYear &&
                    (request.Strategy != StatisticStrategy.ByDay || invoice.CreatedAt.Month == currentMonth))
                .GroupBy(invoice => invoice.CreatedAt);
            var statisticResultItems = new List<StatisticResultItem>();
            var highestIncome = 0d;
            var lowestIncome = double.MaxValue;
            var highestDate = DateTime.Now;
            var lowestDate = DateTime.Now;
            foreach (var group in groupingResult)
            {
                var invoiceList = group.ToList();
                var newInvoiceList = group.Where(invoice => invoice.Status == InvoiceStatus.New).ToList();
                var confirmedInvoiceList = group.Where(invoice => invoice.Status == InvoiceStatus.Confirmed).ToList();
                var estimatedIncome = group.Where(invoice => invoice.Status <= InvoiceStatus.Confirmed)
                        .Sum(invoice => invoice.Details.Sum(detail => detail.Price));
                var actualIncome = confirmedInvoiceList.Sum(invoice => invoice.Details.Sum(detail => detail.Price));
                if (actualIncome > highestIncome)
                {
                    highestIncome = actualIncome;
                    highestDate = group.Key;
                }
                if (actualIncome < lowestIncome)
                {
                    lowestIncome = actualIncome;
                    lowestDate = group.Key;
                }
                statisticResultItems.Add(new StatisticResultItem
                {
                    Data = new StatisticResultItemData
                    {
                        Total = invoiceList.Count,
                        NewInvoiceCount = newInvoiceList.Count,
                        ConfirmedInvoiceCount = confirmedInvoiceList.Count,
                        CanceledInvoiceCount = invoiceList.Count(invoice => invoice.Status == InvoiceStatus.Canceled)
                    },
                    EstimatedIncome = estimatedIncome,
                    ActualIncome = actualIncome
                });
            }
            var statisticResult = new StatisticResult<Invoice>(request.Strategy)
            {
                Details = statisticResultItems.ToArray(),
                HighestIncome = highestIncome,
                LowestIncome = lowestIncome,
                HighestDate = new StatisticDateResult(request.Strategy, highestDate.ToDateOnly()),
                LowestDate = new StatisticDateResult(request.Strategy, lowestDate.ToDateOnly())
            };
            return Task.FromResult(statisticResult);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
