using Data;
using Domain;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using Microsoft.Azure.WebJobs.Extensions.SendGrid;

namespace SpotifyPlaylist_Function
{
    public static class DiscWeeklyRun
    {
        [FunctionName("DiscWeeklyRun")]
        public static void Run([TimerTrigger("0 0 2 * * 0")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"DiscWeeklyRun Timer trigger function executed at: {DateTime.Now}");
            IEmail email = new Email(log);

            try
            {
                ISpotifyMain spotifyMain = new SpotifyMain(log);
                spotifyMain.BackupPlaylist();

                email.SendEmail();
            }
            catch (Exception ex)
            {
                log.LogError(ex, "DiscWeeklyRun encountered an error");
                
                email.SendEmail(ex);
            }
            finally
            {
                log.LogInformation($"DiscWeeklyRun Timer trigger function finished at: {DateTime.Now}");
            }
        }
    }
}
