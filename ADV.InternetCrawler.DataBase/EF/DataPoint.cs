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
    
    public partial class DataPoint
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URI { get; set; }
        public string PageURI { get; set; }
        public string ItemURI { get; set; }
        public string ItemName { get; set; }
        public string ItemPrice { get; set; }
        public bool ItemDeep { get; set; }
        public string ItemDiscountPrice { get; set; }
        public string ItemArticle { get; set; }
    }
}