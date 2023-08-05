namespace Art_Critique_Api.Services.Interfaces {
    public interface IMailService {
        #region Methods
        public Task SendMail(string mail, string subject, string content);
        #endregion
    }
}