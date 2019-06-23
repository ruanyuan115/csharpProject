using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotnetProject.Infrastructure.Repository
{
    public interface IValidationRepository
    {
        Task<Boolean> ValidationAsync(string username,string password);

        Task<Boolean> IsUsedUsernameAsysnc(string username);

        Task<Boolean> IsUsedPhoneAsync(string username);

        Task<Boolean> IsUsedMailAsync(string mail);

        Task<Boolean> RegisteAsync(string username, string password, string mail, string phone);
    }
}
