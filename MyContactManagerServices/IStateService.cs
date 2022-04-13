﻿using ContactWebModels;

namespace MyContactManagerServices
{
    public interface IStateService
    {
        Task<IList<State>> GetAllAsync();
        Task<State?> GetAsync(int id);
        Task<int> AddOrUpdateAsync(State state);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(State state);
        Task<bool> ExistsAsync(int id);
    }
}
