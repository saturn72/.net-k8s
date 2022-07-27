using Admin.Backend.Domain;

namespace Admin.Backend.Services
{
    public interface IValidator<TContext> where TContext : ContextBase
    {
        Task<bool> IsValidFor(TContext context);
    }
}
