namespace invoice_web_api.Interfaces
{
    public interface IUnitOfWork
    {
        ISendMailService sendMailService { get; }
        IUserRepository UserRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IClientRepository ClientRepository { get; }
        IInvoiceRepository InvoicetRepository { get; }
        ISupabaseStorageService SupabaseStorageService { get; }
        IInvoicePdfService InvoicePdfService { get; }

        Task<int> CompleteAsync();
    }
}
