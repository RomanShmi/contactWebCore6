using ContactWebModels;
using MyContactManagersRepositories;

namespace MyContactManagerServices
{
    public class ContactsService : IContactService
    {
        private IContactsRepository _contactsRepository;

        public ContactsService(IContactsRepository contactsRepo)
        {
            _contactsRepository = contactsRepo;
        }

        public async Task<IList<Contact>> GetAllAsync(string UserId)
        {
            return await _contactsRepository.GetAllAsync(UserId);
        }

        public async Task<Contact?> GetAsync(int id, string UserId)
        {
            return await _contactsRepository.GetAsync(id, UserId);
        }

        public async Task<int> AddOrUpdateAsync(Contact contact, string UserId)
        {
            return await _contactsRepository.AddOrUpdateAsync(contact, UserId);
        }

        public async Task<int> DeleteAsync(Contact contact, string UserId)
        {
            return await _contactsRepository.DeleteAsync(contact, UserId);
        }

        public async Task<int> DeleteAsync(int id, string UserId)
        {
            return await _contactsRepository.DeleteAsync(id, UserId);
        }

        public async Task<bool> ExistsAsync(int id, string UserId)
        {
            return await _contactsRepository.ExistsAsync(id, UserId);
        }
    }
}
