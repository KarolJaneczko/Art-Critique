using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique.Core.Services {
    public class PropertiesService : IPropertiesService {
        public double GetWidthByPercent(int percent) {
            return Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * percent / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }

        public int GetWidthPerMille(int perMille) {
            return (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Width * perMille / 1000);
        }

        public double GetHeightByPercent(int percent) {
            return Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * percent / 100) / DeviceDisplay.MainDisplayInfo.Density;
        }

        public int GetHeightPerMille(int perMille) {
            return (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Height * perMille / 1000);
        }
    }
}
