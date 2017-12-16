using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Interface;
using ADV.InternetCrawler.Utility.Logger;
using System.Reflection;
using ADV.InternetCrawler.Utility;

namespace ADV.InternetCrawler
{
    public class GetDataFromPoint : LogMessages
    {
        private DataPoint dataPoint;

        public GetDataFromPoint()
        {
        }

        public DataPoint GetDataPoint(Int32 _id)
        {
            DataPoint l_dataPoint = new DataPoint();

            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_dataPoint = l_containerObject.GetDataPoint(_id);

                this.PointID = _id;
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Получена точка данных {_id}");
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при получении точки данных {_id}: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
                ICException.GetException(this.Messages);
            }

            return l_dataPoint;
        }

        public List<DataPoint> GetDataPoints()
        {
            List<DataPoint> l_dataPoints = new List<DataPoint>();

            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_dataPoints = l_containerObject.GetDataPoints();

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Получено {l_dataPoints.Count} точек данных");
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при получении списка всех точек данных: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
                ICException.GetException(this.Messages);
            }

            return l_dataPoints;
        }

        public void PullDataFromPoint(Int32 _id)
        {
            DataPoint l_dataPoint = new DataPoint();
            List<PointContent> l_pointContents = new List<PointContent>();

            try
            {
                l_dataPoint = GetDataPoint(_id);

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Trace, $"Инициализация получения контента из точки {_id}");

                var l_containerObject = (IParser)Utility.Container.GetObject("Parser");
                var l_messages = l_containerObject.PullRequest(l_dataPoint);
                AddToMessages(l_messages);

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Trace, $"Инициализация сохранения контента из точки {_id}");
                l_pointContents = l_containerObject.PutPointContent();

                SaveDataPointContent(l_pointContents);
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при получении контента из точки {_id}: {l_exc.Message}", l_exc);
            }
        }

        public void PullDataFromPoints()
        {
            List<DataPoint> l_dataPoints = new List<DataPoint>();

            try
            {
                l_dataPoints = GetDataPoints();

                //Переделать на параллелизм
                foreach(DataPoint l_dataPoint in l_dataPoints)
                {
                    this.PointID = l_dataPoint.PointID;
                    AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Trace, $"Инициализация сохранения контента из точки {l_dataPoint.PointID}");

                    var l_containerObject = (IParser)Utility.Container.GetObject("Parser");
                    l_containerObject.PullRequest(l_dataPoint);
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при получении контента из всех точек: {l_exc.Message}", l_exc);
            }
        }

        private void SaveDataPointContent(List<PointContent> _pointContents)
        {
            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_containerObject.SavePointContent(_pointContents);

                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, null, MessageType.Info, $"Контент успешно сохранен.");
            }
            catch (Exception l_exc)
            {
                AddToMessage(this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "", MessageType.Fatal, $"Ошибка при сохранении контента: {l_exc.Message}", l_exc);
            }
            finally
            {
                this.PutMessages();
                ICException.GetException(this.Messages);
            }
        }
    }
}
