using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FunctionAppConsultaContatoId;
using FunctionAppConsultaContatoId.Interface;
using FunctionAppConsultaContatoId.Repository;
using Microsoft.AspNetCore.Connections;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Registrar as interfaces e implementa��es
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<IContatoRepository, ContatoRepository>();

builder.Build().Run();
