namespace FoosBall.Models.Views
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ConfigViewModel
    {
        public Config Settings { get; set; }

        public List<SelectListItem> Users { get; set; } 
    }
}