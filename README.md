# Maui-Template
这是一个基于.NET9 Maui Blazor Hybird App项目模板的应用模板，带有自定义边框，在Blazor中实现窗口的基本功能，例如最大化，最小化，拖动等
## 注意
我的显示器分辨率为2560 * 1440，系统缩放1.25
由于未知原因，使用Microsoft.AspNetCore.Web.Components.MouseEventArgs实例获取的.ScreenX.ScreenY属性的最大值（或者说范围），即鼠标在屏幕右下角时他们的值，既不等于屏幕尺寸（2560，1440），也不等于屏幕尺寸 / 缩放比例 （2048，1152），而是等于屏幕尺寸 / 缩放比例 / 1.1；使用JavaScript获取的值有同样的问题。
因此我仅使用JavaScript判断鼠标是否持续按下或松开，而不采用它获取的属性值。鼠标位置和屏幕位置统一使用windows API获取（他们都是基于物理像素，即2560 * 1440），避免了窗口移动与鼠标移动不同步。
如果你有关于这个问题的一些想法，欢迎与我沟通，或直接在其他分支中修改，谢谢！
## 一些参考网站
[应用生命周期](https://learn.microsoft.com/zh-cn/dotnet/maui/fundamentals/app-lifecycle?view=net-maui-9.0)
[AppWindowTitleBar](https://learn.microsoft.com/zh-cn/windows/windows-app-sdk/api/winrt/microsoft.ui.windowing.appwindowtitlebar?view=windows-app-sdk-1.7)
[OverlappedPresenter](https://learn.microsoft.com/zh-cn/windows/windows-app-sdk/api/winrt/microsoft.ui.windowing.overlappedpresenter?view=windows-app-sdk-1.7)
[MAUI 第六天 使用MASA Blazor 处理界面相关问题](https://blog.csdn.net/xingchengaiwei/article/details/130104293)
