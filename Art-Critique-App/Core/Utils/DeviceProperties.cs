using System;

namespace Art_Critique.Core.Utils {
    public static class DeviceProperties {
        public static double ScreenWidth { get; set; }
        public static double ScreenHeight { get; set; }
        public static void InitializeScreenSizeValues() {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            ScreenWidth = mainDisplayInfo.Width;
            ScreenHeight = mainDisplayInfo.Height;
        }
        public static int GetWidthPercent(int percent) {
            return (int)Math.Ceiling(ScreenWidth * percent / 100);
        }

        public static int GetWidthPerMille(int perMille) {
            return (int)Math.Ceiling(ScreenWidth * perMille / 1000);
        }

        public static int GetHeightPercent(int percent) {
            return (int)Math.Ceiling(ScreenHeight * percent / 100);
        }

        public static int GetHeightPerMille(int perMille) {
            return (int)Math.Ceiling(ScreenHeight * perMille / 1000);
        }
    }
}
