using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using dotnetProject.Entity;

namespace dotnetProject.Services
{
    public interface ExcerciseService
    {
        ResultEntity findOneExerice(int? exerciseId);
        ResultEntity addExercise(Exercise exercise);
        ResultEntity answerAll(List<String> answers, int? studentId, int? chapterId, String type, String comment, int? rate);
        ResultEntity deleteExercise(int? exerciseId);
        ResultEntity alterExercise(Exercise exercise);
        ResultEntity addExerciseChoice(ExerciseChoice exerciseChoice);
        ResultEntity deleteExerciseChoice(int? exerciseChoiceId);
        ResultEntity alterExerciseChoice(ExerciseChoice exerciseChoice);
        ResultEntity findOneAnswer(int? exerciseId, int? studentId);
        ResultEntity findOneAnswerById(int? studentExerciseScoreId);
        ResultEntity answerOne(String answer, int? exerciseId, int? studentId);
        ResultEntity alterAnswer(String answer, int? exerciseId, int? studentId);
        ResultEntity correctOne(int? studentExerciseScoreId, int? score);
        ResultEntity correctAll(List<int?> scores, int? studentId, int? chapterId, String type);
        ResultEntity viewExercise(int? chapterId, String type);
        ResultEntity viewSomeAnswer(int? chapterId, int? studentId, String type);
        ResultEntity rateNumber(int? chapterId);
        List<CourseInfo> findCourses(String courseName, int teacherId);
        List<CourseInfo> findCoursesById(int courseId, int teacherId);
        //List<ChapterNode> copyChapter(int sourceCourseId, int aimCourseId);
        //Boolean copyExercise(int sourceChapterId, int aimChapterId, String type);
        List<int?> exerciseScore(int? studentId, int? chapterId, String type);
        int? calculateScore(int? chapterId, int? studentId);
        List<List<String>> getPrecourse(String courseName);
        List<String> getPrecouseName(String courseName);
        List<String> getCoursesName(List<CourseRelation> courseRelations);
        Boolean learnBad(int studentId, int courseId);
        String getCourseName(int courseId);
        Dictionary<String,float?> userLabel(int studentId);
        List<CourseInfo> currentCourse(int year, String semester);
        List<CourseAndClassList> currentCourseByTeacherId(int teacherId);
        //List<UnratedChapter> getUnratedChapters(int classId);
        List<CourseAndClassList> currentCourseByStudentId(int studentId);
        void setTotalScore(int? chapterId, String type);
    }
}

