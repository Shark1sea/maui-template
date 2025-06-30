using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using MudBlazor.Services;
using maui_template.Services;
#if WINDOWS
using Microsoft.UI.Windowing;
#endif

namespace maui_template
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    // 这里是Windows平台的生命周期事件配置
                    // 你可以在这里添加对窗口创建、激活等事件的处理
                    events.AddWindows(windows =>
                        windows.OnWindowCreated(window =>
                        {
                            MauiWinUIWindow mauiwin = window as MauiWinUIWindow;
                            if (mauiwin == null) { return; }
                            //关闭扩展内容
                            mauiwin.ExtendsContentIntoTitleBar = true;
                            //通过maui窗口句柄获取appwindow---
                            AppWindow appWindow = mauiwin.AppWindow;
                            //对于OverlappedPresenter的解释文档在这个网址
                            //https://learn.microsoft.com/zh-cn/windows/windows-app-sdk/api/winrt/microsoft.ui.windowing.overlappedpresenter?view=windows-app-sdk-1.2
                            var appWindowTitleBar = appWindow.TitleBar;
                            appWindowTitleBar.ExtendsContentIntoTitleBar = true;// 扩展内容到标题栏
                            appWindowTitleBar.PreferredHeightOption = TitleBarHeightOption.Collapsed;// 标题栏高度可折叠
                            var overlappedPresenter = OverlappedPresenter.Create();
                            overlappedPresenter.SetBorderAndTitleBar(true, false);// 保留了调节窗口大小的功能
                            overlappedPresenter.IsResizable = true;
                            overlappedPresenter.IsMaximizable = true;
                            overlappedPresenter.IsMinimizable = true;
                            overlappedPresenter.IsAlwaysOnTop = false;
                            overlappedPresenter.PreferredMinimumHeight = 600;
                            overlappedPresenter.PreferredMinimumWidth = 800;
                            overlappedPresenter.IsModal = false;
                            appWindow.SetPresenter(overlappedPresenter);
                        }));
#else
                    // 这里是其他平台的生命周期事件配置
#endif
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<WindowService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
