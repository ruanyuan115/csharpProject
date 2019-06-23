using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetProject.Infrastructure.Services;
using dotnetProject.Infrastructure.Repository;
using dotnetProject.RespondInfo.Validation;

namespace dotnetProject.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IValidationRepository _validationRepository;

        public ValidationService(IValidationRepository validationRepository)
        {
            _validationRepository = validationRepository;
        }//依赖注入持久层

        public async Task<RegisteRespond> RegisteAsync(string username, string password, string mail, string phone)
        {
            Boolean flag;
            RegisteRespond registeRespond = new RegisteRespond();
            if (await _validationRepository.IsUsedMailAsync(mail)) {
                registeRespond.Success = false;
                registeRespond.Message = "Mail is taken";
                return registeRespond;
            }

            if(await _validationRepository.IsUsedPhoneAsync(phone))
            {
                registeRespond.Success = false;
                registeRespond.Message = "Phone is taken";
                return registeRespond;
            }

            if(await _validationRepository.IsUsedUsernameAsysnc(username))
            {
                registeRespond.Success = false;
                registeRespond.Message = "Username is taken";
                return registeRespond;
            }

            await _validationRepository.RegisteAsync(username, password, mail, phone);
            registeRespond.Success = true;
            registeRespond.Message = "Successfully Registed";
            return registeRespond;
            
        }//注册

        public async Task<bool> ValidationAsync(string username, string password)
        {
            return await _validationRepository.ValidationAsync(username, password);
        }//登陆
    }
}
