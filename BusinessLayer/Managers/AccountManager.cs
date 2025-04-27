using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Dto;
using BusinessLayer.ManagersInterfaces;

using DataAcessLayer.GenericRepo;
using DataAcessLayer.Models;

using Microsoft.AspNetCore.Identity;
using BusinessLayer.Helper;



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
            var (isValid, errorMessage) = PasswordValidator.Validate(user.password);
            if (!isValid)
            {
                return IdentityResult.Failed(new IdentityError { Description = errorMessage });
            }

            var existingUser = await _UserManager.FindByNameAsync(user.user_name);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username is already registered." });
            }

            var newUser = new ApplicationUser
            {
                FirstName = user.firstName,
                LastName = EncryptionHelper.Encrypt(user.lastName),
                UserName = user.user_name,
                Email = user.email,
                PhoneNumber = EncryptionHelper.Encrypt(user.phoneNumber),
                MemberShipDate = DateTime.Now,
                PasswordHash = CustomHasher.Hash(user.password) 
            };

            return await _UserManager.CreateAsync(newUser); 
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
            var adminUser = new ApplicationUser
            {
                FirstName = user.firstName,
                LastName = EncryptionHelper.Encrypt(user.lastName),
                UserName = user.user_name,
                Email = user.email,
                PhoneNumber = EncryptionHelper.Encrypt(user.phoneNumber),
                MemberShipDate = DateTime.Now,
                Role = "Admin",
                PasswordHash = CustomHasher.Hash(user.password) 
            };

            return await _UserManager.CreateAsync(adminUser);
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
            var isPasswordValid = CustomHasher.Verify(_user.password, appuser.PasswordHash);
            if (!isPasswordValid)
            {
                return new LoginResult { Succeeded = false, ErrorMessage = "Invalid Password" };
            }
            return new LoginResult() 
            {
                Succeeded = true,
                Id = appuser.Id,
                email = appuser.Email,
                role = appuser.Role,
            };
        }
        public async Task<AccountDto> GetById(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user == null) return null;

            return new AccountDto
            {
                firstName = user.FirstName,
                lastName = EncryptionHelper.Decrypt(user.LastName),
                user_name = user.UserName,
                email = user.Email,
                phoneNumber = EncryptionHelper.Decrypt(user.PhoneNumber)
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
            if (userName != null && userName.UserName != account.user_name)
            {
                return new LoginResult()
                {
                    ErrorMessage = "UserName is already registered.",
                    Succeeded = false
                };
            }
            olduser.FirstName = account.firstName;
            olduser.LastName = EncryptionHelper.Encrypt(account.lastName); 
            olduser.UserName = account.user_name;
            olduser.PhoneNumber = EncryptionHelper.Encrypt(account.phoneNumber);
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