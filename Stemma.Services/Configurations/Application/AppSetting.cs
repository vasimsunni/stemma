using Microsoft.Extensions.Configuration;
using System;

namespace Stemma.Services.Configurations.Application
{
    public static class AppSetting
    {
        public static AppSet AppSets;
        public static string GetVal(this IConfiguration configuration, string key)
        {
            try
            {
                return configuration[key].ToString();
            }
            catch (Exception ex) { }
            return "";
        }

        public static AppSet AppSettings(this IConfiguration configuration)
        {
            if (AppSets == null)
            {
                PopulateSettings(configuration);
            }

            return AppSets;
        }

        private static void PopulateSettings(IConfiguration configuration)
        {
            AppSets = new AppSet();
            AppSets.ConnectionString = configuration.GetVal("ConnectionString:StemmaDb");
            AppSets.APIBaseURL = configuration.GetVal("Utility:APIBaseURL");
            AppSets.AccountLockoutTimeSpan = Convert.ToInt64(configuration.GetVal("AccountLockOutInfo:LockoutTimeSpan"));
            AppSets.AccountLockoutMaxFailedAccessAttempts = Convert.ToInt32(configuration.GetVal("AccountLockOutInfo:MaxFailedAccessAttempts"));
            AppSets.EmailHost = configuration.GetVal("EmailSender:Host");
            AppSets.EmailPort = Convert.ToInt32(configuration.GetVal("EmailSender:Port"));
            AppSets.EmailEnableSSL = Convert.ToBoolean(configuration.GetVal("EmailSender:EnableSSL"));
            AppSets.EmailUserName = configuration.GetVal("EmailSender:UserName");
            AppSets.EmailPassword = configuration.GetVal("EmailSender:Password");
            AppSets.JWTKey = configuration.GetVal("JWT:key");
            AppSets.JWTIssuer = configuration.GetVal("JWT:Issuer");
            AppSets.JWTAudience = configuration.GetVal("JWT:Audience");
            AppSets.JWTExprationTimeOut = Convert.ToInt32(configuration.GetVal("JWT:JWTExprationTimeOut"));
            AppSets.CORSEnabledURL = configuration.GetVal("CORS:EnabledUrl");
        }
    }

    public class AppSet
    {
        public string ConnectionString { get; set; }
        public string APIBaseURL { get; set; }
        public Int64 AccountLockoutTimeSpan { get; set; }
        public Int32 AccountLockoutMaxFailedAccessAttempts { get; set; }
        public string EmailHost { get; set; }
        public int EmailPort { get; set; }
        public bool EmailEnableSSL { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public string JWTKey { get; set; }
        public string JWTIssuer { get; set; }
        public string JWTAudience { get; set; }
        public int JWTExprationTimeOut { get; set; }
        public string CORSEnabledURL { get; set; }


    }
}
