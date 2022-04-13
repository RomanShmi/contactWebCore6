using ContactWebModels;
using Microsoft.EntityFrameworkCore;
using MyContactManagerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyContactManagersRepositories
{
    public class StatesRepository : IStatesRepository
    {
        private MyContactManagerDbContext _context;    
        public StatesRepository( MyContactManagerDbContext context)
        {
            _context=context;
        }
        
        
        
        public async Task<IList<State>> GetAllAsync()
        {
            return await _context.State.AsNoTracking().ToListAsync();
        }

        public async Task<State?> GetAsync(int id)
        {
            return await _context.State.AsNoTracking().FirstOrDefaultAsync(x=> x.Id ==id);
        }



        public async Task<int> AddOrUpdateAsync(State state)
        {
            if (state.Id > 0)
            {
                return await Update(state);
            }

            return await Insert(state);
        }

        private async Task<int> Insert(State state)
        {
             await _context.State.AddAsync(state);
            await _context.SaveChangesAsync();
            return state.Id;
        }

        private async Task<int> Update(State state)
        {
            var existingState= await _context.State.SingleOrDefaultAsync(x=>x.Id == state.Id);
            if (existingState is null) throw new Exception("State not found");
            existingState.Name = state.Name;
            existingState.Abbreviation = state.Abbreviation;
         
            await _context.SaveChangesAsync();
            return state.Id;
        }



        public async Task<int> DeleteAsync(State state)
        {
            return await DeleteAsync(state.Id);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingState = await _context.State.SingleOrDefaultAsync(x => x.Id ==id);
            if (existingState is null) throw new Exception("Could not delite");
           
            await Task.Run(() => { _context.State.Remove(existingState); });


            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<bool> ExistsAsync(int id)
        {
          return await _context.State.AnyAsync(x => x.Id == id);
        }

        
    }
}
