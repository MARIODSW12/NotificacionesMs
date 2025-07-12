using System.Net.Mail;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Notificaciones.API.Controllers;
using Notificaciones.Infrastructure.DTOs;
using Notificaciones.Infrastructure.Interfaces;
using RestSharp;

namespace Notificaciones.Tests.Notificaciones.API.Controllers;

public class NotificacionesController_CreateNotificaciones_Tests
{
    private readonly Mock<IMediator> MediatorMock;
    private readonly Mock<IRestClient> RestClientMock;
    private readonly Mock<INotificacionService> NotificationService;
    private readonly Mock<ISmtpEmailSender> SmtpService;
    private readonly NotificacionesController Controller;

    public NotificacionesController_CreateNotificaciones_Tests()
    {
        MediatorMock = new Mock<IMediator>();
        RestClientMock = new Mock<IRestClient>();
        NotificationService = new Mock<INotificacionService>();
        SmtpService = new Mock<ISmtpEmailSender>();
        Environment.SetEnvironmentVariable("EMAIL", "test@test.com");
        Controller = new NotificacionesController(MediatorMock.Object, RestClientMock.Object, NotificationService.Object, SmtpService.Object);
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnOk_WhenNotificacionesIsCreatedSuccessfully()
    {
        // Arrange
        var claimDto = new EnviarNotificacionDto
        {
            IdsUsuarios = ["9a2e844b-fc27-4b94-949b-9757a3557415"],
            Cuerpo = "cuerpo",
            Motivo = "motivo"
        };

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(new 
                {
                    email = "test1@test.com"
                })
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await Controller.EnviarNotificacion(claimDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.Equal("Notification sent successfully", okResult.Value.ToString());
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnBadRequest_WhenNoBody()
    {

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(new
                {
                    email = "test1@test.com"
                })
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await Controller.EnviarNotificacion(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var okResult = result as BadRequestObjectResult;
        Assert.Equal("Invalid notification request", okResult.Value.ToString());
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnBadRequest_WhenNoIdsUsuario()
    {
        // Arrange
        var claimDto = new EnviarNotificacionDto
        {
            IdsUsuarios = null,
            Cuerpo = "cuerpo",
            Motivo = "motivo"
        };

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(new
                {
                    email = "test1@test.com"
                })
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await Controller.EnviarNotificacion(claimDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var okResult = result as BadRequestObjectResult;
        Assert.Equal("Invalid notification request", okResult.Value.ToString());
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnBadRequest_WhenIdsUsuarioEmpty()
    {
        // Arrange
        var claimDto = new EnviarNotificacionDto
        {
            IdsUsuarios = [],
            Cuerpo = "cuerpo",
            Motivo = "motivo"
        };

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(new
                {
                    email = "test1@test.com"
                })
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await Controller.EnviarNotificacion(claimDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var okResult = result as BadRequestObjectResult;
        Assert.Equal("Invalid notification request", okResult.Value.ToString());
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnBadRequest_WhenUserFetchFails()
    {
        // Arrange
        var claimDto = new EnviarNotificacionDto
        {
            IdsUsuarios = ["9a2e844b-fc27-4b94-949b-9757a3557415"],
            Cuerpo = "cuerpo",
            Motivo = "motivo"
        };

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = false,
                ResponseStatus = ResponseStatus.Error,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Content = null
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await Controller.EnviarNotificacion(claimDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        var okResult = result as BadRequestObjectResult;
        Assert.Equal("Error al obtener la información del usuario.", okResult.Value.ToString());
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnBadRequest_WhenSaveNotiFails()
    {
        // Arrange
        var claimDto = new EnviarNotificacionDto
        {
            IdsUsuarios = ["9a2e844b-fc27-4b94-949b-9757a3557415"],
            Cuerpo = "cuerpo",
            Motivo = "motivo"
        };

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(new
                {
                    email = "test1@test.com"
                })
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("test"));
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await Controller.EnviarNotificacion(claimDto);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var okResult = result as ObjectResult;
        Assert.Equal(500, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateNotificaciones_ShouldReturnBadRequest_WhenSendNotiFails()
    {
        // Arrange
        var claimDto = new EnviarNotificacionDto
        {
            IdsUsuarios = ["9a2e844b-fc27-4b94-949b-9757a3557415"],
            Cuerpo = "cuerpo",
            Motivo = "motivo"
        };

        RestClientMock.Setup(client => client.ExecuteAsync(It.IsAny<RestRequest>(), CancellationToken.None))
            .ReturnsAsync(new RestResponse
            {
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(new
                {
                    email = "test1@test.com"
                })
            });
        NotificationService.Setup(n =>
                n.GuardarNotificacion(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);
        SmtpService.Setup(n =>
                n.SendMailAsync(It.IsAny<MailMessage>()))
            .ThrowsAsync(new Exception("test"));

        // Act
        var result = await Controller.EnviarNotificacion(claimDto);

        // Assert
        Assert.IsType<ObjectResult>(result);
        var okResult = result as ObjectResult;
        Assert.Equal(500, okResult.StatusCode);
    }
}
