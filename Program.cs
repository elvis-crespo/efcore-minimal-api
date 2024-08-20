using EFC_MinimalApis.Context;
using EFC_MinimalApis.Models;
using EFC_MinimalApis.Services;
using EFC_MinimalApis.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("api"));
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mi Ejemplo minimal APIS", Description = "Configurar OpenApi" });
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/books", async (ApplicationContext context) => Results.Ok(await context.BookEntity.ToListAsync()));

app.MapGet("/api/book/{id}", async (int id, ApplicationContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book != null)
    { 
        return Results.Ok(book);
    }
    return Results.NotFound();
});

app.MapPost("/api/book", async (BookRequest request, IBookService bookService) =>
{
    var createBook = await bookService.CrearLibro(request);

    return Results.Created($"/books/{createBook.id}", createBook);
});

app.MapDelete("/api/book/{id}", async(int id, ApplicationContext context) => 
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    context.BookEntity.Remove(book);
    await context.SaveChangesAsync();   

    return Results.NoContent();
});

app.MapPut("/api/book", async (int id, BookRequest request, ApplicationContext context) =>
{
    var book = await context.BookEntity.FindAsync(id);
    if (book is null)
    {
        return Results.NotFound();
    }
    if (request.Name != null)
    {
        book.Name = request.Name;
    }
    if (request.Isbn != null)
    {
        book.ISBN = request.Isbn;
    }

    await context.SaveChangesAsync();
    return Results.Ok(book);
});

app.Run();
