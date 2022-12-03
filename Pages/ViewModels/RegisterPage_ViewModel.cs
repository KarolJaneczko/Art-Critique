using System.ComponentModel;


namespace Art_Critique.Pages.ViewModels {
    public class RegisterPage_ViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
