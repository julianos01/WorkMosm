namespace WorkMosm.FrontEnd.Models
{
    public record VehicleDto(
    Guid Id,
    string Brand,
    string Model,
    string Year,
    int Price,
    string ImageUrl
    );
}
