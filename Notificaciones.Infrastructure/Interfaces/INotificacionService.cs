using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notificaciones.Infrastructure.Interfaces
{
    public interface INotificacionService
    {
        Task GuardarNotificacion (List<string> para, string motivo, string cuerpo);
    }
}
