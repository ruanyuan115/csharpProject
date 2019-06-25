using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PersistentLayer.Apis;
using PersistentLayer.Mapper;
using dotnetProject.Entity;
using dotnetProject.Services;
using System.Threading;

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
        [HttpPost("addCourse")]
    public ResultEntity addNewCourse(CourseInfo courseInfo)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setState(courseService.addNewCourse(courseInfo));
            courseInfo.courseName=(courseService.getCourseNameByNameID(int.Parse(courseInfo.courseName)).courseName);
            resultEntity.setData(courseInfo);
            resultEntity.setMessage(resultEntity.getState() == 1 ? "新增课程成功！" : "新增课程失败！");
            return resultEntity;
        }
        [HttpPost("addClass")]
    public ResultEntity addClass(CourseClass courseClass)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setState(courseService.addClass(courseClass));
            resultEntity.setData(courseClass);
            resultEntity.setMessage(resultEntity.getState() == 1 ? "新增班级成功！" : "该班级已经存在！");
            return resultEntity;
        }
        [HttpGet("getCourseByCode")]
    public ResultEntity getCourseByCode(String courseCode)
        {
            ResultEntity resultEntity=new ResultEntity();
        resultEntity.setData(courseService.getCourseByCode(courseCode));
        resultEntity.setState(resultEntity.getData()!=null?1:0);
        resultEntity.setMessage(resultEntity.getData()!=null?"":"不存在该课程！");
        return resultEntity;
    }
    [HttpGet("joinCourse")]
    public ResultEntity joinCourse(int? studentID, int? courseClassID)
    {
        ResultEntity resultEntity = new ResultEntity();
        resultEntity.setState(courseService.joinCourse(studentID, courseClassID));
        resultEntity.setMessage(resultEntity.getState() == 1 ? "选课成功！" : "选课失败！");
        return resultEntity;
    }
    [HttpGet("getStuCourseList")]
    public ResultEntity getStuCourseList(int? studentID)
    {
        ResultEntity resultEntity=new ResultEntity();
    resultEntity.setData(courseService.getStuCourseList(studentID));
        resultEntity.setState(resultEntity.getData()!=null?1:0);
        return resultEntity;
    }
