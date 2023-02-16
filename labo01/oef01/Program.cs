var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssemblyContaining<WineValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.MapSwagger();
app.UseSwaggerUI();

var wines = new List<Wine>();
wines.Add(new Wine { WineId = 1, Name = "Chateau de Sours", Country = "France", Color = "Red", Year = 2010, Type = "Bordeaux", Grapes = "Merlot", Price = 15.5M });
wines.Add(new Wine { WineId = 2, Name = "Chateau du Clos", Country = "France", Color = "Red", Year = 2010, Type = "Bordeaux", Grapes = "Merlot", Price = 15.5M });
wines.Add(new Wine { WineId = 3, Name = "Chatea La Nerthe", Country = "France", Color = "Red", Year = 2010, Type = "Bordeaux", Grapes = "Merlot", Price = 15.5M });

app.MapPut("/wine/{wineId}", (int wineId, Wine wine) =>
{
    var existingWine = wines.FirstOrDefault(w => w.WineId == wineId);
    if (existingWine == null)
    {
        return Results.NotFound();
    }
    existingWine.Name = wine.Name;
    existingWine.Country = wine.Country;
    existingWine.Color = wine.Color;
    existingWine.Year = wine.Year;
    existingWine.Type = wine.Type;
    existingWine.Grapes = wine.Grapes;
    existingWine.Price = wine.Price;
    return Results.Ok();
});

app.MapDelete("/wine/{wineId}", (int wineId) =>
{
    var wine = wines.FirstOrDefault(w => w.WineId == wineId);
    if (wine == null)
    {
        return Results.NotFound();
    }
    wines.Remove(wine);
    return Results.Ok();
});


app.MapPost("/wines", (IValidator<Wine> validator, Wine wine) =>
{
    var result = validator.Validate(wine);
    if (!result.IsValid)
    {
        var errors = result.Errors.Select(e => new { errors = e.ErrorMessage });
        return Results.BadRequest(result.Errors);
    }
    wine.WineId = wines.Max(w => w.WineId) + 1;
    wines.Add(wine);
    return Results.Created($"/wine/{wine.WineId}", wine);
});


app.MapGet("/wines", () =>
{
    return Results.Ok(wines);
});
app.MapGet("/Wine/{wineId}", (int wineId) =>
{
    var wine = wines.FirstOrDefault(w => w.WineId == wineId);
    if (wine == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(wine);
});


app.Run("http://localhost:5000");
