using System.Net.Mail;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notificaciones.Infrastructure.DTOs;
using Notificaciones.Infrastructure.Interfaces;
using Notificaciones.Infrastructure.Queries;
using RestSharp;

namespace Notificaciones.API.Controllers
{
    [ApiController]
    [Route("api/notificaciones")]
    public class NotificacionesController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IRestClient RestClient;
        private readonly INotificacionService NotificacionService;
        private readonly ISmtpEmailSender SmtpEmailSender;

        public NotificacionesController(IMediator mediator, IRestClient restClient, INotificacionService notificacionService, ISmtpEmailSender smtpEmailSender)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
            NotificacionService = notificacionService ?? throw new ArgumentNullException(nameof(notificacionService));
            SmtpEmailSender = smtpEmailSender ?? throw new ArgumentNullException(nameof(smtpEmailSender));
        }

        [HttpGet("getUserNotification/{idUsuario}")]
        public async Task<IActionResult> GetNotificacionesUsuario(string idUsuario)
        {
            try
            {
                var query = new GetNotificacionesUsuarioQuery(idUsuario);
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("enviar")]
        public async Task<IActionResult> EnviarNotificacion([FromBody] EnviarNotificacionDto body)
        {
            if (body == null || body.IdsUsuarios == null || !body.IdsUsuarios.Any())
            {
                return BadRequest("Invalid notification request");
            }

            try
            {
                MailMessage notificacion = new MailMessage
                {
                    From = new MailAddress(Environment.GetEnvironmentVariable("EMAIL")),
                    Subject = body.Motivo,
                    Body = body.Cuerpo,
                    IsBodyHtml = true
                };
                Console.WriteLine($"Sending notification to: {string.Join(", ", body.IdsUsuarios)}");
                foreach (var idUsuario in body.IdsUsuarios)
                {
                    Console.WriteLine($"Processing user ID: {idUsuario}");
                    var APIRequest = new RestRequest(Environment.GetEnvironmentVariable("USER_MS_URL") + "/getuserbyid/" + idUsuario, Method.Get);
                    var APIResponse = await RestClient.ExecuteAsync(APIRequest);
                    if (!APIResponse.IsSuccessful)
                    {
                        return BadRequest("Error al obtener la información del usuario.");
                    }
                    var user = JsonDocument.Parse(APIResponse.Content);
                    var email = user.RootElement.GetProperty("email").GetString();
                    notificacion.To.Add(email);
                }
                await NotificacionService.GuardarNotificacion(body.IdsUsuarios, body.Motivo, body.Cuerpo);
                await SmtpEmailSender.SendMailAsync(notificacion);
                return Ok("Notification sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}