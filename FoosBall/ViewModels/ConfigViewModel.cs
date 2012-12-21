namespace FoosBall.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using FoosBall.Models;

    public class ConfigViewModel
    {
        public Config Settings { get; set; }

        public List<SelectListItem> Users { get; set; } 
    }
}