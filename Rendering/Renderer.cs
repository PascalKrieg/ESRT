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
    /// <summary>
    /// Class that abstracts the usage of a raytracer down to a simple Render method that returns the rendered image.
    /// </summary>
    public unsafe class Renderer
    {
        RenderSettings settings;
        Bitmap image;
        Raytracer raytracer;

        /// <summary>
        /// Constructs a Renderer object.
        /// </summary>
        /// <param name="raytracer">The Raytracer to be used. Has to be fully initialized.</param>
        public Renderer(Raytracer raytracer)
        {
            this.settings = raytracer.Settings;
            this.raytracer = raytracer;
            image = new Bitmap(settings.Resolution.width, settings.Resolution.height, PixelFormat.Format24bppRgb);
        }

        /// <summary>
        /// Renders the scene that is set in the raytracer object.
        /// </summary>
        /// <returns>Returns the rendered image.</returns>
        public Bitmap RenderImage()
        {
            // Prepare frame buffer
            BitmapData frameBuffer = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* bufferPointer = (byte*)frameBuffer.Scan0.ToPointer();

            // Start threads with the correct sections
            Thread[] threads = new Thread[settings.AmountThreads];
            for (int i = 0; i < settings.AmountThreads; i++)
            {
                RenderSectionParameters renderSection = new RenderSectionParameters(bufferPointer,
                    0, (i + 1) * (settings.Resolution.width / settings.AmountThreads),
                    0, settings.Resolution.height, raytracer);

                threads[i] = new Thread(new ThreadStart(renderSection.Execute));
                threads[i].Start();
            }

            // Wait for every thread to finish
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            // Convert buffer back to Bitmap
            image.UnlockBits(frameBuffer);

            return image;
        }
    }
}
