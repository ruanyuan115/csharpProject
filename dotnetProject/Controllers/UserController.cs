using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetProject.Services;
using PersistentLayer.Mapper;
using dotnetProject.Entity;

namespace dotnetProject.Controllers
{
    [Route("/")]
    public class UserController:Controller
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("register")]
        public ResultEntity register(UserInfo userInfo)
        {
            return userService.addUser(userInfo);
        }
        [HttpPost("auth")]
        public ResultEntity auth(User user)
        {
            return userService.login(user.username,user.password);
        }
  
    }
}
