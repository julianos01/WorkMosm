namespace WorkMosm.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? ImageName { get; set; }
    }
}
