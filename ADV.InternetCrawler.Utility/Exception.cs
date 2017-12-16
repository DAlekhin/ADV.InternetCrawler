using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADV.InternetCrawler.Utility.Logger;

namespace ADV.InternetCrawler.Utility
{
    public class ICException : AggregateException
    {
        public ICException(String _message, List<Exception> _exc) : base(_message, _exc)
        {
        }

        public static void GetException(List<Message> _logMessages)
        {
            String l_excResult = "";
            List<Exception> l_listOfException = new List<Exception>();

            foreach (Message l_logMessage in _logMessages.Where(w => w.messageType == MessageType.Error || w.messageType == MessageType.Fatal))
            {
                l_excResult += $"{l_logMessage.message}{Environment.NewLine}";
                l_listOfException.Add(new Exception(l_logMessage.message, l_logMessage.exception));
            }

            if (l_excResult.Length != 0)
                throw new ICException(l_excResult, l_listOfException);
        }
    }
}
