using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetProject.Services;
using PersistentLayer.Mapper;
using dotnetProject.Entity;
using System.Threading;

namespace dotnetProject.Controllers
{
    [Route("/")]
    public class UserController:Controller
    {
        private static object objlock = new object();
        public static long userNum=0;
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }
        [HttpGet("getUserNum")]
        public long getUserNum()
        {
            return userNum;
        }
        [HttpPost("register")]
        public ResultEntity register(UserInfo userInfo)
        {
            return userService.addUser(userInfo);
        }
        [HttpPost("auth")]
        public ResultEntity auth(User user)
        {
            ThreadPool.QueueUserWorkItem(addUserNum);
            return userService.login(user.username,user.password);
        }
        private void addUserNum(object state)
        {
            lock (objlock)
            {
                userNum++;
            }
        }
  
    }
}
