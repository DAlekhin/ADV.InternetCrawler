using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Interface;
using ADV.InternetCrawler.Utility.Logger;
using System.Reflection;

namespace ADV.InternetCrawler
{
    public class PointContent : LogMessages
    {
        private Int32 pointID;
        private String itemName;
        private String itemArticle;
        private String itemUri;
        private Double itemPrice;
        private Double itemDiscountPrice;
        private String itemPictureUri;

        public Int32 PointID
        {
            get
            {
                return pointID;
            }
            set
            {
                pointID = value;
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

        public Double ItemPrice
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

        public Double ItemDiscountPrice
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

        public List<PointContent> pointContents;

        public PointContent()
        {
        }

        public void SavePointContent()
        {
            try
            {
                if (pointContents.Count > 0)
                {
                    var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                    l_containerObject.SavePointContent(pointContents);
                }
                else
                {
                    throw new Exception("Отсутствуют строки для сохранения контента.");
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при сохранении контента: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
            }
        }
    }
}
