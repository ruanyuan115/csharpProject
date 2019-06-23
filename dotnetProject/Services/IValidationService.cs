using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetProject.RespondInfo.Validation;

namespace dotnetProject.Infrastructure.Services
{
    public interface IValidationService
    {
        Task<Boolean> ValidationAsync(string username,string password);

        Task<RegisteRespond> RegisteAsync(string username, string password, string mail, string phone);

    }
}
