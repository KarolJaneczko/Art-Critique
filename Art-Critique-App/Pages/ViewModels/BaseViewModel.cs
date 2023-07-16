using Art_Critique.Core.Models.API.Base;
using Art_Critique.Core.Utils.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Art_Critique.Pages.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static async Task<ApiResponse> ExecuteWithTryCatch(Func<Task<ApiResponse>> method) {
            try {
                return await method();
            } catch (AppException ex) {
                await Application.Current.MainPage.DisplayAlert(ex.Title, ex.ErrorMessage, "OK");
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
            }
            return new ApiResponse() {
                IsSuccess = false,
                Title = "Error!",
                Message = "Unknown error happened",
                Data = null
            };
        }
    }
}