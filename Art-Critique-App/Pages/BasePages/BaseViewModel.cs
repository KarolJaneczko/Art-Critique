using Art_Critique.Models.API.Base;
using Art_Critique.Models.Logic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Art_Critique.Pages.BasePages {
    public class BaseViewModel : INotifyPropertyChanged {
        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected static async Task<ApiResponse> ExecuteWithTryCatch(Func<Task<ApiResponse>> method) {
            try {
                return await method();
            } catch (AppException ex) {
                await Application.Current.MainPage.DisplayAlert(ex.Title, ex.ErrorMessage, "OK");
                return new ApiResponse() {
                    IsSuccess = false,
                    Title = ex.Title,
                    Message = ex.ErrorMessage,
                    Data = null
                };
            } catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Unknown error!", ex.Message, "OK");
                return new ApiResponse() {
                    IsSuccess = false,
                    Title = "Error!",
                    Message = "Unknown error happened",
                    Data = null
                };
            }
        }
        #endregion
    }
}