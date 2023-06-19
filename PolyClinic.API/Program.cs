using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PolyClinic.API.Startup;
using PolyClinic.Common.Logger;

var builder = WebApplication.CreateBuilder(args);

// Configuring Custom File Logger
var options = builder.Configuration.GetSection("Logging").GetSection("PolyClinicLogFile").GetSection("Options");
builder.Logging.AddFileLogger(options.Bind);

// Add services to the container.
builder.Services.RegisterServices();

// Adding Authentication with Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var config =
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerService();

app.UseHttpsRedirection();
// Configuring Exception handler
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
