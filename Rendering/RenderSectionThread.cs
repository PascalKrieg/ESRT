using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRT.Entities;

namespace ESRT.Rendering
{
    /// <summary>
    /// Class that contains the information required to run the raytracer on a specific rectangle on the bitmap.
    /// After creation, the Execute method has to be called on a new thread.
    /// </summary>
    unsafe class RenderSectionThread
    {
        byte* rawData;
        int startX, endX, startY, endY;

        private Raytracer raytracer;

        /// <summary>
        /// Constructs the Object containing target bitmap, rectangle and raytracer to use.
        /// </summary>
        /// <param name="rawData">Pointer to the first element of the bitmap raw data.</param>
        /// <param name="startX">The lower bound of the x values in the rectangle.</param>
        /// <param name="endX">The upper bound of the x values in the rectangle.</param>
        /// <param name="startY">The lower bound of the y values in the rectangle.</param>
        /// <param name="endY">The upper bound of the y values in the rectangle.</param>
        /// <param name="raytracer">The raytracer to be used to render the image.</param>
        public RenderSectionThread(byte* rawData, int startX, int endX, int startY, int endY, Raytracer raytracer)
        {
            this.rawData = rawData;
            this.startX = startX;
            this.endX = endX;
            this.startY = startY;
            this.endY = endY;
            this.raytracer = raytracer;
        }

        /// <summary>
        /// Starts rendering the image in the assigned rectangle, using the given raytracer.
        /// The result will be written to the correct index at the rawData memory address.
        /// </summary>
        public void Execute()
        {
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    setPixel(x, y, raytracer.CalculatePixelColor(x, y));
                }
            }
        }

        private void setPixel(int x, int y, Color color)
        {
            int offset = (3 * raytracer.Settings.Resolution.width * y) + 3 * x;
            (byte r, byte g, byte b) tristimulus = color.To24BitRepresentation();
            *(rawData + offset) = tristimulus.b;
            *(rawData + offset + 1) = tristimulus.g;
            *(rawData + offset + 2) = tristimulus.r;
        }
    }
}
