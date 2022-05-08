using EndpointQueryService.Data.Endpoints;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace EndpointQueryService.Configurars
{
    public class FirebaseConfigurar
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            var projectId = configuration["firebase:projectId"] ?? throw new ArgumentNullException("projectId");
            var json = configuration["firebase:json"] ?? throw new ArgumentNullException("json");

            //https://cloud.google.com/dotnet/docs/reference/Google.Cloud.Firestore/latest
            var fireStoreClient = new FirestoreClientBuilder
            {
                CredentialsPath = json
            }.Build();
            var firestoreDb = FirestoreDb.Create(projectId, fireStoreClient);
            services.AddSingleton(firestoreDb);

            services.AddSingleton<IEndpointRepository, FirestoreEndpointRepository>();
        }
    }
}
