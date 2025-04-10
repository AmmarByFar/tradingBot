using BinanceInterceptor.BackgroundTasks;
using BinanceInterceptor.Middlewear;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHostedService<RefreshBinanceValuesService>();
builder.Services.AddHostedService<BinanceWebSocketService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();
app.UseMiddleware<BinanceWebSocketMiddlewear>();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
