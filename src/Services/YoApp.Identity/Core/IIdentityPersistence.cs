using YoApp.Data;
using YoApp.Data.Repositories;

namespace YoApp.Identity.Core
{
    public interface IIdentityPersistence : IUnitOfWork
    {
        IVerificationTokensRepository VerificationTokens { get; }
    }
}