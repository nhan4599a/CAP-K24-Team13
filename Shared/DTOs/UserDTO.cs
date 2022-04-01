namespace Shared.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string BirthDay { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsLockedOut { get; set; }

        public bool IsAvailable { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public uint ReportCount { get; set; }
    }
}
