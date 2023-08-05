using Art_Critique_Api.Entities;
using Art_Critique_Api.Models.Base;
using Art_Critique_Api.Models.User;
using Art_Critique_Api.Services.Interfaces;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;

namespace Art_Critique_Api.Services {
    public class MailService : BaseService, IMailService {
        #region Properties
        private readonly ArtCritiqueDbContext DbContext;
        private readonly SmtpClient SmtpClient = new();
        private readonly ImapClient ImapClient = new();
        #endregion

        #region Constructor
        public MailService(ArtCritiqueDbContext dbContext) {
            DbContext = dbContext;
            Task.Run(async () => await ConnectToMail());
        }
        #endregion

        #region Methods
        private async Task ConnectToMail() {
            await SmtpClient.ConnectAsync("smtp-mail.outlook.com", 587, false);
            SmtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
            SmtpClient.Authenticate("art-critique@outlook.com", "Niewiem123");
            await ImapClient.ConnectAsync("outlook.office365.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
            ImapClient.Authenticate("art-critique@outlook.com", "Niewiem123");
        }

        public async Task SendMail(string mail, string subject, string content) {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("art-critique", "art-critique@outlook.com"));
            message.To.Add(new MailboxAddress("new-user", mail));
            message.Subject = subject;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = content };
            await SmtpClient.SendAsync(message);
        }
        #endregion
    }
}