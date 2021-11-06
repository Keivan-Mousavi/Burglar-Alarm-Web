using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;

namespace BurglarAlarm.FaceDetection
{
    public static class FaceDetectionEmguCV
    {
        static CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        public static void DetectedMultiFace(string path)
        {
            var file = new FileStream(path, FileMode.Open);

            var bitmap = new Bitmap(file);

            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(path);

            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.4, 0);

            foreach (var item in rectangles)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Red, 1))
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
