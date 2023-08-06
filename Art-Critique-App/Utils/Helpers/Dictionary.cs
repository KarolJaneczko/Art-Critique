namespace Art_Critique.Utils.Helpers {
    public static class Dictionary {
        #region Adresses
        public const string ApiAddress = "https://10.0.2.2:7038/api/";
        #endregion

        #region Artwork paths
        public const string ArtworkGetGenres = "Artwork/GetArtworkGenres";
        public const string GetLast3UserArtworks = "Artwork/GetLast3UserArtworks";
        public const string GetUserArtwork = "Artwork/GetUserArtwork";
        public const string GetUserArtworks = "Artwork/GetUserArtworks";

        public const string AddViewToArtwork = "Artwork/AddViewToArtwork";
        public const string EditUserArtwork = "Artwork/EditUserArtwork";
        public const string InsertUserArtwork = "Artwork/InsertUserArtwork";
        #endregion

        #region Profile paths
        public const string ProfileGet = "Profile/GetProfile";
        public const string ProfileViewCount = "Profile/GetTotalViews";
        public const string ProfileEdit = "Profile/EditProfile";
        #endregion

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

        #region Search paths
        public const string GetAllArtworks = "Search/GetAllArtworks";
        public const string GetAllProfiles = "Search/GetAllProfiles";
        public const string GetArtworksByAverageRating = "Search/GetArtworksByAverageRating";
        public const string GetArtworksByTotalViews = "Search/GetArtworksByTotalViews";
        public const string GetProfilesByAverageRating = "Search/GetProfilesByAverageRating";
        public const string GetProfilesByTotalViews = "Search/GetProfilesByTotalViews";
        #endregion

        #region User paths
        public const string CheckFollowing = "User/CheckFollowing";
        public const string UserLogin = "User/Login";
        public const string UserLogout = "User/Logout";
        public const string ActivateAccount = "User/ActivateAccount";
        public const string FollowUser = "User/FollowUser";
        public const string UserRegister = "User/RegisterUser";
        public const string ResendActivationCode = "User/ResendActivationCode";
        #endregion
    }
}