using ContactWebModels;

namespace MyContactManagerServices
{
    public interface IContactService
    {
        Task<IList<Contact>> GetAllAsync(string UserId);
        Task<Contact?> GetAsync(int id, string UserId);
        Task<int> AddOrUpdateAsync(Contact contact, string UserId);
        Task<int> DeleteAsync(int id, string UserId);
        Task<int> DeleteAsync(Contact contact, string UserId);
        Task<bool> ExistsAsync(int id, string UserId);

    }
}
