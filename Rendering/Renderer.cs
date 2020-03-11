using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace ESRT.Rendering
{
    public unsafe class Renderer
    {
        RenderSettings settings;
        Bitmap image;
        Raytracer raytracer;

        Scene scene { get; set; }

        public Renderer(Raytracer raytracer)
        {
            this.scene = raytracer.CurrentScene;
            this.settings = raytracer.Settings;
            this.raytracer = raytracer;
            image = new Bitmap(settings.Resolution.width, settings.Resolution.height, PixelFormat.Format24bppRgb);
        }

        public Bitmap RenderImage()
        {
            BitmapData sourceData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* sourcePointer = (byte*)sourceData.Scan0.ToPointer();

            Thread[] threads = new Thread[settings.AmountThreads];

            for (int i = 0; i < settings.AmountThreads; i++)
            {
                RenderSectionThread rst = new RenderSectionThread(sourcePointer,
                    0, (i + 1) * (settings.Resolution.width / settings.AmountThreads),
                    0, settings.Resolution.height, raytracer);

                threads[i] = new Thread(new ThreadStart(rst.Execute));
                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            image.UnlockBits(sourceData);

            return image;
        }
    }
}
