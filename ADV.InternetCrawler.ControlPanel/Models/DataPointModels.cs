using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ADV.InternetCrawler.ControlPanel.Models
{
    public class DataPointEditModel
    {
        [Display(Name = "#")]
        public Int32? ID { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Длина названия не может быть меньше 5 символов")]
        [MaxLength(50, ErrorMessage = "Длина названия не может быть больше 50 символов")]
        [Display(Name = "Наименование")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Стартовая ссылка")]
        public String URI { get; set; }

        [Display(Name = "Regex ссылка на страницу")]
        public String PageURIStart { get; set; }
        public String PageURIData { get; set; }
        public String PageURIFinish { get; set; }

        [Display(Name = "Regex ссылка на товар")]
        public String ItemURIStart { get; set; }
        public String ItemURIData { get; set; }
        public String ItemURIFinish { get; set; }

        [Display(Name = "Regex название товара")]
        public String ItemNameStart { get; set; }
        public String ItemNameData { get; set; }
        public String ItemNameFinish { get; set; }

        [Display(Name = "Regex цена товара")]
        public String ItemPriceStart { get; set; }
        public String ItemPriceData { get; set; }
        public String ItemPriceFinish { get; set; }

        [Display(Name = "Regex цена со скидкой")]
        public String ItemDiscountPriceStart { get; set; }
        public String ItemDiscountPriceData { get; set; }
        public String ItemDiscountPriceFinish { get; set; }

        [Display(Name = "Regex артикул товара")]
        public String ItemArticleStart { get; set; }
        public String ItemArticleData { get; set; }
        public String ItemArticleFinish { get; set; }

        [Display(Name = "Regex ссылка на изображение товара")]
        public String ItemPictureUriStart { get; set; }
        public String ItemPictureUriData { get; set; }
        public String ItemPictureUriFinish { get; set; }
    }
}