using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.EntityFrameworkCore;
using ProductosSOAP.Data;
using ProductosSOAP.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventarioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddScoped<ProductoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();
app.UseCors("Angular");

app.MapGet("/", () => Results.Redirect("/ProductoService.svc?wsdl"));

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ProductoService>();
    serviceBuilder.AddServiceEndpoint<ProductoService, IProductoService>(
        new BasicHttpBinding(),
        "/ProductoService.svc");
});

var metadata = app.Services.GetRequiredService<ServiceMetadataBehavior>();
metadata.HttpGetEnabled = true;

app.Run();
