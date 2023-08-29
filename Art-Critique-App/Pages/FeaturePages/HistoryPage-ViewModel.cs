using Art_Critique.Models.Logic;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages {
    public class HistoryPageViewModel : BaseViewModel {
        #region Service
        private readonly ICacheService CacheService;
        #endregion

        #region Properties
        private bool isLoading = true;
        private ObservableCollection<HistoryRecord> history = new();

        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public ObservableCollection<HistoryRecord> History { get => history; set { history = value; OnPropertyChanged(nameof(History)); } }
        public ICommand GoCommand => new Command<HistoryRecord>(Go);
        #endregion

        #region Constructor
        public HistoryPageViewModel(ICacheService cacheService) {
            CacheService = cacheService;
            LoadHistory();
        }
        #endregion

        #region Methods
        private void LoadHistory() {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pl-PL");
            var historyList = CacheService.GetHistory();
            historyList.Reverse();
            historyList.ForEach(x => History.Add(new HistoryRecord(x)));
            IsLoading = false;
        }

        public static async void Go(HistoryRecord photo) {
            await Shell.Current.GoToAsync(photo.Path, photo.Parameters);
        }
        #endregion

        #region Local class
        public class HistoryRecord {
            public ImageSource Image {
                get {
                    if (Path == "ProfilePage") {
                        return !string.IsNullOrEmpty(ImageBase) ? ImageBase.Base64ToImageSource() : "defaultuser_icon.png";
                    } else {
                        return ImageBase.Base64ToImageSource();
                    }
                }
            }
            public string ImageBase { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Path { get; set; }
            public Dictionary<string, object> Parameters { get; set; }
            public HistoryRecord(HistoryEntry historyEntry) {
                ImageBase = historyEntry.Image;
                Title = historyEntry.Title;
                Type = historyEntry.Type;
                Date = historyEntry.Date.ToShortDateString();
                Time = historyEntry.Date.ToShortTimeString();
                Path = historyEntry.Path;
                Parameters = historyEntry.Parameters;
            }
        }
        #endregion
    }
}