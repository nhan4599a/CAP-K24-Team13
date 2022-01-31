﻿using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IOrderHistoryClient
    {
        [Get("/useraccount/orderhistory/{userId}")]
        Task<ApiResponse<ApiResult<List<OrderUserHistoryDTO>>>> GetOrderUserHistory(string userId);
    }
}
