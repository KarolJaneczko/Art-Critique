﻿using Art_Critique.Core.Logic;

namespace Art_Critique;

public partial class App : Application {
    public App() {
        InitializeComponent();
        MainPage = new AppShell();
        OnLaunch();
        
    }
    public static void OnLaunch() {
        DeviceProperties.InitializeScreenSizeValues();
    }
}
