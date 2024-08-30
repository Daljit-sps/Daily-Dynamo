namespace DailyDynamo.Shared.Common
{
    public class AppSettingsModel
    {
        public EmailSettings EmailSettings { get; set; }

        public AppURLModel AppURL { get; set; }

        public JWTModel JWT { get; set; }

        public PasswordModel Password { get; set; }

        public EmailTemplatePaths EmailTemplatePaths { get; set; }

        public AssetPaths AssetPaths { get; set; }

    }

    public class EmailSettings
    {
        public string From { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int Port { get; set; }

        public bool SSLEnable { get; set; }

    }

    public class JWTModel
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }

    }

    public class AppURLModel
    {
        public string NewPasswordPageLink { get; set; }
    }

    public class PasswordModel
    {
        public string DefaultPassword { get; set; }
        public string Salt { get; set; }
    }
    public class EmailTemplatePaths
    {
        public string PasswordResetTemplate { get; set; }
        public string ForgotPasswordTemplate { get; set; }
        public string PasswordChangedTemplate { get; set; }
        public string DSRSubmittedTemplate { get; set; }
    }

    public class AssetPaths
    {
        public string ProfileImagesDirectoryPath { get; set; }
    }
}