[HttpPost("addCourseNotice")]
    public ResultEntity addCourseNotice(int? courseID, String courseNotice)
{
    CourseNotice couNotice = courseService.getNoticeByCouID(courseID);
    if (couNotice == null)//未曾有过课程介绍
    {
        couNotice = new CourseNotice();
        couNotice.courseID=(courseID);
    }
    couNotice.courseNotice=(courseNotice);

    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.addCourseNotice(couNotice));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "公告发布成功！" : "公告发布失败！");
    return resultEntity;
}
[HttpGet("getNoticeByCouID")]
    public ResultEntity getNoticeByCouID(int? courseID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getNoticeByCouID(courseID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
[HttpPost("alterCourseInfo")]
    public ResultEntity alterCourseInfo(CourseInfo courseInfo)
{
    ResultEntity resultEntity = new ResultEntity();
    CourseInfo temp = courseService.getCourseInfoByID(courseInfo.courseID);
    if (temp != null)
    {
        //courseInfo.setUpdateTime(temp.getUpdateTime());
        resultEntity.setState(courseService.addNewCourse(courseInfo));
        resultEntity.setMessage(resultEntity.getState() == 1 ? "修改成功！" : "修改失败！");
    }
    else
    {
        resultEntity.setState(0);
        resultEntity.setMessage("该课程不存在！");
    }
    return resultEntity;
}
[HttpPost("alterClassInfo")]
    public ResultEntity alterClassInfo(CourseClass courseClass)
{
    ResultEntity resultEntity = new ResultEntity();
    CourseClass temp = courseService.getClassInfoByID(courseClass.id);
    if (temp != null)
    {
        //courseInfo.setUpdateTime(temp.getUpdateTime());
        resultEntity.setState(courseService.alertClassInfo(courseClass));
        resultEntity.setMessage(resultEntity.getState() == 1 ? "修改成功！" : resultEntity.getState() == -1 ? "修改内容已经存在！" : "修改失败！");
    }
    else
    {
        resultEntity.setState(0);
        resultEntity.setMessage("该班级不存在！");
    }
    return resultEntity;
}
[HttpGet("getCourseInfoByID")]
    public ResultEntity getCourseInfoByID(int? courseID)
{
    ResultEntity resultEntity = new ResultEntity();
    CourseInfo temp = courseService.getCourseInfoByID(courseID);
    if (temp != null)
        temp.courseName=(courseService.getCourseNameByNameID(int.Parse(temp.courseName)).courseName);
    resultEntity.setData(temp);
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    resultEntity.setMessage(resultEntity.getData() != null ? "" : "不存在该课程！");
    return resultEntity;
}
[HttpGet("getClassInfoByID")]
    public ResultEntity getClassInfoByID(int? courseClassID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getClassInfoByID(courseClassID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    resultEntity.setMessage(resultEntity.getData() != null ? "" : "不存在该班级！");
    return resultEntity;
}

[HttpPost("addChapter")]
    public ResultEntity addChapter(ChapterNode chapterNode)
{
    ResultEntity resultEntity = new ResultEntity();
    if (courseService.getCourseInfoByID(chapterNode.courseID) == null)
    {
        resultEntity.setState(0);
        resultEntity.setMessage("不存在该课程！");
    }
    else
    {
        resultEntity.setData(courseService.addChapter(chapterNode));
        resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    }
    return resultEntity;
}
[HttpPost("alertChapter")]
    public ResultEntity alertChapter(ChapterNode chapterNode)
{
    ResultEntity resultEntity = new ResultEntity();
    ChapterNode temp = courseService.getChapterByID(chapterNode.id);
    if (temp != null)//如果有该章节
    {
        resultEntity.setData(courseService.addChapter(chapterNode));
        resultEntity.setState(1);
        resultEntity.setMessage("修改成功！");
    }
    else
    {
        resultEntity.setState(0);
        resultEntity.setMessage("该章节不存在！");
    }
    return resultEntity;
}
[HttpGet("getChapterByID")]
    public ResultEntity getChapterByID(int? chapterID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getChapterByID(chapterID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    resultEntity.setMessage(resultEntity.getData() != null ? "" : "不存在该章节！");
    return resultEntity;
}
[HttpGet("getCourseCatalog")]
    public ResultEntity getCourseCatalog(int? courseID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getCourseCatalog(courseID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    resultEntity.setMessage(resultEntity.getData() != null ? "" : "该课程不存在目录！");
    return resultEntity;
}
        [HttpGet( "getCourseScoreAndComment")]
    public ResultEntity getCourseScoreAndComment(int? courseID, int? studentID)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setData(courseService.getCourseScoreAndComment(courseID, studentID));
            resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
            resultEntity.setMessage(resultEntity.getData() != null ? "" : "该学生尚未作出评论！");
            return resultEntity;
        }
        [HttpGet( "getCurrentProgress")]
    public ResultEntity getCurrentProgress(int? courseClassID, int? studentID)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setData(courseService.getCurrentProgress(courseClassID, studentID));
            resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
            resultEntity.setMessage(resultEntity.getData() != null ? "" : "选课状态有误！");
            return resultEntity;
        }
        [HttpGet( "alertCurrentProgress")]
    public ResultEntity alertCurrentProgress(int? courseClassID, int? studentID, int? chapterID)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setState(courseService.alertCurrentProgress(courseClassID, studentID, chapterID));
            resultEntity.setMessage(resultEntity.getState() == 1 ? "修改成功！" : "无该选课记录！");
            return resultEntity;
        }
        [HttpGet( "deleteChapter")]
    public ResultEntity deleteChapter(int? chapterID)
        {
            ResultEntity resultEntity = new ResultEntity();
            ChapterNode temp = courseService.getChapterByID(chapterID);
            if (temp != null)
            {
                CourseCatalog courseCatalog = new CourseCatalog();
                courseCatalog.setChapterNode(temp);
                courseService.deleteChapter(courseCatalog);
                resultEntity.setState(courseService.getChapterByID(chapterID) == null ? 1 : 0);
            }
            else
                resultEntity.setState(0);
            resultEntity.setMessage(resultEntity.getState() == 1 ? "删除成功！" : "删除失败,该章节不存在！");
            return resultEntity;
        }
        [HttpGet( "getClassesByCourseID")]
    public ResultEntity getClassesByCourseID(int? courseID)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setData(courseService.getClassesByCourseID(courseID));
            resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
            return resultEntity;
        }
        [HttpGet( "deleteClass")]
    public ResultEntity deleteClass(int? courseClassID)
        {
            ResultEntity resultEntity = new ResultEntity();
            resultEntity.setState(courseService.deleteClass(courseClassID));
            resultEntity.setMessage(resultEntity.getState() == 1 ? "删除成功！" : "该班级并不存在！");
            return resultEntity;
        }
        [HttpGet( "getCoursesByTeacherID")]
    public ResultEntity getCoursesByTeacherID(int? teacherID)
        {
            ResultEntity resultEntity=new ResultEntity();
        resultEntity.setData(courseService.getCoursesByTeacherID(teacherID));
        resultEntity.setState(resultEntity.getData()!=null?1:0);
        resultEntity.setMessage(resultEntity.getData()!=null?"":"无该老师的课程！");
        return resultEntity;
    }
    [HttpGet( "getStudentsByClassID")]
    public ResultEntity getStudentsByClassID(int? courseClassID)
    {
        ResultEntity resultEntity = new ResultEntity();
        resultEntity.setData(courseService.getStudentsByClassID(courseClassID));
        resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
        resultEntity.setMessage(resultEntity.getData() != null ? "" : "尚无学生进入该班级！");
        return resultEntity;
    }
    [HttpPost( "alertChapterExerciseTitle")]
    public ResultEntity alertChapterExerciseTitle(int? chapterID, String title)
    {
        ResultEntity resultEntity = new ResultEntity();
        ChapterNode temp = courseService.getChapterByID(chapterID);
        if (temp != null)//如果有该章节
        {
            temp.exerciseTitle=(title);
            resultEntity.setData(courseService.addChapter(temp));
            resultEntity.setState(1);
            resultEntity.setMessage("修改成功！");
        }
        else
        {
            resultEntity.setState(0);
            resultEntity.setMessage("该章节不存在！");
        }
        return resultEntity;
    }
    [HttpGet( "getAllCourses")]
    public ResultEntity getAllCourses()
    {
        ResultEntity resultEntity=new ResultEntity();
    resultEntity.setData(courseService.getAllCourses());
        resultEntity.setState(1);
        return resultEntity;
    }
[HttpGet( "getAllCoursesRelation")]
    public ResultEntity getAllCoursesRelation()
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getAllCoursesRelation());
    resultEntity.setState(1);
    return resultEntity;
}
[HttpGet( "getChapterRelationByCourseID")]
    public ResultEntity getChapterRelationByCourseID(int? courseID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getChapterRelationByCourseID(courseID));
    resultEntity.setState(1);
    return resultEntity;
}
[HttpPost( "addCourseName")]
    public ResultEntity addCourseName(String courseName)
{
    ResultEntity resultEntity = new ResultEntity();
    if (courseName != null)
    {
        resultEntity.setData(courseService.addCourseName(courseName));
        resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
        resultEntity.setMessage(resultEntity.getState() == 1 ? "添加课程成功！" : "该课程已经存在！");
        if (resultEntity.getState() == 1)
        {
            CourseName temp = (CourseName)resultEntity.getData();
            courseService.addCourseRelation(temp.courseNameID, 0);
        }
    }
    else
    {
        resultEntity.setState(-1);
        resultEntity.setMessage("课程名不能为空！");
    }
    return resultEntity;
}
[HttpGet( "getCourseList")]
    public ResultEntity getCourseList()
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getCourseList());
    resultEntity.setState(1);
    return resultEntity;
}
[HttpPost( "alertCourseName")]
    public ResultEntity alertCourseName(int? courseNameID, String courseName)
{
    CourseName temp = new CourseName(courseNameID, courseName);
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.alertCourseName(temp));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "修改成功！" : resultEntity.getState() == 0 ? "该课程名已经存在！" : "无该课程信息！");
    return resultEntity;
}
[HttpGet( "getAllCoursesByNameID")]
    public ResultEntity getAllCoursesByNameID(int? courseNameID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getAllCoursesByNameID(courseNameID.ToString()));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
