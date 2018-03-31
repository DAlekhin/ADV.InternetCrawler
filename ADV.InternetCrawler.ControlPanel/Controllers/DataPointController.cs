using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADV.InternetCrawler.ControlPanel.Models;
using ADV.InternetCrawler.Models;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADV.InternetCrawler.ControlPanel.Controllers
{
    public class DataPointController : Controller
    {
        [HttpGet]
        public ActionResult EditDataPoint(Int32? ID)
        {
            DataPointEditModel l_model = null;

            try
            {
                if (ID != null)
                {
                    DataPoint l_dataPoint = null;
                    Int32 l_id = ID.GetValueOrDefault();

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

                        ItemDiscountPriceStart = SeparateRegex(l_dataPoint.ItemDiscountPrice, "Start"),
                        ItemDiscountPriceData = SeparateRegex(l_dataPoint.ItemDiscountPrice, "Data"),
                        ItemDiscountPriceFinish = SeparateRegex(l_dataPoint.ItemDiscountPrice, "Finish"),

                        ItemArticleStart = SeparateRegex(l_dataPoint.ItemArticle, "Start"),
                        ItemArticleData = SeparateRegex(l_dataPoint.ItemArticle, "Data"),
                        ItemArticleFinish = SeparateRegex(l_dataPoint.ItemArticle, "Finish"),

                        ItemPictureUriStart = SeparateRegex(l_dataPoint.ItemPictureUri, "Start"),
                        ItemPictureUriData = SeparateRegex(l_dataPoint.ItemPictureUri, "Data"),
                        ItemPictureUriFinish = SeparateRegex(l_dataPoint.ItemPictureUri, "Finish")
                    };
                }
                else
                {
                    l_model = new DataPointEditModel();
                }
            }
            catch
            {

            }

            return View(l_model);
        }

        [HttpGet]
        public ActionResult ShowDataPointLog(Int32? HeaderID)
        {
            DataPointLogModel l_model = new DataPointLogModel();

            ViewBag.Title = "Журнал обработки";

            return View(l_model);
        }

        [HttpPost]
        public JsonResult ShowDataPointLog(Int32 HeaderID)
        {
            List<DataPointLogModel> l_dataPointStats = new List<DataPointLogModel>();

            try
            {
                GetDataFromPoint l_dataPoint = new GetDataFromPoint();

                l_dataPointStats = l_dataPoint.GetDataPointLog(HeaderID);
            }
            catch { }

            return new LargeJsonResult() { Data = l_dataPointStats };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDataPoint(DataPointEditModel Model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DataPoint l_dataPoint = new DataPoint
                    {
                        ID = Model.ID ?? 0,
                        Name = Model.Name,
                        Uri = Model.URI,

                        PageUri = $"{Model.PageURIStart}(?<Data>{Model.PageURIData}){Model.PageURIFinish}",
                        ItemUri = $"{Model.ItemURIStart}(?<Data>{Model.ItemURIData}){Model.ItemURIFinish}",
                        ItemName = $"{Model.ItemNameStart}(?<Data>{Model.ItemNameData}){Model.ItemNameFinish}",
                        ItemPrice = $"{Model.ItemPriceStart}(?<Data>{Model.ItemPriceData}){Model.ItemPriceFinish}",
                        ItemDiscountPrice = $"{Model.ItemDiscountPriceStart}(?<Data>{Model.ItemDiscountPriceData}){Model.ItemDiscountPriceFinish}",
                        ItemArticle = $"{Model.ItemArticleStart}(?<Data>{Model.ItemArticleData}){Model.ItemArticleFinish}",
                        ItemPictureUri = $"{Model.ItemPictureUriStart}(?<Data>{Model.ItemPictureUriData}){Model.ItemPictureUriFinish}"
                    };

                    l_dataPoint.Save();

                    return RedirectToAction("ShowDataPoint", "DataPoint");
                }
                catch (Exception l_exc)
                {
                    ModelState.AddModelError("", l_exc.Message);
                }
            }

            return View(Model);
        }

        public ActionResult CleanDataPoint(Int32 ID)
        {
            try
            {
                DataPoint l_dataPoint = new DataPoint();
                l_dataPoint.ID = ID;

                l_dataPoint.Clean();
            }
            catch { }

            return RedirectToAction("ShowDataPoint");
        }

        public ActionResult RemoveDataPoint(Int32 ID)
        {
            try
            {
                DataPoint l_dataPoint = new DataPoint();
                l_dataPoint.ID = ID;

                l_dataPoint.Remove();
            }
            catch { }

            return RedirectToAction("ShowDataPoint");
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

        [HttpPost]
        public async Task<JsonResult> LaunchDataPoint(Int32 PointID)
        {
            try
            {
                GetDataFromPoint l_pullData = new GetDataFromPoint();

                //l_pullData.PullDataFromPoint(PointID);
                //l_pullData.PullDataFromPointAsync(PointID);

                await Task.Run(() =>
                {
                    l_pullData.PullDataFromPoint(PointID);
                });
            }
            catch (Exception l_exc) { }

            return Json(new { Status = true });
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