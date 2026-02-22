using Currency.API.Application.Users.Commands;
using Currency.API.Application.Users.Queries;
using Currency.API.Data;
using Currency.API.Middleware;
using Currency.API.Models.DTOs;
using Currency.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Configura el uso de SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"))
);

// Configura el uso de MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configura el uso de FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency.API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

// POST: Crear un usuario
app.MapPost("/POST/users", async (UserDTO dto, IMediator mediator) =>
{
    var command = new CreateUserCommand(dto);
    var userId = await mediator.Send(command);
    return Results.Created($"/api/users/{userId}", userId);
})
.WithName("CreateUser");

// GET: Listar usuarios
app.MapGet("/GET/users", async (bool? isActive, IMediator mediator) =>
{
    var query = new GetUsersQuery(isActive);
    var users = await mediator.Send(query);

    return Results.Ok(users);
})
.WithName("GetUsers");

// GET: Obtener usuario segun ID
app.MapGet("/GET/users/{id:int}", async (int id, IMediator mediator) =>
{
    var user = await mediator.Send(new GetUserByIdQuery(id));

    // Si es nulo, retorna 404. Si existe, retorna 200 con el usuario.
    return user is not null
        ? Results.Ok(user)
        : Results.NotFound(new { message = $"Usuario con ID {id} no encontrado." });
})
.WithName("GetUserById");

app.Run();
