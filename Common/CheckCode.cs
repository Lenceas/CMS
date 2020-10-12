using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Common
{
    public class CheckCode : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        protected string checkname = "code";

        protected int codeW = 100;

        protected int codeH = 43;

        protected int fontSize = 17;

        protected int yPianyi = 0;

        protected string CodeColor = "";

        protected string BackColor = "";

        public static string getCheckCode()
        {
            return getCheckCode(new Random());
        }

        public static string getCheckCode(Random rnd)
        {
            string chkCode = string.Empty;
            char[] character = { '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            //生成验证码字符串 
            for (int i = 0; i < 4; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            return chkCode;
        }

        /// <summary>
        /// 色系
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        Color[] GetColorSer(string color)
        {
            Color[] colors = { };
            switch (color)
            {
                case "red":
                    colors = new Color[] { Color.Red };
                    break;
                case "blue":
                    colors = new Color[] { Color.Blue };
                    break;
                case "black":
                    colors = new Color[] { Color.Black };
                    break;
                case "gray":
                    colors = new Color[] { Color.Gray };
                    break;
                case "green":
                    colors = new Color[] { Color.Green };
                    break;
                default:
                    colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.DarkGoldenrod, Color.Chocolate, Color.Black, Color.Orange, Color.Brown, Color.Brown, Color.DarkBlue };
                    break;
            }
            return colors;
        }

        public void ProcessRequest(HttpContext context)
        {
            //颜色列表，用于验证码、噪线、噪点 
            Color[] color = GetColorSer(CodeColor);
            //字体列表，用于验证码 
            string[] font = { "MicroSoft YaHei", "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" }; //{ "Times New Roman", "Verdana", "Arial"};
            //验证码的字符集，去掉了一些容易混淆的字符 
            Random rnd = new Random();
            string chkCode = getCheckCode(rnd);

            //写入Session
            HttpContext.Current.Session[checkname] = chkCode;
            HttpContext.Current.Session.Timeout = 5;

            //创建画布
            Bitmap bmp = new Bitmap(codeW, codeH);
            Graphics g = Graphics.FromImage(bmp);

            if (string.IsNullOrEmpty(BackColor))
            {
                g.Clear(Color.White);
            }
            else
            {
                g.Clear(ColorTranslator.FromHtml("#" + BackColor));
            }
            //Color.FromArgb(
            //画噪线 
            for (int i = 0; i < 8; i++)
            {
                int x1 = rnd.Next(codeW);
                int y1 = rnd.Next(codeH);
                int x2 = rnd.Next(codeW);
                int y2 = rnd.Next(codeH);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串 
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, fontSize);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * (codeW / 4) + 1, (float)yPianyi);
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * (codeW / 4) + 3, (float)yPianyi);
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * (codeW / 4) + 2, (float)(yPianyi + 1));
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * (codeW / 4) + 2, (float)(yPianyi - 1));
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(Color.White), (float)i * (codeW / 4) + 2, (float)yPianyi);
            }

            //画噪点 
            for (int i = 0; i < 80; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }
            //清除该页输出缓存，设置该页无缓存
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            context.Response.AppendHeader("Pragma", "No-Cache");
            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出 


            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    context.Response.ClearContent();
                    context.Response.ContentType = "image/Jpeg";
                    context.Response.BinaryWrite(ms.ToArray());
                }
            }
            catch (Exception)
            {
                
            }
            finally
            {
                //显式释放资源 
                bmp.Dispose();
                g.Dispose();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
