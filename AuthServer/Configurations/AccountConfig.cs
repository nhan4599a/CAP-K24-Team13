namespace AuthServer.Configurations
{
    public class AccountConfig
    {
        public static bool AllowRememberMe => true;

        public static bool RequireEmailConfirmation => false;

        public static bool AccountLockedOutEnabled => true;

        public static int MinAge => 18;

        public static int MaxAge => 50;

        public static TimeSpan RememberMeDuration => TimeSpan.FromMinutes(30);
    }
}
