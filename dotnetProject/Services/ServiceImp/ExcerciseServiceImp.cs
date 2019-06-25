using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;
using PersistentLayer.Apis;
using dotnetProject.Entity;

namespace dotnetProject.Services.ServiceImp
{
    public class ExcerciseServiceImp : ExcerciseService
    {
        public ResultEntity findOneExerice(int? exerciseId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseId != null)
            {
                Exercise exercise = ExerciseApi.findByExerciseId(exerciseId);
                if (exercise != null)
                {
                    if (exercise.exerciseType % 3 != 0)
                    {
                        List<ExerciseChoice> exerciseChoices = ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exerciseId);
                        resultEntity.setData(new ExerciseSet(exercise, exerciseChoices));
                    }
                    else
                        resultEntity.setData(new ExerciseSet(exercise));
                    resultEntity.setState(1);
                    resultEntity.setMessage("搜索习题成功！");
                }
                else
                {
                    resultEntity.setMessage("搜索习题失败！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }


        public ResultEntity addExercise(Exercise exercise)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exercise != null)
            {
                resultEntity.setData(ExerciseApi.insert(exercise));
                if (resultEntity.getData() != null)
                {
                    if (exercise.exerciseType <= 3)
                        setTotalScore(exercise.chapterId, "preview");
                    else
                        setTotalScore(exercise.chapterId, "review");
                    resultEntity.setState(1);
                    resultEntity.setMessage("创建习题成功！");
                }
                else
                {
                    resultEntity.setMessage("创建习题失败！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }



        public ResultEntity deleteExercise(int? exerciseId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseId != null)
            {
                Exercise exercise = ExerciseApi.findByExerciseId(exerciseId);
                if (exercise != null)
                {
                    ExerciseApi.delete(exercise.exerciseId);
                    resultEntity.setState(1);
                    resultEntity.setMessage("习题删除成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应习题！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }


        public ResultEntity alterExercise(Exercise exercise)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exercise != null)
            {
                //fragile
                Exercise exercise1 = ExerciseApi.findByExerciseId(exercise.exerciseId);
                if (exercise1 != null)
                {
                    resultEntity.setData(ExerciseApi.insert(exercise));
                    resultEntity.setState(1);
                    resultEntity.setMessage("习题修改成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应习题！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }


        public ResultEntity addExerciseChoice(ExerciseChoice exerciseChoice)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseChoice != null)
            {
                resultEntity.setData(ExerciseChoiceApi.insert(exerciseChoice));
                if (resultEntity.getData() != null)
                {
                    resultEntity.setState(1);
                    resultEntity.setMessage("创建选项成功！");
                }
                else
                {
                    resultEntity.setMessage("创建选项失败！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }


        public ResultEntity deleteExerciseChoice(int? exerciseChoiceId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseChoiceId != null)
            {
                ExerciseChoice exerciseChoice = ExerciseChoiceApi.getByID(exerciseChoiceId);
                if (exerciseChoice != null)
                {
                    //fragile
                    ExerciseChoiceApi.delete(exerciseChoiceId);
                    resultEntity.setState(1);
                    resultEntity.setMessage("选项删除成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应选项！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }


        public ResultEntity alterExerciseChoice(ExerciseChoice exerciseChoice)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseChoice != null)
            {
                //fragile
                ExerciseChoice exerciseChoice1 = ExerciseChoiceApi.getByID(int.Parse(exerciseChoice.exerciceChoiceId));
                if (exerciseChoice1 != null)
                {
                    resultEntity.setData(ExerciseChoiceApi.insert(exerciseChoice));
                    resultEntity.setState(1);
                    resultEntity.setMessage("选项修改成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应选项！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }


        public ResultEntity findOneAnswerById(int? studentExerciseScoreId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (studentExerciseScoreId != null)
            {
                //fragile
                StudentExerciseScore studentExerciseScore = StudentExerciseScoreApi.getByID(studentExerciseScoreId);
                if (studentExerciseScore != null)
                {
                    resultEntity.setData(studentExerciseScore);
                    resultEntity.setState(1);
                    resultEntity.setMessage("学生答案搜寻成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应学生答案！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }

        public ResultEntity findOneAnswer(int? exerciseId, int? studentId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseId != null && studentId != null)
            {
                StudentExerciseScore studentExerciseScore = StudentExerciseScoreApi.findByExerciseIdAndStudentId(exerciseId, studentId);
                if (studentExerciseScore != null)
                {
                    resultEntity.setData(studentExerciseScore);
                    resultEntity.setState(1);
                    resultEntity.setMessage("学生答案搜寻成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应学生答案！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }
        
        
    public ResultEntity answerOne(String answer, int? exerciseId, int? studentId)
        {
            ResultEntity resultEntity = new ResultEntity();
            StudentExerciseScore studentExerciseScore = new StudentExerciseScore(studentId, exerciseId, answer, 0.ToString());
            studentExerciseScore.corrected = 0;
            Exercise exercise = ExerciseApi.findByExerciseId(exerciseId);
            if (studentExerciseScore != null)
            {
                if (exercise.exerciseType % 3 != 0)
                {
                    if (answer.Equals(exercise.exerciseAnswer))
                    {
                        studentExerciseScore.corrected=1;
                        studentExerciseScore.exerciseScore=exercise.exercisePoint.ToString();
                    }
                }
                resultEntity.setData(StudentExerciseScoreApi.insert(studentExerciseScore));
                if (resultEntity.getData() != null)
                {
                    resultEntity.setState(1);
                    resultEntity.setMessage("答题成功 ！");
                }
                else
                {
                    resultEntity.setMessage("答题失败！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;

        }
        
        
        public ResultEntity answerAll(List<String> answers, int? studentId, int? chapterId, String type, String comment, int? rate)
        {
                ResultEntity resultEntity=new ResultEntity();
                if(answers!=null&&type!=null&&chapterId!=null&&studentId!=null){
                List<ExerciseSet> exerciseSets = new List<ExerciseSet>();
                int type1 = 0;
                int type2 = 0;
                int type3 = 0;
                if(type.Equals("preview")){
                    type1=1;
                    type3=2;
                    type2=3;
                }
                else{
                    type1=4;
                    type3=5;
                    type2=6;
                }
                List<int?> exerciseIds = new List<int?>();
                List<Exercise> exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type1);
                foreach (Exercise exercise in exercises){
                    exerciseSets.Add(new ExerciseSet(exercise, ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exercise.exerciseId)));
                    exerciseIds.Add(exercise.exerciseId);
                }
                exercises=ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type3);
                foreach(Exercise exercise in exercises){
                    exerciseSets.Add(new ExerciseSet(exercise, ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exercise.exerciseId)));
                    exerciseIds.Add(exercise.exerciseId);
                }
                exercises=ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type2);
                foreach (Exercise exercise in exercises){
                    exerciseSets.Add(new ExerciseSet(exercise));
                }
                for(int i=0;i<answers.Count;i++){
                    answerOne(answers[i], exerciseSets[i].getExercise().exerciseId, studentId);
                }
                int score = 0;
                foreach(int? exerciseId in exerciseIds){
                    score+=int.Parse(StudentExerciseScoreApi.findByExerciseIdAndStudentId(exerciseId, studentId).exerciseScore);
                }
                StudentChapter studentChapter;
                if(!StudentChapterApi.existsByChapterIDAndStudentID(chapterId, studentId)){
                    studentChapter=new StudentChapter();
                    studentChapter.chapterID=chapterId;
                    studentChapter.studentID=studentId;
                    StudentChapterApi.insert(studentChapter);
                }
                if(!type.Equals("preview")){
                    studentChapter=StudentChapterApi.findByChapterIDAndStudentID(chapterId, studentId);
                    studentChapter.rate=rate;
                    studentChapter.comment=comment;
                    studentChapter.totalScore_2=score;
                    studentChapter.scored_2=0;
                    StudentChapterApi.insert(studentChapter);
                    //LaterImplementation
                    //new NLPUtil().setCommentNLPRate(comment, chapterId, studentId);
                }
                else{
                    studentChapter=StudentChapterApi.findByChapterIDAndStudentID(chapterId, studentId);
                    studentChapter.totalScore_1=score;
                    studentChapter.scored_1=0;
                    StudentChapterApi.insert(studentChapter);
                }
                resultEntity.setState(1);
                resultEntity.setMessage("答题成功 ！");
            }
            else
            {
                resultEntity.setMessage("传入参数有空值！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }
    
    
        public ResultEntity alterAnswer(String answer, int? exerciseId, int? studentId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (exerciseId != null && studentId != null)
            {
                StudentExerciseScore studentExerciseScore = StudentExerciseScoreApi.findByExerciseIdAndStudentId(exerciseId, studentId);
                if (studentExerciseScore != null)
                {
                    studentExerciseScore.studentAnswer=answer;
                    resultEntity.setData(StudentExerciseScoreApi.insert(studentExerciseScore));
                    resultEntity.setState(1);
                    resultEntity.setMessage("学生答案修改成功！");
                }
                else
                {
                    resultEntity.setMessage("未找到对应学生答案！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }

        public ResultEntity correctOne(int? studentExerciseScoreId, int? score)
        {
            ResultEntity resultEntity = new ResultEntity();
            StudentExerciseScore studentExerciseScore = StudentExerciseScoreApi.getByID(studentExerciseScoreId);
            if (studentExerciseScore != null)
            {
                studentExerciseScore.exerciseScore=score.ToString();
                resultEntity.setData(StudentExerciseScoreApi.insert(studentExerciseScore));
                if (resultEntity.getData() != null)
                {
                    resultEntity.setState(1);
                    resultEntity.setMessage("批改成功 ！");
                }
                else
                {
                    resultEntity.setMessage("批改失败！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }
        
        
    public ResultEntity correctAll(List<int?> scores, int? studentId, int? chapterId, String type)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (studentId != null && chapterId != null && type != null)
            {
                int trueType = 0;
                int type1 = 0;
                int type2 = 0;
                if (type.Equals("preview"))
                {
                    trueType = 3;
                    type1 = 1;
                    type2 = 2;
                }
                else
                {
                    trueType = 6;
                    type1 = 4;
                    type2 = 5;
                }
                List<Exercise> exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, trueType);
                List<int?> exerciseIds = new List<int?>();
                foreach (Exercise exercise in exercises)
                {
                    exerciseIds.Add(exercise.exerciseId);
                }
                StudentExerciseScore studentExerciseScore;
                for (int i = 0; i < scores.Count; i++)
                {
                    studentExerciseScore = StudentExerciseScoreApi.findByExerciseIdAndStudentId(exerciseIds[i], studentId);
                    studentExerciseScore.exerciseScore=scores[i].ToString();
                    studentExerciseScore.corrected=1;
                    StudentExerciseScoreApi.insert(studentExerciseScore);
                }
                exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type1);
                foreach (Exercise exercise in exercises)
                {
                    exerciseIds.Add(exercise.exerciseId);
                }
                exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type2);
                foreach (Exercise exercise in exercises)
                {
                    exerciseIds.Add(exercise.exerciseId);
                }
                int score = 0;
                foreach (int? exerciseId in exerciseIds)
                {
                    score += int.Parse(StudentExerciseScoreApi.findByExerciseIdAndStudentId(exerciseId, studentId).exerciseScore);
                }
                if (type.Equals("preview"))
                {
                    StudentChapter studentChapter = StudentChapterApi.findByChapterIDAndStudentID(chapterId, studentId);
                    studentChapter.totalScore_1=score;
                    studentChapter.scored_1=1;
                    StudentChapterApi.insert(studentChapter);
                }
                else
                {
                    StudentChapter studentChapter = StudentChapterApi.findByChapterIDAndStudentID(chapterId, studentId);
                    studentChapter.totalScore_2=score;
                    studentChapter.scored_2=1;
                    StudentChapterApi.insert(studentChapter);
                }
                resultEntity.setState(1);
                resultEntity.setMessage("批改成功 ！");
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }

        
        
    public ResultEntity viewExercise(int? chapterId, String type)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (chapterId != null)
            {
                List<ExerciseSet> exerciseSets = new List<ExerciseSet>();
                int type1 = 0;
                int type2 = 0;
                int type3 = 0;
                if (type.Equals("preview"))
                {
                    type1 = 1;
                    type3 = 2;
                    type2 = 3;
                }
                else
                {
                    type1 = 4;
                    type3 = 5;
                    type2 = 6;
                }
                List<Exercise> exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type1);
                foreach (Exercise exercise in exercises)
                {
                    exerciseSets.Add(new ExerciseSet(exercise, ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exercise.exerciseId)));
                }
                exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type3);
                foreach (Exercise exercise in exercises)
                {
                    exerciseSets.Add(new ExerciseSet(exercise, ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exercise.exerciseId)));
                }
                exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type2);
                foreach (Exercise exercise in exercises)
                {
                    exerciseSets.Add(new ExerciseSet(exercise));
                }
                resultEntity.setData(exerciseSets);
                if (resultEntity.getData() != null)
                {
                    if (type.Equals("preview"))
                    {
                        //fragile
                        if (ChapterContentApi.getByID(chapterId).exerciseVisible_1 != null && ChapterContentApi.getByID(chapterId).exerciseVisible_1)
                        {
                            DateTime now = DateTime.Now;
                            if (ChapterContentApi.getByID(chapterId).exerciseDeadline_1 != null && now > ChapterContentApi.getByID(chapterId).exerciseDeadline_1)
                            {
                                resultEntity.setState(4);
                                resultEntity.setMessage("习题已过deadline！");
                            }
                            else
                            {
                                resultEntity.setState(1);
                                resultEntity.setMessage("查看成功！");
                            }
                        }
                        else
                        {
                            resultEntity.setState(2);
                            resultEntity.setMessage("习题当前设置为不可见！");
                        }
                    }
                    else
                    {
                        if (ChapterContentApi.getByID(chapterId).exerciseDeadline_2 != null && ChapterContentApi.getByID(chapterId).exerciseVisible_2)
                        {
                            DateTime now = DateTime.Now;
                            if (ChapterContentApi.getByID(chapterId).exerciseDeadline_2 != null && now> ChapterContentApi.getByID(chapterId).exerciseDeadline_2)
                            {
                                resultEntity.setState(4);
                                resultEntity.setMessage("习题已过deadline！");
                            }
                            else
                            {
                                resultEntity.setState(1);
                                resultEntity.setMessage("查看成功！");
                            }
                        }
                        else
                        {
                            resultEntity.setState(2);
                            resultEntity.setMessage("习题当前设置为不可见！");
                        }
                    }
                }
                else
                {
                    resultEntity.setMessage("无题目！");
                    resultEntity.setState(3);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }
        
        
    public ResultEntity viewSomeAnswer(int? chapterId, int? studentId, String type)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (chapterId != null && studentId != null)
            {
                List<ExerciseSet> exerciseSets = new List<ExerciseSet>();
                int type1 = 0;
                int type2 = 0;
                int type3 = 0;
                if (type.Equals("preview"))
                {
                    type1 = 1;
                    type2 = 3;
                    type3 = 2;
                }
                else
                {
                    type1 = 4;
                    type2 = 6;
                    type3 = 5;
                }
                List<Exercise> exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type1);
                foreach (Exercise exercise in exercises)
                {
                    if (StudentExerciseScoreApi.existsByExerciseIdAndStudentId(exercise.exerciseId, studentId) == false)
                    {
                        resultEntity.setMessage("学生未答题！");
                        resultEntity.setState(0);
                        return resultEntity;
                    }
                    exerciseSets.Add(new ExerciseSet(exercise, ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exercise.exerciseId), StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).studentAnswer));
                }
                exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type3);
                foreach (Exercise exercise in exercises)
                {
                    if (StudentExerciseScoreApi.existsByExerciseIdAndStudentId(exercise.exerciseId, studentId) == false)
                    {
                        resultEntity.setMessage("学生未答题！");
                        resultEntity.setState(0);
                        return resultEntity;
                    }
                    exerciseSets.Add(new ExerciseSet(exercise, ExerciseChoiceApi.findByExerciseIdOrderByExerciceChoiceId(exercise.exerciseId), StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).studentAnswer));
                }
                exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type2);
                foreach (Exercise exercise in exercises)
                {
                    if (StudentExerciseScoreApi.existsByExerciseIdAndStudentId(exercise.exerciseId, studentId) == false)
                    {
                        resultEntity.setMessage("学生未答题！");
                        resultEntity.setState(0);
                        return resultEntity;
                    }
                    exerciseSets.Add(new ExerciseSet(exercise, StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).studentAnswer));
                }
                List<int?> scores = exerciseScore(studentId, chapterId, type);
                resultEntity.setData(new ExerciseSetsDetails(exerciseSets, scores));
                if (resultEntity.getData() != null)
                {
                    resultEntity.setState(1);
                    resultEntity.setMessage("查看成功！");
                }
                else
                {
                    resultEntity.setMessage("查看失败！");
                    resultEntity.setState(0);
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }

        
        
    public int? calculateScore(int? chapterId, int? studentId)
        {
            List<Exercise> exercises = ExerciseApi.findByChapterId(chapterId);
            int? score = 0;
            foreach (Exercise exercise in exercises)
            {
                score += int.Parse(StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).exerciseScore);
            }
            return score;
        }

        
        
     public ResultEntity rateNumber(int? chapterId)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (chapterId != null)
            {
                List<int?> numbers = new List<int?>();
                for (int i = 1; i <= 5; i++)
                {
                    numbers.Add(StudentChapterApi.countByChapterIDAndRate(chapterId, i));
                }
                resultEntity.setData(numbers);
                resultEntity.setState(1);
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }

        
        
    public List<CourseInfo> findCourses(String courseName, int teacherId)
        {
            List<CourseInfo> courseInfos = CourseInfoApi.findByCourseNameAndTeacherID(courseName, teacherId);
            return courseInfos;
        }
        
        
    public List<CourseInfo> findCoursesById(int courseId, int teacherId)
        {
            String courseName = CourseInfoApi.findByCourseID(courseId).courseName;
            return findCourses(courseName, teacherId);
        }

        public List<int?> exerciseScore(int? studentId, int? chapterId, String type)
        {
            int type1 = 0;
            int type2 = 0;
            int type3 = 0;
            if (type.Equals("preview"))
            {
                type1 = 1;
                type3 = 2;
                type2 = 3;
            }
            else
            {
                type1 = 4;
                type3 = 5;
                type2 = 6;
            }

            List<Exercise> exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type1);
            List<int?> scores = new List<int?>();
            foreach (Exercise exercise in exercises)
            {
                if (!StudentExerciseScoreApi.existsByExerciseIdAndStudentId(exercise.exerciseId, studentId))
                    return null;
                scores.Add(int.Parse(StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).exerciseScore));
            }
            exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type3);
            foreach (Exercise exercise in exercises)
            {
                scores.Add(int.Parse(StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).exerciseScore));
            }
            exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type2);
            foreach (Exercise exercise in exercises)
            {
                if (StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).corrected != 0)
                    scores.Add(int.Parse(StudentExerciseScoreApi.findByExerciseIdAndStudentId(exercise.exerciseId, studentId).exerciseScore));
            }
            return scores;
        }

        
        
    public List<List<String>> getPrecourse(String courseName)
        {
            List<List<String>> preCourseSet = new List<List<String>>();
            if (!CourseNameApi.existsByCourseName(courseName))
            {
                return null;
            }
            else
            {
                List<String> temp = getPrecouseName(courseName);
                List<String> temp1 = new List<String>();
                List<String> temp2 = new List<String>();
                preCourseSet.Add(temp);
                foreach (String i in temp)
                {
                    temp1 = getPrecouseName(i);
                    foreach (String j in temp1)
                    {
                        if (!temp2.Contains(j))
                            temp2.Add(j);
                    }
                }
                preCourseSet.Add(temp2);
            }
            return preCourseSet;
        }

        
        
    public List<String> getPrecouseName(String courseName)
        {
            if (!CourseNameApi.existsByCourseName(courseName))
            {
                return null;
            }
            else
            {
                int? couseId = CourseNameApi.findByCourseName(courseName).courseNameID;
                List<CourseRelation> courseRelations = CourseRelationApi.findByCourseNameID(couseId);
                return getCoursesName(courseRelations);
            }
        }

        
        
    public List<String> getCoursesName(List<CourseRelation> courseRelations)
        {
            List<String> temp = new List<String>();
            foreach (CourseRelation courseRelation in courseRelations)
            {
                temp.Add(CourseNameApi.findByCourseNameID(courseRelation.preCourseNameID).courseName);
            }
            return temp;
        }

        
        
    public Boolean learnBad(int studentId, int courseId)
        {
            List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseID(courseId);
            List<StudentChapter> studentChapters = StudentChapterApi.findByChapterIDBetweenAndStudentIDOrderByChapterIDDesc(chapterNodes[0].id, chapterNodes[chapterNodes.Count - 1].id, studentId);
            int? chapterId1 = 0;
            int? chapterId2 = 0;
            for (int i = 0; i < studentChapters.Count; i++)
            {
                if (studentChapters[i].comment != null)
                {
                    chapterId1 = studentChapters[i].chapterID;
                    chapterId2 = chapterId1;
                    for (int j = i + 1; j < studentChapters.Count; j++)
                    {
                        if (studentChapters[j].comment != null)
                        {
                            chapterId2 = studentChapters[j].chapterID;
                            break;
                        }
                    }
                    break;
                }
            }
            if (chapterId1 == 0)
                return false;
            List<StudentChapter> temp1 = StudentChapterApi.findByChapterID(chapterId1);
            List<int?> scores1 = new List<int?>();
            foreach (StudentChapter i in temp1)
            {
                scores1.Add(i.totalScore_2);
            }
            List<StudentChapter> temp2 = StudentChapterApi.findByChapterID(chapterId2);
            List<int?> scores2 = new List<int?>();
            foreach (StudentChapter i in temp2)
            {
                scores2.Add(i.totalScore_2);
            }
            //fragile
            scores1.Sort();
            scores2.Sort();
            if (scores1[(int)(0.4 * temp1.Count)] > StudentChapterApi.findByChapterIDAndStudentID(chapterId1, studentId).totalScore_2 && scores2[(int)(0.4 * temp2.Count)] > StudentChapterApi.findByChapterIDAndStudentID(chapterId2, studentId).totalScore_2)
                return true;
            return false;
        }

        
        
    public String getCourseName(int courseId)
        {
            //fragile
            return CourseNameApi.findByCourseNameID(int.Parse(CourseInfoApi.findByCourseID(courseId).courseName)).courseName;
        }


        //fragile
        public Dictionary<String, float?> userLabel(int studentId)
        {
            List<Takes> takesList = TakesApi.findByStudentID(studentId);
            List<List<int?>> courseList = new List<List<int?>>();
            List<float?> scores = new List<float?>();
            TypeMapper typeMapper = new TypeMapper();
            for (int i = 0; i < 4; i++)
            {
                courseList.Add(new List<int?>());
            }
            foreach (Takes takes in takesList)
            {
                int? temp = CourseClassApi.getByID(takes.courseClassID).courseID;
                int anotherTemp = int.Parse(CourseInfoApi.findByCourseID(temp).courseName);
                if (typeMapper.mapper.ContainsKey(anotherTemp))
                {
                    courseList[typeMapper.mapper[anotherTemp].Value - 1].Add(temp);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (courseList[i].Count == 0)
                {
                    scores.Add(-1.0f);
                    continue;
                }
                List<int?> chapterList = new List<int?>();
                foreach (int? courseId in courseList[i])
                {
                    List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseID(courseId);
                    foreach (ChapterNode chapterNode in chapterNodes)
                        chapterList.Add(chapterNode.id);
                }
                float count = 0;
                float? total = 0;
                foreach (int? integer in chapterList)
                {
                    StudentChapter studentChapter = StudentChapterApi.findByChapterIDAndStudentID(integer, studentId);
                    if (studentChapter != null && studentChapter.scored_2 != null && studentChapter.scored_2 == 1)
                    {
                        count++;
                        total += 100 * studentChapter.totalScore_2 / ChapterContentApi.getByID(integer).exerciseTotal_2;
                    }
                }
                if (count == 0)
                    scores.Add(-1.0f);
                else
                    scores.Add(total / count);
            }
            Dictionary<String, float?> label = new Dictionary<string, float?>();
            label.Add("软件工程理论能力", scores[0]);
            label.Add("基本编程能力", scores[0]);
            label.Add("实践能力", scores[0]);
            label.Add("专业方向能力", scores[0]);
            return label;
        }


        public List<CourseInfo> currentCourse(int year, String semester)
        {
            return CourseInfoApi.findByCourseYearAndCourseSemester(year, semester);
        }

        
        
    /*public List<UnratedChapter> getUnratedChapters(int classId)
        {
            List<UserInfo> userInfos = CourseService.getStudentsByClassID(classId);
            List<ChapterNode> chapterNodes = ChapterContentApi.findByCourseID(CourseClassApi.getByID(classId).courseID);
            List<ChapterNode> chapterNodeList = new List<ChapterNode>();
            foreach (ChapterNode chapterNode in chapterNodes)
            {
                if (chapterNode.exerciseTitle != null)
                    chapterNodeList.Add(chapterNode);
            }
            List<UnratedChapter> unratedChapters = new List<UnratedChapter>();
            foreach (ChapterNode chapterNode in chapterNodeList)
            {
                foreach (UserInfo userInfo in userInfos)
                {
                    StudentChapter studentChapter = StudentChapterApi.findByChapterIDAndStudentID(chapterNode.id, userInfo.userID);
                    if (studentChapter != null)
                    {
                        int? temp = studentChapter.scored_2;
                        if (temp != null)
                        {
                            if (temp == 0)
                            {
                                unratedChapters.Add(new UnratedChapter(chapterNode, userInfo.userID));
                                break;
                            }
                        }
                    }
                }
            }
            return unratedChapters;
        }

        */
        
    public List<CourseAndClassList> currentCourseByTeacherId(int teacherId)
        {
            //fragile
            int month = DateTime.Now.Month + 1;
            int year = DateTime.Now.Year;
            if (month <= 2)
                year--;
            String semester = "";
            if (month >= 3 && month <= 8)
                semester = "春季";
            else
                semester = "秋季";
            List<CourseInfo> courseInfos = CourseInfoApi.findByTeacherIDAndCourseYearAndCourseSemester(teacherId, year, semester);
            List<CourseAndClassList> courseAndClassLists = new List<CourseAndClassList>();
            foreach (CourseInfo courseInfo in courseInfos)
                courseAndClassLists.Add(new CourseAndClassList(courseInfo, CourseClassApi.findByCourseID(courseInfo.courseID), CourseNameApi.findByCourseNameID(int.Parse(courseInfo.courseName)).courseName));
            return courseAndClassLists;
        }

        
        
    public List<CourseAndClassList> currentCourseByStudentId(int studentId)
        {
            //fragile
            int month = DateTime.Now.Month + 1;
            int year = DateTime.Now.Year;
            if (month <= 2)
                year--;
            String semester = "";
            if (month >= 3 && month <= 8)
                semester = "春季";
            else
                semester = "秋季";
            List<Takes> takesList = TakesApi.findByStudentID(studentId);
            List<CourseClass> courseClasses = new List<CourseClass>();
            foreach (Takes takes in takesList)
                courseClasses.Add(CourseClassApi.getByID(takes.courseClassID));
            List<CourseAndClassList> courseAndClassLists = new List<CourseAndClassList>();
            foreach (CourseClass courseClass in courseClasses)
            {
                CourseInfo courseInfo = CourseInfoApi.findByCourseID(courseClass.courseID);
                if (courseInfo.courseYear.Equals(year) && courseInfo.courseSemester.Equals(semester))
                {
                    courseAndClassLists.Add(new CourseAndClassList(courseInfo, courseClass, CourseNameApi.findByCourseNameID(int.Parse(courseInfo.courseName)).courseName));
                }
            }
            return courseAndClassLists;
        }

        
        
    public void setTotalScore(int? chapterId, String type)
        {
            int type1 = 0;
            int type2 = 0;
            int type3 = 0;
            if (type.Equals("preview"))
            {
                type1 = 1;
                type3 = 2;
                type2 = 3;
            }
            else
            {
                type1 = 4;
                type3 = 5;
                type2 = 6;
            }
            //fragile
            List<Exercise> exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type1);
            List<Exercise> temp = new List<Exercise>();
            temp.AddRange(exercises);
            exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type3);
            temp.AddRange(exercises);
            exercises = ExerciseApi.findByChapterIdAndExerciseTypeOrderByExerciseNumber(chapterId, type2);
            temp.AddRange(exercises);
            int? total = 0;
            foreach (Exercise exercise in temp)
                total += exercise.exercisePoint;
            if (type.Equals("preview"))
            {
                //fragile
                ChapterNode chapterNode = ChapterContentApi.getByID(chapterId);
                chapterNode.exerciseTotal_1=total;
                ChapterContentApi.insert(chapterNode);
            }
            else
            {
                ChapterNode chapterNode = ChapterContentApi.getByID(chapterId);
                chapterNode.exerciseTotal_2=total;
                ChapterContentApi.insert(chapterNode);
            }
        }


    }
}
