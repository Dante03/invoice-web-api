namespace invoice_web_api.Interfaces
{
    public interface IUnitOfWork
    {
        ISendMailService sendMailService { get; }
        IUserRepository UserRepository { get; }

        Task<int> CompleteAsync();
    }
}
