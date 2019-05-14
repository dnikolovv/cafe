namespace Cafe.Api.Hateoas.Resources.Menu
{
    public class MenuItemResource : Resource
    {
        public int Number { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
