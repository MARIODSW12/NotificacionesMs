using MediatR;
using Notificaciones.Infrastructure.DTOs;

namespace Notificaciones.Infrastructure.Queries
{
    public class GetNotificacionesUsuarioQuery : IRequest<List<NotificacionDto>>
    {
        public string IdUsuario { get; set; }
        public GetNotificacionesUsuarioQuery(string idUsuario)
        {
            IdUsuario = idUsuario;
        }
    }
}
