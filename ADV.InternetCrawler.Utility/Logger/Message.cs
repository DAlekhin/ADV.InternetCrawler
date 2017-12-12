using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ADV.InternetCrawler.Utility.Logger
{
    public class Header
    {
        public Int32 id;
        public Int32 pointID;
        public DateTime startSession;
        public DateTime finishSession;
        public List<Message> messages;
    }

    public class Message
    {
        public Int32 headerID;
        public String moduleName;
        public DateTime proccessDateTime;
        public String uri;
        public MessageType messageType;
        public String message;
        public Exception exception;
    }
}