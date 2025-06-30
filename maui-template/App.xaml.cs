using maui_template.ForWindows.Windows;
namespace maui_template
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
#if WINDOWS
            var mainWindow = new MainWindow();
            return mainWindow;
#else
    // 其他平台默认窗口
            return new Window(new MainPage());
#endif
        }
    }
}
