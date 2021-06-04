using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BurglarAlarm.FaceDetection
{
    public static class FaceDetectionEmguCV
    {
        static CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        public static void DetectedMultiFace(Image imageStream)
        {
            var bitmap = new Bitmap(imageStream);
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);//System.Drawing
            BitmapData bmpData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);//System.Drawing.Imaging

            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bitmap.Width, bitmap.Height, bmpData.Stride, bmpData.Scan0);//(IntPtr)

            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.4, 0);

            foreach(var item in rectangles)
            {
                using(Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using(Pen pen= new Pen(Color.Red, 1))
                    {
                        graphics.DrawRectangle(pen, item);
                    }
                }
            }

            var ii = ImageToByte(bitmap);

            var sb = Convert.ToBase64String(ii);
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
