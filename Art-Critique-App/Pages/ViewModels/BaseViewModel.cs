using Art_Critique.Core.Utils.Base;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Art_Critique.Pages.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static async Task ExecuteWithTryCatch(Action method) {
            try {
                method();
            } catch (AppException ex) {
                await Application.Current.MainPage.DisplayAlert(ex.title, ex.message, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
        }
    }
}
