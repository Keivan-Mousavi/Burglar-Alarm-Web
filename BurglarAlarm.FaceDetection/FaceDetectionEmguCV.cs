using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace BurglarAlarm.FaceDetection
{
    public static class FaceDetectionEmguCV
    {
        static CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade/haarcascade_frontalface_alt_tree.xml");

        public static void DetectedMultiFace(Image imageStream)
        {
            var bitmap = new Bitmap(imageStream);
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);//System.Drawing
            BitmapData bmpData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);//System.Drawing.Imaging

            Image<Bgr, byte> image = new Image<Bgr, byte>(bitmap.Width, bitmap.Height, bmpData.Stride, bmpData.Scan0);//(IntPtr)
        }
    }
}
