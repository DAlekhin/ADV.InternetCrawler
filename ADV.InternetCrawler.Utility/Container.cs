using System;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace ADV.InternetCrawler.Utility
{
    public static class Container
    {
        public static Object GetObject(String _objectName)
        {
            try
            {
                using (WindsorContainer l_container = new WindsorContainer(new XmlInterpreter()))
                {
                    Object l_containerObject = l_container.Resolve<Object>(_objectName);

                    return l_containerObject;
                }
            }
            catch (Exception l_exc)
            {
                throw new Exception(l_exc.Message, l_exc);
            }
        }
    }
}
