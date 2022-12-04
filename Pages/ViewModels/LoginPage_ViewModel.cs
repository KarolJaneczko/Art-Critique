using System.ComponentModel;


namespace Art_Critique.Pages.ViewModels {
    public class LoginPage_ViewModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        public string Login { get; set; }
        public string Password { get; set; }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
