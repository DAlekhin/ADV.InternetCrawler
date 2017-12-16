using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADV.InternetCrawler;

namespace ADV.InternetCrawler.UnitTest
{
    [TestClass]
    public class SaveDataPoint
    {
        [TestMethod]
        public void PositiveSaveConnect()
        {
            try
            {
                DataPoint l_dataPoint = new DataPoint
                {
                    Name = "Бетховен",
                    Uri = "https://www.bethowen.ru/catalogue/dogs/",
                    PageUri = "/catalogue/dogs/\\?PAGEN_1=(?<Data>\\d+)",
                    ItemUri = "<div class=\"h4\"[^>]*>\\s*<a href=\"(?<Data>[^\"]+)\"",
                    ItemName = "<h1 class=\"product\\s*-\\s*title\"[^>]+>(?<Data>[^<]+)</h1>",
                    ItemArticle = "<h3 class=\"product\\s*-\\s*code\">\\s*Артикул:\\s*(?<Data>[^<]+)</h3>",
                    ItemPrice = "<span class=\"price\\s*-\\s*sales\\s*product\\s*-\\s*price\">\\s*(?<Data>[^<]+)<",
                    ItemDiscountPrice = "<span class=\"skidkapokarte\"[^>]+>(?<Data>[^<]+)<|<span class=\"price\\s*-\\s*standard product\\s*-\\s*price\">(?<Data>[^<]+)<",
                    ItemPictureUri = "<div class=\"slider\\s*-\\s*for\">\\s*(<div>\\s*)?<img\\s*src\\s*=\\s*\"(?<Data>[^\"]+)\""
                };

                l_dataPoint.Save();
            }
            catch (Exception l_exc)
            {
                Assert.Fail(l_exc.Message);
            }
        }

        [TestMethod]
        public void NegativeSaveConnect()
        {
            try
            {
                DataPoint l_dataPoint = new DataPoint
                {
                    Name = "Бетховен",
                    Uri = "https://www.bethowen.ru/catalogue/dogs1/"
                };

                l_dataPoint.Save();

                Assert.Fail("Ожидалось исключение.");
            }
            catch (AssertFailedException l_assertExc)
            {
                throw l_assertExc;
            }
            catch (Exception l_exc)
            {
                Assert.IsNotNull(l_exc.Message, String.Format("Ошибка не получена."));
            }
        }

        [TestMethod]
        public void PositiveGetDataPoint()
        {
            try
            {
                Int32 l_id = 1;

                GetDataFromPoint l_getData = new GetDataFromPoint();

                var l_dataPoint = l_getData.GetDataPoint(l_id);

                Assert.IsNotNull(l_dataPoint, $"Не удалось получить DataPoint");

            }
            catch (Exception l_exc)
            {
                Assert.Fail(l_exc.Message);
            }
        }

        [TestMethod]
        public void NegativeGetDataPoint()
        {
            try
            {
                Int32 l_id = -1;

                GetDataFromPoint l_getData = new GetDataFromPoint();

                var l_dataPoint = l_getData.GetDataPoint(l_id);

                Assert.Fail("Ожидалось исключение.");

            }
            catch (AssertFailedException l_assertExc)
            {
                throw l_assertExc;
            }
            catch (Exception l_exc)
            {
                Assert.IsNotNull(l_exc.Message, String.Format("Ошибка не получена."));
            }
        }

        [TestMethod]
        public void CheckGetDataPoints()
        {
            try
            {
                GetDataFromPoint l_getData = new GetDataFromPoint();

                var l_dataPoints = l_getData.GetDataPoints();

                if (l_dataPoints.Count > 0)
                    Assert.IsTrue(true);
                else
                    Assert.Fail("Строки не получены.");
            }
            catch (Exception l_exc)
            {
                Assert.Fail(l_exc.Message);
            }
        }
    }
}
