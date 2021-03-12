using System;
using System.Drawing;

namespace ForageBuddy
{
    public interface IScreenshotService
    {
        Bitmap CaptureWindow(IntPtr handle);
    }
}
