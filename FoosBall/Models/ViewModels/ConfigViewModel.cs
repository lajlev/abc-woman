namespace FoosBall.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using FoosBall.Models.Base;

    public class ConfigViewModel : BaseViewModel
    {
        public List<SelectListItem> Users { get; set; } 
    }
}