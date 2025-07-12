using MongoDB.Bson;
using MongoDB.Driver;
using Notificaciones.Infrastructure.Configurations;
using Notificaciones.Infrastructure.Interfaces;

namespace Notificaciones.Infrastructure.Persistences.Repositories.MongoRead
{
    public class NotificacionReadRepository: INotificacionReadRepository
    {
        private readonly IMongoCollection<BsonDocument> NotificacionColexion;

        public NotificacionReadRepository(MongoReadNotificacionesDbConfig mongoConfig)
        {
            NotificacionColexion = mongoConfig.db.GetCollection<BsonDocument>("notificaciones_read");
        }

        async public Task AgregarNotificacion(BsonDocument notificacion)
        {
            try
            {
                await NotificacionColexion.InsertOneAsync(notificacion);
            }
            catch (Exception ex) {
                throw;
            }
        }

        async public Task<List<BsonDocument>> GetNotificacionesPorIdUsuario(string idUsuario)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("idsUsuarios", idUsuario);
                var pago = await NotificacionColexion.Find(filter).ToListAsync();
                return pago;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
