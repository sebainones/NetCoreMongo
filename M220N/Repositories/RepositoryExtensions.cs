using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace M220N.Repositories
{
    public static class RepositoryExtensions
    {
        public static void RegisterMongoDbRepositories(this IServiceCollection servicesBuilder)
        {
            //https://docs.mongodb.com/manual/reference/connection-string/

            servicesBuilder.AddSingleton<IMongoClient, MongoClient>(s =>
            {
                int maxConnections = int.Parse(s.GetRequiredService<IConfiguration>()["MongoMaxConnections"]);
                var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];

                var mongoClientSettings = MongoClientSettings.FromConnectionString(uri);
                mongoClientSettings.MaxConnectionPoolSize = maxConnections;

                return new MongoClient(mongoClientSettings);
                //return new MongoClient(uri);
                //_client = new MongoClient(Constants.MongoDbConnectionUriWithMaxPoolSize).WithWriteConcern(
                //new WriteConcern(wTimeout: TimeSpan.FromMilliseconds(2500))) as MongoClient;

            });
            servicesBuilder.AddSingleton<MoviesRepository>();
            servicesBuilder.AddSingleton<UsersRepository>();
            servicesBuilder.AddSingleton<CommentsRepository>();
            servicesBuilder.AddSingleton(s => s.GetRequiredService<IConfiguration>()["JWT_SECRET"]);
        }
    }
}
