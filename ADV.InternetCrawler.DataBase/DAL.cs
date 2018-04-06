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
                    Int32 l_pullScopeID = l_icEntity.PointContent.Max(m => m.PullScopeID) + 1;

                    foreach (PointContent l_pointContent in _pointContent)
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
                            SaveDateTime = DateTime.Now,
                            PullScopeID = l_pullScopeID
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

        public void StartOperation(Int32 _pointID)
        {
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    if (l_icEntity.DataPointOperations.Where(w => w.PointID == _pointID && w.EndDateTime == null).Count() > 0)
                    {
                        throw new Exception($"Нельзя инициализировать запуск парсинга точки данных {_pointID} пока не закончена прошлая версия.");
                    }

                    EF.DataPointOperations l_dataPointOperation = new DataPointOperations
                    {
                        PointID = _pointID,
                        StartDateTime = DateTime.UtcNow
                    };

                    l_icEntity.DataPointOperations.Add(l_dataPointOperation);

                    l_icEntity.SaveChanges();
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public void EndOperation(Int32 _pointID)
        {
            Int32 l_operationID = 0;

            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    l_operationID = l_icEntity.DataPointOperations.Where(w => w.PointID == _pointID && w.EndDateTime == null).Select(s => s.ID).FirstOrDefault();

                    if (l_operationID == 0)
                    {
                        throw new Exception($"Произошла ошибка при закрытии операции для точки данных {_pointID}");
                    }

                    EF.DataPointOperations l_dataPointOperation = l_icEntity.DataPointOperations.Where(w => w.ID == l_operationID).FirstOrDefault();

                    l_dataPointOperation.EndDateTime = DateTime.UtcNow;

                    l_icEntity.SaveChanges();
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public List<DataPointLogModel> GetDataPointLog(Int32 _headerID)
        {
            List<DataPointLogModel> l_dataPointLog = new List<DataPointLogModel>();

            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    if (_headerID != 0)
                        l_dataPointLog.AddRange(l_icEntity.Logger.Where(w => w.HeaderID == _headerID).Select(s => new DataPointLogModel
                        {
                            ID = s.ID,
                            Module = s.ModuleName,
                            ProcessDateTime = s.ProccessDateTime,
                            URI = s.URI,
                            Level = s.Level,
                            Message = s.Message
                        }).OrderByDescending(o => o.ID));
                    else
                        l_dataPointLog.AddRange(l_icEntity.Logger.Select(s => new DataPointLogModel
                        {
                            ID = s.ID,
                            Module = s.ModuleName,
                            ProcessDateTime = s.ProccessDateTime,
                            URI = s.URI,
                            Level = s.Level,
                            Message = s.Message
                        }).OrderByDescending(o => o.ID));
                };
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_dataPointLog;
        }

        public void CleanDataPoint(Int32 _pointID)
        {
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    l_icEntity.DataPointOperations.RemoveRange(l_icEntity.DataPointOperations.Where(w => w.PointID == _pointID));
                    l_icEntity.PointContent.RemoveRange(l_icEntity.PointContent.Where(w => w.PointID == _pointID));
                    
                    foreach(LoggerHeader l_loggerHeader in l_icEntity.LoggerHeader.Where(w => w.PointID == _pointID).ToList())
                    {
                        l_icEntity.Logger.RemoveRange(l_icEntity.Logger.Where(w => w.HeaderID == l_loggerHeader.ID));
                        l_icEntity.LoggerHeader.Remove(l_loggerHeader);
                    }

                    l_icEntity.SaveChanges();
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public void RemoveDataPoint(Int32 _pointID)
        {
            try
            {
                CleanDataPoint(_pointID);

                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    l_icEntity.DataPoint.RemoveRange(l_icEntity.DataPoint.Where(w => w.ID == _pointID));

                    l_icEntity.SaveChanges();
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public DataPointScheduleModel GetDataPointSchedule(Int32 _pointID)
        {
            DataPointScheduleModel l_dataPointSchedule = new DataPointScheduleModel();

            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    EF.DataPointScheduler l_efDataPointSchedule = l_icEntity.DataPointScheduler.Where(w => w.PointID == _pointID).FirstOrDefault();

                    if (l_efDataPointSchedule == null)
                    {
                        l_dataPointSchedule.PointID = _pointID;
                        l_dataPointSchedule.Enabled = false;
                        l_dataPointSchedule.Monday = false;
                        l_dataPointSchedule.Tuesday = false;
                        l_dataPointSchedule.Wednesday = false;
                        l_dataPointSchedule.Thursday = false;
                        l_dataPointSchedule.Friday = false;
                        l_dataPointSchedule.Saturday = false;
                        l_dataPointSchedule.Sunday = false;

                        l_dataPointSchedule.Interval = 0;
                        l_dataPointSchedule.IntervalType = 0;
                    }
                    else
                    {
                        l_dataPointSchedule.PointID = l_efDataPointSchedule.PointID;
                        l_dataPointSchedule.Enabled = l_efDataPointSchedule.Enabled;
                        l_dataPointSchedule.Monday = l_efDataPointSchedule.Monday;
                        l_dataPointSchedule.Tuesday = l_efDataPointSchedule.Tuesday;
                        l_dataPointSchedule.Wednesday = l_efDataPointSchedule.Wednesday;
                        l_dataPointSchedule.Thursday = l_efDataPointSchedule.Thursday;
                        l_dataPointSchedule.Friday = l_efDataPointSchedule.Friday;
                        l_dataPointSchedule.Saturday = l_efDataPointSchedule.Saturday;
                        l_dataPointSchedule.Sunday = l_efDataPointSchedule.Sunday;

                        if (l_efDataPointSchedule.Interval < 60)
                        {
                            l_dataPointSchedule.Interval = l_efDataPointSchedule.Interval;
                            l_dataPointSchedule.IntervalType = 0; //Минут
                        }
                        else if (l_efDataPointSchedule.Interval < 1440)
                        {
                            l_dataPointSchedule.Interval = l_efDataPointSchedule.Interval / 60;
                            l_dataPointSchedule.IntervalType = 1; //Часов
                        }
                        else if (l_efDataPointSchedule.Interval > 1440)
                        {
                            l_dataPointSchedule.Interval = l_efDataPointSchedule.Interval / 1440;
                            l_dataPointSchedule.IntervalType = 2; //Дней
                        }

                    }
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }

            return l_dataPointSchedule;
        }

        public void SetDataPointSchedule(DataPointScheduleModel _dataPointSchedule)
        {
            try
            {
                using (InternetCrawlerEntities l_icEntity = new InternetCrawlerEntities(this.connectionString))
                {
                    int l_ID = 0;

                    l_ID = l_icEntity.DataPointScheduler.Where(w => w.PointID == _dataPointSchedule.PointID).Select(s => s.ID).FirstOrDefault();

                    if (l_ID == 0)
                    {
                        l_icEntity.DataPointScheduler.Add(new DataPointScheduler
                        {
                            PointID = _dataPointSchedule.PointID,
                            Enabled = _dataPointSchedule.Enabled,
                            Monday = _dataPointSchedule.Monday,
                            Tuesday = _dataPointSchedule.Tuesday,
                            Wednesday = _dataPointSchedule.Wednesday,
                            Thursday = _dataPointSchedule.Thursday,
                            Friday = _dataPointSchedule.Friday,
                            Saturday = _dataPointSchedule.Saturday,
                            Sunday = _dataPointSchedule.Sunday,
                            Interval = _dataPointSchedule.Interval
                        });

                        l_icEntity.SaveChanges();
                    }
                    else
                    {
                        EF.DataPointScheduler l_dataPointSchedule = l_icEntity.DataPointScheduler.Where(w => w.ID == l_ID).FirstOrDefault();

                        l_dataPointSchedule.Enabled = _dataPointSchedule.Enabled;
                        l_dataPointSchedule.Monday = _dataPointSchedule.Monday;
                        l_dataPointSchedule.Tuesday = _dataPointSchedule.Tuesday;
                        l_dataPointSchedule.Wednesday = _dataPointSchedule.Wednesday;
                        l_dataPointSchedule.Thursday = _dataPointSchedule.Thursday;
                        l_dataPointSchedule.Friday = _dataPointSchedule.Friday;
                        l_dataPointSchedule.Saturday = _dataPointSchedule.Saturday;
                        l_dataPointSchedule.Sunday = _dataPointSchedule.Sunday;
                        l_dataPointSchedule.Interval = _dataPointSchedule.Interval;

                        l_icEntity.SaveChanges();
                    }
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

    }
}
