using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADV.InternetCrawler.ControlPanel.Models;
using ADV.InternetCrawler.Models;

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

        //[HttpPost]
        //public JsonResult GetDataPointStats()
        //{
        //    List<DataPointStatsModel> l_dataPointStats = new List<DataPointStatsModel>();
            
        //    try
        //    {

        //    }
        //    catch (Exception l_exc)
        //    {

        //    }

        //    return l_dataPointStats;
        //}
    }
}