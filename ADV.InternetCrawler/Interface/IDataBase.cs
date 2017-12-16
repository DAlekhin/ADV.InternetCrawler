﻿using System;
using System.Collections.Generic;
using ADV.InternetCrawler.Models;

namespace ADV.InternetCrawler.Interface
{
    public interface IDataBase
    {
        String ConnectionString { get; set; }

        void SaveDataPoint(DataPoint DataPoint);

        void SavePointContent(List<PointContent> PointContent);

        DataPoint GetDataPoint(Int32 ID);

        List<DataPoint> GetDataPoints();

        List<DataPointStatsModel> GetDataPointStats();
    }
}
