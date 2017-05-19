namespace Oshmyasko.Clients.Web.Models.Product
{
    public class Product : Entity
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Composition { get; set; }

        public double? Quantity { get; set; }

        public virtual Category.Category Category { get; set; }
    }
}
