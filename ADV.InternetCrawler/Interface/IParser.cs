using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler;

namespace ADV.InternetCrawler.Interface
{
    public interface IParser
    {
        void PullRequest(DataPoint DataPoint);

        List<PointContent> PutPointContent();
    }
}
