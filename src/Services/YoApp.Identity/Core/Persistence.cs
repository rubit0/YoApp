using YoApp.Data;
using YoApp.Data.Repositories;

namespace YoApp.Identity.Core
{
    public class Persistence : UnitOfWorkBase, IIdentityPersistence
    {
        public IVerificationTokensRepository VerificationTokens { get; }

        public Persistence(ApplicationDbContext context, IVerificationTokensRepository verificationTokensRepository) 
            : base(context)
        {
            VerificationTokens = verificationTokensRepository;
        }
    }
}
