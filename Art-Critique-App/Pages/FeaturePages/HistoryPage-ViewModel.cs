using Art_Critique.Models.Logic;
using Art_Critique.Pages.BasePages;
using Art_Critique.Services.Interfaces;
using Art_Critique.Utils.Helpers;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages {
    public class HistoryPageViewModel : BaseViewModel {
        #region Services
        private readonly ICacheService CacheService;
        #endregion

        #region Properties
        private ObservableCollection<HistoryRecord> history = new();
        public ObservableCollection<HistoryRecord> History { get => history; set { history = value; OnPropertyChanged(nameof(History)); } }

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion

        #region Commands
        public ICommand GoCommand => new Command<HistoryRecord>(Go);
        #endregion
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
            public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
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