﻿using Microsoft.AspNetCore.Identity;
using RPInventory.Models;
using RPInventory.ViewModels;

namespace RPInventory.Helpers;

public class UserFactory
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserFactory(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public User CreateUser(UserRegisterViewModel userVM)
    {
        var user = new User()
        {
            Id = userVM.Id,
            Lastname = userVM.Lastname,
            CelPhone = userVM.CelPhone,
            Email = userVM.Email,
            Name = userVM.Name,
            ProfileId = userVM.ProfileId,
            Username = userVM.Username
        };

        user.Password = _passwordHasher.HashPassword(user, userVM.Password);
        return user;
    }

    public UserEditViewModel CreateUserEdit(User user)
    {
        return new UserEditViewModel()
        {
            Id = user.Id,
            Lastname = user.Lastname,
            CelPhone = user.CelPhone,
            Email = user.Email,
            Name = user.Name,
            ProfileId = user.ProfileId,
            Username = user.Username,
            Photo = user.Photo
        };

    }

    public void UpdateUserData(UserEditViewModel user, User userDb)
    {
        userDb.CelPhone = user.CelPhone;
        userDb.Email = user.Email;
        userDb.Name = user.Name;
        userDb.Lastname = user.Lastname;
        userDb.ProfileId = user.ProfileId;
    }

    public UserChangePasswordViewModel CreateUserChangePassword(User user)
    {
        return new UserChangePasswordViewModel
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public UserChangePasswordViewModel ChangePassword(UserChangePasswordViewModel userVM, User userDb)
    {
        userDb.Password = _passwordHasher.HashPassword(userDb, userVM.Password);
        return userVM;
    }
}
