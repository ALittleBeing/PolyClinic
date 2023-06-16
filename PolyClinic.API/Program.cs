using Microsoft.Extensions.Configuration;
using PolyClinic.API.Startup;
using PolyClinic.Common.Logger;

var builder = WebApplication.CreateBuilder(args);

// Configuring Custom File Logger
var options = builder.Configuration.GetSection("Logging").GetSection("PolyClinicLogFile").GetSection("Options");
builder.Logging.AddFileLogger(options.Bind);


// Add services to the container.
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureSwagger();

app.UseHttpsRedirection();
// Configuring Exception handler
app.UseExceptionHandler("/error");

app.UseAuthorization();

app.MapControllers();

app.Run();
