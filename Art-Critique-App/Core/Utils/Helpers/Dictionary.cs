namespace Art_Critique.Core.Utils.Helpers {
    public static class Dictionary {
        public const string ApiAddress = "https://10.0.2.2:7038/api/";
        public const string UserRegister = "User/RegisterUser";
        public const string UserLogin = "User/Login";
        public const string UserLogout = "User/Logout";
        public const string ProfileGet = "Profile/GetProfile";
        public const string ProfileEdit = "Profile/EditProfile";
        public const string ProfileViewCount = "Profile/GetTotalViews";
        public const string ArtworkGetGenres = "Artwork/GetArtworkGenres";
        public const string InsertUserArtwork = "Artwork/InsertUserArtwork";
        public const string GetUserArtwork = "Artwork/GetUserArtwork";
        public const string GetLast3UserArtworks = "Artwork/GetLast3UserArtworks";
        public const string EditUserArtwork = "Artwork/EditUserArtwork";
        public const string AddViewToArtwork = "Artwork/AddViewToArtwork";
        public const string GetUserArtworks = "Artwork/GetUserArtworks";

        #region Review paths
        public const string GetArtworkReview = "Review/GetArtworkReview";
        public const string GetArtworkReviews = "Review/GetArtworkReviews";
        public const string GetAverageRatingInfo = "Review/GetAverageRatingInfo";
        public const string GetRating = "Review/GetRating";

        public const string CreateOrUpdateReview = "Review/CreateOrUpdateReview";
        public const string RateArtwork = "Review/RateArtwork";
        public const string RemoveRating = "Review/RemoveRating";
        public const string RemoveReview = "Review/RemoveReview";
        #endregion
    }
}