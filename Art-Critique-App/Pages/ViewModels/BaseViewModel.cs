using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Art_Critique.Pages.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        #region Fields
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
