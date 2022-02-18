namespace AuthServer.Models
{
    public record ChangePasswordModel(string Password, string NewPassword, string ConfirmPassword)
    {
    }
}
