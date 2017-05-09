﻿using System;
using System.ComponentModel.DataAnnotations;
using Oshmyasko.Clients.Web.Models.Product;

namespace Oshmyasko.Clients.Web.Models.Order
{
    public class OrderViewModel
    {
        public int? Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Значение должно быть больше ноля.")]
        public double Count { get; set; }

        public bool? Confirmed { get; set; }
        
        public DateTime Created { get; set; }

        public ApplicationUser ClientInfo { get; set; }

        public ProductViewModel Product { get; set; }
    }
}
