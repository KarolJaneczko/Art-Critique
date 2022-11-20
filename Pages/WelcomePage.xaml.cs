using Art_Critique.Core.Logic;

namespace Art_Critique;

public partial class WelcomePage : ContentPage {
    public WelcomePage() {
        InitializeComponent();
        OnPageCreating();
    }
    
    private void OnPageCreating() {
        WelcomeLabel.Margin = new Thickness(DeviceProperties.GetHeightPercent(1), DeviceProperties.GetHeightPercent(5), 0, 0);
    }
}