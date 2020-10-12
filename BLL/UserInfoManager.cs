using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class UserInfoManager
    {
        /// <summary>
        /// 查询用户所有信息
        /// </summary>
        /// <returns>返回list</returns>
        public List<UserInfo> SelectAllUserInfo()
        {
            return new UserInfoServices().SelectAll();
        }
    }
}
