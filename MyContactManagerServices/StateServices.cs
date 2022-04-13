using ContactWebModels;
using MyContactManagersRepositories;

namespace MyContactManagerServices
{
    public class StateServices : IStateService
    {

        private IStatesRepository _stateRepository;
        
        public StateServices(IStatesRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public  async Task<IList<State>> GetAllAsync()
        {
            var states = await _stateRepository.GetAllAsync();
            return states.OrderBy(x=> x.Name).ToList();
        }

        public async Task<State?> GetAsync(int id)
        {
            return await _stateRepository.GetAsync(id);
        }

        public async Task<int> AddOrUpdateAsync(State state)
        {
           return await _stateRepository.AddOrUpdateAsync(state);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _stateRepository.DeleteAsync(id);
        }

        public async Task<int> DeleteAsync(State state)
        {
            return await _stateRepository.DeleteAsync(state);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _stateRepository.ExistsAsync(id);
        }

       
    }
}