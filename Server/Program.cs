using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/recipes", () => 
{
    Data data = new Data();
    data.EditTitle(data.GetRecipes()[0].Id, "Scrambled Eggs");
    data.SaveRecipes();
    return JsonSerializer.Serialize(data.GetRecipes());

});

app.Run();