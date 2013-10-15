namespace FoosBall.Models.ViewModels
{
    using Base;
    using Domain;

    public class PlayerBaseDataViewModel : BaseViewModel
    {
        public PlayerBaseDataViewModel()
        {
            this.ReferralUrl = "/Players";
        }

        public Player Player { get; set; }

        public bool SaveSuccess { get; set; }

        public string ReferralUrl { get; set; }
    }
}
