using System;
using System.Drawing;

namespace ForageBuddy
{
    public class ScreenshotService : IScreenshotService
    {
        // TODO: This works for now, but should surely be a bit nicer...
        public Bitmap CaptureWindow(IntPtr handle)
        {
            using var bitmap = new Bitmap(1920, 1080);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0,
                bitmap.Size, CopyPixelOperation.SourceCopy);
            }

            return bitmap;
        }

       
    }
}
