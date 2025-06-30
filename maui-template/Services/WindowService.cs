using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using maui_template.ForWindows.WindowsTools;
#if WINDOWS
using Microsoft.UI.Windowing;
using WinRT.Interop;
#endif

namespace maui_template.Services
{
    internal class WindowService
    {
        private double lastMouseX = 0;
        private double lastMouseY = 0;
        private double lastWindowX = 0;
        private double lastWindowY = 0;
        private bool isWindowMoving = false;
        public WindowService() { }
#if WINDOWS
        private IntPtr GetWindowHandle(Window? window)
        {
            if (window?.Handler?.PlatformView is Microsoft.UI.Xaml.Window winUIWindow)
            {
                return WindowNative.GetWindowHandle(winUIWindow);
            }
            return IntPtr.Zero;
        }
#endif
        public void MinimizeWindow()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                WindowManager.MinimizeWindow(hWnd); // 最小化窗口
            }
            //还可以用WinUI的AppWindow和OverlappedPresenter来最大化窗口
            //var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            //var appWindow = AppWindow.GetFromWindowId(windowId);
            //if (appWindow.Presenter is OverlappedPresenter presenter)
            //{
            //    if (presenter.State != OverlappedPresenterState.Minimized)
            //    {
            //       presenter.Minimize(); // 最小化窗口
            //    }
            //}
#elif MACCATALYST
            // 在Mac Catalyst中，可能需要使用其他方式来最小化窗口
#endif
        }
        public void MaximizeWindow()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);

            if (hWnd != IntPtr.Zero)
            {
                //第一种方式使用WinUI的AppWindow和OverlappedPresenter来最大化窗口
                //var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
                //var appWindow = AppWindow.GetFromWindowId(windowId);
                //if (appWindow.Presenter is OverlappedPresenter presenter)
                //{
                //    if(presenter.State == OverlappedPresenterState.Maximized)
                //    {
                //        presenter.Restore(); // 如果已经最大化，则恢复窗口
                //    }
                //    else
                //    {
                //        presenter.Maximize(); // 最大化窗口
                //    }
                //}
                //第二种方式通过在WindowManager调用Win32API来最大化窗口
                WindowManager.MaximizeWindow(hWnd); // 最大化窗口
            }
#elif MACCATALYST
            // 在Mac Catalyst中，可能需要使用其他方式来最大化窗口
#endif
        }
        public void MinimizeToTray()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                WindowManager.MinimizeToTray(hWnd); // 最小化到托盘
            }
#endif
        }
        public void QuitApplication()
        {
            Application.Current?.Quit();
        }

        public (int Left, int Top, int Bottom, int Right) GetWindowPositon()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                WindowManager.RECT rect = WindowManager.GetWindowRect(hWnd);
                return (rect.Left, rect.Top, rect.Bottom, rect.Right);
            }
#endif
            return (0, 0, 0, 0);
        }
        public void StartWindowMove()
        {
            (lastMouseX, lastMouseY) = GetCursorPosition();
            var position = GetWindowPositon();
            lastWindowX = position.Left;
            lastWindowY = position.Top;
            isWindowMoving = true;
        }
        public void MoveWindow()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                // 计算鼠标移动的增量
                (double mouseX, double mouseY) = GetCursorPosition();
                var deltaX = mouseX - lastMouseX;
                var deltaY = mouseY - lastMouseY;

                // 计算新窗口位置
                var newWindowX = lastWindowX + deltaX;
                var newWindowY = lastWindowY + deltaY;

                WindowManager.MoveWindow(hWnd, Convert.ToInt32(newWindowX), Convert.ToInt32(newWindowY));

                // 更新上一次的位置
                lastMouseX = mouseX;
                lastMouseY = mouseY;
                lastWindowX = newWindowX;
                lastWindowY = newWindowY;
            }
#endif
        }
        public void EndWindowMove()
        {
            lastMouseX = 0;
            lastMouseY = 0;
            lastWindowX = 0;
            lastWindowY = 0;
            isWindowMoving = false;
        }
        public (int Width, int Height) GetScreenSize()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                var size = WindowManager.GetScreenSize();
                return (size.Width, size.Height);
            }
#endif
            return (0, 0);
        }
        public double GetScreenScale()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                return WindowManager.GetScreenScale(hWnd);
            }
#endif 
            return 1.0; // 默认返回1.0，表示100%缩放
        }
        public static string GetDisplayInfo()
        {
            var sb = new StringBuilder();
            sb.Append($"Size:{DeviceDisplay.Current?.MainDisplayInfo.Width.ToString()}" ?? "0");
            sb.Append($",{DeviceDisplay.Current?.MainDisplayInfo.Height.ToString()}" ?? "0");
            sb.Append($"密度{DeviceDisplay.Current?.MainDisplayInfo.Density.ToString()}" ?? "1.0");
            sb.Append($"方向{DeviceDisplay.Current?.MainDisplayInfo.Orientation.ToString()}" ?? "Unknown");
            sb.Append($"旋转{DeviceDisplay.Current?.MainDisplayInfo.Rotation.ToString()}" ?? "0");
            sb.Append($"刷新率{DeviceDisplay.Current?.MainDisplayInfo.RefreshRate.ToString()}" ?? "0");
            return sb.ToString();
        }
        public (int X, int Y) GetCursorPosition()
        {
            var mauiWindow = Application.Current?.Windows.FirstOrDefault();
#if WINDOWS
            var hWnd = GetWindowHandle(mauiWindow);
            if (hWnd != IntPtr.Zero)
            {
                WindowManager.POINT point;
                if (WindowManager.GetCursorPos(out point))
                {
                    return (point.X, point.Y);
                }
            }
#endif
            return (0, 0); // 如果无法获取光标位置，返回默认值
        }
    }
}
