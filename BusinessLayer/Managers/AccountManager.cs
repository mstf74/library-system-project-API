﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;

using DataAcessLayer.GenericRepo;
using DataAcessLayer.Models;

using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Managers
{
    public class AccountManager:IAccountManager
    {
        IGenericRepo<ApplicationUser> _UserRepo;
        UserManager<ApplicationUser> _UserManager;

        public AccountManager(IGenericRepo<ApplicationUser> userRepo, UserManager<ApplicationUser> UserManager)
        {
            _UserManager = UserManager;
            _UserRepo = userRepo;
        }
        public async Task<IdentityResult> Register(RegistrationDto user)
        {
            var userName = await _UserManager.FindByNameAsync(user.user_name);
            if (userName != null) 
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "UserName is already registered."
                });
            }
            ApplicationUser iduser = new ApplicationUser()
            {
                FirstName = user.firstName,
                LastName = user.lastName,
                UserName = user.user_name,
                Email = user.email,
                PhoneNumber = user.phoneNumber,
                MemberShipDate = DateTime.Now,
            };
            var result = await _UserManager.CreateAsync(iduser, user.password);
            return result;
        }
        public async Task<IdentityResult> RegisterAdmin(RegistrationDto user)
        {
            var userName = await _UserManager.FindByNameAsync(user.user_name);
            if (userName != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "UserName is already registered."
                });
            }
            ApplicationUser iduser = new ApplicationUser()
            {
                FirstName = user.firstName,
                LastName = user.lastName,
                UserName = user.user_name,
                Email = user.email,
                PhoneNumber = user.phoneNumber,
                MemberShipDate = DateTime.Now,
                Role = "Admin"
            };
            var result = await _UserManager.CreateAsync(iduser, user.password);
            return result;
        }
        public async Task<LoginResult> Login(LoginDto _user)
        {
            var appuser = await _UserManager.FindByEmailAsync(_user.email);
            if (appuser == null)
            {
                return new LoginResult()
                {
                    Succeeded = false,
                    ErrorMessage = "Invalid Email"
                };
            }
            var check = await _UserManager.CheckPasswordAsync(appuser, _user.password);
            if (!check)
            {
                return new LoginResult()
                {
                    Succeeded = false,
                    ErrorMessage = "Invalid Password"
                };
            }
            return new LoginResult() 
            {
                Succeeded = true,
                Id = appuser.Id,
                user_name = appuser.UserName,
                email = appuser.Email,
                role = appuser.Role,
                
            };

        }
        public async Task<AccountDto> GetById(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null)
                return null;
            
            return new AccountDto()
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                user_name = user.UserName,
                email = user.Email,
                phoneNumber = user.PhoneNumber
            };

        }
        public async Task<LoginResult> UpdateUser(string id , AccountDto account)
        {
            var olduser = await _UserManager.FindByIdAsync(id);
            if (olduser == null)
            {
                return new LoginResult()
                {
                    ErrorMessage = "couldn't find the user",
                    Succeeded= false
                };
            }
            var userName = await _UserManager.FindByNameAsync(account.user_name);
            if (userName != null)
            {
                return new LoginResult()
                {
                    ErrorMessage = "UserName is already registered.",
                    Succeeded = false
                };
            }
            olduser.FirstName = account.firstName;
            olduser.LastName = account.lastName;
            olduser.UserName = account.user_name;
            olduser.PhoneNumber = account.phoneNumber;
            var result = await _UserManager.UpdateAsync(olduser);
            if (!result.Succeeded)
            {
                return new LoginResult()
                {
                    ErrorMessage = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Succeeded = false
                };
            }
            return new LoginResult()
            {
                ErrorMessage = "your account is updated successfully",
                Succeeded = true
            };
        }

    }
}
