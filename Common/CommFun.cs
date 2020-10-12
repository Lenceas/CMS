using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Security.Cryptography;
using System.Web.SessionState;

namespace Common
{
    public static class CommFun
    {

        #region 写文件日志
        /// <summary>
        /// 测试-写文件
        /// </summary>
        /// <param name="msgs">要写入的数据</param>
        public static void WirteFile(string msgs)
        {
            /*--------写文件,测试有无数据----------*/
            string filepath = System.Web.HttpContext.Current.Server.MapPath("~/log.txt");
            bool IsHas = File.Exists(filepath);
            if (IsHas)
            {
                StreamWriter sw = new StreamWriter(filepath, true);
                sw.WriteLine(msgs + " , " + DateTime.Now.ToString());
                sw.Close();
            }
            else
            {
                FileStream fs = new FileStream(filepath, FileMode.Append);
                fs.Close();
                bool IsCreate = File.Exists(filepath);
                if (IsCreate)
                {
                    StreamWriter sw = new StreamWriter(filepath, true);
                    sw.WriteLine(msgs + " , " + DateTime.Now.ToString());
                    sw.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(filepath, true);
                    sw.WriteLine(DateTime.Now.ToString() + ",失败");
                    sw.Close();
                }
            }
            /*--------写文件,测试有无数据----------*/
        }

        public static void Log(string filename, string cont)
        {
            System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath("~/" + filename + ".txt"), cont);
        }
        #endregion

        #region 时间戳
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
        #endregion

