using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TokenWebApiContext>(optionsBuilder =>
{
 string  connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.ConfigureCors(); //Extensiones
builder.Services.AddControllers();
builder.Services.AddJwt(builder.Configuration);
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

//Importancia del orden...
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
