using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Web.UI
{
    public class Index : Base, IHttpHandler, IRequiresSessionState
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Common.CommFun.ReadSession("Lenceas") as string))
                {
                    Response.Redirect("/admin/login.aspx");
                }
            }
        }
    }
}
