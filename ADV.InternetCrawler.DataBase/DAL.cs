using System;
using ADV.InternetCrawler.Interface;
using ADV.InternetCrawler.Models;
using ADV.InternetCrawler.Utility.Logger.Interface;
using ADV.InternetCrawler.Utility.Logger;
using ADV.InternetCrawler.DataBase.EF;
using System.Linq;
using System.Collections.Generic;

namespace ADV.InternetCrawler.DataBase
{
    public class DAL : IDataBase, ILogger
    {
        private String connectionString;

        public String ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        public void SaveDataPoint(DataPoint _dataPoint)
        {
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    int l_ID = 0;

                    l_ID = l_icEntity.DataPoint.Where(w => w.Name == _dataPoint.Name).Select(s => s.ID).FirstOrDefault();

                    if (l_ID == 0)
                    {
                        EF.DataPoint l_dataPoint = new EF.DataPoint()
                        {
                            Name = _dataPoint.Name,
                            URI = _dataPoint.Uri,
                            PageURI = _dataPoint.PageUri,
                            ItemURI = _dataPoint.ItemUri,
                            ItemName = _dataPoint.ItemName,
                            ItemArticle = _dataPoint.ItemArticle,
                            ItemPrice = _dataPoint.ItemPrice,
                            ItemDiscountPrice = _dataPoint.ItemDiscountPrice,
                            ItemPictureURI = _dataPoint.ItemPictureUri
                        };

                        l_icEntity.DataPoint.Add(l_dataPoint);

                        l_icEntity.SaveChanges();

                        l_ID = l_dataPoint.ID;
                    }
                    else
                    {
                        EF.DataPoint l_dataPoint = l_icEntity.DataPoint.Where(w => w.ID == l_ID).FirstOrDefault();

                        l_dataPoint.URI = _dataPoint.Uri;
                        l_dataPoint.PageURI = _dataPoint.PageUri;
                        l_dataPoint.ItemURI = _dataPoint.ItemUri;
                        l_dataPoint.ItemName = _dataPoint.ItemName;
                        l_dataPoint.ItemArticle = _dataPoint.ItemArticle;
                        l_dataPoint.ItemPrice = _dataPoint.ItemPrice;
                        l_dataPoint.ItemDiscountPrice = _dataPoint.ItemDiscountPrice;
                        l_dataPoint.ItemPictureURI = _dataPoint.ItemPictureUri;

                        l_icEntity.SaveChanges();
                    }

                    _dataPoint.ID = l_ID;
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public DataPoint GetDataPoint(Int32 _id)
        {
            DataPoint l_dataPoint = null;

            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    l_dataPoint = l_icEntity.DataPoint.Where(w => w.ID == _id).Select(s => new DataPoint
                    {
                        ID = s.ID,
                        Name = s.Name,
                        Uri = s.URI,
                        PageUri = s.PageURI,
                        ItemUri = s.ItemURI,
                        ItemName = s.ItemName,
                        ItemArticle = s.ItemArticle,
                        ItemPrice = s.ItemPrice,
                        ItemDiscountPrice = s.ItemDiscountPrice,
                        ItemPictureUri = s.ItemPictureURI
                    }).FirstOrDefault();

                    if (l_dataPoint == null)
                    {
                        throw new Exception($"Запись {_id} не найдена.");
                    }
                };
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_dataPoint;
        }

        public List<DataPoint> GetDataPoints()
        {
            List<DataPoint> l_dataPoints = new List<DataPoint>();

            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    l_dataPoints.AddRange(l_icEntity.DataPoint.Select(s => new DataPoint
                    {
                        ID = s.ID,
                        Name = s.Name,
                        Uri = s.URI,
                        PageUri = s.PageURI,
                        ItemUri = s.ItemURI,
                        ItemName = s.ItemName,
                        ItemArticle = s.ItemArticle,
                        ItemPrice = s.ItemPrice,
                        ItemDiscountPrice = s.ItemDiscountPrice,
                        ItemPictureUri = s.ItemPictureURI
                    }));
                };
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_dataPoints;
        }

        public void SavePointContent(List<PointContent> _pointContent)
        {
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    foreach(PointContent l_pointContent in _pointContent)
                    {
                        EF.PointContent l_efPointContent = new EF.PointContent()
                        {
                            PointID = l_pointContent.PointID,
                            ItemName = l_pointContent.ItemName,
                            ItemArticle = l_pointContent.ItemArticle,
                            ItemPrice = (Decimal)l_pointContent.ItemPrice,
                            ItemDiscountPrice = (Decimal)l_pointContent.ItemDiscountPrice,
                            ItemUri = l_pointContent.ItemUri,
                            ItemPictureUri = l_pointContent.ItemPictureUri,
                            SaveDateTime = DateTime.Now
                        };

                        l_icEntity.PointContent.Add(l_efPointContent);
                    }

                    l_icEntity.SaveChanges();
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public Int32 GetNewHeaderID()
        {
            Int32 l_headerID = 0;
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    EF.LoggerHeader l_loggerHeader = new EF.LoggerHeader()
                    {
                        StartSession = DateTime.UtcNow
                    };

                    l_icEntity.LoggerHeader.Add(l_loggerHeader);
                    l_icEntity.SaveChanges();

                    l_headerID = l_loggerHeader.ID;
                }

                if (l_headerID == 0)
                    throw new Exception($"Невозможно получить новый номер заголовка журналирования.");
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_headerID;
        }

        public void PutLogMessages(Header _loggerHeader)
        {
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    EF.LoggerHeader l_loggerHeader = new EF.LoggerHeader()
                    {
                        PointID = _loggerHeader.pointID,
                        StartSession = _loggerHeader.startSession,
                        FinishSession = _loggerHeader.finishSession
                    };

                    l_icEntity.LoggerHeader.Add(l_loggerHeader);
                    l_icEntity.SaveChanges();

                    if (_loggerHeader.messages.Count > 0)
                        foreach(Message l_message in _loggerHeader.messages)
                        {
                            Logger l_logger = new Logger()
                            {
                                HeaderID = l_loggerHeader.ID,
                                ModuleName = l_message.moduleName,
                                ProccessDateTime = l_message.proccessDateTime,
                                URI = l_message.uri,
                                Level = (int)l_message.messageType,
                                Message = l_message.message,
                                Exception = l_message.exception?.ToString()
                            };

                            l_icEntity.Logger.Add(l_logger);
                        }

                    l_icEntity.SaveChanges();
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public List<DataPointStatsModel> GetDataPointStats()
        {
            List<DataPointStatsModel> l_dataPointStats = new List<DataPointStatsModel>();

            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    var l_result = l_icEntity.Get_DataPointStats();
                    l_dataPointStats.AddRange(l_result);
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_dataPointStats;
        }
    }
}
