using MediatR;
using Notificaciones.Infrastructure.DTOs;
using Notificaciones.Infrastructure.Interfaces;

namespace Notificaciones.Infrastructure.Queries.QueryHandler
{
    public class GetNotificacionesUsuarioQueryHandler : IRequestHandler<GetNotificacionesUsuarioQuery, List<NotificacionDto>>
    {
        private readonly INotificacionReadRepository NotificacionRepository;

        public GetNotificacionesUsuarioQueryHandler(INotificacionReadRepository notificacionRepository)
        {
            NotificacionRepository = notificacionRepository;
        }

        public async Task<List<NotificacionDto>> Handle(GetNotificacionesUsuarioQuery idUsuario, CancellationToken cancellationToken)
        {
            try
            {
                var pagos = await NotificacionRepository.GetNotificacionesPorIdUsuario(idUsuario.IdUsuario);

                var returnPagos = new List<NotificacionDto>();
                foreach (var pago in pagos)
                {
                    var returnPago = new NotificacionDto
                    {
                        Id = pago["_id"].AsString,
                        IdsUsuarios = pago["idsUsuarios"].AsBsonArray.Select(x => x.AsString).ToList(),
                        Motivo = pago["motivo"].AsString,
                        Cuerpo = pago["cuerpo"].AsString,
                        FechaCreacion = pago["fechaCreacion"].ToUniversalTime()
                    };
                    returnPagos.Add(returnPago);
                }

                return returnPagos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
