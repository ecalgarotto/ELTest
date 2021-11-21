namespace Swoop.EL.Company.Common
{
    public class CustomAppSettings: ICustomAppSettings
    {
        public int NumberOfRecords { get; set; }
        public string ApiURL { get; set; }
        public string ApiKey { get; set; }

    }
}
