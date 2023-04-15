using Art_Critique.Core.Services;
using Art_Critique.Core.Services.Interfaces;

namespace Art_Critique {
    public partial class App : Application {
        public App() {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}