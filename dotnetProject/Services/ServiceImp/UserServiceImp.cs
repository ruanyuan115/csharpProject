using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetProject.Entity;
using PersistentLayer.Mapper;
using PersistentLayer.Apis;

namespace dotnetProject.Services.ServiceImp
{
    public class UserServiceImp :UserService
    {
        public ResultEntity addUser(UserInfo userInfo)
        {
            ResultEntity resultEntity = new ResultEntity();
            if (userInfo != null)
            {
                if (UserApi.findByMail(userInfo.mail) == null)
                {
                    userInfo.password=(BCrypt.Net.BCrypt.HashPassword(userInfo.password));

                    int? isSuccess = UserApi.insert(userInfo);
                    UserInfo result = UserApi.findByMail(userInfo.mail);
                    result.password="";
                    resultEntity.setData(result);

                    if (result.userID != null)
                    {
                        resultEntity.setState(1);
                        resultEntity.setMessage("注册成功！");
                    }
                    else
                    {
                        resultEntity.setMessage("数据插入失败！");
                        resultEntity.setState(0);
                    }
                }
                else
                {
                    resultEntity.setState(0);
                    resultEntity.setMessage("该邮箱已被注册！");
                }
            }
            else
            {
                resultEntity.setMessage("传入参数为空！");
                resultEntity.setState(0);
            }
            return resultEntity;
        }
        public ResultEntity login(String userName,String password)
        {
            ResultEntity resultEntity = new ResultEntity();
            UserInfo userInfo = UserApi.findByMail(userName);
            if (userInfo != null&&BCrypt.Net.BCrypt.Verify(password, userInfo.password))
            {
                Dictionary<String, Object> resultMap = new Dictionary<string, object>();
                resultMap.Add("userID", userInfo.userID);
                resultMap.Add("mail", userInfo.mail);
                resultMap.Add("gender", userInfo.gender);
                resultMap.Add("name", userInfo.name);
                resultMap.Add("role", userInfo.role);
                resultMap.Add("workID", userInfo.workID);
                resultMap.Add("token", "abc");
                resultEntity.setState(1);
                resultEntity.setMessage("登陆成功！");
                resultEntity.setData(resultMap);
            }
            else
            {
                resultEntity.setState(0);
                resultEntity.setMessage("登陆失败！");
                resultEntity.setData(null);
            }
            return resultEntity;
        }
    }
}
