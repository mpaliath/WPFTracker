using System.Runtime.InteropServices;

namespace WPFTracker
{
    //
    // Summary:
    //     Win32 API imports.
    internal static class WinApi
    {


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetCursorPos(ref Point lpPoint);
    }

    //
    // Summary:
    //     Win API struct providing coordinates for a single point.
    public struct Point
    {
        //
        // Summary:
        //     X coordinate.
        public int X;

        //
        // Summary:
        //     Y coordinate.
        public int Y;
    }
}
