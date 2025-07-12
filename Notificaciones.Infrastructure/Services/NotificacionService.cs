using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Notificaciones.Infrastructure.Interfaces;

namespace Notificaciones.Infrastructure.Services
{
    public class NotificacionService: INotificacionService
    {
        private readonly INotificacionReadRepository _notificacionReadRepository;
        public NotificacionService( INotificacionReadRepository notificacionReadRepository)
        {
            _notificacionReadRepository = notificacionReadRepository;
        }

        public async Task GuardarNotificacion(List<string> para, string motivo, string cuerpo)
        {
            try
            {
                var notificacion = new BsonDocument
                {
                    { "_id", Guid.NewGuid().ToString() },
                    { "idsUsuarios", new BsonArray(para)},
                    { "motivo", motivo },
                    { "cuerpo", cuerpo },
                    { "fechaCreacion", DateTime.UtcNow }
                };

                await _notificacionReadRepository.AgregarNotificacion(notificacion);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
