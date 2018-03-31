using System;
using System.Net;
using System.IO;
using ADV.InternetCrawler.Utility;
using ADV.InternetCrawler.Utility.Logger;
using ADV.InternetCrawler.Interface;
using System.Reflection;

namespace ADV.InternetCrawler
{
    public class DataPoint : LogMessages
    {
        private Int32 id;
        private String name;
        private String uri;
        private String pageUri;
        private String itemUri;
        private String itemName;
        private String itemArticle;
        private String itemPrice;
        private String itemDiscountPrice;
        private String itemPictureUri;

        public Int32 ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.PointID = value;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public String Uri
        {
            get
            {
                return uri;
            }
            set
            {
                uri = value;
            }
        }

        public String PageUri
        {
            get
            {
                return pageUri;
            }
            set
            {
                pageUri = value;
            }
        }

        public String ItemUri
        {
            get
            {
                return itemUri;
            }
            set
            {
                itemUri = value;
            }
        }

        public String ItemName
        {
            get
            {
                return itemName;
            }
            set
            {
                itemName = value;
            }
        }

        public String ItemArticle
        {
            get
            {
                return itemArticle;
            }
            set
            {
                itemArticle = value;
            }
        }

        public String ItemPrice
        {
            get
            {
                return itemPrice;
            }
            set
            {
                itemPrice = value;
            }
        }

        public String ItemDiscountPrice
        {
            get
            {
                return itemDiscountPrice;
            }
            set
            {
                itemDiscountPrice = value;
            }
        }

        public String ItemPictureUri
        {
            get
            {
                return itemPictureUri;
            }
            set
            {
                itemPictureUri = value;
            }
        }

        public DataPoint()
        {
        }

        public void Save()
        {
            try
            {
                CheckConnect();

                if (CheckError())
                {
                    var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                    l_containerObject.SaveDataPoint(this);

                    this.PointID = this.id;
                    AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Сохранена точка данных {id}");
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при сохранении точки данных: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
                ICException.GetException(this.Messages);
            }
        }

        public void Clean()
        {
            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_containerObject.CleanDataPoint(this.PointID);

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Очищена точка данных {id}");
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при очищении точки данных: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
                ICException.GetException(this.Messages);
            }
        }

        public void Remove()
        {
            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_containerObject.RemoveDataPoint(this.PointID);

                this.PointID = this.id;
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Удалена точка данных {id}");
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при удалении точки данных: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
                ICException.GetException(this.Messages);
            }
        }

        private void CheckConnect()
        {
            HttpStatusCode l_statusCode;

            try
            {
                HttpWebRequest l_request = (HttpWebRequest)WebRequest.Create(uri);
                l_request.Method = "GET";
                l_request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
                using (WebResponse l_response = l_request.GetResponse())
                using (Stream l_stream = l_response.GetResponseStream())
                using (StreamReader l_reader = new StreamReader(l_stream))
                {
                    l_statusCode = ((HttpWebResponse)l_response).StatusCode;
                }

                if (l_statusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Получен код ответа [{l_statusCode}] с описанием: {l_statusCode.ToString()}");
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Error, $"Ошибка при сохранении точки данных: {l_exc.Message}", l_exc);
            }
        }
    }
}
