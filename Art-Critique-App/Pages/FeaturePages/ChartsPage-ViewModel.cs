using Art_Critique.Pages.BasePages;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages {
    public class ChartsPageViewModel : BaseViewModel {
        #region Properties
        private ObservableCollection<ChartRecord> artworkBestRatings = new(), artworkMostViews = new(), profileBestRatings = new(), profileMostViews = new();

        public ObservableCollection<ChartRecord> ArtworkBestRatings { get => artworkBestRatings; set { artworkBestRatings = value; OnPropertyChanged(nameof(ArtworkBestRatings)); } }
        public ObservableCollection<ChartRecord> ArtworkMostViews { get => artworkMostViews; set { artworkMostViews = value; OnPropertyChanged(nameof(ArtworkMostViews)); } }
        public ObservableCollection<ChartRecord> ProfileBestRatings { get => profileBestRatings; set { profileBestRatings = value; OnPropertyChanged(nameof(ProfileBestRatings)); } }
        public ObservableCollection<ChartRecord> ProfileMostViews { get => profileMostViews; set { profileMostViews = value; OnPropertyChanged(nameof(ProfileMostViews)); } }

        #region Visibility flags
        private bool isLoading = true;
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        #endregion
        #region Commands
        public ICommand DisplayRecordCommand => new Command<ChartRecord>(DisplayRecord);
        #endregion
        #endregion

        #region Constructor
        public ChartsPageViewModel(List<ApiChartResult> artworksAverageRating, List<ApiChartResult> artworksTotalViews, List<ApiChartResult> profilesAverageRating, List<ApiChartResult> profilesTotalViews) {
            FillChartsPage(artworksAverageRating, artworksTotalViews, profilesAverageRating, profilesTotalViews);
        }
        #endregion

        #region Methods
        private void FillChartsPage(List<ApiChartResult> artworksAverageRating, List<ApiChartResult> artworksTotalViews, List<ApiChartResult> profilesAverageRating, List<ApiChartResult> profilesTotalViews) {
            artworksAverageRating.ForEach(x => ArtworkBestRatings.Add(new ChartRecord(x)));
            artworksTotalViews.ForEach(x => ArtworkMostViews.Add(new ChartRecord(x)));
            profilesAverageRating.ForEach(x => ProfileBestRatings.Add(new ChartRecord(x)));
            profilesTotalViews.ForEach(x => ProfileMostViews.Add(new ChartRecord(x)));
            IsLoading = false;
        }

        public static async void DisplayRecord(ChartRecord record) {
            if (record.Type == "ProfilePage") {
                await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>() { { "Login", record.Parameter } });
            } else if (record.Type == "ArtworkPage") {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object>() { { "ArtworkId", record.Parameter } });
            }
        }
        #endregion

        #region Local class
        public class ChartRecord {
            public ImageSource Image { get { return ImageBase.Base64ToImageSource(); } }
            public string ImageBase { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Parameter { get; set; }
            public string Value { get; set; }
            public ChartRecord(ApiChartResult chartResult) {
                ImageBase = chartResult.Image;
                Title = chartResult.Title;
                Type = chartResult.Type;
                Parameter = chartResult.Parameter;
                Value = chartResult.Value;
            }
        }
        #endregion
    }
}