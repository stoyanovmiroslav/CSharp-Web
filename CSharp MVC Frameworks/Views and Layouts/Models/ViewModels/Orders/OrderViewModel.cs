namespace Models.ViewModels.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string Product { get; set; }

        public string Client { get; set; }

        public string OrderedOn { get; set; }
    }
}