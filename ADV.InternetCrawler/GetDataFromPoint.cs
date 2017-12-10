using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Interface;

namespace ADV.InternetCrawler
{
    public class GetDataFromPoint
    {
        public DataPoint dataPoint;

        public DataPoint GetDataPoint(Int32 _id)
        {
            DataPoint l_dataPoint = new DataPoint();

            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_dataPoint = l_containerObject.GetDataPoint(_id);
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
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_dataPoints = l_containerObject.GetDataPoints();
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
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

                var l_containerObject = (IParser)Utility.Container.GetObject("Parser");
                l_containerObject.PullRequest(l_dataPoint);
                l_pointContents = l_containerObject.PutPointContent();

                SaveDataPointContent(l_pointContents);
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
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
                    var l_containerObject = (IParser)Utility.Container.GetObject("Parser");
                    l_containerObject.PullRequest(l_dataPoint);
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        private void SaveDataPointContent(List<PointContent> _pointContents)
        {
            try
            {
                var l_containerObject = (IDataBase)Utility.Container.GetObject("DAL");
                l_containerObject.SavePointContent(_pointContents);
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }
    }
}
