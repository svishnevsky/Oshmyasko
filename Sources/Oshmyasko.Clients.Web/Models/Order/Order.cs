using System;

namespace Oshmyasko.Clients.Web.Models.Order
{
    public class Order : Entity
    {
        public int ProductId { get; set; }
        
        public double Count { get; set; }

        public string Client { get; set; }

        public bool? Confirmed { get; set; }

        public DateTime Created { get; set; }

        public virtual Product.Product Product { get; set; }

        public virtual ApplicationUser ClientInfo { get; set; }
    }
}