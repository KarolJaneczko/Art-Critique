﻿using Art_Critique.Pages.BasePages;
using Art_Critique.Utils.Helpers;
using Art_Critique_Api.Models.Search;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Art_Critique.Pages.FeaturePages {
    public class SearchPageViewModel : BaseViewModel {
        #region Properties
        private readonly ObservableCollection<SearchRecord> Profiles = new(), Artworks = new();
        private ObservableCollection<SearchRecord> searchResult = new();
        private bool isLoading = true;

        public ObservableCollection<SearchRecord> SearchResults { get => searchResult; set { searchResult = value; OnPropertyChanged(nameof(SearchResults)); } }
        public bool IsLoading { get => isLoading; set { isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }

        #region Commands
        public ICommand PerformSearch => new Command<string>((query) => {
            SearchResults.Clear();
            foreach (var profile in from profile in Profiles where profile.Title.Contains(query, StringComparison.OrdinalIgnoreCase) select profile) {
                SearchResults.Add(profile);
            }
            foreach (var artwork in from artwork in Artworks where artwork.Title.Contains(query, StringComparison.OrdinalIgnoreCase) select artwork) {
                SearchResults.Add(artwork);
            }
        });
        public ICommand DisplayRecordCommand => new Command<SearchRecord>(DisplayRecord);
        #endregion
        #endregion

        #region Constructor
        public SearchPageViewModel(List<ApiSearchResult> profiles, List<ApiSearchResult> artworks) {
            FillSearchPage(profiles, artworks);
        }
        #endregion

        #region Methods
        private void FillSearchPage(List<ApiSearchResult> profiles, List<ApiSearchResult> artworks) {
            profiles.ForEach(x => Profiles.Add(new SearchRecord(x)));
            artworks.ForEach(x => Artworks.Add(new SearchRecord(x)));
            IsLoading = false;
        }

        public static async void DisplayRecord(SearchRecord record) {
            if (record.Type == "ProfilePage") {
                await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>() { { "Login", record.Parameter } });
            } else if (record.Type == "ArtworkPage") {
                await Shell.Current.GoToAsync(nameof(ArtworkPage), new Dictionary<string, object>() { { "ArtworkId", record.Parameter } });
            }
        }
        #endregion

        #region Local class
        public class SearchRecord {
            public ImageSource Image {
                get {
                    if (Type == "ProfilePage") {
                        return !string.IsNullOrEmpty(ImageBase) ? ImageBase.Base64ToImageSource() : "defaultuser_icon.png";
                    } else {
                        return ImageBase.Base64ToImageSource();
                    }
                }
            }
            public string ImageBase { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Parameter { get; set; }
            public SearchRecord(ApiSearchResult searchResult) {
                ImageBase = searchResult.Image;
                Title = searchResult.Title;
                Type = searchResult.Type;
                Parameter = searchResult.Parameter;
            }
        }
        #endregion
    }
}