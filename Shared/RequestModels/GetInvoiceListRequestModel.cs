﻿using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels
{
    public class GetInvoiceListRequestModel
    {
        public InvoiceStatus? InvoiceStatus { get; set; }
    }
}
