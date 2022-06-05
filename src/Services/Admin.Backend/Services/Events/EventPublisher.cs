using Admin.Backend.Domain;

namespace Admin.Backend.Services.Events
{
    public class EventPublisher : IEventPublisher
    {
        public Task PublishCreated<TModel>(CreateContext<TModel> context)
        {
            return Task.CompletedTask;
        }
    }
}
