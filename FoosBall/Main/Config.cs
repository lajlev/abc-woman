namespace FoosBall.Main
{
    using System.Collections.Generic;
    using System.Xml;

    public static class AppConfig
    {
        private static readonly string XmlFileName = System.Web.HttpContext.Current.Server.MapPath("/Config/AppDefault.Config");

        public static void InitalizeConfig()
        {
            var configCollection = Db.GetDataBaseHandle().GetCollection<Models.Config>("Config");
            var config = configCollection.FindOne();

            if (config == null)
            {
                var appConfig = new XmlDocument();
                appConfig.Load(XmlFileName);
                
                config = new Models.Config
                    {
                        Name = appConfig.GetElementsByTagName("name")[0].InnerText,
                        Version = appConfig.GetElementsByTagName("version")[0].InnerText,
                        Domain = appConfig.GetElementsByTagName("domain")[0].InnerText,
                        AdminAccount = appConfig.GetElementsByTagName("adminAccount")[0].InnerText,
                        RequireDepartment = XmlConvert.ToBoolean(appConfig.GetElementsByTagName("requireDepartment")[0].InnerText),
                        RequireDomainValidation = XmlConvert.ToBoolean(appConfig.GetElementsByTagName("requireDomainValidation")[0].InnerText),
                        AllowOneOnOneMatches = XmlConvert.ToBoolean(appConfig.GetElementsByTagName("allowOneOnOneMatches")[0].InnerText),
                        GenderSpecificMatches = XmlConvert.ToBoolean(appConfig.GetElementsByTagName("genderSpecificMatches")[0].InnerText)
                    };

                configCollection.Save(config);
            }
        }
    }
}