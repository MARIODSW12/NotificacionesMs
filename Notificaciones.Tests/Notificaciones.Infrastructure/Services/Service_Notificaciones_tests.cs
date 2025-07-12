using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Moq;
using Notificaciones.Infrastructure.Interfaces;
using Notificaciones.Infrastructure.Services;

namespace Notificaciones.Tests.Notificaciones.Infraestructure.Services
{
    public class Service_Notificaciones_tests
    {
        private readonly Mock<INotificacionReadRepository> NotiRepositoryMock;
        private readonly NotificacionService handler;

        public Service_Notificaciones_tests()
        {
            NotiRepositoryMock = new Mock<INotificacionReadRepository>();
            handler = new NotificacionService(NotiRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateNoti()
        {

            NotiRepositoryMock.Setup(repo => repo.AgregarNotificacion(It.IsAny<BsonDocument>()))
                .Returns(Task.CompletedTask);
               
            // Act
            await handler.GuardarNotificacion(["para"], "test", "test");

            // Assert
            NotiRepositoryMock.Verify(repo => repo.AgregarNotificacion(It.IsAny<BsonDocument>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowIfFail()
        {

            NotiRepositoryMock.Setup(repo => repo.AgregarNotificacion(It.IsAny<BsonDocument>()))
                .ThrowsAsync(new Exception("Database Exception"));

            // Act
            Assert.ThrowsAsync<Exception>(() =>handler.GuardarNotificacion(["para"], "test", "test"));

            // Assert
            NotiRepositoryMock.Verify(repo => repo.AgregarNotificacion(It.IsAny<BsonDocument>()), Times.Once);
        }
    }
}
