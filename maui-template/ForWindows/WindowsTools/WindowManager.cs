using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace maui_template.ForWindows.WindowsTools
{
    internal static class WindowManager
    {
        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_MAXIMIZE = 3;
        private const int SW_SHOWNOACTIVE = 4;
        private const int SW_SHOW = 5;
        private const int SW_MINIMIZE = 6;
        private const int SW_SHOWMINNOACTIVE = 7;
        private const int SW_SHOWNA = 8;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWDEFAULT = 10;
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsZoomed(IntPtr hWnd);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);
        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hWnd);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static void SetWindowVisibility(IntPtr hWnd, bool isVisible)
        {
            if (isVisible)
            {
                ShowWindow(hWnd, SW_SHOW); // 显示窗口
            }
            else
            {
                ShowWindow(hWnd, SW_HIDE); // 隐藏窗口
            }
        }
        public static void MinimizeWindow(IntPtr hWnd)
        {
            ShowWindow(hWnd, SW_MINIMIZE);
        }
        public static void MaximizeWindow(IntPtr hWnd)
        {
            if (IsZoomed(hWnd)) // 检查窗口是否已经最大化
            {
                RestoreWindow(hWnd); // 如果已经最大化，则恢复窗口
            }
            else
            {
                ShowWindow(hWnd, SW_MAXIMIZE);
            }
        }
        public static void RestoreWindow(IntPtr hWnd)
        {
            ShowWindow(hWnd, SW_RESTORE); // 恢复窗口
        }
        public static void HideWindow(IntPtr hWnd)
        {
            ShowWindow(hWnd, SW_HIDE); // 隐藏窗口
        }
        public static void MinimizeToTray(IntPtr hWnd)
        {
            HideWindow(hWnd); // 最小化到托盘
        }
        public static RECT GetWindowRect(IntPtr hWnd)
        {
            GetWindowRect(hWnd, out RECT rect);
            return rect;
        }
        public static (int X, int Y, int Width, int Height) GetWindowPositionAndSize(IntPtr hWnd)
        {
            var rect = GetWindowRect(hWnd);
            int x = rect.Left;
            int y = rect.Top;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            return (x, y, width, height);
        }
        public static void MoveWindow(IntPtr hWnd, int x, int y)
        {
            var positionNSize = GetWindowPositionAndSize(hWnd);
            MoveWindow(hWnd, x, y, positionNSize.Width, positionNSize.Height, true); // 移动窗口到指定位置和大小
        }
        public static (int Width, int Height) GetScreenSize()
        {
            const int SM_CXSCREEN = 0;
            const int SM_CYSCREEN = 1;
            int width = GetSystemMetrics(SM_CXSCREEN);
            int height = GetSystemMetrics(SM_CYSCREEN);
            return (width, height);
        }

        public static double GetScreenScale(IntPtr hWnd)
        {
            // 标准DPI为96，缩放比例=当前DPI/96
            uint dpi = GetDpiForWindow(hWnd);
            return dpi / 96.0;
        }
        public static (int X, int Y) GetCursorPosition()
        {
            POINT point;
            GetCursorPos(out point);
            return (point.X, point.Y);
        }
    }
}
