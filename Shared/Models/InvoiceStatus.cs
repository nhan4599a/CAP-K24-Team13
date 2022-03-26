using System;

namespace Shared.Models
{
    public enum InvoiceStatus
    {
        New, Confirmed, ShipperReceived, Succeed, Canceled
    }

    public static class InvoiceStatusExtension
    {
        public static string GetDescription(this InvoiceStatus status)
        {
            return status switch
            {
                InvoiceStatus.New => "New",
                InvoiceStatus.Confirmed => "Shop owner confirmed",
                InvoiceStatus.ShipperReceived => "Delivered to shipper",
                InvoiceStatus.Succeed => "Customer received",
                InvoiceStatus.Canceled => "Canceled",
                _ => throw new NotImplementedException()
            };
        }
    }
}
