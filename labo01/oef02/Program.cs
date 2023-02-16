var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<BrandValidators>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.MapSwagger();
app.UseSwaggerUI();


var brands = new List<Brand>();
brands.Add(new Brand { BrandId = 1, Name = "Audi", Country = "Germany", Logo = "https://www.audi.be/content/dam/gbp2/brand/logos/audi-logo.png" });
brands.Add(new Brand { BrandId = 2, Name = "BMW", Country = "Germany", Logo = "https://www.bmw.be/content/dam/bmw/common/all-models/3-series/sedan/2019/navigation/bmw-3-series-sedan.png" });
brands.Add(new Brand { BrandId = 3, Name = "Mercedes", Country = "Germany", Logo = "https://www.mercedes-benz.be/content/dam/mercedes-benz/mercedes-benz-corporate-site/brand-assets/brand-logo/MB-Logo-Black.png" });
brands.Add(new Brand { BrandId = 4, Name = "Ferrari", Country = "Italy", Logo = "https://www.ferrari.com/content/dam/ferrari/brand-assets/brand-logo/ferrari-logo.png" });

var models = new List<CarModel>();
models.Add(new CarModel { CarModelId = 1, Name = "A3", Brand = brands[0] });
models.Add(new CarModel { CarModelId = 2, Name = "A4", Brand = brands[0] });
models.Add(new CarModel { CarModelId = 3, Name = "A5", Brand = brands[0] });
models.Add(new CarModel { CarModelId = 4, Name = "m5", Brand = brands[1] });
models.Add(new CarModel { CarModelId = 5, Name = "m2", Brand = brands[1] });
models.Add(new CarModel { CarModelId = 6, Name = "cla", Brand = brands[2] });

app.MapGet("/brands", () =>
{
    return Results.Ok(brands);
});

app.MapGet("/brands/{country}", (string country) =>
{
    var result = brands.Where(b => b.Country == country);
    return Results.Ok(result);
});

app.MapPost("/brands", (IValidator<Brand> validator, Brand brand) =>
{
    var result = validator.Validate(brand);
    if (!result.IsValid)
    {
        var errors = result.Errors.Select(e => new { errors = e.ErrorMessage });
        return Results.BadRequest(result.Errors);
    }
    brand.BrandId = brands.Max(b => b.BrandId) + 1;
    brands.Add(brand);
    return Results.Created($"/brands/{brand.BrandId}", brand);
});

app.MapGet("/brand/{brand}", (string brand) =>
{
    var result = brands.Where(b => b.Name == brand);
    return Results.Ok(result);
});

app.MapGet("/models", () =>
{
    return Results.Ok(models);
});

app.MapGet("/models/{brand}", (string brand) =>
{
    var result = models.Where(m => m.Brand.Name == brand);
    return Results.Ok(result);
});

app.MapGet("/model/{model}", (string model) =>
{
    var result = models.Where(m => m.Name == model);
    return Results.Ok(result);
});

app.MapGet("/carmodels/{country}", (string country) =>
{
    var result = models.Where(m => m.Brand.Country == country);
    return Results.Ok(result);
});

app.Run("http://localhost:5000");
