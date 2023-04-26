using Microsoft.AspNetCore.Mvc;
using TestTask;
using TestTask.Contract.Cache;
using TestTask.Middleware;
using TestTask.Services.Cache;
using TestTask.Services.Providers.Register;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();


builder.Services.AddProviders();
builder.Services.AddScoped(typeof(ICacheProvider<>), typeof(RedisCacheRepository<>));
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<LoggingHandler>();

app.MapGet("/ping", async (ISearchService pingProvider) =>
{
    if (await pingProvider.IsAvailableAsync(new CancellationToken()))
    {
        return Results.Ok();
    }

    return Results.StatusCode(500);
});

app.MapPost("/search", async ([FromBody] SearchRequest request, ISearchService service) =>
{
    var result = await service.SearchAsync(request, new CancellationToken());
    return Results.Json<SearchResponse>(result);
}).Accepts<SearchRequest>("application/json").Produces<SearchResponse>();

app.Run();
