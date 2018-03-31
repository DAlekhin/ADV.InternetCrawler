using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADV.InternetCrawler.Models
{
    public class DataPointStatsModel
    {
        [Key]
        [Display(Name = "#")]
        public Int32 PointID { get; set; }

        [Display(Name = "Наименование")]
        public String Name { get; set; }

        [Display(Name = "Обходы")]
        public Int32 Rounds { get; set; }

        [Display(Name = "Товары")]
        public Int32 Items { get; set; }

        [Display(Name = "Посл. Обхода")]
        public DateTime LastRound { get; set; }

        [Display(Name = "Ошибки")]
        public Int32 ErrorLink { get; set; }

        [Display(Name = "Готовность")]
        public Boolean Ready { get; set; }

        [Display(Name = "След. Обход")]
        public DateTime NextRound { get; set; }
    }

    public class DataPointLogModel
    {
        [Display(Name = "ID")]
        public Int32 ID { get; set; }

        [Display(Name = "Модуль")]
        public String Module { get; set; }

        [Display(Name = "Время Обработки")]
        public DateTime ProcessDateTime { get; set; }

        [Display(Name = "Ссылка")]
        public String URI { get; set; }

        [Display(Name = "Уровень")]
        public Int32 Level { get; set; }

        [Display(Name = "Сообщение")]
        public String Message { get; set; }
    }
}