using AspNetCoreSharedComponent.Mail;
using DatabaseAccessor.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Background
{
    public class SendAlertEmailBackgroundJob
    {
        private readonly MailHelper _mailer;

        private readonly ApplicationDbContext _dbContext;

        public SendAlertEmailBackgroundJob(MailHelper mailer, ApplicationDbContext dbContext)
        {
            _mailer = mailer;
            _dbContext = dbContext;
        }
        
        public async Task SendEmailAsync(string userId)
        {
            var parseResult = Guid.TryParse(userId, out Guid guid);
            if (parseResult)
            {
                var receiver = _dbContext.Users
                    .AsNoTracking()
                    .Where(user => user.Id == guid).Select(user => user.Email).AsEnumerable().First();
                var mailRequest = new MailRequest
                {
                    Receiver = receiver,
                    Subject = "Warning!",
                    Body = @"You are reported once. If you are reported one more time, you will be banned for 14 days",
                    IsHtmlMessage = false
                };
                await _mailer.SendEmail(mailRequest);
            }
        }
    }
}
