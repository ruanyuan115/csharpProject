using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetProject.Entity;
using PersistentLayer.Mapper;

namespace dotnetProject.Entity
{
    public class StudentChapterEntity
    {
        public StudentChapter studentChapter;
        public String chapterName;
        public StudentChapterEntity(StudentChapter studentChapter, String chapterName)
        {
            this.studentChapter = studentChapter;
            this.chapterName = chapterName;
        }
    }
}
