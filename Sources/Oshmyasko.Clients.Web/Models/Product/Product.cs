namespace Oshmyasko.Clients.Web.Models.Product
{
    public class Product
    {
        public int? Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Composition { get; set; }
    }
}
