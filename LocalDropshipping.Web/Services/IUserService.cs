﻿using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using System.Security.Claims;

namespace LocalDropshipping.Web.Services
{
    public interface IUserService
    {
        User? Add(User user);
        List<User?> GetAll();
        List<User?> GetAllStaffMember();
        User? GetById(string userId);
        User? Delete(string userId);
        User? DisableUser(string userId);
        User? Update(string userId, UserDto userDto);
        Task<User?> GetCurrentUserAsync();
        Task<bool> IsUserSignedIn();
        Task UpdateUserAsync(User user);
    }
}