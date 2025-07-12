using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notificaciones.Infrastructure.DTOs
{
    public class EnviarNotificacionDto
    {
        public List<string> IdsUsuarios { get; set; }
        public string Motivo { get; set; }
        public string Cuerpo { get; set; }
    }
}
