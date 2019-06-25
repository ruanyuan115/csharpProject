using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersistentLayer.Apis;
using PersistentLayer.Mapper;
using dotnetProject.Entity;
using dotnetProject.Services;


namespace dotnetProject.Controllers
{
    [Route("/")]
    public class CourseController:Controller
    {
        private readonly UserService userService;
        private readonly CourseService courseService;

        public CourseController(UserService userService,CourseService courseService)
        {
            this.userService = userService;
            this.courseService = courseService;
        }
    }
}
