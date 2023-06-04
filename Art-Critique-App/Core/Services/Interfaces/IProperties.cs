namespace Art_Critique.Core.Services.Interfaces {
    public interface IProperties {
        public int GetWidthPercent(int percent);
        public int GetWidthPerMille(int perMille);
        public int GetHeightPercent(int percent);
        public int GetHeightPerMille(int perMille);
    }
}