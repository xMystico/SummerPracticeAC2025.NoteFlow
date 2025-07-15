using FluentValidation;
using NoteFlow.Application;
using NoteFlow.Domain.Common;
using NoteFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

var configuration = builder.Configuration.Get<AppSettings>()!;

builder.Services.AddInfrastructuresService(configuration);
builder.Services.AddApplicationService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }