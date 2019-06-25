using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;

namespace dotnetProject.Entity
{
    public class CourseRelationEntity
    {
        public CourseName courseName;
        public List<CourseName> preCoursesName;
        public List<CourseName> subCoursesName;
        public CourseRelationEntity(CourseName courseName, List<CourseName> preCoursesName, List<CourseName> subCoursesName)
        {
            this.courseName = courseName;
            this.preCoursesName = preCoursesName;
            this.subCoursesName = subCoursesName;
        }
    }
}
