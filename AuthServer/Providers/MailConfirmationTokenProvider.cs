using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AuthServer.Providers
{
    public class MailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public MailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<DataProtectionTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {
            options.Value.TokenLifespan = TimeSpan.FromMinutes(30);
        }
    }
}
