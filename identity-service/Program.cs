using identity_service;
using SkyApm.Utilities.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => {
        options.Authority = builder.Configuration["AuthServer"];
        options.Audience = "identity-service";
        options.RequireHttpsMetadata = false;

        //options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters {
        //    ValidateAudience = false
        //};
    });


builder.Services.AddSkyApmExtensions();

builder.Services.AddDbContext<MyDbContext>(dbContextOptionsBuilder =>
{
    dbContextOptionsBuilder.UseMySQL(builder.Configuration["Db"], options => { 
    });
    dbContextOptionsBuilder.EnableSensitiveDataLogging();
});

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

app.MapControllers();

app.Run();
