using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADV.InternetCrawler.Models
{
    public class DataPointStatsModel
    {
        [Display(Name = "#")]
        public int PointID { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display(Name = "Кол-во Обходов")]
        public int Rounds { get; set; }

        [Display(Name = "Кол-во Товаров")]
        public string Items { get; set; }

        [Display(Name = "Дата Посл. Обхода")]
        public DateTime LastRound { get; set; }

        [Display(Name = "Ошибки")]
        public bool Errors { get; set; }
    }
}