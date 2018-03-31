using System;
using System.Collections.Generic;
using ADV.InternetCrawler.Models;

namespace ADV.InternetCrawler.Interface
{
    public interface IDataBase
    {
        String ConnectionString { get; set; }

        void SaveDataPoint(DataPoint DataPoint);

        void StartOperation(Int32 PointID);

        void EndOperation(Int32 PointID);

        void SavePointContent(List<PointContent> PointContent);

        void CleanDataPoint(Int32 PointID);

        void RemoveDataPoint(Int32 PointID);

        DataPoint GetDataPoint(Int32 ID);

        List<DataPoint> GetDataPoints();

        List<DataPointStatsModel> GetDataPointStats();

        List<DataPointLogModel> GetDataPointLog(Int32 HeaderID);
    }
}
