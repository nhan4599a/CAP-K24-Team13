namespace UserService.RequestModels
{
    public class BanUserRequestModel
    {
        public uint? DayCount { get; set; }

        public string? BanMessage { get; set; }
    }
}
