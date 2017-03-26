using YoApp.Data;
using YoApp.Data.Repositories;

namespace YoApp.Identity
{
    public class DataWorker : UnitOfWorkBase
    {
        public IVerificationTokensRepository VerificationTokens { get; }

        public DataWorker(ApplicationDbContext context, IVerificationTokensRepository verificationTokensRepository) 
            : base(context)
        {
            VerificationTokens = verificationTokensRepository;
        }
    }
}
