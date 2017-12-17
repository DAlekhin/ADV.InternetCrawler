using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADV.InternetCrawler.ControlPanel.Models;
using ADV.InternetCrawler.Models;
using System.Web.Script.Serialization;

namespace ADV.InternetCrawler.ControlPanel.Controllers
{
    public class DataPointController : Controller
    {
        public ActionResult NewDataPoint()
        {
            return View();
        }

        public ActionResult ShowDataPoint()
        {
            DataPointStatsModel l_dataPointStats = new DataPointStatsModel();

            ViewBag.Title = "Список точек загрузки данных";

            return View(l_dataPointStats);
        }

        [HttpPost]
        public JsonResult GetDataPointStats()
        {
            List<DataPointStatsModel> l_dataPointStats = new List<DataPointStatsModel>();

            try
            {
                GetDataFromPoint l_dataPoint = new GetDataFromPoint();

                l_dataPointStats = l_dataPoint.GetDataPointStats();
            }
            catch
            {
            }

            return new LargeJsonResult() { Data = l_dataPointStats };
        }
    }
}