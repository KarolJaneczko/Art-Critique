namespace Art_Critique.Core.Services.Interfaces {
    public interface IPropertiesService {
        public double GetWidthByPercent(int percent);
        public int GetWidthPerMille(int perMille);
        public double GetHeightByPercent(int percent);
        public int GetHeightPerMille(int perMille);
    }
}