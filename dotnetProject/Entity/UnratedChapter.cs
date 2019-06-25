using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;


namespace dotnetProject.Entity
{
    public class UnratedChapter
    {
        public ChapterNode chapterNode;
        public int? studentId;

        public UnratedChapter()
        {
        }

        public UnratedChapter(ChapterNode chapterNode, int? studentId)
        {
            this.chapterNode = chapterNode;
            this.studentId = studentId;
        }
    }
}
