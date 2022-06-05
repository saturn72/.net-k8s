using Admin.Backend.Domain;

namespace Admin.Backend.Services.Events
{
    public interface IEventPublisher
    {
        Task PublishCreated<TModel>(CreateContext<TModel> context);
    }
}
