using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace Web.Admin.ashx
{
    /// <summary>
    /// CheckCode 的摘要说明
    /// </summary>
    public class CheckCode : Common.CheckCode
    {
        public CheckCode()
        {
            string checkname = CommFun.GetQueryString("name");
            string color = CommFun.GetQueryString("color");
            string height = CommFun.GetQueryString("height");
            string width = CommFun.GetQueryString("width");
            string fontsize = CommFun.GetQueryString("size");
            string offset = CommFun.GetQueryString("offset");
            string bgcolor = CommFun.GetQueryString("bgcolor");

            base.checkname = string.IsNullOrEmpty(checkname) ? "admin" : checkname;
            base.codeH = string.IsNullOrEmpty(height) ? 43 : int.Parse(height);
            base.codeW = string.IsNullOrEmpty(width) ? 100 : int.Parse(width);
            base.fontSize = string.IsNullOrEmpty(fontsize) ? 17 : int.Parse(fontsize);
            base.yPianyi = string.IsNullOrEmpty(offset) ? 0 : int.Parse(offset);
            base.CodeColor = string.IsNullOrEmpty(color) ? "" : color;
            base.BackColor = string.IsNullOrEmpty(bgcolor) ? "" : bgcolor;
        }
    }
}