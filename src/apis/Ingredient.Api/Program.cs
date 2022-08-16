using Models = Ingredient.Api.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using PizzaShop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddSingleton(new CosmosRepository(builder.Configuration["CosmosDb:CnxString"],
                                                   builder.Configuration["CosmosDB:DatabaseName"],
                                                   builder.Configuration["CosmosDB:ContainerName"]));
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//var scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";

app.MapGet("/", async (ICosmosRepository repository) => 
{
    return await repository.QueryAsync<Models.Ingredient>("select * from i");
})
.WithName("GetAllIngredients");

app.MapPost("/", async (ICosmosRepository repository, Models.Ingredient ingredient) => 
{
    var newItem = await repository.CreateReplaceItemAsync(ingredient, ingredient.IngredientType.Value);
    return newItem;
})
.WithName("CreateIngredient");

//app.MapGet("/weatherforecast", (HttpContext httpContext) =>
//{
//    //httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

//    var forecast =  Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        (
//            DateTime.Now.AddDays(index),
//            Random.Shared.Next(-20, 55),
//            summaries[Random.Shared.Next(summaries.Length)]
//        ))
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.RequireAuthorization();

app.Run();
