using eCommerce.Application.ExternalServices.Interfaces.Email;
using eCommerce.Application.ExternalServices.Interfaces.Jobs;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Infrastructure.ExternalServices.Jobs
{
    public class HangfireJobService : IBackgroundJobService
    {
        private readonly IEmailService emailService;
        public HangfireJobService(IEmailService emailService)
        {
            this.emailService = emailService;
        }
        public void EnqueueEmail(string to, string subject, string body)
        {
            BackgroundJob.Enqueue(
                () => emailService.SendEmailAsync(to, subject, body)
                );
        }
    }
}
