using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADV.InternetCrawler;

namespace ADV.InternetCrawler.UnitTest
{
    [TestClass]
    public class DataParser
    {
        [TestMethod]
        public void GetDataParser()
        {
            try
            {
                Int32 l_id = 1;

                GetDataFromPoint l_getData = new GetDataFromPoint();

                l_getData.PullDataFromPoint(l_id);
            }
            catch (Exception l_exc)
            {
                Assert.Fail(l_exc.Message);
            }

        }
    }
}