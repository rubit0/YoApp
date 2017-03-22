using System.Threading.Tasks;
using YoApp.Data.Repositories;

namespace YoApp.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IVerificationTokensRepository VerificationTokensRepository { get; }

        void Complete();
        Task CompleteAsync();
    }
}
