using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;


namespace dotnetProject.Entity
{
    public class CourseCatalog
    {
        public int? id;
        public int? courseID;
        public String contentName;
        public int? parentID;
        public int? siblingID;
        public String content;
        public String exerciseTitle;
        public Boolean exerciseVisible_1;
        public Boolean exerciseVisible_2;
        public DateTime exerciseDeadline_1;
        public DateTime exerciseDeadline_2;
        public int? exerciseTotal_1;
        public int? exerciseTotal_2;
        public List<CourseCatalog> subCatalog;
        public CourseCatalog()
        {
            subCatalog = new List<CourseCatalog>();
        }
        public void setChapterNode(ChapterNode chapterNode)
        {
            this.id = chapterNode.id;
            this.courseID = chapterNode.courseID;
            this.contentName = chapterNode.contentName;
            this.parentID = chapterNode.parentID;
            this.siblingID = chapterNode.siblingID;
            this.content = chapterNode.content;
            this.exerciseTitle = chapterNode.exerciseTitle;
            this.exerciseVisible_1 = chapterNode.exerciseVisible_1;
            this.exerciseVisible_2 = chapterNode.exerciseVisible_2;
            this.exerciseDeadline_1 = chapterNode.exerciseDeadline_1;
            this.exerciseDeadline_2 = chapterNode.exerciseDeadline_2;
            this.exerciseTotal_1 = chapterNode.exerciseTotal_1;
            this.exerciseTotal_2 = chapterNode.exerciseTotal_2;
        }
    }
}
