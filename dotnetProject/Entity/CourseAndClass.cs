using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using dotnetProject.Services;
using dotnetProject.Services.ServiceImp;

namespace dotnetProject.Entity
{
    public class CourseAndClass
    {
        public CourseInfo courseInfo;
        public CourseClass courseClass;
        public String courseNameID;
        public CourseAndClass(CourseInfo courseInfo, CourseClass courseClass)
        {
            
        CourseInfo temp = courseInfo;
        temp.courseName=(new CourseServiceImp().getCourseNameByNameID(int.Parse(courseInfo.courseName)).courseName);
        this.courseInfo=temp;
        this.courseClass=courseClass;
        this.courseNameID=courseInfo.courseName;
    }
}
}
