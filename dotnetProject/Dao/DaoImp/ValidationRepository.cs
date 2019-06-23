using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetProject.Infrastructure.Repository;
using PersistentLayer.Apis;

namespace dotnetProject.Repository
{
    public class ValidationRepository : IValidationRepository
    {
        public async Task<bool> ValidationAsync(string username,string password)
        {
            return AccountApi.Login(username, password);
        }

        public async Task<Boolean> IsUsedUsernameAsysnc(string username)
        {
            return await AccountApi.IsUsedUsernameAsync(username);
        }

        public async Task<Boolean> IsUsedPhoneAsync(string username)
        {
            return await AccountApi.IsUsedPhoneAsync(username); 
        }

        public async Task<Boolean> IsUsedMailAsync(string mail)
        {
            return await AccountApi.IsUsedMailAsync(mail);
        }

        public async Task<Boolean> RegisteAsync(string username, string password, string mail, string phone)
        {
            return AccountApi.Register(username, password, mail, phone);
        }

    }
}
