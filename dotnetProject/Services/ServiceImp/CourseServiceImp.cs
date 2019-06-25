using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using PersistentLayer.Apis;
using dotnetProject.Entity;
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        CourseInfoApi.delete(courseID);
        return 1;
    }
    else
        return 0;
}



    public int? deleteCourseNotice(int? courseID)
{
    if (CourseNoticeApi.findByCourseID(courseID) != null)
    {
        CourseNoticeApi.deleteByCourseID(courseID);
        return 1;
    }
    else
        return 0;
}



    public ChapterNode addChapter(ChapterNode chapterNode)
{
    if (chapterNode != null)
    {
        ChapterContentApi.insert(chapterNode);
        ChapterNode temp = ChapterContentApi.findByCourseIDAndParentIDAndSiblingID(chapterNode.courseID,chapterNode.parentID, chapterNode.siblingID);
        return temp != null ? (temp.id != null ? temp : null) : null;
    }
    else
        return null;
}


    public ChapterNode getChapterByID(int? chapterID)
{
    ChapterNode temp = ChapterContentApi.getByID(chapterID);
    return temp!=null?temp:null;
}


    public List<CourseCatalog> getCourseCatalog(int? courseID)
{
    List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseID(courseID);
    if (chapterNodes != null && chapterNodes.Count() > 0)
    {
        CourseCatalog bookCatalog = new CourseCatalog();
        ChapterNode book = new ChapterNode();
        book.id=0;
        bookCatalog.setChapterNode(book);

        makeCatalog(bookCatalog, chapterNodes);
        return bookCatalog.subCatalog;
    }
    else
        return null;
}
public void getSubNodes(CourseCatalog courseCatalog, List<ChapterNode> chapterNodes)
{
    int? parentID = courseCatalog.id;
    int? siblingID = 0;
    for (int i =0;i< chapterNodes.Count(); i++)//如果还有没被加入的节点
    {
        ChapterNode temp = chapterNodes[i];
        if (temp.parentID==parentID && temp.siblingID==siblingID)//如果该节点符合要求
        { 
            CourseCatalog subCatalog = new CourseCatalog();
            subCatalog.setChapterNode(temp);

            courseCatalog.subCatalog.Add(subCatalog);

            chapterNodes.Remove(temp);        //将该节点从数组中移除
            siblingID = temp.id;
            i = -1;       //再次遍历获取兄弟节点
        }
    }
}
private void makeCatalog(CourseCatalog courseCatalog, List<ChapterNode> chapterNodes)
{
    getSubNodes(courseCatalog, chapterNodes);    //先获取该节点的子节点 再递归操作获取子子节点
    if(courseCatalog.subCatalog.Count>0)
       foreach (var i in courseCatalog.subCatalog)
         if (chapterNodes.Count() != 0)
         {
             makeCatalog(i, chapterNodes);
         }
        }
        public List<StudentChapterEntity> getCourseScoreAndComment(int? courseID, int? studentID)
        {
            List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseIDAndParentID(courseID, 0);
            UserInfo userTemp = UserApi.getByID(studentID);
            if (chapterNodes != null && chapterNodes.Count() > 0 && userTemp != null)
            {
                CourseCatalog bookCatalog = new CourseCatalog();
                ChapterNode book = new ChapterNode();
                book.id = (0);
                bookCatalog.setChapterNode(book);

                getSubNodes(bookCatalog, chapterNodes);

                List<StudentChapterEntity> arrayList = new List<StudentChapterEntity>();
                foreach (CourseCatalog i in bookCatalog.subCatalog)
                {
                    StudentChapter temp = StudentChapterApi.findByChapterIDAndStudentID(i.id, studentID);
                    if (temp != null)
                    {
                        StudentChapterEntity tempEntity = new StudentChapterEntity(temp, getChapterByID(temp.chapterID).contentName);
                        arrayList.Add(tempEntity);
                    }
                }
                return arrayList.Count() > 0 ? arrayList : null;
            }
            else
                return null;
        }
        public List<CourseClass> getClassesByCourseID(int? courseID)
        {
            if (CourseInfoApi.findByCourseID(courseID) != null)
            {
                return CourseClassApi.findByCourseID(courseID);
            }
            else
                return null;
        }
        //n
        public List<UserInfo> getStudentsByClassID(int? courseClassId)
        {
            List<Takes> takes = TakesApi.findByCourseClassID(courseClassId);
            List<UserInfo> userInfos = new List<UserInfo>();
            if (takes.Count() > 0)
            {
                foreach (Takes i in takes)
                {
                    if (i.studentID != null)
                        userInfos.Add(UserApi.getByID(i.studentID));
                }
                return userInfos;
            }
            else
                return null;
        }
        public Dictionary<string, Object> getCourseClassNLPRate(int? courseID)
        {
            List<CourseClass> courseClasses = getClassesByCourseID(courseID);
            List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseIDAndParentID(courseID, 0);
            if (courseClasses != null && courseClasses.Count() > 0)
            {
                int studentNum = 0;
                int positiveNum = 0;
                int negativeNum = 0;
                List<Dictionary<string, Object>> classInfoMap = new List<Dictionary<string, Object>>();
                foreach (CourseClass i in courseClasses)
                {
                    int classStudentNum = 0;
                    int classPositiveNum = 0;
                    int classNegativeNum = 0;
                    List<UserInfo> students = getStudentsByClassID(i.id);
                    if (students != null && students.Count() > 0)
                    {
                        foreach (UserInfo u in students)
                        {
                            float? NLPRateSum = 0;
                            int NLPRateNum = 0;
                            foreach (ChapterNode c in chapterNodes)
                            {
                                String tempStr = StudentChapterApi.getNLPRateByChapterIDAndStudentID(c.id, u.userID);
                                if (tempStr != null)
                                {
                                    float? temp = float.Parse(tempStr);
                                    NLPRateSum += temp;
                                    NLPRateNum += 1;
                                }
                            }
                            classStudentNum++;
                            if (NLPRateNum != 0)
                            {
                                classPositiveNum += NLPRateSum >= 0 ? 1 : 0;
                                classNegativeNum += NLPRateSum < 0 ? 1 : 0;
                            }
                        }
                    }
                    studentNum += classStudentNum;
                    positiveNum += classPositiveNum;
                    negativeNum += classNegativeNum;
                    Dictionary<string, Object>classMap = new Dictionary<string, object>();
                    classMap.Add("classNum", i.classNum);
                    classMap.Add("classStudentNum", classStudentNum);
                    classMap.Add("classPositiveNum", classPositiveNum);
                    classMap.Add("classNegativeNum", classNegativeNum);
                    classInfoMap.Add(classMap);
                }
                Dictionary<string, Object> resultMap = new Dictionary<string, object>();
                resultMap.Add("studentNum", studentNum);
                resultMap.Add("positiveNum", positiveNum);
                resultMap.Add("negativeNum", negativeNum);
                resultMap.Add("classInfo", classInfoMap);

                return resultMap;
            }
            else
                return null;
        }

        //加索引->Druid监控发现开了事务->关闭事务快了1/3->mybatis批量查询->关闭sql日志
        public Dictionary<string, Object> getCourseClassAvgScore(int? courseID)
        {
            List<CourseClass> courseClasses = getClassesByCourseID(courseID);
            List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseIDAndParentID(courseID, 0);

            if (courseClasses != null && courseClasses.Count() > 0)
            {
                int courseBoyNum = 0;
                int courseGirlNum = 0;
                float courseBoyScoreSum1 = 0;
                int courseBoyScoreNum1 = 0;
                float courseBoyScoreSum2 = 0;
                int courseBoyScoreNum2 = 0;
                float courseGirlScoreSum1 = 0;
                int courseGirlScoreNum1 = 0;
                float courseGirlScoreSum2 = 0;
                int courseGirlScoreNum2 = 0;
                float courseBoyScoreSum = 0;
                int courseBoyScoreNum = 0;
                float courseGirlScoreSum = 0;
                int courseGirlScoreNum = 0;
                float courseBoyRateSum = 0;
                int courseBoyRateNum = 0;
                float courseGirlRateSum = 0;
                int courseGirlRateNum = 0;

                List<int?> courseBoyScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                List<int?> courseGirlScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                List<int?> courseBoyScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                List<int?> courseGirlScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                List<int?> courseBoyScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                List<int?> courseGirlScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });

                List<int?> courseBoyRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                List<int?> courseGirlRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });

                List<Dictionary<string, Object>> resultDictionary = new List<Dictionary<string, Object>>();

                List<Dictionary<string, int?>> tempList = new List<Dictionary<string, int?>>();
                Dictionary< int ?, List < UserInfo >> classStudentDictionary = new Dictionary<int?, List<UserInfo>>();
                foreach (CourseClass i in courseClasses)
                {
                    List<UserInfo> students = getStudentsByClassID(i.id);
                    classStudentDictionary.Add(i.classNum, students);
                    if (students != null)
                        foreach (UserInfo u in students)
                        {
                            foreach (ChapterNode c in chapterNodes)
                            {
                                Dictionary< String, int?> tempDictionary= new Dictionary<string, int?>();
                                tempDictionary.Add("chapterID", c.id);
                                tempDictionary.Add("studentID", u.userID);
                                tempList.Add(tempDictionary);
                            }
                        }
                }
                List<StudentScoreRate> tempScoreRate = tempList.Count() > 0 ? StudentScoreRateApi.getStudentScoreRate(tempList) : new List<StudentScoreRate>();
                Dictionary< int?, List < StudentScoreRate >> studentScoreDictionary = new Dictionary<int?, List<StudentScoreRate>>();
                foreach (StudentScoreRate i in tempScoreRate)
                {
                    if (!studentScoreDictionary.ContainsKey(i.studentID))
                        studentScoreDictionary.Add(i.studentID, new List<StudentScoreRate>());
                    studentScoreDictionary[i.studentID].Add(i);
                }
                foreach (CourseClass i in courseClasses)
                {
                    int boyNum = 0;
                    int girlNum = 0;
                    float boyScoreSum1 = 0;      //男生各个章节课前成绩求和
                    int boyScoreNum1 = 0;
                    float boyScoreSum2 = 0;      //男生各个章节课后成绩求和
                    int boyScoreNum2 = 0;
                    float girlScoreSum1 = 0;      //女生各个章节课前成绩求和
                    int girlScoreNum1 = 0;
                    float girlScoreSum2 = 0;      //女生各个章节课后成绩求和
                    int girlScoreNum2 = 0;
                    float boyScoreSum = 0;       //男生各个章节课前课后平均成绩求和
                    int boyScoreNum = 0;
                    float girlScoreSum = 0;       //女生各个章节课前课后平均成绩求和
                    int girlScoreNum = 0;
                    float boyRateSum = 0;        //男生各个章节评分求和
                    int boyRateNum = 0;
                    float girlRateSum = 0;        //女生各个章节评分求和
                    int girlRateNum = 0;
                    List<int?> boyScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<int?> girlScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<int?> boyScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<int?> girlScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<int?> boyScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<int?> girlScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });

                    List<int?> boyRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<int?> girlRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0 });
                    List<UserInfo> students = classStudentDictionary[i.classNum];
                    if (students != null)
                    {
                        foreach (UserInfo u in students)
                        {
                            float studentScoreSum1 = 0;
                            int studentScoreNum1 = 0;
                            float studentScoreSum2 = 0;
                            int studentScoreNum2 = 0;
                            float studentScoreSum = 0;
                            int studentScoreNum = 0;
                            float studentRateSum = 0;
                            int studentRateNum = 0;

                            List<StudentScoreRate> temp = studentScoreDictionary[u.userID];
                            if (temp == null)
                                continue;
                            foreach (StudentScoreRate s in temp)
                            {
                                if (s.totalScore_1!= null)
                                {
                                    studentScoreSum1 += (float)s.totalScore_1;
                                    studentScoreNum1 += 1;
                                }
                                if (s.totalScore_2!= null)
                                {
                                    studentScoreSum2 += (float)s.totalScore_2;
                                    studentScoreNum2 += 1;
                                }
                                if (s.rate != null)
                                {
                                    studentRateSum += (float)s.rate;
                                    studentRateNum += 1;
                                }
                                if (s.totalScore_1!= null && s.totalScore_2!= null)
                                {
                                    studentScoreSum += (float)(s.totalScore_1 + s.totalScore_2) / 2;
                                    studentScoreNum += 1;
                                }
                            }
                            int indexScore1 = (int)(studentScoreSum1 / studentScoreNum1 / 10 < 6 ? 0 : studentScoreSum1 / studentScoreNum1 == 100 ? 4 : studentScoreSum1 / studentScoreNum1 / 10 - 5);
                            int indexScore2 = (int)(studentScoreSum2 / studentScoreNum2 / 10 < 6 ? 0 : studentScoreSum2 / studentScoreNum2 == 100 ? 4 : studentScoreSum2 / studentScoreNum2 / 10 - 5);
                            int indexScore = (int)(studentScoreSum / studentScoreNum / 10 < 6 ? 0 : studentScoreSum / studentScoreNum == 100 ? 4 : studentScoreSum / studentScoreNum / 10 - 5);
                            int indexRate = (int)(studentRateSum / studentRateNum == 5 ? 4 : studentRateSum / studentRateNum);
                            if (u.gender == ("男"))
                            {
                                boyNum += 1;
                                courseBoyNum += 1;
                                if (studentScoreSum1 != 0)
                                {
                                    boyScoreSum1 += studentScoreSum1 / studentScoreNum1;
                                    boyScoreNum1 += 1;
                                    courseBoyScoreSum1 += studentScoreSum1 / studentScoreNum1;
                                    courseBoyScoreNum1 += 1;
                                }
                                if (studentScoreSum2 != 0)
                                {
                                    boyScoreSum2 += studentScoreSum2 / studentScoreNum2;
                                    boyScoreNum2 += 1;
                                    courseBoyScoreSum2 += studentScoreSum2 / studentScoreNum2;
                                    courseBoyScoreNum2 += 1;
                                }
                                if (studentScoreSum != 0)
                                {
                                    boyScoreSum += studentScoreSum / studentScoreNum;
                                    boyScoreNum += 1;
                                    courseBoyScoreSum += studentScoreSum / studentScoreNum;
                                    courseBoyScoreNum += 1;
                                }
                                if (studentRateSum != 0)
                                {
                                    boyRateSum += studentRateSum / studentRateNum;
                                    boyRateNum += 1;
                                    courseBoyRateSum += studentRateSum / studentRateNum;
                                    courseBoyRateNum += 1;
                                }
                                boyScore1[indexScore1] = boyScore1[indexScore1] + 1;
                                boyScore2[indexScore2] = boyScore2[indexScore2] + 1;
                                boyScoreAvgDis[indexScore] = boyScoreAvgDis[indexScore] + 1;
                                boyRateDis[indexRate] = boyRateDis[indexRate] + 1;

                                courseBoyScore1[indexScore1] = courseBoyScore1[indexScore1] + 1;
                                courseBoyScore2[indexScore2] = courseBoyScore2[indexScore2] + 1;
                                courseBoyScoreAvgDis[indexScore] = courseBoyScoreAvgDis[indexScore] + 1;
                                courseBoyRateDis[indexRate] = courseBoyRateDis[indexRate] + 1;
                            }
                            else
                            {
                                courseGirlNum += 1;
                                girlNum += 1;
                                if (studentScoreSum1 != 0)
                                {
                                    girlScoreSum1 += studentScoreSum1 / studentScoreNum1;
                                    girlScoreNum1 += 1;
                                    courseGirlScoreSum1 += studentScoreSum1 / studentScoreNum1;
                                    courseGirlScoreNum1 += 1;
                                }
                                if (studentScoreSum2 != 0)
                                {
                                    girlScoreSum2 += studentScoreSum2 / studentScoreNum2;
                                    girlScoreNum2 += 1;
                                    courseGirlScoreSum2 += studentScoreSum2 / studentScoreNum2;
                                    courseGirlScoreNum2 += 1;
                                }
                                if (studentScoreSum != 0)
                                {
                                    girlScoreSum += studentScoreSum / studentScoreNum;
                                    girlScoreNum += 1;
                                    courseGirlScoreSum += studentScoreSum / studentScoreNum;
                                    courseGirlScoreNum += 1;
                                }
                                if (studentRateSum != 0)
                                {
                                    girlRateSum += studentRateSum / studentRateNum;
                                    girlRateNum += 1;
                                    courseGirlRateSum += studentRateSum / studentRateNum;
                                    courseGirlRateNum += 1;
                                }
                                girlScore1[indexScore1] = girlScore1[indexScore1] + 1;
                                girlScore2[indexScore2] = girlScore2[indexScore2] + 1;
                                girlScoreAvgDis[indexScore] = girlScoreAvgDis[indexScore] + 1;
                                girlRateDis[indexRate] = girlRateDis[indexRate] + 1;

                                courseGirlScore1[indexScore1] = courseGirlScore1[indexScore1] + 1;
                                courseGirlScore2[indexScore2] = courseGirlScore2[indexScore2] + 1;
                                courseGirlScoreAvgDis[indexScore] = courseGirlScoreAvgDis[indexScore] + 1;
                                courseGirlRateDis[indexRate] = courseGirlRateDis[indexRate] + 1;
                            }
                        }
                    }
                    Dictionary< String, Object > classMap = new Dictionary<string, object>();


                    classMap.Add("classNum", i.id);
                    classMap.Add("boyNum", boyNum);
                    classMap.Add("girlNum", girlNum);
                    classMap.Add("boyAvgScore1", boyScoreSum1 == 0 ? 0 : boyScoreSum1 / boyScoreNum1);
                    classMap.Add("boyAvgScore2", boyScoreSum2 == 0 ? 0 : boyScoreSum2 / boyScoreNum2);
                    classMap.Add("boyAvgScore", boyScoreSum == 0 ? 0 : boyScoreSum / boyScoreNum);
                    classMap.Add("girlAvgScore1", girlScoreSum1 == 0 ? 0 : girlScoreSum1 / girlScoreNum1);
                    classMap.Add("girlAvgScore2", girlScoreSum2 == 0 ? 0 : girlScoreSum2 / girlScoreNum2);
                    classMap.Add("girlAgeScore", girlScoreSum == 0 ? 0 : girlScoreSum / girlScoreNum);
                    classMap.Add("boyAvgRate", boyRateSum == 0 ? 0 : boyRateSum / boyRateNum);
                    classMap.Add("girlAvgRate", girlRateSum == 0 ? 0 : girlRateSum / girlRateNum);
                    classMap.Add("boyScoreDis1", boyScore1);
                    classMap.Add("boyScoreDis2", boyScore2);
                    classMap.Add("boyScoreDis", boyScoreAvgDis);
                    classMap.Add("girlScoreDis1", girlScore1);
                    classMap.Add("girlScoreDis2", girlScore2);
                    classMap.Add("girlScoreDis", girlScoreAvgDis);

                    resultDictionary.Add(classMap);
                }
                Dictionary< String, Object > infoMap = new Dictionary<string, object>();
                infoMap.Add("courseBoyNum", courseBoyNum);
                infoMap.Add("courseGirlNum", courseGirlNum);
                infoMap.Add("courseBoyAvgScore1", courseBoyScoreSum1 == 0 ? 0 : courseBoyScoreSum1 / courseBoyScoreNum1);
                infoMap.Add("courseBoyAvgScore2", courseBoyScoreSum2 == 0 ? 0 : courseBoyScoreSum2 / courseBoyScoreNum2);
                infoMap.Add("courseBoyAvgScore", courseBoyScoreSum == 0 ? 0 : courseBoyScoreSum / courseBoyScoreNum);
                infoMap.Add("courseGirlAvgScore1", courseGirlScoreSum1 == 0 ? 0 : courseGirlScoreSum1 / courseGirlScoreNum1);
                infoMap.Add("courseGirlAvgScore2", courseGirlScoreSum2 == 0 ? 0 : courseGirlScoreSum2 / courseGirlScoreNum2);
                infoMap.Add("courseGirlAgeScore", courseGirlScoreSum == 0 ? 0 : courseGirlScoreSum / courseGirlScoreNum);
                infoMap.Add("courseBoyAvgRate", courseBoyRateSum == 0 ? 0 : courseBoyRateSum / courseBoyRateNum);
                infoMap.Add("courseGirlAvgRate", courseGirlRateSum == 0 ? 0 : courseGirlRateSum / courseGirlRateNum);
                infoMap.Add("courseBoyScoreDis1", courseBoyScore1);
                infoMap.Add("courseBoyScoreDis2", courseBoyScore2);
                infoMap.Add("courseBoyScoreDis", courseBoyScoreAvgDis);
                infoMap.Add("courseGirlScoreDis1", courseGirlScore1);
                infoMap.Add("courseGirlScoreDis2", courseGirlScore2);
                infoMap.Add("courseGirlScoreDis", courseGirlScoreAvgDis);
                infoMap.Add("classInfo", resultDictionary);
                return infoMap;
            }
            else
                return null;
        }


        public Dictionary<string, Object> getChapterNLPRate(int? chapterID)
        {
            ChapterNode chapterNode = ChapterContentApi.getByID(chapterID);
            if (chapterNode==null)
                return null;
            Dictionary< String, Object > resultMap = new Dictionary<string, object>();
            List<CourseClass> classesTemp = getClassesByCourseID(chapterNode.courseID);
            if (classesTemp != null && classesTemp.Count() != 0)
            {
                int studentNum = 0;
                int positiveNum = 0;
                int negativeNum = 0;
                List<Dictionary<string, int?>> classInfoMap = new List<Dictionary<string, int?>>();
                foreach (CourseClass i in classesTemp)
                {
                    int classStudentNum = 0;
                    int classPositiveNum = 0;
                    int classNegativeNum = 0;
                    List<UserInfo> students = getStudentsByClassID(i.id);
                    if (students != null && students.Count() > 0)
                    {
                        foreach (UserInfo u in students)
                        {
                            classStudentNum++;
                            String tempStr = StudentChapterApi.getNLPRateByChapterIDAndStudentID(chapterID, u.userID);
                            if (tempStr != null)
                            {
                                float? temp = float.Parse(tempStr);
                                classPositiveNum += temp >= 0 ? 1 : 0;
                                classNegativeNum += temp < 0 ? 1 : 0;
                            }
                        }
                    }
                    studentNum += classStudentNum;
                    positiveNum += classPositiveNum;
                    negativeNum += classNegativeNum;
                    Dictionary< String, int?> classMap = new Dictionary<string, int?>();
                    classMap.Add("classNum", i.classNum);
                    classMap.Add("classStudentNum", classStudentNum);
                    classMap.Add("classPositiveNum", classPositiveNum);
                    classMap.Add("classNegativeNum", classNegativeNum);
                    classInfoMap.Add(classMap);
                }
                resultMap.Add("studentNum", studentNum);
                resultMap.Add("positiveNum", positiveNum);
                resultMap.Add("negativeNum", negativeNum);
                resultMap.Add("classInfo", classInfoMap);
            }
            return resultMap;
        }


        public List<Dictionary<string, Object>> getChapterScoreAndCommentByGender(int? chapterID, int? getDetail, int? courseClassID)
        {
            //获取章节信息后 获取班级信息 然后获取学生在该章的信息(studentChapter) 遍历班级查找该班学生的性别 分类计算均值

            ChapterNode chapterNode = ChapterContentApi.getByID(chapterID);
            if (chapterNode != null)
            {
                List<Dictionary<string, Object>> resultMap = new List<Dictionary<string, Object>>();
                List<CourseClass> classesTemp = new List<CourseClass>();

                if (courseClassID == null || courseClassID == 0)
                    classesTemp = getClassesByCourseID(chapterNode.courseID);
                else if (courseClassID > 0)
                {
                    CourseClass classTemp = CourseClassApi.getByID(courseClassID);
                    if (classTemp!=null)
                        classesTemp.Add(classTemp);
                }
                List<StudentChapter> tempList = StudentChapterApi.findByChapterID(chapterNode.id);//该章节下所有的学生成绩

                foreach (CourseClass c in classesTemp)
                {
                    Dictionary< String, Object > classMap = new Dictionary<string, object>();
                    List<UserInfo> classStudents = getStudentsByClassID(c.id);
                    List<int?> studentIDs = new List<int?>();
                    foreach (UserInfo u in classStudents)
                    {
                        studentIDs.Add(u.userID);
                    }

                    Dictionary< String, Object > chapterMap = new Dictionary<string, object>();
                    chapterMap.Add("chapterName", chapterNode.contentName);
                    chapterMap.Add("chapterID", chapterNode.id);
                    chapterMap.Add("siblingID", chapterNode.siblingID);

                    if (tempList != null && tempList.Count() != 0)
                    {
                        List<StudentChapter> boysList = new List<StudentChapter>();
                        List<StudentChapter> girlsList = new List<StudentChapter>();

                        List<int?> boyScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> girlScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> totalScore1 = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> totalScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> boyScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> girlScore2 = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> boyScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> girlScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> totalScoreAvgDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });

                        List<int?> boyRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> girlRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });
                        List<int?> totalRateDis = new List<int?>(new int?[] { 0, 0, 0, 0, 0, 0 });

                        int? boyNum = 0;
                        int? girlNum = 0;
                        int? totalNum = 0;
                        float boySum1 = 0;
                        int? boyNum1 = 0;
                        float boySum2 = 0;
                        int? boyNum2 = 0;
                        float girlSum1 = 0;
                        int? girlNum1 = 0;
                        float girlSum2 = 0;
                        int? girlNum2 = 0;
                        float boyRateSum = 0;
                        int? boyRateNum = 0;
                        float girlRateSum = 0;
                        int? girlRateNum = 0;

                        for (int j = tempList.Count() - 1; j >= 0; j--)                                             //性别筛选
                        {
                            if (studentIDs.Contains(tempList[j].studentID))                   //如果在这个班
                            {
                                totalNum += 1;
                                int? index1 = tempList[j].totalScore_1 == null ? 5 : tempList[j].totalScore_1 / 10 < 6 ? 0 : tempList[j].totalScore_1 / 10 == 10 ? 4 : tempList[j].totalScore_1 / 10 - 5;
                                int? index2 = tempList[j].totalScore_2 == null ? 5 : tempList[j].totalScore_2 / 10 < 6 ? 0 : tempList[j].totalScore_2 / 10 == 10 ? 4 : tempList[j].totalScore_2 / 10 - 5;
                                int? rateIndex = tempList[j].rate == null ? 5 : tempList[j].rate == 5 ? 4 : tempList[j].rate;
                                int? avgScoreIndex = 5;
                                if (tempList[j].totalScore_1 != null && tempList[j].totalScore_2 != null)
                                {
                                    int? temp = (tempList[j].totalScore_1 + tempList[j].totalScore_2) / 2;
                                    avgScoreIndex = temp / 10 < 6 ? 0 : temp / 10 == 10 ? 4 : temp / 10 - 5;
                                }

                                if (classStudents[studentIDs.IndexOf(tempList[j].studentID)].gender==("男"))
                                {
                                    boysList.Add(tempList[j]);
                                    boyNum += 1;
                                    if (tempList[j].totalScore_1 != null)
                                    {
                                        boySum1 += (float)tempList[j].totalScore_1;
                                        boyNum1 += 1;
                                    }
                                    if (tempList[j].totalScore_2 != null)
                                    {
                                        boySum2 += (float)tempList[j].totalScore_2;
                                        boyNum2 += 1;
                                    }
                                    if (tempList[j].rate != null)
                                    {
                                        boyRateSum += (float)tempList[j].rate;
                                        boyRateNum += 1;
                                    }
                                    boyScore1[index1.Value] = boyScore1[index1.Value] + 1;
                                    boyScore2[index2.Value] = boyScore2[index2.Value] + 1;
                                    boyRateDis[rateIndex.Value] = boyRateDis[rateIndex.Value] + 1;
                                    boyScoreAvgDis[avgScoreIndex.Value] = boyScoreAvgDis[avgScoreIndex.Value] + 1;
                                }
                                else
                                {
                                    girlsList.Add(tempList[j]);
                                    girlNum += 1;
                                    if (tempList[j].totalScore_1 != null)
                                    {
                                        girlSum1 += (float)tempList[j].totalScore_1;
                                        girlNum1 += 1;
                                    }
                                    if (tempList[j].totalScore_2 != null)
                                    {
                                        girlSum2 += (float)tempList[j].totalScore_2;
                                        girlNum2 += 1;
                                    }
                                    if (tempList[j].rate != null)
                                    {
                                        girlRateSum += (float)tempList[j].rate;
                                        girlRateNum += 1;
                                    }

                                    girlScore1[index1.Value] = girlScore1[index1.Value] + 1;
                                    girlScore2[index2.Value] = girlScore2[index2.Value] + 1;
                                    girlRateDis[rateIndex.Value] = girlRateDis[rateIndex.Value] + 1;
                                    girlScoreAvgDis[avgScoreIndex.Value] = girlScoreAvgDis[avgScoreIndex.Value] + 1;
                                }
                                totalScore1[index1.Value] = totalScore1[index1.Value] + 1;
                                totalScore2[index2.Value] = totalScore2[index2.Value] + 1;
                                totalRateDis[rateIndex.Value] = totalRateDis[rateIndex.Value] + 1;
                                totalScoreAvgDis[avgScoreIndex.Value] = totalScoreAvgDis[avgScoreIndex.Value] + 1;

                                tempList.RemoveAt(j);
                            }
                        }



                        chapterMap.Add("boysNum", boyNum);
                        chapterMap.Add("girlsNum", girlNum);
                        chapterMap.Add("totalNum", totalNum);
                        chapterMap.Add("boyAverage1", boySum1 != 0 ? boySum1 / boyNum1 : 0);
                        chapterMap.Add("boyAverage2", boySum2 != 0 ? boySum2 / boyNum2 : 0);
                        chapterMap.Add("girlAverage1", girlSum1 != 0 ? girlSum1 / girlNum1 : 0);
                        chapterMap.Add("girlAverage2", girlSum2 != 0 ? girlSum2 / girlNum2 : 0);
                        chapterMap.Add("totalAverage1", (boySum1 + girlSum1) != 0 ? (boySum1 + girlSum1) / (boyNum1 + girlNum1) : 0);
                        chapterMap.Add("totalAverage2", (boySum2 + girlSum2) != 0 ? (boySum2 + girlSum2) / (boyNum2 + girlNum2) : 0);
                        chapterMap.Add("boyScoreDistribute1", boyScore1);
                        chapterMap.Add("boyScoreDistribute2", boyScore2);
                        chapterMap.Add("girlScoreDistribute1", girlScore1);
                        chapterMap.Add("girlScoreDistribute2", girlScore2);
                        chapterMap.Add("totalScoreDistribute1", totalScore1);
                        chapterMap.Add("totalScoreDistribute2", totalScore2);
                        chapterMap.Add("boyScoreAvgDistribute", boyScoreAvgDis);
                        chapterMap.Add("girlScoreAvgDistribute", girlScoreAvgDis);
                        chapterMap.Add("totalScoreAvgDistribute", totalScoreAvgDis);
                        chapterMap.Add("boyRateAvg", boyRateSum != 0 ? boyRateSum / boyRateNum : 0);
                        chapterMap.Add("girlRateAvg", girlRateSum != 0 ? girlRateSum / girlRateNum : 0);
                        chapterMap.Add("totalRateAvg", (boyRateSum + girlRateSum) != 0 ? (boyRateSum + girlRateSum) / (boyRateNum + girlRateNum) : 0);
                        chapterMap.Add("boyRateDistribute", boyRateDis);
                        chapterMap.Add("girlRateDistribute", girlRateDis);
                        chapterMap.Add("totalRateDistribute", totalRateDis);

                        if (getDetail != null && getDetail > 0)
                        {
                            chapterMap.Add("boys", boysList);
                            chapterMap.Add("girls", girlsList);
                        }

                    }
                    classMap.Add("classNum", c.classNum);
                    classMap.Add("scoreInfo", chapterMap);
                    resultMap.Add(classMap);
                }
                return resultMap;
            }
            else
                return null;
        }
        
    public ChapterNode getCurrentProgress(int? courseClassID, int? studentID)
        {
            Takes takeTemp = TakesApi.findByStudentIDAndCourseClassID(studentID, courseClassID);

            return takeTemp == null ? null : getChapterByID(takeTemp.currentProgress);
        }

        
        
    public int? alertCurrentProgress(int? courseClassID, int? studentID, int? chapterID)
        {
            Takes t = TakesApi.findByStudentIDAndCourseClassID(studentID, courseClassID);
            if (t != null)
            {
                t.currentProgress=(chapterID);
                TakesApi.insert(t);
                return 1;
            }
            else
                return 0;
        }

        
        
    public void deleteChapter(CourseCatalog courseCatalog)
        {
            List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseID(courseCatalog.courseID);
            getSubNodes(courseCatalog, chapterNodes);
            foreach (var it in courseCatalog.subCatalog)
            {
                deleteChapter(it);
            }
            if (courseCatalog.id > 0)
                ChapterContentApi.delete(courseCatalog.id);
            if (courseCatalog.parentID == 0)//为章节点 需要删除习题
            {
                //删除习题
            }
        }
        
        
    public int? deleteClass(int? courseClassID)
        {
            if (CourseClassApi.getByID(courseClassID)!=null)
            {
                CourseClassApi.delete(courseClassID);
                return 1;
            }
            else
                return 0;
        }

        
    public List<CourseAndClass> getCoursesByTeacherID(int? teacherID)
        {
            List<CourseInfo>courseInfos=CourseInfoApi.findByTeacherID(teacherID);
        if (courseInfos.Count()>0)
        {
            List<CourseAndClass> courseAndClasses = new List<CourseAndClass>();
            foreach (CourseInfo i in courseInfos)
            {
                List<CourseClass> classes = CourseClassApi.findByCourseID(i.courseID);
                foreach (CourseClass j in classes)
                {
                    courseAndClasses.Add(new CourseAndClass(i, j));
                }
}
            return courseAndClasses;
        }
        else
        {
            return null;
        }
    }

    public List<CourseInfo> getAllCourses()
{
    List<CourseInfo>temp=CourseInfoApi.getAll();
    List<CourseInfo>newList=new List<CourseInfo>();
        foreach(CourseInfo i in temp)
        {
            CourseInfo tempc = i;
            tempc.courseName=(getCourseNameByNameID(int.Parse(i.courseName)).courseName);
            newList.Add(tempc);
        }
        return newList;
    }

    
    public List<CourseRelationEntity> getAllCoursesRelation()
{
    List<CourseRelationEntity> list = new List<CourseRelationEntity>();
    List<CourseRelation> courseRelations = CourseRelationApi.getAll();
    Dictionary<CourseName, List<CourseName>> courseMap = new Dictionary<CourseName, List<CourseName>>();
    Dictionary<CourseName, List<CourseName>> subCourseMap = new Dictionary<CourseName, List<CourseName>>();
    foreach (CourseRelation i in courseRelations)
    {
        CourseName temp = getCourseNameByNameID(i.courseNameID);
        if (temp != null)
        {
                    if (!courseMap.ContainsKey(temp))
                        courseMap.Add(temp, new List<CourseName>());
                    if (!subCourseMap.ContainsKey(temp))
                        courseMap.Add(temp, new List<CourseName>());

            if (i.preCourseNameID != 0)
            {
                CourseName tempName = getCourseNameByNameID(i.preCourseNameID);
                if (tempName != null)
                    courseMap[temp].Add(tempName);
            }
            if (subCourseMap[temp].Count() == 0)
            {
                List<CourseRelation> subCoursesList = CourseRelationApi.findByPreCourseNameID(i.courseNameID);
                if (subCoursesList != null && subCoursesList.Count() != 0)
                {
                    List<CourseName> subCoursesName = new List<CourseName>();
                    foreach (CourseRelation j in subCoursesList)
                    {
                        CourseName tempName = getCourseNameByNameID(j.courseNameID);
                        if (tempName != null)
                            subCoursesName.Add(tempName);
                    }
                    subCourseMap.Add(temp, subCoursesName);
                }
            }
        }
    }
    HashSet<CourseName> courseNames = courseMap.Keys.ToHashSet();

    while (courseNames.Count() != 0)
    {
        List<CourseName> preList = new List<CourseName>();
        foreach (CourseName i in courseNames)
        {
            int num = 0;
            foreach (CourseName j in courseMap[i])
            {
                if (courseNames.Contains(j))
                {
                    num++;
                    break;
                }
            }
            if (num == 0)
                preList.Add(i);
        }
        foreach (CourseName i in preList)
        {
            list.Add(new CourseRelationEntity(i, courseMap[i], subCourseMap[i]));
            courseNames.Remove(i);
        }
        preList.Clear();
    }
    return list;
}


    public List<ChapterRelationEntity> getChapterRelationByCourseID(int? courseID)
{
    List<ChapterRelationEntity> list = new List<ChapterRelationEntity>();
    List<ChapterRelation> chapterRelations = new List<ChapterRelation>();
    List<ChapterNode> courseChapters = ChapterContentApi.findByCourseID(courseID);
    foreach (ChapterNode i in courseChapters)
    {
        chapterRelations.AddRange(ChapterRelationApi.findByChapterID(i.id));
    }
    Dictionary<int?, List<ChapterNode>> chapterMap = new Dictionary<int?, List<ChapterNode>>();
    Dictionary<int?, List<ChapterNode>> subChapterMap = new Dictionary<int?, List<ChapterNode>>();
    foreach (ChapterRelation i in chapterRelations)
    {
                if (!chapterMap.ContainsKey(i.chapterID))
                    chapterMap.Add(i.chapterID, new List<ChapterNode>());
                if (!subChapterMap.ContainsKey(i.chapterID))
                    chapterMap.Add(i.chapterID, new List<ChapterNode>());
  
        if (i.preChapterID != 0)
        {
            ChapterNode tempNode = getChapterByID(i.preChapterID);
            if (tempNode != null)
                chapterMap[i.chapterID].Add(tempNode);
        }
        if (subChapterMap[i.chapterID].Count() == 0)
        {
            List<ChapterRelation> subChaptersList = ChapterRelationApi.findByPreChapterID(i.chapterID);
            if (subChaptersList != null && subChaptersList.Count() != 0)
            {
                List<ChapterNode> subChapters = new List<ChapterNode>();
                foreach (ChapterRelation j in subChaptersList)
                {
                    ChapterNode temp = ChapterContentApi.getByID(j.chapterID);
                            if (temp != null)
                                subChapters.Add(temp);
                    
                }
                subChapterMap.Add(i.chapterID, subChapters);
            }
        }
    }
    HashSet<int?> chapterIDs = chapterMap.Keys.ToHashSet();

    while (chapterIDs.Count() != 0)
    {
        List<int?> preList = new List<int?>();
        foreach (int? i in chapterIDs)
        {
            int num = 0;
            foreach (ChapterNode j in chapterMap[i])
            {
                if (chapterIDs.Contains(j.id))
                {
                    num++;
                    break;
                }
            }
            if (num == 0)
                preList.Add(i);
        }
        preList.Sort();
        foreach (int? i in preList)
        {
            list.Add(new ChapterRelationEntity(ChapterContentApi.getByID(i), chapterMap[i], subChapterMap[i]));
            chapterIDs.Remove(i);
        }
        preList.Clear();
    }
    return list;
}


    public CourseName addCourseName(String courseName)
{
    if (CourseNameApi.getByCourseName(courseName) == null)//检查是否名称冲突
    {
        CourseName temp = new CourseName();
        temp.courseName=(courseName);
                CourseNameApi.insert(temp);
        return CourseNameApi.getByCourseName(courseName);
    }
    else
        return null;
}


    public List<CourseName> getCourseList()
{
    return CourseNameApi.getAll();
}

    public int? alertCourseName(CourseName courseName)
{
    CourseName temp = CourseNameApi.getByID(courseName.courseNameID);
    if (temp!=null)
    {
        if (CourseNameApi.getByCourseName(courseName.courseName) == null)//检查是否名称冲突
        {
            temp.courseName=(courseName.courseName);
            CourseNameApi.insert(temp);
            return 1;
        }
        else
            return 0;
    }
    else
        return -1;
}


    public List<CourseAndClassList> getAllCoursesByNameID(String nameID)
{
    List<CourseAndClassList> courseAndClasses = new List<CourseAndClassList>();
    List<CourseInfo> courseInfos = CourseInfoApi.findByCourseName(nameID);
    foreach (CourseInfo i in courseInfos)
    {
        List<CourseClass> classes = CourseClassApi.findByCourseID(i.courseID);
        courseAndClasses.Add(new CourseAndClassList(i, classes));
    }
    return courseAndClasses;
}



    public int? addCourseRelation(int? courseNameID, int? preCourseNameID)
{
    if (CourseRelationApi.findByCourseNameIDAndPreCourseNameID(courseNameID, preCourseNameID) == null)//如果不存在该关系
    {
        CourseRelation courseRelation = new CourseRelation();
        courseRelation.courseNameID=(courseNameID);
        courseRelation.preCourseNameID=(preCourseNameID);
        CourseRelationApi.insert(courseRelation);
        return 1;
    }
    else
        return 0;

}



    public int? addChapterRelation(int? chapterID, int? preChapterID)
{
    if (ChapterRelationApi.findByChapterIDAndPreChapterID(chapterID, preChapterID) == null)//如果不存在该关系
    {
        ChapterRelation chapterRelation = new ChapterRelation();
        chapterRelation.chapterID=(chapterID);
        chapterRelation.preChapterID=(preChapterID);
        ChapterRelationApi.insert(chapterRelation);
        return 1;
    }
    else
        return 0;
}



    public int? deleteChapterRelation(int? chapterID, int? preChapterID)
{
    ChapterRelation temp = ChapterRelationApi.findByChapterIDAndPreChapterID(chapterID, preChapterID);
    if (temp != null)//检查关系是否已经存在
    {
        ChapterRelationApi.delete(temp.id);
        return 1;
    }
    else
        return 0;
}



    public int? deleteCourseRelation(int? courseNameID, int? preCourseNameID)
{
    CourseRelation temp = CourseRelationApi.findByCourseNameIDAndPreCourseNameID(courseNameID, preCourseNameID);
    if (temp != null)//检查关系是否已经存在
    {
        CourseRelationApi.delete(temp.id);
        return 1;
    }
    else
        return 0;
}


    public Dictionary<String, int?> getStudentNumByTeacher(int? teacherID)
{
    List<CourseAndClass>courseAndClass=getCoursesByTeacherID(teacherID);
    Dictionary<String,int?>courseToNum=new Dictionary<string, int?>();
        if (courseAndClass!=null)
        {
            List<CourseName> courseNames =CourseNameApi.getAll();
                foreach (CourseName i in courseNames)
                    if (!courseToNum.ContainsKey(i.courseName))
                        courseToNum.Add(i.courseName,0);

            foreach (CourseAndClass i in courseAndClass)
            {
                String name = i.courseInfo.courseName;
List<UserInfo> tempUser = getStudentsByClassID(i.courseClass.id);
int? num = courseToNum[name] + (tempUser!=null?tempUser.Count():0);
courseToNum.Add(name, num);
            }
        }
        return courseToNum;
    }
        
    public Dictionary<String, int?> getStudentNumBySemesterAndYear(int? year, String semester)
        {
            List<CourseAndClassList> courseAndClassLists = new List<CourseAndClassList>();
            List<CourseInfo> courseInfos = CourseInfoApi.findByCourseYearAndCourseSemester(year, semester);
            if (courseInfos != null)
                foreach (CourseInfo i in courseInfos)
                {
                    List<CourseClass> classes = CourseClassApi.findByCourseID(i.courseID);
                    courseAndClassLists.Add(new CourseAndClassList(i, classes));
                }
            Dictionary<String, int?> courseToNum = new Dictionary<string, int?>();

            foreach (CourseAndClassList i in courseAndClassLists)
            {
                String name = getCourseNameByNameID(int.Parse(i.courseInfo.courseName)).courseName;
                if (!courseToNum.ContainsKey(i.courseName))
                    courseToNum.Add(i.courseName, 0);
                int? num = 0;
                if (i.courseClasses != null)
                {

                    foreach (CourseClass j in i.courseClasses)
                    {
                        List<UserInfo> temp = getStudentsByClassID(j.id);
                        if (temp != null)
                            num += getStudentsByClassID(j.id).Count();
                    }
                }
                courseToNum.Add(name, courseToNum[name] + num);
            }
            return courseToNum;
        }

        
    public Dictionary<String, int?> getStudentNumByYear(int? year)
        {
            List<CourseAndClassList> courseAndClassLists = new List<CourseAndClassList>();
            List<CourseInfo> courseInfos = CourseInfoApi.findByCourseYear(year);
            if (courseInfos != null)
                foreach (CourseInfo i in courseInfos)
                {
                    List<CourseClass> classes = CourseClassApi.findByCourseID(i.courseID);
                    courseAndClassLists.Add(new CourseAndClassList(i, classes));
                }
            Dictionary<String, int?> courseToNum = new Dictionary<string, int?>();

            foreach (CourseAndClassList i in courseAndClassLists)
            {
                String name = getCourseNameByNameID(int.Parse(i.courseInfo.courseName)).courseName;
                if (!courseToNum.ContainsKey(i.courseName))
                    courseToNum.Add(i.courseName, 0);
                int? num = 0;
                if (i.courseClasses != null)
                {

                    foreach (CourseClass j in i.courseClasses)
                    {
                        List<UserInfo> temp = getStudentsByClassID(j.id);
                        if (temp != null)
                            num += getStudentsByClassID(j.id).Count();
                    }
                }
                courseToNum.Add(name, courseToNum[name] + num);
            }
            return courseToNum;
        }

        
    public List<Dictionary<String, Object>> getRateBySemesterAndYear(String courseNameID)
        {
            List<CourseInfo> courseInfos = CourseInfoApi.findByCourseName(courseNameID);
            Dictionary<SemesterAndYear, List<float?>> semYearToRateMap = new Dictionary<SemesterAndYear, List<float?>>();
            foreach (CourseInfo i in courseInfos)
            {
                SemesterAndYear temp = new SemesterAndYear(i.courseSemester, i.courseYear);
                if (!semYearToRateMap.ContainsKey(temp))
                    semYearToRateMap.Add(temp, new List<float?>());
                semYearToRateMap[temp].Add(i.rate);
            }
            List<SemesterAndYear> sortedSemAndYear = new List<SemesterAndYear>(semYearToRateMap.Keys.GetHashCode());

            List<Dictionary<String, Object>> resultMap = new List<Dictionary<String, Object>>();
            foreach (SemesterAndYear i in sortedSemAndYear)
            {
                Dictionary<String, Object> rateMap = new Dictionary<string, object>();
                float sum = 0F;
                foreach (float j in semYearToRateMap[i])
                    sum += j;
                if (semYearToRateMap[i].Count() != 0)
                    sum /= semYearToRateMap[i].Count();
                rateMap.Add("semesterAndYear", i);
                rateMap.Add("avgRate", sum);
                resultMap.Add(rateMap);
            }
            return resultMap;
        }

        
    public List<Dictionary<String, Object>> getClassesByNIDAndTID(String courseNameID, int? teacherID)
        {
            List<Dictionary<String, Object>> resultMap=new List<Dictionary<String, Object>>();
        List<CourseInfo> courseInfos = CourseInfoApi.findByCourseNameAndTeacherID(courseNameID, teacherID);
        if (courseInfos!=null)
            foreach(CourseInfo i in courseInfos)
            {
                List<CourseClass> temp = getClassesByCourseID(i.courseID);
                if (temp!=null)
                {
                    Dictionary<String, Object> courseClassMap = new Dictionary<string, object>();
        courseClassMap.Add("courseInfo",i);
                    courseClassMap.Add("classes",temp);
                    resultMap.Add(courseClassMap);
                }
}
        return resultMap;
    }
    public List<Dictionary<String, Object>> getTeacherListByNID(String courseNameID)
{
            Regex regExp = new Regex(@"^\d+$");
            if (courseNameID != null && !regExp.IsMatch(courseNameID))
    {
        CourseName nameTemp = CourseNameApi.getByCourseName(courseNameID);
        if (nameTemp != null)
            courseNameID = nameTemp.courseNameID.ToString();
        else
            return null;
    }
    List<CourseInfo> courseInfos = courseNameID == null ? CourseInfoApi.getAll() : CourseInfoApi.findByCourseName(courseNameID);
    //if (courseInfos!=null)
    List<Dictionary<String, Object>> teacherInfoMap = new List<Dictionary<String, Object>>();
    foreach (CourseInfo i in courseInfos)
    {
        Dictionary<String, Object> tempMap = new Dictionary<string, object>();
        tempMap.Add("courseInfo", i);
        teacherInfoMap.Add(tempMap);
    }
    return teacherInfoMap;
}


    public int? addClassComment(int? courseClassID, int? studentID, String comment, int? rate)
{
    Takes takes = TakesApi.findByStudentIDAndCourseClassID(studentID, courseClassID);
    if (takes != null)
    {
        takes.comment=(comment);
        takes.rate=(rate);
        TakesApi.insert(takes);
        return 1;
    }
    else
        return 0;
}
        public Dictionary<String, Dictionary<String, Dictionary<String, Object>>> getCourseYearAvgScoreRate(int? courseNameID, int? teacherID)
        {
            List<CourseInfo> courseInfos = new List<CourseInfo>();
            Dictionary<int?, Dictionary<String, Object>> yearMap = new Dictionary<int?, Dictionary<string, object>>();
            Dictionary<String, Dictionary<String, Object>> semesterMap = new Dictionary<string, Dictionary<string, object>>();
            if (teacherID != null)
                courseInfos = CourseInfoApi.findByCourseNameAndTeacherID(courseNameID.ToString(), teacherID);
            else
                courseInfos = CourseInfoApi.findByCourseName(courseNameID.ToString());
            if (courseInfos != null && courseInfos.Count() != 0)
            {
                foreach (CourseInfo i in courseInfos)
                {
                    Dictionary<string,Object> temp = getCourseClassAvgScore(i.courseID);
                    if (temp == null)
                        continue;
                if(!yearMap.ContainsKey(i.courseYear))
                    {
                        Dictionary<String, Object> tempMap = new Dictionary<string, object>();
                        tempMap.Add("score", new List<float>());
                        tempMap.Add("rate", new List<float>());
                        yearMap.Add(i.courseYear, tempMap);
                    }
                if(!semesterMap.ContainsKey(i.courseYear + i.courseSemester))
                    {
                        Dictionary<String, Object> tempMap = new Dictionary<string, object>();
                        tempMap.Add("score", new List<float>());
                        tempMap.Add("rate", new List<float>());
                        semesterMap.Add(i.courseYear + i.courseSemester, tempMap);
                    }

                float? boyScore = (float?)temp["courseBoyAvgScore"];
                float? girlScore = (float?)temp["courseGirlAvgScore"];
                float? boyRate = (float?)temp["courseBoyAvgRate"];
                float? girlRate = (float?)temp["courseGirlAvgRate"];
                if (boyScore == null)
                    boyScore = girlScore;
                if (girlScore == null)
                    girlScore = boyScore;
                if (boyRate == null)
                    boyRate = girlRate;
                if (girlRate == null)
                    girlRate = boyRate;

                if (boyScore != null)
                {
                    Dictionary<String, Object> tempMap = yearMap[i.courseYear];
                    List<float?> tempList = (List<float?>)tempMap["score"];
                    tempList.Add(0.5F * boyScore + 0.5F * girlScore);
                    tempMap = semesterMap[i.courseYear + i.courseSemester];
                    tempList = (List<float?>)tempMap["score"];
                    tempList.Add(0.5F * boyScore + 0.5F * girlScore);
                }
                if (boyRate != null)
                {
                    Dictionary<String, Object> tempMap = yearMap[i.courseYear];
                    List<float?> tempList = (List<float?>)tempMap["rate"];
                    tempList.Add(0.5F * boyRate + 0.5F * girlRate);
                    tempMap = semesterMap[i.courseYear + i.courseSemester];
                    tempList = (List<float?>)tempMap["rate"];
                    tempList.Add(0.5F * boyRate + 0.5F * girlRate);
                }
            }
            Dictionary<String, Dictionary<String, Dictionary<String, Object>>> resultMap = new Dictionary<string, Dictionary<String, Dictionary<String, Object>>>();

            HashSet<int?> yearKey = yearMap.Keys.ToHashSet();
            HashSet<String> semesterKey = semesterMap.Keys.ToHashSet();
            float? num = 0F;
            foreach (int i in yearKey)
            {
                num = 0F;
                Dictionary<String, Object> tempMap = yearMap[i];
                List<float?> tempList = (List<float?>)tempMap["score"];
                foreach (float? j in tempList)
                    num += j;
                tempMap.Add("score", num / tempList.Count());
                num = 0F;
                tempList = (List<float?>)tempMap["rate"];
                foreach (float? j in tempList)
                    num += j;
                tempMap.Add("rate", num / tempList.Count());

            }
            foreach (String i in semesterKey)
            {
                num = 0F;
                Dictionary<String, Object> tempMap = semesterMap[i];
                List<float?> tempList = (List<float?>)tempMap["score"];
                foreach (float j in tempList)
                    num += j;
                tempMap.Add("score", num / tempList.Count());
                num = 0F;
                tempList = (List<float?>)tempMap["rate"];
                foreach (float j in tempList)
                    num += j;
                tempMap.Add("rate", num / tempList.Count());
            }
            List<int?> sortedYear = new List<int?>(yearKey);
            sortedYear.Sort();
            Dictionary<String, Dictionary<String, Object>> sortedYearMap = new Dictionary<string, Dictionary<string, object>>();
            foreach (int i in sortedYear)
            {
                sortedYearMap.Add(i + "年", yearMap[i]);
            }
            List<String> sortedSem = new List<String>(semesterKey);
            sortedSem.Sort((s1, s2)=>int.Parse(s1.Substring(0, 4)) < int.Parse(s2.Substring(0, 4)) ? 1 : int.Parse(s1.Substring(0, 4)) == int.Parse(s2.Substring(0, 4)) ? s1.Substring(4)==("春季") ? 1 : s1==(s2) ? 0 : -1 : -1);
            Dictionary<String, Dictionary<String, Object>> sortedSemMap = new Dictionary<string, Dictionary<string, object>>();
            foreach (String i in sortedSem)
            {
                sortedSemMap.Add(i, semesterMap[i]);
            }
            resultMap.Add("year", sortedYearMap);
            resultMap.Add("semester", sortedSemMap);
            return resultMap;
        }
        else
            return null;
    }


}
}

    

