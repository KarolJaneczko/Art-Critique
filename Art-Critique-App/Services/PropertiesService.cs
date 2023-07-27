using Art_Critique.Services.Interfaces;

namespace Art_Critique.Services {
    public class PropertiesService : IPropertiesService {
        #region Methods
        public double GetWidthByPercent(int percent) {
            return Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * percent / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }

        public double GetHeightByPercent(int percent) {
            return Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * percent / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }
        #endregion
    }
}