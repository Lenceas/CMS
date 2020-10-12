using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class AdminUserManager
    {
        public bool CheckLogin(string adminName,string adminPwd)
        {
            return new AdminUserServices().CheckLogin(adminName,adminPwd);
        }
        public bool CheckRegister(Dictionary<string, object> model)
        {
            return new AdminUserServices().CheckRegister(model);
        }
        public bool CheckHad(string adminName)
        {
            return new AdminUserServices().CheckHad(adminName);
        }
    }
}
