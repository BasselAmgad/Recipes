using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/recipes", () =>
{
    Data data = new();
    return data.GetRecipes();
});

app.Run();