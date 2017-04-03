using YoApp.Data.Repositories;

namespace YoApp.Identity
{
    public interface IIdentityPersistence
    {
        IVerificationTokensRepository VerificationTokens { get; }
    }
}