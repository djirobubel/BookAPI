using BookAPI.Data;
using BookAPI.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/books", async (DataContext context) =>
    await context.Books.ToListAsync());
   


app.MapGet("/books/{id}", async (DataContext context, int id) =>
    await context.Books.FindAsync(id) is Book book ? 
        Results.Ok(book) : 
        Results.NotFound("Sorry, this book does not exist."));

app.MapPost("/books", async (DataContext context, Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();
    return Results.Ok("Book has been succesfully added.");    
});

app.MapPut("/books/{id}", async (DataContext context, Book updatedBook, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book == null)
        return Results.NotFound("Sorry, this book does not exist.");

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    await context.SaveChangesAsync();

    return Results.Ok("Information about the book has been updated successfully");
});

app.MapDelete("/books/{id}", async (DataContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);
    if (book == null)
        return Results.NotFound("Sorry, this book does not exist");

    context.Books.Remove(book);
    await context.SaveChangesAsync();
    return Results.Ok("Infromation about the book has been successfuly deleted.");
});

app.Run();
