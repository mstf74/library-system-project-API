﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;
using DataAcessLayer.Models;

namespace BusinessLayer.ManagersInterfaces
{
    public interface IAccountManager
    {
        Task<IdentityResult> Register(RegistrationDto user);
        Task<LoginResult> Login(LoginDto _user);
         Task<IdentityResult> RegisterAdmin(RegistrationDto user);
        Task<AccountDto> GetById(string id);
        Task<LoginResult> UpdateUser(string id, AccountDto account);

    }
}
