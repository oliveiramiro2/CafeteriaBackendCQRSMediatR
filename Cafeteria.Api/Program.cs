using Cafeteria.Application.Pedidos;
using Cafeteria.Infrastructure.Data;
using Cafeteria.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar a conexão com o SQLite (salvando na infraestrutura)
builder.Services.AddDbContext<CafeteriaContext>(options =>
    options.UseSqlite("Data Source=cafeteria_clean.db"));

// 2. Registrar o Repositório no container de Injeção de Dependência
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// 3. Registrar o MediatR apontando para o assembly da camada Application (onde estão os Handlers)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CriarPedidoCommand).Assembly));

// 4. Adicionar suporte ao Swagger e Controladores/Endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 5. Criar a rota HTTP POST /pedidos limpa e desacoplada usando o MediatR
app.MapPost("/pedidos", async (CriarPedidoCommand command, MediatR.IMediator mediator) =>
{
    try
    {
        // A API apenas despacha o comando para o MediatR e não sabe como ele é processado!
        int pedidoId = await mediator.Send(command);

        return Results.Created($"/pedidos/{pedidoId}", new { id = pedidoId, mensagem = "Pedido criado com sucesso!" });
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { erro = ex.Message });
    }
    catch (System.InvalidOperationException ex)
    {
        return Results.UnprocessableEntity(new { erro = ex.Message });
    }
})
.WithName("CriarPedido")
.WithOpenApi();

app.Run();