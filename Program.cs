using Microsoft.EntityFrameworkCore;
using StargateAPI.Models;
using StargateAPI.Interfaces;
using StargateAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AstroActsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AstroActsDatabase")));
builder.Services.AddLogging();
builder.Logging.AddLog4Net();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IAstronautDutyService, AstronautDutyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


