namespace AnnosWebAPI.Data
{
    public class AdvertismentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
