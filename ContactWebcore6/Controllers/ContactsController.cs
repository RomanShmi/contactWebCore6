
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactWebModels;
using Microsoft.Extensions.Caching.Memory;
using MyContactManagerServices;
using ContactWebcore6.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ContactWebEFCore6.Controllers
{
   [Authorize]
    public class ContactsController : Controller
    {
        private readonly IContactService _contactsService;
        private readonly IStateService _statesService;
        private static IList<State> _allStates;
        private static SelectList _statesData;
        private IMemoryCache _cache;

        public ContactsController(IContactService contactsService, IStateService statesService, IMemoryCache cache)
        {
            _contactsService = contactsService;
            _statesService = statesService;
            _cache = cache;
            SetAllStatesCachingData();
            _statesData = new SelectList(_allStates, "Id", "Abbreviation");
        }

        private void SetAllStatesCachingData()
        {
            var allStates = new List<State>();
            if (!_cache.TryGetValue(ContactCacheContacts.ALL_STATES, out allStates))
            {
                var allStatesData = Task.Run(() => _statesService.GetAllAsync()).Result;

                _cache.Set(ContactCacheContacts.ALL_STATES, allStatesData, TimeSpan.FromDays(1));
                allStates = _cache.Get(ContactCacheContacts.ALL_STATES) as List<State>;
            }
            _allStates = allStates;
        }

        private async Task UpdateStateAndResetModelState(Contact contact)
        {
            ModelState.Clear();
            var state = _allStates.SingleOrDefault(x => x.Id == contact.StateId);
            contact.State = state;
            TryValidateModel(contact);
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserID();
            var contacts = await _contactsService.GetAllAsync(userId);
            return View(contacts);
        }

        protected async Task<string> GetCurrentUserID()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = await GetCurrentUserID();
            var contact = await _contactsService.GetAsync((int)id, userId);

            if (id == null)
            {
                return NotFound();
            }
              if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = _statesData;
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,StateId,Zip,UserId")] Contact contact)
        {
            var userId = await GetCurrentUserID();
            contact.UserId = userId;

            await UpdateStateAndResetModelState(contact);
            if (ModelState.IsValid)
            {
                  await _contactsService.AddOrUpdateAsync(contact, userId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = _statesData;
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = await GetCurrentUserID();
            var contact = await _contactsService.GetAsync((int)id, userId);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["StateId"] = _statesData;
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,StateId,Zip,UserId")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }
            var userId = await GetCurrentUserID();
            contact.UserId = userId;

            await UpdateStateAndResetModelState(contact);
            if (ModelState.IsValid)
            {
                
                try
                {
                    await _contactsService.AddOrUpdateAsync(contact, userId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ContactExists(contact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"] = _statesData;
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = await GetCurrentUserID();
            
            var contact = await _contactsService.GetAsync((int)id, userId);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = await GetCurrentUserID();
            await _contactsService.DeleteAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ContactExists(int id)
        {
            var userId = await GetCurrentUserID();
            return await _contactsService.ExistsAsync(id, userId);
        }
    }
}
