using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using dotnetProject.Entity;

namespace dotnetProject.Services
{
    public interface CourseService
    {
        CourseName getCourseNameByNameID(int? courseNameID);
        int? addNewCourse(CourseInfo courseInfo);
        int? addClass(CourseClass courseClass);
        int? alertClassInfo(CourseClass courseClass);
        List<CourseAndClass> getStuCourseList(int? studentID);
        CourseAndClass getCourseByCode(String courseCode);
        int? joinCourse(int? studentID, int? courseID);
        int? addCourseNotice(CourseNotice courseNotice);
        CourseNotice getNoticeByCouID(int? courseID);
        CourseInfo getCourseInfoByID(int? courseID);
        CourseClass getClassInfoByID(int? courseClassID);
        int? deleteCourse(int? courseID);
        int? deleteCourseNotice(int? courseID);
        ChapterNode addChapter(ChapterNode chapterNode);
        ChapterNode getChapterByID(int? chapterID);
        List<CourseCatalog> getCourseCatalog(int? courseID);
        Dictionary<string, Object> getCourseClassNLPRate(int? courseID);
        Dictionary<string, Object> getCourseClassAvgScore(int? courseID);
        Dictionary<string, Object> getChapterNLPRate(int? chapterID);
        List<Dictionary<string, Object>> getChapterScoreAndCommentByGender(int? chapterID, int? getDetail, int? courseClassID);
        List<StudentChapterEntity> getCourseScoreAndComment(int? courseID, int? studentID);
        List<CourseClass> getClassesByCourseID(int? courseID);
        
        ChapterNode getCurrentProgress(int? courseID, int? studentID);
        int? alertCurrentProgress(int? courseID, int? studentID, int? chapterID);
        int? deleteClass(int? courseClassID);
        List<CourseAndClass> getCoursesByTeacherID(int? teacherID);
        List<UserInfo> getStudentsByClassID(int? courseClassId);
        List<CourseInfo> getAllCourses();
        List<CourseRelationEntity> getAllCoursesRelation();
        List<ChapterRelationEntity> getChapterRelationByCourseID(int? courseID);
        CourseName addCourseName(String courseName);
        List<CourseName> getCourseList();
        int? alertCourseName(CourseName courseName);
        List<CourseAndClassList> getAllCoursesByNameID(String nameID);
        int? addCourseRelation(int? courseNameID, int? preCourseNameID);
        int? deleteCourseRelation(int? courseNameID, int? preCourseNameID);
        int? addChapterRelation(int? chapterID, int? preChapterID);
        int? deleteChapterRelation(int? chapterID, int? preChapterID);
        Dictionary<String, int?> getStudentNumByTeacher(int? teacherID);

        Dictionary<String, int?> getStudentNumBySemesterAndYear(int? year, String semester);
        Dictionary<String, int?> getStudentNumByYear(int? year);
        List<Dictionary<string,Object>> getRateBySemesterAndYear(String courseName);
        List<Dictionary<string,Object>> getClassesByNIDAndTID(String courseNameID, int? teacherID);
        List<Dictionary<string,Object>> getTeacherListByNID(String courseNameID);
        int? addClassComment(int? courseClassID, int? studentID, String comment, int? rate);
        Dictionary<String, Dictionary<String, Dictionary<String, Object>>> getCourseYearAvgScoreRate(int? courseNameID, int? teacherID);
        void deleteChapter(CourseCatalog courseCatalog);

        
    }
}
