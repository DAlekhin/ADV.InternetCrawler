using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Interface;
using System.Text.RegularExpressions;
using ADV.InternetCrawler.Utility.Logger;
using System.Reflection;

namespace ADV.InternetCrawler.Core.Test
{
    public class Parser : LogMessages, IParser
    {
        private DataPoint dataPoint;
        private String userAgent;
        private Int32 timePause;
        private List<Int32> pageNumbers = new List<Int32>();
        private List<String> uriItems = new List<String>();
        private List<PointContent> pointContents = new List<PointContent>();

        public String UserAgent
        {
            get
            {
                return userAgent;
            }
            set
            {
                userAgent = value;
            }
        }

        public Int32 TimePause
        {
            get
            {
                return timePause;
            }
            set
            {
                timePause = value;
            }
        }

        public Parser()
        {
        }

        public List<Message> PullRequest(DataPoint _dataPoint)
        {
            dataPoint = _dataPoint;

            try
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Инициализация тестового парсера веб-ресурса.");

                String l_startBody = PageBody.GetPageBody(dataPoint.Uri);

                GetItemUriList(l_startBody);

                if (CheckError())
                {
                    SaveItemContent();
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка инициализации парсера: {l_exc.Message}", l_exc);
            }

            return this.Messages;
        }

        private void GetItemUriList(String _contentBody)
        {
            try
            {
                Regex l_regexItems = new Regex(dataPoint.ItemUri);
                MatchCollection l_matchItems = l_regexItems.Matches(_contentBody);

                foreach (Match l_matchItem in l_matchItems)
                {
                    uriItems.Add(GetFullUri(l_matchItem.Groups["Data"].Value, dataPoint.Uri));
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        private String GetFullUri(String _uri, String _pattern)
        {
            String l_fullUri = null;

            try
            {
                Regex l_uriPattern = new Regex(@"http(s)?://[^/]+(?=/)");
                l_fullUri = l_uriPattern.Match(_pattern).Value + _uri;

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, l_fullUri, MessageType.Trace, $"Получена ссылка.");
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_fullUri;
        }

        private void SaveItemContent()
        {
            try
            {
                foreach (String l_uri in uriItems.Distinct())
                {
                    try
                    {
                        AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, l_uri, MessageType.Info, $"Инициализация парсинга товара {l_uri}");

                        System.Threading.Thread.Sleep((int)TimeSpan.FromSeconds(timePause).TotalMilliseconds);

                        String l_contentBody = PageBody.GetPageBody(l_uri);

                        Regex l_regexItemName = new Regex(dataPoint.ItemName ?? "");
                        Match l_matchItemName = l_regexItemName.Match(l_contentBody);

                        Regex l_regexItemArticle = new Regex(dataPoint.ItemArticle ?? "");
                        Match l_matchItemArticle = l_regexItemArticle.Match(l_contentBody);

                        Regex l_regexItemPrice = new Regex(dataPoint.ItemPrice ?? "");
                        Match l_matchItemPrice = l_regexItemPrice.Match(l_contentBody);

                        Regex l_regexItemDiscountPrice = new Regex(dataPoint.ItemDiscountPrice ?? "");
                        Match l_matchItemDiscountPrice = l_regexItemDiscountPrice.Match(l_contentBody);

                        Regex l_regexItemPicture = new Regex(dataPoint.ItemPictureUri ?? "");
                        Match l_matchItemPicture = l_regexItemPicture.Match(l_contentBody);

                        pointContents.Add(new PointContent
                        {
                            PointID = dataPoint.ID,
                            ItemName = l_matchItemName.Success ? l_matchItemName.Groups["Data"].Value : "",
                            ItemArticle = l_matchItemArticle.Success ? l_matchItemArticle.Groups["Data"].Value : "",
                            ItemPrice = l_matchItemPrice.Success ? Double.Parse(l_matchItemPrice.Groups["Data"].Value) : 0,
                            ItemDiscountPrice = l_matchItemDiscountPrice.Success ? Double.Parse(l_matchItemDiscountPrice.Groups["Data"].Value) : 0,
                            ItemUri = l_uri,
                            ItemPictureUri = l_matchItemPicture.Success ? GetFullUri(l_matchItemPicture.Groups["Data"].Value, dataPoint.Uri) : ""
                        });

                        AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, l_uri, MessageType.Info, $"Товар {l_matchItemName.Groups["Data"].Value} успешно сохранен.");
                    }
                    catch (Exception l_exc)
                    {
                        AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, l_uri, MessageType.Fatal, $"Ошибка при парсинге товара: {l_exc.Message}", l_exc);
                    }
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при парсинге товаров: {l_exc.Message}", l_exc);
            }
        }

        public List<PointContent> PutPointContent()
        {
            return pointContents;
        }
    }
}
