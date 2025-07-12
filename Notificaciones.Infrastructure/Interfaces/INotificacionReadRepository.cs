using MongoDB.Bson;

namespace Notificaciones.Infrastructure.Interfaces
{
    public interface INotificacionReadRepository
    {
        Task<List<BsonDocument>> GetNotificacionesPorIdUsuario(string idUsuario);
        Task AgregarNotificacion(BsonDocument notificacion);
    }
}
