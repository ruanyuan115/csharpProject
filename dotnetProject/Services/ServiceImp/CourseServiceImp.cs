using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using PersistentLayer.Apis;
using dotnetProject.Entity;

namespace dotnetProject.Services.ServiceImp
{
    public class CourseServiceImp:CourseService
    {
        public CourseName getCourseNameByNameID(int? courseNameID)
        {
            return CourseNameApi.getByID(courseNameID);
        }
        public int? addNewCourse(CourseInfo courseInfo)
        {
            if (courseInfo != null)
            {
                return CourseInfoApi.insert(courseInfo)==1? 1 : 0;
            }
            else
                return 0;
        }

        
        
    public int? addClass(CourseClass courseClass)
        {
            if (courseClass != null && CourseClassApi.findByCourseIDAndClassNum(courseClass.courseID, courseClass.classNum) == null)
            {
                courseClass.classCode=("" + Math.Abs(DateTime.Now.GetHashCode() % 10000));
                return CourseClassApi.insert(courseClass)==1? 1 : 0;
            }
            else
                return 0;
        }

        
        
    public int? alertClassInfo(CourseClass courseClass)
        {
            if (courseClass != null)
            {
                CourseClass temp = CourseClassApi.findByCourseIDAndClassNum(courseClass.courseID, courseClass.classNum);
                if (temp == null || temp.id==(courseClass.id))//要么是更改第几班 要么更改当前作业进度
                    return CourseClassApi.insert(courseClass)==1 ? 1 : 0;
                else
                    return -1;
            }
            else
                return 0;
        }

        
    public List<CourseAndClass> getStuCourseList(int? studentID)
        {
        if(studentID!=null&&studentID>0)
        {
            List<CourseAndClass> courseList = new List<CourseAndClass>();
        List<Takes> takesList = TakesApi.findByStudentID(studentID);
            if(takesList!=null)
            {
                foreach(Takes i in takesList)
                {
                    CourseClass courseClass = CourseClassApi.getByID(i.courseClassID);
                    CourseInfo temp = CourseInfoApi.findByCourseID(courseClass.courseID);
                    if(temp!=null)
                        courseList.Add(new CourseAndClass(temp, courseClass));
                }
}
            return courseList;
        }
        return null;
    }

    
    public CourseAndClass getCourseByCode(String courseCode)
{
    CourseClass temp=CourseClassApi.findByClassCode(courseCode);

        return temp!=null?new CourseAndClass(CourseInfoApi.findByCourseID(temp.courseID),temp):null;
    }

    
    
    public int? joinCourse(int? studentID, int? courseClassID)
{
    Takes takes = new Takes();
    takes.courseClassID=courseClassID;
    takes.studentID=studentID;
    if (TakesApi.findByStudentIDAndCourseClassID(studentID, courseClassID) == null)//该学生之前没选过这门课,且该课程存在
        return (CourseClassApi.getByID(courseClassID)!=null && TakesApi.insert(takes)==1) ? 1 : 0;
    else
        return -1;
}


    public int? addCourseNotice(CourseNotice courseNotice)
{
    if (courseNotice != null && CourseInfoApi.findByCourseID(courseNotice.courseID) != null)//如果有该课程
        return CourseNoticeApi.insert(courseNotice)==1 ? 1 : 0;
    else
        return -1;
}


    public CourseNotice getNoticeByCouID(int? courseID)
{
    return courseID > 0 ? CourseNoticeApi.findByCourseID(courseID) : null;
}


    public CourseInfo getCourseInfoByID(int? courseID)
{
    return courseID > 0 ? CourseInfoApi.findByCourseID(courseID) : null;
}


    public CourseClass getClassInfoByID(int? courseClassID)
{
    CourseClass temp = CourseClassApi.getByID(courseClassID);
    return courseClassID > 0 ? temp : null;
}



    public int? deleteCourse(int? courseID)
{
    if (CourseInfoApi.findByCourseID(courseID) != null)
    {
        courseInfoApi.deleteById(courseID);
        return 1;
    }
    else
        return 0;
}



    public int? deleteCourseNotice(int? courseID)
{
    if (courseNoticeApi.findByCourseID(courseID) != null)
    {
        courseNoticeApi.deleteByCourseID(courseID);
        return 1;
    }
    else
        return 0;
}



    public ChapterNode addChapter(ChapterNode chapterNode)
{
    if (chapterNode != null)
    {
        ChapterNode temp = chapterContentApi.saveAndFlush(chapterNode);
        return temp.getId() != null ? temp : null;
    }
    else
        return null;
}


    public ChapterNode getChapterByID(int? chapterID)
{
    Optional<ChapterNode> temp = chapterContentApi.findById(chapterID);
    return temp.orElse(null);
}


    public ArrayList<CourseCatalog> getCourseCatalog(int? courseID)
{
    ArrayList<ChapterNode> chapterNodes = chapterContentApi.findByCourseID(courseID);
    if (chapterNodes != null && chapterNodes.size() > 0)
    {
        CourseCatalog bookCatalog = new CourseCatalog();
        ChapterNode book = new ChapterNode();
        book.setId(0);
        bookCatalog.setChapterNode(book);

        makeCatalog(bookCatalog, chapterNodes);
        return bookCatalog.getSubCatalog();
    }
    else
        return null;
}
private void getSubNodes(CourseCatalog courseCatalog, ArrayList<ChapterNode> chapterNodes)
{
    int? parentID = courseCatalog.getId();
    int? siblingID = 0;
    Iterator<ChapterNode> it = chapterNodes.iterator();
    while (it.hasNext())//如果还有没被加入的节点
    {
        ChapterNode temp = it.next();
        if (temp.getParentID().equals(parentID) && temp.getSiblingID().equals(siblingID))//如果该节点符合要求
        {
            CourseCatalog subCatalog = new CourseCatalog();
            subCatalog.setChapterNode(temp);

            courseCatalog.getSubCatalog().add(subCatalog);

            chapterNodes.remove(temp);        //将该节点从数组中移除
            siblingID = temp.getId();

            it = chapterNodes.iterator();       //再次遍历获取兄弟节点
        }
    }
}
private void makeCatalog(CourseCatalog courseCatalog, ArrayList<ChapterNode> chapterNodes)
{
    getSubNodes(courseCatalog, chapterNodes);    //先获取该节点的子节点 再递归操作获取子子节点
    Iterator<CourseCatalog> it = courseCatalog.getSubCatalog().iterator();
    while (it.hasNext() && !chapterNodes.isEmpty())
    {
        makeCatalog(it.next(), chapterNodes);
    }
}
    }
}
