using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Utility.Logger;

namespace ADV.InternetCrawler.Utility.Logger.Interface
{
    public interface ILogger
    {
        String ConnectionString { get; set; }

        Int32 GetNewHeaderID();

        void PutLogMessages(Header LoggerHeader);
    }
}
