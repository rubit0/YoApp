namespace YoApp.Backend.Data.Repositories
{
    public interface IUserRepository
    {
        bool IsPhoneNumberTaken(string phoneNumber);
    }
}
