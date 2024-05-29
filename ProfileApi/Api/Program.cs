using Api.Swagger;
using Core.Authentication;
using Core.Logging;
using Core.Mapping;
using Core.Middlewares;
using Logic;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Host.AddLoggingServices();

builder.Services.AddMappingServices();

builder.Services.AddGlobalExceptionHandlerServices();

builder.Services.AddLogicServices();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddAuthenticationHelper();

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

