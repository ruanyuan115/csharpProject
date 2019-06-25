using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;

namespace dotnetProject.Entity
{
    public class ChapterRelationEntity
    {
        public ChapterNode chapterNode;
        public List<ChapterNode> preChapterNodes;
        public List<ChapterNode> subChapterNodes;
        public ChapterRelationEntity(ChapterNode chapterNode, List<ChapterNode> preChapterNodes, List<ChapterNode> subChapterNodes)
        {
            this.chapterNode = chapterNode;
            this.preChapterNodes = preChapterNodes;
            this.subChapterNodes = subChapterNodes;
        }
    }
}
