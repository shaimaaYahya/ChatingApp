using System;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface UserRepositry
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<PagedList<AppUser>> GetUsersAsync(UserParams userParams);
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
}
