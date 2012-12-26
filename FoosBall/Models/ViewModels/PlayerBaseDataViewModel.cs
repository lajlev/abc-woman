namespace FoosBall.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using FoosBall.Models.Base;
    using FoosBall.Models.Domain;

    public class PlayerBaseDataViewModel : FoosBallViewModel
    {
        public PlayerBaseDataViewModel()
        {
            var genderList = new List<SelectListItem>
                                 {
                                     new SelectListItem { Selected = false, Text = "Male", Value = "Male" },
                                     new SelectListItem { Selected = false, Text = "Female", Value = "Female" }
                                 };

            this.Genders = genderList;
            this.ReferralUrl = "/Players";
        }

        public Player Player { get; set; }

        public bool SaveSuccess { get; set; }

        public string ReferralUrl { get; set; }

        public List<SelectListItem> Genders { get; set; }
    }
}
