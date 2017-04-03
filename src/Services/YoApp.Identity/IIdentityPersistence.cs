using YoApp.Data;
using YoApp.Data.Repositories;

namespace YoApp.Identity
{
    public interface IIdentityPersistence : IUnitOfWork
    {
        IVerificationTokensRepository VerificationTokens { get; }
    }
}