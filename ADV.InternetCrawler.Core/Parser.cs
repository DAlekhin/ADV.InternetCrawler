using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Interface;
using System.Text.RegularExpressions;
using ADV.InternetCrawler.Utility.Logger;
using System.Reflection;

namespace ADV.InternetCrawler.Core
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
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Инициализация парсера веб-ресурса.");

                String l_startBody = PageBody.GetPageBody(dataPoint.Uri);

                GetPageUriList(l_startBody);

                for(Int32 l_page = pageNumbers.Min(); l_page < pageNumbers.Max(); l_page++)
                {
                    String l_pageUri = null;
                    String l_fullUri = null;

                    try
                    {
                        l_pageUri = GetPageUri(l_page, dataPoint.PageUri);
                        l_fullUri = GetFullUri(l_pageUri, dataPoint.Uri);
                    }
                    catch (Exception l_exc)
                    {
                        AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при построении ссылок: {l_exc.Message}", l_exc);
                    }

                    if (CheckError())
                    {
                        GetItemUriList(l_startBody);

                        System.Threading.Thread.Sleep((int)TimeSpan.FromSeconds(timePause).TotalMilliseconds);

                        String l_contentBody = PageBody.GetPageBody(l_fullUri);

                        GetItemUriList(l_contentBody);
                    }                }

                if (CheckError())
                {
                    SaveItemContent();
                }
            }
            catch(Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка инициализации парсера: {l_exc.Message}", l_exc);
            }

            return this.Messages;
        }

        private void GetPageUriList(String _contentBody, Boolean _newIteration = true)
        {
            List<Int32> l_numPages = new List<Int32>();

            try
            {
                if (_newIteration)
                    AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Debug, $"Инициализация загрузки списка ссылок на товары со страниц.");

                Regex l_regexPages = new Regex(dataPoint.PageUri);
                MatchCollection l_matchPages = l_regexPages.Matches(_contentBody);

                foreach(Match l_matchPage in l_matchPages)
                {
                    l_numPages.Add(Int32.Parse(l_matchPage.Groups["Data"].ToString()));
                }

                pageNumbers.AddRange(l_numPages.Distinct().ToList());

                if (_newIteration == true)
                {
                    String l_maxPageUri = GetFullUri(GetPageUri(l_numPages.Max(), dataPoint.PageUri), dataPoint.Uri);
                    GetPageUriList(PageBody.GetPageBody(l_maxPageUri), false);
                }

                if (l_numPages.Max() != pageNumbers.Max())
                {
                    String l_maxPageUri = GetFullUri(dataPoint.PageUri, dataPoint.Uri);
                    GetPageUriList(PageBody.GetPageBody(l_maxPageUri), false);
                }

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Debug, $"Получены индекс максимальный страницы в итерации - {l_numPages.Max()}");

                pageNumbers = pageNumbers.Distinct().ToList();
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка получения страниц: {l_exc.Message}", l_exc);
            }
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

        private String GetPageUri(Int32 _pageNum, String _pattern)
        {
            String l_pageUri = null;

            try
            {
                l_pageUri = _pattern.Replace("(?<Data>\\d+)", _pageNum.ToString());
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_pageUri;
        }

        private void SaveItemContent()
        {
            String l_itemName = "";
            String l_itemArticle = "";
            Double l_itemPrice = 0;
            Double l_itemDiscountPrice = 0;
            String l_itemPictureUri = "";

            try
            {
                foreach(String l_uri in uriItems.Distinct())
                {
                    try
                    {
                        AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, l_uri, MessageType.Trace, $"Инициализация парсинга товара.");

                        System.Threading.Thread.Sleep((int)TimeSpan.FromSeconds(timePause).TotalMilliseconds);

                        String l_contentBody = PageBody.GetPageBody(l_uri);

                        Regex l_regexItemName = new Regex(dataPoint.ItemName);
                        Match l_matchItemName = l_regexItemName.Match(l_contentBody);

                        Regex l_regexItemArticle = new Regex(dataPoint.ItemArticle);
                        Match l_matchItemArticle = l_regexItemArticle.Match(l_contentBody);

                        Regex l_regexItemPrice = new Regex(dataPoint.ItemPrice);
                        Match l_matchItemPrice = l_regexItemPrice.Match(l_contentBody);

                        Regex l_regexItemDiscountPrice = new Regex(dataPoint.ItemDiscountPrice);
                        Match l_matchItemDiscountPrice = l_regexItemDiscountPrice.Match(l_contentBody);

                        Regex l_regexItemPicture = new Regex(dataPoint.ItemPictureUri);
                        Match l_matchItemPicture = l_regexItemPicture.Match(l_contentBody);

                        if (l_matchItemName.Success)
                        {
                            l_itemName = l_matchItemName.Groups["Data"].Value;
                        }

                        if (l_matchItemArticle.Success)
                        {
                            l_itemArticle = l_matchItemArticle.Groups["Data"].Value;
                        }

                        if (l_matchItemPrice.Success)
                        {
                            try
                            {
                                l_itemPrice = Double.Parse(l_matchItemPrice.Groups["Data"].Value);
                            }
                            catch { }
                        }

                        if (l_matchItemDiscountPrice.Success)
                        {
                            try
                            {
                                l_itemDiscountPrice = Double.Parse(l_matchItemDiscountPrice.Groups["Data"].Value);
                            }
                            catch { }
                        }

                        if (l_matchItemPicture.Success)
                        {
                            l_itemPictureUri = GetFullUri(l_matchItemPicture.Groups["Data"].Value, dataPoint.Uri);
                        }

                        pointContents.Add(new PointContent
                        {
                            PointID = dataPoint.ID,
                            ItemName = l_itemName,
                            ItemArticle = l_itemArticle,
                            ItemPrice = l_itemPrice,
                            ItemDiscountPrice = l_itemDiscountPrice,
                            ItemUri = l_uri,
                            ItemPictureUri = l_itemPictureUri
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
