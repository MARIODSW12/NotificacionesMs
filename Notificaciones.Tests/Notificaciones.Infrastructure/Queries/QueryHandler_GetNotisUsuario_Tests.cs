using MongoDB.Bson;
using Moq;
using Notificaciones.Infrastructure.Interfaces;
using Notificaciones.Infrastructure.Queries;
using Notificaciones.Infrastructure.Queries.QueryHandler;


namespace Notificaciones.Tests.Notificaciones.Infrastructure.Queries
{
    public class GetNotisUsuarioQueryHandlerTests
    {
        private readonly Mock<INotificacionReadRepository> NotificacionReadRepositoryMock;
        private readonly GetNotificacionesUsuarioQueryHandler Handler;

        public GetNotisUsuarioQueryHandlerTests()
        {
            NotificacionReadRepositoryMock = new Mock<INotificacionReadRepository>();
            Handler = new GetNotificacionesUsuarioQueryHandler(NotificacionReadRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ReturnsNotificacion_WhenNotificacionExist()
        {
            var query = new GetNotificacionesUsuarioQuery("9a2e844b-fc27-4b94-949b-9757a3557411");
            
            NotificacionReadRepositoryMock.Setup(r => r.GetNotificacionesPorIdUsuario(query.IdUsuario))
                .ReturnsAsync([
                    new BsonDocument
                    {
                        { "_id", "9a2e844b-fc27-4b94-949b-9757a3557511" },
                        {
                            "idsUsuarios", new BsonArray((IEnumerable<string>) ["9a2e844b-fc27-4b94-949b-9757a3557411"])
                        },
                        { "motivo", "motivo" },
                        { "cuerpo", "cuerpo" },
                        { "fechaCreacion", new BsonDateTime(new DateTime()) },
                    }
                ]);

            var result = await Handler.Handle(query, CancellationToken.None);

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task Handle_ReturnsNull_WhenNotificacionDoesNotExist()
        {
            var query = new GetNotificacionesUsuarioQuery("");

            NotificacionReadRepositoryMock.Setup(r => r.GetNotificacionesPorIdUsuario(query.IdUsuario))
                .ReturnsAsync([]);

            var result = await Handler.Handle(query, CancellationToken.None);

            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenRepositoryThrows()
        {
            var query = new GetNotificacionesUsuarioQuery("9a2e844b-fc27-4b94-949b-9757a3557411");

            NotificacionReadRepositoryMock.Setup(r => r.GetNotificacionesPorIdUsuario(query.IdUsuario))
                .ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<Exception>(() => Handler.Handle(query, CancellationToken.None));
        }

    }
}
