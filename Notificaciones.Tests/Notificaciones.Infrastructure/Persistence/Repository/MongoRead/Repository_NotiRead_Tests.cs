using System.Security.Claims;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Notificaciones.Infrastructure.Configurations;
using Notificaciones.Infrastructure.Persistences.Repositories.MongoRead;

namespace Notificaciones.Tests.Notificaciones.Infrastructure.Persistence.Repository.MongoRead
{
    public class MongoReadNotiRepositoryTests
    {
        private readonly Mock<IMongoDatabase> mongoDatabaseMock;
        private readonly Mock<IMongoCollection<BsonDocument>> claimCollectionMock;
        private readonly NotificacionReadRepository repository;

        public MongoReadNotiRepositoryTests()
        {
            mongoDatabaseMock = new Mock<IMongoDatabase>();
            claimCollectionMock = new Mock<IMongoCollection<BsonDocument>>();

            mongoDatabaseMock.Setup(d => d.GetCollection<BsonDocument>("notificaciones_read", It.IsAny<MongoCollectionSettings>()))
                              .Returns(claimCollectionMock.Object);

            Environment.SetEnvironmentVariable("MONGODB_CNN_READ", "mongodb://localhost:27017");
            Environment.SetEnvironmentVariable("MONGODB_NAME_READ", "test_database");
            var mongoConfigMock = new MongoReadNotificacionesDbConfig();
            mongoConfigMock.db = mongoDatabaseMock.Object;

            repository = new NotificacionReadRepository(mongoConfigMock);
        }

        [Fact]
        public async Task AgregarNoti_throwsError()
        {

            claimCollectionMock.Setup(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null,CancellationToken.None)).ThrowsAsync(new Exception("Database Exception"));

            Assert.ThrowsAsync<Exception>(() => repository.AgregarNotificacion(new BsonDocument { { "_id", "1" } }));

            claimCollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task AgregarNoti_addsNoti()
        {

            claimCollectionMock.Setup(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, CancellationToken.None)).Returns(Task.CompletedTask);

            await repository.AgregarNotificacion(new BsonDocument { { "_id", "1" } });

            claimCollectionMock.Verify(c => c.InsertOneAsync(It.IsAny<BsonDocument>(), null, CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GetNotisIdUsuario_getsNotis()
        {
            var cursorMock = new Mock<IAsyncCursor<BsonDocument>>();
            cursorMock.SetupSequence(c => c.MoveNextAsync(default))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            cursorMock.Setup(c => c.Current).Returns([]);
            claimCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default)).ReturnsAsync(cursorMock.Object);

            var result = await repository.GetNotificacionesPorIdUsuario("9a2e844b-fc27-4b94-949b-9757a3557411");

            claimCollectionMock.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default), Times.Once);
            Assert.Equal([], result);
        }

        [Fact]
        public async Task GetNotisIdUsuario_throwsError()
        {
           
            claimCollectionMock.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default)).ThrowsAsync(new Exception("Database Exception"));

            Assert.ThrowsAsync<Exception>(()=>repository.GetNotificacionesPorIdUsuario("9a2e844b-fc27-4b94-949b-9757a3557411"));

            claimCollectionMock.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<BsonDocument>>(),
                It.IsAny<FindOptions<BsonDocument, BsonDocument>>(), default), Times.Once);
        }

    }
}
