﻿using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
    /// <summary>
    /// Interface for User-Repository.
    /// Defines all methods, that are necessary for the repo
    /// </summary>
    public interface IUserRepo
    {
        Task CreateModelAsync(User _model);
        Task<List<User>> GetAllModelsAsync(string _compCode);
        Task<List<User>> GetAllModelsAsync(int _roleNr);
        Task UpdateModelAsync(User _model);
    }
}
