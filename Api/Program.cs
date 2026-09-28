using Api.data;
using Api.model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Vue", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();
app.UseCors("Vue");
// Apliquen migrations al arrancar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// GET ALL
app.MapGet("/api/products", async (AppDbContext db) =>
{
    return await db.Products.ToListAsync();
});

// GET BY ID
app.MapGet("/api/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    return product is null
        ? Results.NotFound()
        : Results.Ok(product);
});

// CREATE
app.MapPost("/api/products", async (Product product, AppDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();

    return Results.Created($"/api/products/{product.Id}", product);
});

// UPDATE
app.MapPut("/api/products/{id}", async (int id, Product product, AppDbContext db) =>
{
    var existing = await db.Products.FindAsync(id);

    if (existing is null)
        return Results.NotFound();

    existing.Name = product.Name;
    existing.Price = product.Price;

    await db.SaveChangesAsync();

    return Results.Ok(existing);
});

// DELETE
app.MapDelete("/api/products/{id}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);

    if (product is null)
        return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


app.Run();
