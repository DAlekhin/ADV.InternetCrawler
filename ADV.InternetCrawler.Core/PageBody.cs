using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

namespace ADV.InternetCrawler.Core
{
    public static class PageBody
    {
        public static String GetPageBody(String _pageUri)
        {
            String l_pageBody = null;
            HttpStatusCode l_statusCode;

            try
            {
                HttpWebRequest l_request = (HttpWebRequest)WebRequest.Create(_pageUri);
                l_request.Method = "GET";
                //l_request.Headers.Add("Accept-Language", "ru");
                //l_request.Headers.Add("Accept-Encoding", "gzip, deflate");
                l_request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                using (WebResponse l_response = l_request.GetResponse())
                using (Stream l_stream = l_response.GetResponseStream())
                using (StreamReader l_reader = new StreamReader(l_stream, Encoding.Default))
                {
                    l_statusCode = ((HttpWebResponse)l_response).StatusCode;
                    l_pageBody = l_reader.ReadToEnd();
                }

                if (l_statusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Получение данных прервано. Получен код ответа [{l_statusCode}] с описанием: {l_statusCode.ToString()}.");
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_pageBody;
        }

    }
}