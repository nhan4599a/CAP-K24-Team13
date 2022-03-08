﻿using DatabaseAccessor.Converters;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var invoices = _repository.DbContext.Invoices
                .AsNoTracking()
                .Include(e => e.Details)
                .Where(invoice => invoice.CreatedAt.Year == currentYear);
            if (request.Strategy == StatisticStrategy.ByDay)
                invoices = invoices.Where(invoice => invoice.CreatedAt.Month == currentMonth);
            var groupingResult = invoices.ToList().GroupBy(invoice => invoice.CreatedAt);
            var highestIncome = 0d;
            var lowestIncome = double.MaxValue;
            DateTime? highestDate = null;
            DateTime? lowestDate = null;
            var statisticResultItems = new SortedDictionary<StatisticDateResult, StatisticResultItem>();
            foreach (var group in groupingResult)
            {
                var invoiceList = group.ToList();
                var newInvoiceList = group.Where(invoice => invoice.Status == InvoiceStatus.New).ToList();
                var confirmedInvoiceList = group.Where(invoice => invoice.Status == InvoiceStatus.Confirmed).ToList();
                var estimatedIncome = group.Sum(invoice => invoice.Details.Sum(detail => detail.Price));
                var actualIncome = group.Where(invoice => invoice.Status == InvoiceStatus.Succeed)
                        .Sum(invoice => invoice.Details.Sum(detail => detail.Price));
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
                statisticResultItems.Add(new StatisticDateResult(request.Strategy, group.Key.ToDateOnly()), new StatisticResultItem
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
            if (request.Strategy == StatisticStrategy.ByDay)
            {
                for (int i = 1; i <= DateTime.Now.Day; i++)
                {
                    try
                    {
                        var dateOnlyObj = DateOnly.FromDateTime(new DateTime(currentYear, currentMonth, i));
                        statisticResultItems.Add(new StatisticDateResult(request.Strategy, dateOnlyObj), new StatisticResultItem());
                    }
                    catch (ArgumentException) { }
                }
            }
            else
            {
                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    try
                    {
                        var dateOnlyObj = DateOnly.FromDateTime(new DateTime(currentYear, i, 1));
                        statisticResultItems.Add(new StatisticDateResult(request.Strategy, dateOnlyObj), new StatisticResultItem());
                    }
                    catch (ArgumentException) { }
                }
            }
            var statisticResult = new StatisticResult<Invoice>(request.Strategy)
            {
                Details = statisticResultItems.ToDictionary(e => e.Key.ToString(), e => e.Value),
                HighestIncome = highestIncome,
                LowestIncome = lowestIncome
            };
            if (highestDate.HasValue)
                statisticResult.HighestDate = new StatisticDateResult(request.Strategy, highestDate.Value.ToDateOnly());
            if (lowestDate.HasValue)
                statisticResult.LowestDate = new StatisticDateResult(request.Strategy, lowestDate.Value.ToDateOnly());
            return Task.FromResult(statisticResult);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