        #region 生成指定长度的字符串
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }

            return ReturnStr;
        }
        #endregion

        #region 生成日期随机码
        /// <summary>
        /// 生成日期随机码
        /// </summary>
        /// <returns></returns>
        public static string GetRamCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        #endregion

        #region 生成随机字母或数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <returns></returns>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        /// <summary>
        /// 生成随机字母字符串(数字字母混和)
        /// </summary>
        /// <param name="codeCount">待生成的位数</param>
        public static string GetCheckCode(int codeCount)
        {
            string str = string.Empty;
            int rep = 0;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// 根据日期和随机码生成订单号
        /// </summary>
        /// <returns></returns>
        public static string GetOrderNumber()
        {
            string num = DateTime.Now.ToString("yyMMddHHmmss");//yyyyMMddHHmmssms
            return num + Number(2, true).ToString();
        }
        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数  
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
        #endregion

        #region 截取字符长度

        /// <summary>
        /// 截断字符串
        /// <para>默认DropHtml</para>
        /// </summary>
        /// <param name="cont">输入字符串</param>
        /// <param name="length">要截取的长度</param>
        /// <param name="connStr">字数超出的后缀 默认 ...</param>
        /// <returns></returns>
        public static string CutString(string cont, int length, string connStr = "")
        {
            if (string.IsNullOrEmpty(cont))
            {
                return "";
            }
            cont = DropHTML(cont);
            if (cont.Length > length + 1)
            {
                cont = cont.Substring(0, length) + connStr;
            }
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = "...";
            }
            return cont + connStr;
        }

        /// <summary>
        /// 截取字符串 , 按照字符长度截取
        /// <para>1个汉字一个字母均为1个字符计算</para>
        /// </summary>
        /// <param name="inputString">输入的字符串</param>
        /// <param name="len">截取长度</param>
        /// <param name="DropHtml">是否去掉html</param>
        /// <param name="moreStr">超出的占位符 , 默认 ... </param>
        /// <returns></returns>
        public static string CutString(string inputString, int len, bool DropHtml, string moreStr = "")
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return "";
            }
            if (DropHtml)
            {
                inputString = DropHTML(inputString);
            }
            if (inputString.Length > len + 1)
            {
                inputString = inputString.Substring(0, len);
            }
            if (string.IsNullOrEmpty(moreStr))
            {
                moreStr = "...";
            }
            return inputString + moreStr;
        }

        /// <summary>
        /// 截取字符长度 , 字符使用 ASCIIEncoding 字节来标示
        /// <para> 并非使用字符串的length  , 使用的时候注意区别 </para>
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString2(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }


        /// <summary>
        /// 取得内容的前X字符
        /// </summary>
        /// <param name="cont">内容</param>
        /// <param name="length">取得位数</param>
        /// <param name="moreStr">超出后的字符</param>
        /// <returns></returns>
        public static string GetIntroString(string cont, int length, string moreStr)
        {
            return CutString(cont, length, "");
        }
        #endregion

        #region 清除HTML标记
        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring.Replace("&emsp;", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }
        #endregion


        #region 过滤特殊字符
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string Htmls(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!string.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }
        #endregion

        #region 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.Length < 1)
            {
                return "";
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="_url">页面地址</param>
        /// <param name="_keys">参数名称</param>
        /// <param name="_values">参数值</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        #endregion

        #region URL请求数据
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

        #endregion

        #region 取得内容中的图片
        /// <summary>
        /// 利用正则表达式获取符合条件的字符串
        /// </summary>
        /// <param name="Cont">传入的内容</param>
        /// <param name="RegStr">正则表达式</param>
        /// <param name="KeyName">关键属性名</param>
        /// <returns></returns>
        public static ArrayList GetPropByRegStr(string Cont, string RegStr, string KeyName)
        {
            ArrayList resultStr = new ArrayList();
            Regex r = new Regex(RegStr, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(Cont);
            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups[KeyName].Value.ToLower());
            }
            if (resultStr.Count > 0)
            {
                return resultStr;
            }
            else
            {
                //没有东西的时候返回空字符
                resultStr.Add("");
                return resultStr;
            }
        }
        /// <summary>
        /// 获取文章中的第一个图片,如果不存在,返回默认图片
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="isImg">是否需要返回默认图片</param>
        /// <returns></returns>
        public static string GetFirstImg(string contents, bool isImg)
        {
            return GetImgSrcByCont(HttpContext.Current.Server.UrlDecode(contents), 0, isImg);
        }
        /// <summary>
        /// 获取文章中的第n个图片,如果不存在,返回默认图片
        /// </summary>
        /// <param name="contents">内容</param>
        /// <param name="index">索引 0 开始</param>
        /// <param name="isImg">是否需要返回默认图片</param>
        /// <returns></returns>
        public static string GetImgSrcByCont(string contents, int index, bool isImg)
        {
            string ImgPath = GetPropByRegStr(contents, @"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", "src")[index].ToString();
            if (!string.IsNullOrEmpty(ImgPath))
            {
                return ImgPath;
            }
            else
            {
                if (isImg)
                {
                    return GetAppSetting("DefaultWebImg");
                }
                else
                {
                    return "";
                }
            }
        }
        //使用方法
        //Image1.ImageUrl =ImgHelper.getImgUrl("字符串",@"<IMG[^>]+src=\s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>\s]+))\s*[^>]*>", "src")[0].ToString();//这里是获取数组中第一个图片地址，当然也可以获取文章中其他图片，只需修改索引号。
        #endregion



        #region 获取参数 querystring from params
        /// <summary>
        /// 获取QueryString 为空的话返回null
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetQueryString(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString[val]))
                {
                    return null;
                }
                else
                {
                    return System.Web.HttpContext.Current.Request.QueryString[val].ToString();
                }
            }
        }
        /// <summary>
        /// 获取QueryString 为空的话返回null
        /// </summary>
        /// <param name="context"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetQueryString(System.Web.HttpContext context, string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(context.Request.QueryString[val]))
                {
                    return null;
                }
                else
                {
                    return context.Request.QueryString[val].ToString();
                }
            }
        }
        /// <summary>
        /// 获取Form传过来的值  为空返回null
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetForm(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form[val]))
                {
                    return null;
                }
                else
                {
                    return System.Web.HttpContext.Current.Request.Form[val].ToString();
                }
            }
        }
        /// <summary>
        /// 获取Params 为空返回null
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetParams(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }
            else
            {
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Params[val]))
                {
                    return null;
                }
                else
                {
                    return System.Web.HttpContext.Current.Request.Params[val].ToString();
                }
            }
        }
        #endregion

        #region Cookie


        #region 设置Cookie方法 


        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="CookieValue">Cookie值</param>
        /// <param name="MinExpires">过期时间 , 分钟数 默认 20分钟 <para>如果设置为0 , 则浏览器关闭后Cookie失效</para> <para>如果设置为负数 , 则为删除Cookie</para> </param>
        /// <param name="Domain">设置Cookie的域名类型 <para>[ 1=当前网站域名【Request.Url.Host】（如www.baidu.com）]</para> <para>[ 2=当前网站顶级域名【Request.Url.Host最后两部分】（如.baidu.com）]</para></param>
        /// <param name="Path">Cookie路径，默认为"/",可指定路径（如/test/,/test/temp/）</param>
        public static void AddCookie(string CookieName, string CookieValue, double MinExpires = 20.0, int Domain = 1, string Path = "/")
        {
            var cookie = new HttpCookie(CookieName);
            cookie.Value = CookieValue;
            if (MinExpires > 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(MinExpires);
            }
            if (MinExpires < 0)
            {
                cookie.Expires = DateTime.Now.AddMinutes(MinExpires);
            }
            if (Domain == 1)
            {
                cookie.Domain = HttpContext.Current.Request.Url.Host.ToLower();
            }
            else if (Domain == 2)
            {
                var host = HttpContext.Current.Request.Url.Host.ToLower();
                var arr = host.Split('.');
                if (arr.Length > 2)
                    host = "." + arr[arr.Length - 2] + "." + arr[arr.Length - 1];
                cookie.Domain = host;
            }

            cookie.Path = Path;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <returns>Cookie值</returns>
        public static string GetCookie(string CookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[CookieName];
            return cookie == null ? "" : cookie.Value;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="CookieName">Cookie名称</param>
        /// <param name="Domain">设置Cookie的域名类型</param>
        /// <param name="Path">Cookie路径，默认为"/",可指定路径（如/test/,/test/temp/）</param>
        public static void DelCookie(string CookieName, int Domain = 1, string Path = "/")
        {
            AddCookie(CookieName, "", -99, Domain, Path);
        }
        #endregion 


        #region 过时方法
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        [Obsolete("此方法不再推荐 , 应当使用 AddCookie 代替它", true)]
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        [Obsolete("此方法不再推荐 , 应当使用 GetCookie 代替它", true)]
        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = strValue;
            //cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        [Obsolete("此方法不再推荐 , 应当使用 AddCookie 代替它", true)]
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = strValue;
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        [Obsolete("此方法不再推荐 , 应当使用 GetCookie 代替它", true)]
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return HttpContext.Current.Request.Cookies[strName][key].ToString();

            return "";
        }
        #endregion


        #endregion

        #region Session
        [Obsolete("此方法已停用 , 建议使用AddSession代替", true)]
        public static void WriteSession(string strName, object strValue)
        {
            HttpContext.Current.Session[strName] = strValue;
        }
        public static object ReadSession(string strName)
        {
            if (HttpContext.Current.Session[strName] == null)
            {
                return null;
            }
            return HttpContext.Current.Session[strName];
        }

        /// <summary>
        /// 添加Session，调动有效期为30分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        public static void AddSession(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 30;
        }
        /// <summary>
        /// 添加Session，调动有效期为30分钟
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        public static void AddsSession(string strSessionName, string[] strValues)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = 30;
        }
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void AddSession(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }
        /// <summary>
        /// 添加Session
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValues">Session值数组</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void AddsSession(string strSessionName, string[] strValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }
        /// <summary>
        /// 读取某个Session对象值
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string GetSession(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }
        /// <summary>
        /// 读取某个Session对象值数组
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值数组</returns>
        public static string[] GetsSession(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[strSessionName];
            }
        }
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void DelSession(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
        #endregion

        #region 获取webconfig配置
        /// <summary>
        /// 获取WebConfig中AppSetting的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
        }
        /// <summary>
        /// 获取WebConfig中Connection的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnection(string key)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        #endregion



        #region MD5
        /// <summary>
        /// MD5 32位大写
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static string Upper32(string s)
        {
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();
            return s.ToUpper();
        }

        /// <summary>
        /// MD5 32位小写
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static string Lower32(string s)
        {
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();
            return s.ToLower();
        }

        /// <summary>
        /// MD5 16位大写
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static string Upper16(string s)
        {
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();
            return s.ToUpper();
        }

        /// <summary>
        /// MD5 16位小写
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static string Lower16(string s)
        {
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();
            return s.ToLower();
        }

        #endregion

        #region 重启当前Web应用程序
        /// <summary>
        /// 重启当前Web应用程序
        /// </summary>
        public static void ReStartCurWeb()
        {
            File.SetLastWriteTime(HttpContext.Current.Server.MapPath("~/web.config"), DateTime.Now);
        }
        #endregion

        #region   时间转换类
        /// <summary>  
        /// 本地时间转成GMT时间  
        /// string s = ToGMTString(DateTime.Now);
        /// 本地时间为：2014-9-29 15:04:39
        /// 转换后的时间为：Thu, 29 Sep 2014 07:04:39 GMT
        /// </summary>  
        public static string ToGMTString(DateTime dt)
        {
            return dt.ToUniversalTime().ToString("r");
        }

        /// <summary>  
        /// 本地时间转成GMT格式的时间
        /// string s = ToGMTFormat(DateTime.Now);
        /// 本地时间为：2014-9-29 15:04:39
        /// 转换后的时间为：Thu, 29 Sep 2014 15:04:39 GMT+0800
        /// </summary>  
        public static string ToGMTFormat(DateTime dt)
        {
            return dt.ToString("r") + dt.ToString("zzz").Replace(":", "");
        }

        /// <summary>  
        /// GMT时间转成本地时间 
        /// DateTime dt1 = GMT2Local("Thu, 29 Sep 2014 07:04:39 GMT");
        /// 转换后的dt1为：2014-9-29 15:04:39
        /// DateTime dt2 = GMT2Local("Thu, 29 Sep 2014 15:04:39 GMT+0800");
        /// 转换后的dt2为：2014-9-29 15:04:39
        /// </summary>  
        /// <param name="gmt">字符串形式的GMT时间</param>  
        /// <returns></returns>  
        public static DateTime GMT2Local(string gmt)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                string pattern = "";
                if (gmt.IndexOf("+0") != -1)
                {
                    gmt = gmt.Replace("GMT", "");
                    pattern = "ddd, dd MMM yyyy HH':'mm':'ss zzz";
                }
                if (gmt.ToUpper().IndexOf("GMT") != -1)
                {
                    pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
                }
                if (pattern != "")
                {
                    dt = DateTime.ParseExact(gmt, pattern, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                    dt = dt.ToLocalTime();
                }
                else
                {
                    dt = Convert.ToDateTime(gmt);
                }
            }
            catch
            {
            }
            return dt;
        }

        /// <summary>
        /// 获取国家授时中心网提供的时间。（授时中心连接经常出状况，暂时用百度网代替）
        /// 通过分析网页报头，查找Date对应的值，获得GMT格式的时间。可通过GMT2Local(string gmt)方法转化为本地时间格式。
        /// 用法 DateTime netTime = GetNetTime.GMT2Local(GetNetTime.GetNetDate());
        /// </summary>
        /// <returns></returns>
        public static string GetNetDate()
        {
            try
            {
                //WebRequest request = WebRequest.Create("http://www.time.ac.cn");//国家授时中心经常连接不上
                WebRequest request = WebRequest.Create("http://www.baidu.com");
                request.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection myWebHeaderCollection = response.Headers;
                for (int i = 0; i < myWebHeaderCollection.Count; i++)
                {
                    string header = myWebHeaderCollection.GetKey(i);
                    string[] values = myWebHeaderCollection.GetValues(header);
                    if (header.Length <= 0 || header == null)
                    {
                        return null;
                    }
                    else if (header == "Date")
                    {
                        return values[0];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SessionId获取
        /// <summary>
        /// 返回SessionID
        /// </summary>
        /// <returns></returns>
        public static string ReturnSessionID()
        {
            return HttpContext.Current.Session.SessionID;
        }
        /// <summary>
        /// 返回SessionID
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string ReturnSessionID_1(HttpContext context)
        {
            return context.Session.SessionID;
        }
        #endregion

        #region 实现类似JS的ESCAPE方法
        /// <summary>
        /// Escape 转码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                sb.Append((Char.IsLetterOrDigit(c)
                || c == '-' || c == '_' || c == '\\'
                || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
            }
            return sb.ToString();
        }
        /// <summary>
        /// Escape 解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UnEscape(string str)
        {
            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }
            return sb.ToString();
        }
        #endregion

    }
}
