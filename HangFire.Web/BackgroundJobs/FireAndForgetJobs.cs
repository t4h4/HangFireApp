using HangFire.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Web.BackgroundJobs
{
    public class FireAndForgetJobs
    {
        public static void EmailSendToUserJob(string userId, string message) //kolay ulaşabilmek için static method
        {
            //Enqueue tek sefer çalışan işler için kullanılan method. 
            //IEmailSender kullandık, startup.cs'de services.AddScoped<IEmailSender, EmailSender>(); eklemiştik.
            Hangfire.BackgroundJob.Enqueue<IEmailSender>(x => x.Sender(userId, message));

        }
    }
}
