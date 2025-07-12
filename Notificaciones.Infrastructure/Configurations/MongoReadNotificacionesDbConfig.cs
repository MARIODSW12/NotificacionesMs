using MongoDB.Driver;

namespace Notificaciones.Infrastructure.Configurations
{
    public class MongoReadNotificacionesDbConfig
    {
        public MongoClient client;
        public IMongoDatabase db;

        public MongoReadNotificacionesDbConfig()
        {
            try
            {
                string connectionUri = Environment.GetEnvironmentVariable("MONGODB_CNN_READ");

                if (string.IsNullOrWhiteSpace(connectionUri))
                {
                    throw new Exception("La conexion no puede ser nula");
                }

                var settings = MongoClientSettings.FromConnectionString(connectionUri);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);

                client = new MongoClient(settings);

                string databaseName = Environment.GetEnvironmentVariable("MONGODB_NAME_READ");
                if (string.IsNullOrWhiteSpace(databaseName))
                {
                    throw new Exception("El nombre de la base de datos no puede ser nulo");
                }

                db = client.GetDatabase(databaseName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
