using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using dotnetProject.Entity;

namespace dotnetProject.Services
{
    public interface UserService
    {
        ResultEntity addUser(UserInfo userInfo);
        ResultEntity login(String userName, String password);
    }
}
