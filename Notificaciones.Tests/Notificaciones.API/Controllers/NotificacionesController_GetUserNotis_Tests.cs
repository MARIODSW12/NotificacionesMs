using System.Net.Mail;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Notificaciones.API.Controllers;
using Notificaciones.Infrastructure.DTOs;
using Notificaciones.Infrastructure.Interfaces;
using Notificaciones.Infrastructure.Queries;
using RestSharp;

namespace Notificaciones.Tests.Notificaciones.API.Controllers;

public class NotificacionesController_GetUserNotis_Tests
{
    private readonly Mock<IMediator> MediatorMock;
    private readonly Mock<IRestClient> RestClientMock;
    private readonly Mock<INotificacionService> NotificationService;
    private readonly Mock<ISmtpEmailSender> SmtpService;
    private readonly NotificacionesController Controller;

    public NotificacionesController_GetUserNotis_Tests()
    {
        MediatorMock = new Mock<IMediator>();
        RestClientMock = new Mock<IRestClient>();
        NotificationService = new Mock<INotificacionService>();
        SmtpService = new Mock<ISmtpEmailSender>();
        Environment.SetEnvironmentVariable("EMAIL", "test@test.com");
        Controller = new NotificacionesController(MediatorMock.Object, RestClientMock.Object, NotificationService.Object, SmtpService.Object);
    }

    [Fact]
    public async Task GetUserNotis_ShouldReturnOk_WhenNotificacionesExists()
    {
        // Arrange
        MediatorMock.Setup(n =>
                n.Send(It.IsAny<GetNotificacionesUsuarioQuery>(), CancellationToken.None))
            .ReturnsAsync([]);

        // Act
        var result = await Controller.GetNotificacionesUsuario("9a2e844b-fc27-4b94-949b-9757a3557415");

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
    }

    [Fact]
    public async Task GetUserNotis_ShouldReturnBadRequest_WhenGetNotificacionesFails()
    {
        // Arrange
        MediatorMock.Setup(n =>
                n.Send(It.IsAny<GetNotificacionesUsuarioQuery>(), CancellationToken.None))
            .ThrowsAsync(new Exception());

        // Act
        var result = await Controller.GetNotificacionesUsuario("9a2e844b-fc27-4b94-949b-9757a3557415");

        // Assert
        Assert.IsType<ObjectResult>(result);
        var okResult = result as ObjectResult;
        Assert.Equal(500, okResult.StatusCode);
    }

}
