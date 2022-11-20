using System;
using System.Collections.Generic;
using System.Linq;

namespace Art_Critique.Core.Logic {
    public static class DeviceProperties {
        public static double ScreenWidth { get; set; }
        public static double ScreenHeight { get; set; }
        public static void InitializeScreenSizeValues() {
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            ScreenWidth = mainDisplayInfo.Width;
            ScreenHeight = mainDisplayInfo.Height;
        }
        public static int GetWidthPercent(int percent) {
            return (int)Math.Floor(ScreenWidth * percent / 100);
        }
        public static int GetHeightPercent(int percent) { 
            return (int)Math.Floor(ScreenHeight* percent / 100);
        }
    }
}
