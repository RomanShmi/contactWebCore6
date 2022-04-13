using ContactWebModels;
using Microsoft.EntityFrameworkCore;
using MyContactManagerData;


namespace MyContactManagersRepositories
{
    public class ContactsRepository : IContactsRepository
    {

        private MyContactManagerDbContext _context;

        public ContactsRepository(MyContactManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Contact>> GetAllAsync(string UserId)
        {
            var result = await _context.Contacts.Include(x => x.State).AsNoTracking()
                .Where(x => x.UserId == UserId)
                .ToListAsync();
            return result.OrderBy(x=> x.LastName).ThenBy(x=> x.FirstName).ToList();
        }

        public async Task<Contact?> GetAsync(int id, string UserId)
        {
            return await _context.Contacts.Include(x=>x.State).AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId);
        }

       
           private async Task<int> Insert(Contact contact, string UserId)
        {
            await GetExistingStateReference(contact);
             await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact.Id;
        }

       private async Task GetExistingStateReference(Contact contact)
        {
            var existingState= await _context.State.SingleOrDefaultAsync(x => x.Id == contact.StateId);
            if (existingState is not null)
            {
                contact.State = existingState;

            }


        }


        private async Task<int> Update(Contact contact)
        {
            var existingcontact= await _context.Contacts.SingleOrDefaultAsync(x=>x.Id == contact.Id);
            if (existingcontact is null) throw new Exception("Contact not found");

            existingcontact.Birthday = contact.Birthday;
            existingcontact.FirstName = contact.FirstName;
            existingcontact.LastName = contact.LastName;
           // existingcontact.State = contact.State;
            existingcontact.City= contact.City;
            existingcontact.Email= contact.Email;
            existingcontact.PhonePrimary= contact.PhonePrimary;
            existingcontact.PhoneSecondary= contact.PhoneSecondary;
            existingcontact.StreetAddress1= contact.StreetAddress1;
            existingcontact.StreetAddress2= contact.StreetAddress2;
            existingcontact.UserId= contact.UserId; 
            existingcontact.UserId=contact.UserId;
         



            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<int> AddOrUpdateAsync(Contact contact, string UserId)
        {
            if (contact.Id > 0)
            {
                if(!await ExistsAsync(contact.Id, UserId))
                {
                    throw new Exception("contact not found");
                }
                return await Update(contact);
            }
            return await Insert(contact, UserId);
        }

        public async Task<int> DeleteAsync(int id, string UserId)
        {
           var existinngContact = await _context.Contacts.SingleOrDefaultAsync(x=>x.Id == id && x.UserId == UserId);
            if (existinngContact is null) throw new Exception("Can not delete ");
            
            await Task.Run(() => { _context.Contacts.Remove(existinngContact); });
            await _context.SaveChangesAsync();
            
            
            return id;
        }

        public async Task<int> DeleteAsync(Contact contact, string UserId)
        {
            return await DeleteAsync(contact.Id, UserId);
        }

        public async Task<bool> ExistsAsync(int id, string UserId)
        {
            return await _context.Contacts.AsNoTracking().AnyAsync(x => x.Id == id && x.UserId == UserId);
        }

       
    }
}
