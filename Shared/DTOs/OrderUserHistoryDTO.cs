﻿using System;

namespace Shared.DTOs
{
    public class OrderUserHistoryDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }   

        public DateTime CreatedDate { get; set; }

        public int Price { get; set; }   
    }
}
