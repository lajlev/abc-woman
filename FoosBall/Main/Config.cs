namespace FoosBall.Main
{
    using System.Configuration;
    using System.Xml;

    using FoosBall.Models.Base;

    public static class AppConfig
    {
        public static void InitalizeConfig()
        {
            var environment = GetEnvironment();
            var configCollection = new Db(environment).Dbh.GetCollection<Models.Config>("Config");
            var config = configCollection.FindOne();

            if (config == null)
            {
                config = new Models.Config();
                configCollection.Save(config);
            }
        }

        public static Environment GetEnvironment()
        {
            Environment environment;
            var environmentString = ConfigurationManager.AppSettings["Environment"];

            switch (environmentString)
            {
                case "Production":
                    environment = Environment.Production;
                    break;
                case "Staging":
                    environment = Environment.Staging;
                    break;
                default:
                    environment = Environment.Local;
                    break;
            }

            return environment;
        } 
    }
}