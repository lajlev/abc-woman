namespace FoosBall.Models.ViewModels
{
    using FoosBall.Models.Domain;
    using FoosBall.Models.Base;

    public class PlayerBaseDataViewModel : FoosBallViewModel
    {
        public Player Player { get; set; }
    }
}
