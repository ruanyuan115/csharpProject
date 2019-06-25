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
    [Route("/question")]

    public class ExerciseController : Controller
    {
        private readonly ExcerciseService _exerciseService;

        public String tokenHeader = "Authorization";

        public ExerciseController(ExcerciseService excerciseService)
        {
            _exerciseService = excerciseService;
        }

        [HttpGet("/findOneExercise")]
    public ResultEntity findOneExercise(int? exerciseId)
        {
            return _exerciseService.findOneExerice(exerciseId);
        }

        [HttpPost("/addExercise")]
    public ResultEntity addExercise(Exercise exercise)
        {
            return _exerciseService.addExercise(exercise);
        }


        [HttpPost("/alterExercise")]
    public ResultEntity alterExercise(Exercise exercise)
        {
            return _exerciseService.alterExercise(exercise);
        }

        [HttpPost("/addChoice")]
    public ResultEntity addChoice(ExerciseChoice exerciseChoice)
        {
            return _exerciseService.addExerciseChoice(exerciseChoice);
        }


        [HttpPost("/alterChoice")]
    public ResultEntity alterChoice(ExerciseChoice exerciseChoice)
        {
            return _exerciseService.alterExerciseChoice(exerciseChoice);
        }

        [HttpGet("/findOneAnswer")]
    public ResultEntity findOneAnswer(int? exerciseId, int? studentId)
        {
            return _exerciseService.findOneAnswer(exerciseId, studentId);
        }

        [HttpGet("/findOneAnswerById")]
    public ResultEntity findOneAnswerById(int? studentExerciseScoreId)
        {
            return _exerciseService.findOneAnswerById(studentExerciseScoreId);
        }

        [HttpPost("/addAnswer")]
    public ResultEntity addAnswer(String answer, int? exerciseId, int? userId)
        {
            return _exerciseService.answerOne(answer, exerciseId, userId);
        }

        [HttpPost("/answerAll")]
    public ResultEntity answerAll(List<String> answers, int? studentId, int? chapterId, String type, String comment, int? rate)
        {
        return _exerciseService.answerAll(answers, studentId, chapterId, type, comment, rate);
    }

    [HttpPut( "/alterAnswer")]
    public ResultEntity alterAnswer(String answer, int? exerciseId, int? studentId)
    {
        return _exerciseService.alterAnswer(answer, exerciseId, studentId);
    }

    [HttpPut( "/correctOne")]
    public ResultEntity correctOne(int? studentExerciseScoreId, int? score)
    {
        return _exerciseService.correctOne(studentExerciseScoreId, score);
    }

    [HttpPost( "/correctAll")]
    public ResultEntity correctAll(List<int?> scores, int? studentId,int? chapterId, String type){
        return _exerciseService.correctAll(scores, studentId, chapterId, type);
    }

[HttpGet( "/view")]
    public ResultEntity viewExercise(int? chapterId, String type)
{
    return _exerciseService.viewExercise(chapterId, type);
}

[HttpGet("/viewSomeAnswer")]
    public ResultEntity viewSomeAnswer(int? chapterId, int? studentId, String type)
{
    return _exerciseService.viewSomeAnswer(chapterId, studentId, type);
}

[HttpGet( "/getScore")]
    public ResultEntity getScore(int? chapterId, int? studentId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (chapterId != null && studentId != null)
    {
        resultEntity.setData(_exerciseService.calculateScore(chapterId, studentId));
        resultEntity.setState(1);
        resultEntity.setMessage("查看成功！");
    }
    else
    {
        resultEntity.setMessage("有输入为空！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet( "/rateNumber")]
    public ResultEntity rateNumber(int? chapterId)
{
    return _exerciseService.rateNumber(chapterId);
}

[HttpGet("/sameCoursesByName")]
    public ResultEntity sameCoursesByName(String courseName, int teacherId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (courseName != null && teacherId != null)
    {
        List<CourseInfo> courseInfos = _exerciseService.findCourses(courseName, teacherId);
        if (courseInfos.Count==0)
        {
            resultEntity.setMessage("未找到对应课程！");
            resultEntity.setState(0);
        }
        else
        {
            resultEntity.setData(courseInfos);
            resultEntity.setState(1);
        }
    }
    else
    {
        resultEntity.setMessage("传入参数为空！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet("/sameCoursesById")]
    public ResultEntity sameCoursesById(int courseId, int teacherId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (courseId != null && teacherId != null)
    {
        List<CourseInfo> courseInfos = _exerciseService.findCoursesById(courseId, teacherId);
        if (courseInfos.Count==0)
        {
            resultEntity.setMessage("未找到对应课程！");
            resultEntity.setState(0);
        }
        else
        {
            resultEntity.setData(courseInfos);
            resultEntity.setState(1);
        }
    }
    else
    {
        resultEntity.setMessage("传入参数为空！");
        resultEntity.setState(0);
    }
    return resultEntity;
}


[HttpGet("/exerciseScore")]
    public ResultEntity exerciseScore(int studentId, int chapterId, String type)
{
    ResultEntity resultEntity = new ResultEntity();
    if (studentId != null && chapterId != null && type != null)
    {
        List<int?> temp = _exerciseService.exerciseScore(studentId, chapterId, type);
        if (temp != null)
        {
            resultEntity.setData(temp);
            resultEntity.setState(1);
        }
        else
        {
            resultEntity.setMessage("学生未答题");
            resultEntity.setState(0);
        }
    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet("/getPrecourse")]
    public ResultEntity getPrecourse(int courseId, int studentId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (courseId != null && studentId != null)
    {
        if (_exerciseService.learnBad(studentId, courseId))
        {
            List<List<String>> temp = _exerciseService.getPrecourse(_exerciseService.getCourseName(courseId));
            if (temp != null)
            {
                resultEntity.setData(temp);
                resultEntity.setState(1);
                resultEntity.setMessage("你需要学习一些前置课程");
            }
            else
            {
                resultEntity.setMessage("未找到对应课程！");
                resultEntity.setState(0);
            }
        }
        else
        {
            resultEntity.setMessage("你此课程最近学习状况尚可！");
            resultEntity.setState(1);
        }

    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet("/userLabel")]
    public ResultEntity userLabel(int? studentId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (studentId != null)
    {
        resultEntity.setState(1);
        resultEntity.setData(_exerciseService.userLabel(studentId.Value));
    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet(("/currentCourse")]
    public ResultEntity currentCourse(int year, String semester)
{
    ResultEntity resultEntity = new ResultEntity();
    if (year != null && semester != null)
    {
        List<CourseInfo> courseInfos = _exerciseService.currentCourse(year, semester);
        if (courseInfos.Count!=0)
        {
            resultEntity.setState(1);
            resultEntity.setData(courseInfos);
        }
        else
        {
            resultEntity.setState(0);
            resultEntity.setMessage("未找到相应课程");
        }
    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet("/getUnratedChapters")]
    public ResultEntity getUnratedChapters(int? classId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (classId != null)
    {
        List<UnratedChapter> unratedChapters = _exerciseService.getUnratedChapters(classId.Value);
        if (unratedChapters.Count!=0)
        {
            resultEntity.setState(1);
            resultEntity.setData(unratedChapters);
        }
        else
        {
            resultEntity.setState(0);
            resultEntity.setMessage("未找到相应章节");
        }
    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet("/currentCourseByTeacherId")]
    public ResultEntity currentCourseByTeacherId(int? teacherId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (teacherId != null)
    {
        List<CourseAndClassList> courseAndClassLists = _exerciseService.currentCourseByTeacherId(teacherId.Value);
        if (courseAndClassLists.Count !=0)
        {
            resultEntity.setState(1);
            resultEntity.setData(courseAndClassLists);
        }
        else
        {
            resultEntity.setState(0);
            resultEntity.setMessage("未找到相应课程");
        }
    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}

[HttpGet("/currentCourseByStudentId")]
    public ResultEntity currentCourseByStudentId(int? studentId)
{
    ResultEntity resultEntity = new ResultEntity();
    if (studentId != null)
    {
        List<CourseAndClassList> courseAndClassLists = _exerciseService.currentCourseByStudentId(studentId.Value);
        if (courseAndClassLists.Count!=0)
        {
            resultEntity.setState(1);
            resultEntity.setData(courseAndClassLists);
        }
        else
        {
            resultEntity.setState(0);
            resultEntity.setMessage("未找到相应课程");
        }
    }
    else
    {
        resultEntity.setMessage("传入参数不全！");
        resultEntity.setState(0);
    }
    return resultEntity;
}


    }
}
