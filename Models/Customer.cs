namespace LineService.Models
{
    public class Customer
    {
        public string userId { get; set; }

        public string displayName { get; set; }

        public string pictureUrl { get; set; }

        public string language { get; set; }

        public string editName { get; set; }

        public string tag { get;set; }
        

      

    }
    public class Schedule
    {
        public string startDate { get; set; }

        public string endDate { get; set; }

        public string time { get; set; }

        public string repeat_interval { get; set; }

        public string message { get; set; }

        public string target { get; set; }

        public int increment { get; set; }

    }
}
