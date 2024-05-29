using System.Reflection;
using API.Swagger;
using Application;
using Core.Authentication;
using Core.HttpLogic;
using Core.Logger;
using Core.Mapping;
using Core.Middlewares;
using Persistence;
using ProfileConnection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
	{
		var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		
		c.IncludeXmlComments(xmlPath);
	}
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddGlobalExceptionHandlerServices();
builder.Services.AddOperationCanceledMiddlewareServices();

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Host.AddLoggerServices();
builder.Services.AddMappingServices();

builder.Services.TryAddTraceId();

builder.Services.AddAuthenticationHelper();

builder.Services.AddHttpRequestService();

builder.Services.AddProfileConnectionServices(builder.Configuration);

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();

app.Run();