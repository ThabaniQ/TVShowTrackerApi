using TVShowTracker.Domain;
using TVShowTracker.Contracts.Request;

namespace TVShowTracker.Repository.Interface
{
    public interface IIdentityService : IRepositoryBase<UserRegistrationDTO>
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationDTO userRegistation);
        Task<AuthenticationResult> LoginAsync(UserLoginDTO userLogin);
    }
}
