using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Collections.Concurrent;

namespace ESRT.Rendering
{
    /// <summary>
    /// Class that abstracts the usage of a raytracer down to a simple Render method that returns the rendered image.
    /// </summary>
    public unsafe class Renderer
    {
        readonly RenderSettings settings;
        readonly Bitmap image;
        readonly Raytracer raytracer;

        // Framebuffer related variables
        BitmapData frameBuffer;
        byte* bufferPointer;

        ConcurrentQueue<RenderSection> sectionQueue = new ConcurrentQueue<RenderSection>();

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
            return RenderImage(new Action<Bitmap>((bitmap) => { } ), 0);
        }

        /// <summary>
        /// Renders the scene that is set in the raytracer object.
        /// Experimental overload: While rendering callbacks will be made to allow for displaying rendering progress.
        /// The interval can not be set directly, but the minimum interval between callbacks can be set to free more
        /// CPU time for rendering threads.
        /// </summary>
        /// <param name="periodicCallback">Callback method that takes a Bitmap as sole parameter.
        /// On every call, the method is passed a Bitmap containing the current rendering progress.</param>
        /// <param name="minimumInterval">Optional. The minimum delay between callbacks.
        /// This will rarely have a big impact, as generating the intermediate image takes a lot of time (usually over 1.5s).</param>
        /// <returns>Returns the rendered image.</returns>
        public Bitmap RenderImage(Action<Bitmap> periodicCallback, int minimumInterval = 0)
        {
            // Prepare frame buffer
            frameBuffer = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            bufferPointer = (byte*)frameBuffer.Scan0.ToPointer();

            sectionQueue = new ConcurrentQueue<RenderSection>();

            // Start render threads
            Thread renderThread = new Thread(new ThreadStart(() => RunRenderThreads()));
            renderThread.Start();

            // Perform periodic callbacks while the threads are rendering
            while(renderThread.IsAlive)
            {
                periodicCallback(GetIntermediateResult());
                Thread.Sleep(minimumInterval);
            }

            renderThread.Join();

            image.UnlockBits(frameBuffer);
            return image;
        }

        /// <summary>
        /// Starts the rendering threads according to the render settings and only returns when all rendering threads have terminated.
        /// Important: The Framebuffer has to be initialized beforehand.
        /// </summary>
        private void RunRenderThreads()
        {
            int gridResolution = 8;

            // Generate sections
            int sectionWidth = settings.Resolution.width / gridResolution;
            int sectionHeight = settings.Resolution.height / gridResolution;
            for (int i = 0; i < gridResolution; i++)
            {
                for (int j = 0; j < gridResolution; j++)
                {
                    (int start, int end) xDimensions = (i * sectionWidth, (i + 1) * sectionWidth);
                    (int start, int end) yDimensions = (j * sectionHeight, (j + 1) * sectionHeight);

                    // Handle edge cases where width and height aren't evenly divisible by the grid resolution
                    if (i == gridResolution - 1)
                        xDimensions.end = settings.Resolution.width;

                    if (j == gridResolution - 1)
                        yDimensions.end = settings.Resolution.height;

                    RenderSection renderSection = new RenderSection(bufferPointer,
                        xDimensions.start, xDimensions.end,
                        yDimensions.start, yDimensions.end, raytracer);

                    sectionQueue.Enqueue(renderSection);
                }
            }

            // Start threads with the correct sections
            Thread[] threads = new Thread[settings.AmountThreads];
            for (int i = 0; i < settings.AmountThreads; i++)
            {
                threads[i] = new Thread(new ThreadStart(() =>
                {
                    while (sectionQueue.TryDequeue(out RenderSection section))
                    {
                        section.CalculateSection();
                    }
                }));
                threads[i].Start();
            }

            // Wait for every thread to finish
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

        /// <summary>
        /// Copies the current framebuffer into a new bitmap that can be unlocked and returned.
        /// Important: The Framebuffer has to be initialized beforehand.
        /// </summary>
        /// <returns>Returns a bitmap containing a copy of the current framebuffer state.</returns>
        private Bitmap GetIntermediateResult()
        {
            Bitmap intermediateResult = new Bitmap(settings.Resolution.width, settings.Resolution.height, PixelFormat.Format24bppRgb);
            BitmapData intermediateData = intermediateResult.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* intermediateDataPointer = (byte*)intermediateData.Scan0.ToPointer();

            for (int i = 0; i < image.Width * image.Height * 3; i++)
            {
                intermediateDataPointer[i] = bufferPointer[i];
            }

            intermediateResult.UnlockBits(intermediateData);

            return intermediateResult;
        }
    }
}
