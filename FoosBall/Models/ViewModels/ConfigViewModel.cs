namespace FoosBall.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using FoosBall.Models.Domain;

    public class ConfigViewModel
    {
        public Config Settings { get; set; }

        public List<SelectListItem> Users { get; set; } 
    }
}