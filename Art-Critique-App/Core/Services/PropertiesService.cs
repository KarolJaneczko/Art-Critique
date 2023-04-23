using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Core.Services {
    public class PropertiesService : IProperties {
        #region Implementation of methods
        public int GetWidthPercent(int percent) {
            return (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * percent / 100);
        }

        public int GetWidthPerMille(int perMille) {
            return (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * perMille / 1000);
        }

        public int GetHeightPercent(int percent) {
            return (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * percent / 100);
        }

        public int GetHeightPerMille(int perMille) {
            return (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * perMille / 1000);
        }
        #endregion
    }
}
