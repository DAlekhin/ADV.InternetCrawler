using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler;
using ADV.InternetCrawler.Utility.Logger;

namespace ADV.InternetCrawler.Interface
{
    public interface IParser
    {
        List<Message> PullRequest(DataPoint DataPoint);

        List<PointContent> PutPointContent();
    }
}
