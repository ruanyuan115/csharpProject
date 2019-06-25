using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;

namespace dotnetProject.Entity
{
    public class CourseAndClassList
    {
        public CourseInfo courseInfo;
        public List<CourseClass> courseClasses;
        public String courseName;
        public CourseAndClassList(CourseInfo courseInfo, List<CourseClass> courseClasses)
        {
            this.courseInfo = courseInfo;
            this.courseClasses = courseClasses;
        }

        public CourseAndClassList(CourseInfo courseInfo, List<CourseClass> courseClasses, String courseName)
        {
            this.courseInfo = courseInfo;
            this.courseClasses = courseClasses;
            this.courseName = courseName;
        }

        public CourseAndClassList(CourseInfo courseInfo, CourseClass courseClass, String courseName)
        {
            this.courseInfo = courseInfo;
            courseClasses = new List<CourseClass>();
            this.courseClasses.Add(courseClass);
            this.courseName = courseName;
        }
    }
}
