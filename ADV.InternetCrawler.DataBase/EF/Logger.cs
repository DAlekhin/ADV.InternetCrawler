//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADV.InternetCrawler.DataBase.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Logger
    {
        public int ID { get; set; }
        public int HeaderID { get; set; }
        public string ModuleName { get; set; }
        public System.DateTime ProccessDateTime { get; set; }
        public string URI { get; set; }
        public int Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
