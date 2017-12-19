using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADV.InternetCrawler.ControlPanel.Models;
using ADV.InternetCrawler.Models;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace ADV.InternetCrawler.ControlPanel.Controllers
{
    public class DataPointController : Controller
    {
        [HttpGet]
        public ActionResult EditDataPoint(int? ID)
        {
            DataPointEditModel l_model = null;

            try
            {
                if (ID != null)
                {
                    DataPoint l_dataPoint = null;
                    int l_id = ID.GetValueOrDefault();

                    GetDataFromPoint l_detDataPoint = new GetDataFromPoint();

                    l_dataPoint = l_detDataPoint.GetDataPoint(l_id);

                    l_model = new DataPointEditModel
                    {
                        ID = l_dataPoint.ID,
                        Name = l_dataPoint.Name,
                        URI = l_dataPoint.Uri,

                        PageURIStart = SeparateRegex(l_dataPoint.PageUri, "Start"),
                        PageURIData = SeparateRegex(l_dataPoint.PageUri, "Data"),
                        PageURIFinish = SeparateRegex(l_dataPoint.PageUri, "Finish"),

                        ItemURIStart = SeparateRegex(l_dataPoint.ItemUri, "Start"),
                        ItemURIData = SeparateRegex(l_dataPoint.ItemUri, "Data"),
                        ItemURIFinish = SeparateRegex(l_dataPoint.ItemUri, "Finish"),

                        ItemNameStart = SeparateRegex(l_dataPoint.ItemName, "Start"),
                        ItemNameData = SeparateRegex(l_dataPoint.ItemName, "Data"),
                        ItemNameFinish = SeparateRegex(l_dataPoint.ItemName, "Finish"),

                        ItemPriceStart = SeparateRegex(l_dataPoint.ItemPrice, "Start"),
                        ItemPriceData = SeparateRegex(l_dataPoint.ItemPrice, "Data"),
                        ItemPriceFinish = SeparateRegex(l_dataPoint.ItemPrice, "Finish"),

                        ItemDiscountPriceStart = SeparateRegex(l_dataPoint.ItemPictureUri, "Start"),
                        ItemDiscountPriceData = SeparateRegex(l_dataPoint.ItemPictureUri, "Data"),
                        ItemDiscountPriceFinish = SeparateRegex(l_dataPoint.ItemPictureUri, "Finish"),

                        ItemArticleStart = SeparateRegex(l_dataPoint.ItemArticle, "Start"),
                        ItemArticleData = SeparateRegex(l_dataPoint.ItemArticle, "Data"),
                        ItemArticleFinish = SeparateRegex(l_dataPoint.ItemArticle, "Finish"),

                        ItemPictureUriStart = SeparateRegex(l_dataPoint.ItemPictureUri, "Start"),
                        ItemPictureUriData = SeparateRegex(l_dataPoint.ItemPictureUri, "Data"),
                        ItemPictureUriFinish = SeparateRegex(l_dataPoint.ItemPictureUri, "Finish")
                    };
                }
            }
            catch
            {

            }

            return View(l_model);
        }

        [HttpPost]
        public JsonResult EditDataPoint(DataPointEditModel Model)
        {
            try
            {
                DataPoint l_dataPoint = new DataPoint
                {
                    ID = Model.ID ?? 0,
                    Name = Model.Name,
                    Uri = Model.URI,

                    PageUri = Model.PageURIStart + Model.PageURIData + Model.PageURIFinish,
                    ItemUri = Model.ItemURIStart + Model.ItemURIData + Model.ItemURIFinish,
                    ItemName = Model.ItemNameStart + Model.ItemNameData + Model.ItemNameFinish,
                    ItemPrice = Model.ItemPriceStart + Model.ItemPriceData + Model.ItemPriceFinish,
                    ItemDiscountPrice = Model.ItemDiscountPriceStart + Model.ItemDiscountPriceData + Model.ItemDiscountPriceFinish,
                    ItemArticle = Model.ItemArticleStart + Model.ItemArticleData + Model.ItemArticleFinish,
                    ItemPictureUri = Model.ItemDiscountPriceStart + Model.ItemDiscountPriceData + Model.ItemDiscountPriceFinish
                };

                l_dataPoint.Save();
            }
            catch (Exception l_exc)
            {
                return Json(new { Status = false, Error = l_exc.Message });
            }

            return Json(new { Status = true });
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

        private String SeparateRegex(String _regex, String _partName)
        {
            String l_separatedPart = null;

            try
            {
                Regex l_regex = new Regex(@"(?<Start>.*)\(\?<Data>(?<Data>[^\)]+)\)(?<Finish>.*)");
                Match l_regexMatch = l_regex.Match(_regex);

                l_separatedPart = l_regexMatch.Groups[_partName].Value;
            }
            catch
            {

            }

            return l_separatedPart;
        }
    }
}