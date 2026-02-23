using Currency.API.Application.Addresses.Commands;
using Currency.API.Application.Addresses.Queries;
using Currency.API.Application.Currency.Conversion;
using Currency.API.Application.Currency.Queries;
using Currency.API.Application.Users.Commands;
using Currency.API.Application.Users.Queries;
using Currency.API.Data;
using Currency.API.Middleware;
using Currency.API.Models;
using Currency.API.Models.DTOs;
using Currency.API.Validators;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Ingresa tu llave de API",
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("ApiKey", document)] = new List<string>()
    });
});

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
app.UseMiddleware<ApiKeyMiddleware>();

// POST: Crear un usuario
app.MapPost("/users", async (UserDTO dto, IMediator mediator) =>
{
    var command = new CreateUserCommand(dto);
    var userId = await mediator.Send(command);
    return Results.Created($"/api/users/{userId}", userId);
})
.WithName("CreateUser");

// GET: Listar usuarios
app.MapGet("/users", async (bool? isActive, IMediator mediator) =>
{
    var query = new GetUsersQuery(isActive);
    var users = await mediator.Send(query);

    return Results.Ok(users);
})
.WithName("GetUsers");

// GET: Obtener usuario segun ID
app.MapGet("/users/{id:int}", async (int id, IMediator mediator) =>
{
    var user = await mediator.Send(new GetUserByIdQuery(id));

    // Si es nulo, retorna 404. Si existe, retorna 200 con el usuario.
    return user is not null
        ? Results.Ok(user)
        : Results.NotFound(new { message = $"Usuario con ID {id} no encontrado." });
})
.WithName("GetUserById");

// PUT: Actualizar usuario segun ID
app.MapPut("/user/{id:int}", async (int id, UpdateUserDTO dto, IMediator mediator) =>
{
    var command = new UpdateUserCommand(id, dto);
    var updated = await mediator.Send(command);

    return updated
        ? Results.NoContent()
        : Results.NotFound(new { message = $"Usuario con ID {id} no encontrado." });
})
.WithName("UpdateUser");

// DELETE: Eliminar usuario segun ID
app.MapDelete("/user/{id:int}", async (int id, IMediator mediator) =>
{
    var deleted = await mediator.Send(new DeleteUserCommand(id));

    return deleted
        ? Results.NoContent()
        : Results.NotFound(new { message = $"No se encontró el usuario con ID {id}." });
})
.WithName("DeleteUser");

// POST: Crear Address para usuario
app.MapPost("/users/{userId:int}/addresses", async (int userId, AddressDTO dto, IMediator mediator) =>
{
    var command = new CreateAddressCommand(userId, dto);
    var addressId = await mediator.Send(command);
    return Results.Created($"/api/users/{userId}/addresses/{addressId}", addressId);
})
.WithName("CreateAddressForUser");

// GET: Listar Addresses de un usuario
app.MapGet("/users/{userId:int}/addresses", async (int userId, IMediator mediator) =>
{
    var query = new GetAddressesQuery(userId);
    var addresses = await mediator.Send(query);

    return Results.Ok(addresses);
})
.WithName("GetUserAddresses");

// PUT: Actualizar Address segun ID
app.MapPut("/addresses/{id:int}", async (int id, AddressDTO dto, IMediator mediator) =>
{
    var command = new UpdateAddressCommand(id, dto);
    var updated = await mediator.Send(command);

    return updated
        ? Results.NoContent()
        : Results.NotFound(new { message = $"Address con ID {id} no encontrado." });
})
.WithName("UpdateAddress");

// DELETE: Eliminar Address segun ID
app.MapDelete("/addresses/{id:int}", async (int id, IMediator mediator) =>
{
    var deleted = await mediator.Send(new DeleteAddressCommand(id));

    return deleted
        ? Results.NoContent()
        : Results.NotFound(new { message = $"No se encontró el Address con ID {id}." });
})
.WithName("DeleteAddress");

// POST: Crear una currency / moneda
app.MapPost("/currencies", async (CurrencyDTO dto, IMediator mediator) =>
{
    var command = new CreateCurrencyCommand(dto);
    var currencyId = await mediator.Send(command);
    return Results.Created($"/api/currencies/{currencyId}", currencyId);
})
.WithName("CreateCurrency");

// GET: Listar currencies / monedas
app.MapGet("/currencies", async (IMediator mediator) =>
{
    var query = new GetCurrenciesQuery();
    var currencies = await mediator.Send(query);

    return Results.Ok(currencies);
})
.WithName("GetCurrencies");

// POST: Conversion de divisas
app.MapPost("/currency/convert", async (CurrencyConversionDTO dto, IMediator mediator) => {
    var convert = new CurrencyConversionCalculate(dto);
    var converted = await mediator.Send(convert);

    return Results.Ok(converted);
})
.WithName("CalculateConversion");

app.Run();