[HttpGet( "addCourseRelation")]
    public ResultEntity addCourseRelation(int? courseNameID, int? preCourseNameID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.addCourseRelation(courseNameID, preCourseNameID));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "新增成功！" : "该关系已经存在！");
    return resultEntity;
}
[HttpGet( "deleteCourseRelation")]
    public ResultEntity deleteCourseRelation(int? courseNameID, int? preCourseNameID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.deleteCourseRelation(courseNameID, preCourseNameID));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "删除成功！" : "删除失败，该关系并不存在！");
    return resultEntity;
}
[HttpGet( "addChapterRelation")]
    public ResultEntity addChapterRelation(int? chapterID, int? preChapterID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.addChapterRelation(chapterID, preChapterID));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "新增成功！" : "该关系已经存在！");
    return resultEntity;
}
[HttpGet( "deleteChapterRelation")]
    public ResultEntity deleteChapterRelation(int? chapterID, int? preChapterID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.deleteChapterRelation(chapterID, preChapterID));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "删除成功！" : "删除失败，该关系并不存在！");
    return resultEntity;
}
[HttpGet( "getStudentNumByTeacher")]
    public ResultEntity getStudentNumByTeacher(int? teacherID)
{
    ResultEntity resultEntity=new ResultEntity();
resultEntity.setData(courseService.getStudentNumByTeacher(teacherID));
        resultEntity.setState(1);
        return resultEntity;
    }
    [HttpGet( "getStudentNumBySemesterAndYear")]
    public ResultEntity getStudentNumBySemesterAndYear(int? year, String semester)
{
    ResultEntity resultEntity=new ResultEntity();
resultEntity.setData(courseService.getStudentNumBySemesterAndYear(year, semester));
        resultEntity.setState(1);
        return resultEntity;
    }
    [HttpGet( "getStudentNumByYear")]
    public ResultEntity getStudentNumByYear(int? year)
{
    ResultEntity resultEntity=new ResultEntity();
resultEntity.setData(courseService.getStudentNumByYear(year));
        resultEntity.setState(1);
        return resultEntity;
    }
    [HttpGet( "getRateBySemesterAndYear")]
    public ResultEntity getRateBySemesterAndYear(String courseNameID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getRateBySemesterAndYear(courseNameID));
    resultEntity.setState(1);
    return resultEntity;
}
[HttpGet( "getClassesByNIDAndTID")]
    public ResultEntity getAllClasses(String courseNameID, int? teacherID)
{
    ResultEntity resultEntity=new ResultEntity();
resultEntity.setData(courseService.getClassesByNIDAndTID(courseNameID, teacherID));
        resultEntity.setState(1);
        return resultEntity;
    }
    [HttpGet( "getTeacherInfoByNID")]
    public ResultEntity getTeacherInfoByNID(String courseNameID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getTeacherListByNID(courseNameID));
    resultEntity.setState(1);
    return resultEntity;
}
    [HttpPost( "addClassComment")]
    public ResultEntity addClassComment(int? courseClassID, int? studentID, String comment, int? rate)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setState(courseService.addClassComment(courseClassID, studentID, comment, rate));
    resultEntity.setMessage(resultEntity.getState() == 1 ? "评论成功！" : "该学生未曾选过这门课！");
    return resultEntity;
}
[HttpGet("getCourseScoreAndCommentByGender")]
    public ResultEntity getChapterScoreAndCommentByGender(int? chapterID, int? getDetail, int? courseClassID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getChapterScoreAndCommentByGender(chapterID, getDetail, courseClassID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
[HttpGet( "getCourseClassAvgScore")]
    public ResultEntity getCourseClassAvgScore(int? courseID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getCourseClassAvgScore(courseID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
[HttpGet( "getCourseYearAvgScoreRate")]
    public ResultEntity getCourseYearAvgScoreRate(int? courseNameID, int? teacherID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getCourseYearAvgScoreRate(courseNameID, teacherID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
[HttpGet( "getCourseClassNLPRateNum")]
    public ResultEntity getCourseClassNLPRateNum(int? courseID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getCourseClassNLPRate(courseID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
[HttpGet( "getChapterNLPRateNum")]
    public ResultEntity getChapterClassNLPRateNum(int? chapterID)
{
    ResultEntity resultEntity = new ResultEntity();
    resultEntity.setData(courseService.getChapterNLPRate(chapterID));
    resultEntity.setState(resultEntity.getData() != null ? 1 : 0);
    return resultEntity;
}
    }
}
