using TestTask;
using TestTask.Services.Providers.Register;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddProviders();
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/ping", async (ISearchService pingProvider) =>
{
    return await pingProvider.IsAvailableAsync(new CancellationToken());
});

app.MapPost("/search", async (SearchRequest request, ISearchService service) =>
{
    return await service.SearchAsync(request, new CancellationToken());
});

app.Run();
