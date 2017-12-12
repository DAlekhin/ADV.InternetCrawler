using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ADV.InternetCrawler.Utility.Logger.Interface;

namespace ADV.InternetCrawler.Utility.Logger
{
    public class LogMessages
    {
        private Header logHeader = new Header();
        private List<Message> logMessages = new List<Message>();
        private Int32 pointID;

        public List<Message> Messages
        {
            get
            {
                return logMessages;
            }
            set
            {
                logMessages = value;
            }
        }

        public Int32 PointID
        {
            get
            {
                return pointID;
            }
            set
            {
                pointID = value;
            }
        }

        public LogMessages() 
        {
            logHeader.startSession = DateTime.UtcNow;
        }

        //public void GetNewHeaderID()
        //{
        //    try
        //    {
        //        var l_containerObject = (ILogger)Utility.Container.GetObject("DAL");
        //        l_containerObject.GetNewHeaderID(logHeader);
        //    }
        //    catch (Exception l_exc)
        //    {
        //        throw new Exception(l_exc.Message, l_exc);
        //    }
        //}

        public void AddToMessage(String _moduleName, String _uri, MessageType _messageType, String _message)
        {
            AddToMessage(_moduleName, _uri, _messageType, _message, null);
        }

        public void AddToMessage(String _moduleName, String _uri, MessageType _messageType, String _message, Exception _exception)
        {
            logMessages.Add(new Message
            {
                moduleName = _moduleName,
                proccessDateTime = DateTime.UtcNow,
                uri = _uri,
                messageType = _messageType,
                message = _message,
                exception = _exception
            });
        }

        public void AddToMessages(Message _message)
        {
            _message.proccessDateTime = DateTime.UtcNow;
            logMessages.Add(_message);
        }

        public void PutMessages()
        {
            try
            {
                logHeader.pointID = pointID;
                logHeader.finishSession = DateTime.UtcNow;
                logHeader.messages= logMessages;

                var l_containerObject = (ILogger)Utility.Container.GetObject("DAL");
                l_containerObject.PutLogMessages(logHeader);
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }

        public Boolean CheckError()
        {
            Boolean l_result;

            try
            {
                if (this.Messages.Where(w => w.messageType == MessageType.Error || w.messageType == MessageType.Fatal).Count() > 0)
                {
                    l_result = false;
                }
                else
                {
                    l_result = true;
                }
            }
            catch (Exception l_exc)
            {
                AddToMessage("CheckError", null, MessageType.Fatal, l_exc.Message, l_exc);

                l_result = false;
            }

            return l_result;
        }
    }
}