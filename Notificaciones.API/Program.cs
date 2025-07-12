

using DotNetEnv;
using Notificaciones.Infrastructure.Configurations;
using Notificaciones.Infrastructure.Interfaces;
using Notificaciones.Infrastructure.Persistences.Repositories.MongoRead;
using Notificaciones.Infrastructure.Queries.QueryHandler;
using Notificaciones.Infrastructure.Services;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<MongoReadNotificacionesDbConfig>();
builder.Services.AddSingleton<IRestClient>(new RestClient());

builder.Services.AddScoped<INotificacionReadRepository, NotificacionReadRepository>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<ISmtpEmailSender, SmtpEmailSenderService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetNotificacionesUsuarioQueryHandler).Assembly));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite cualquier dominio
            .AllowAnyMethod()  // GET, POST, PUT, DELETE, etc.
            .AllowAnyHeader(); // Cualquier cabecera
    });
});
var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
