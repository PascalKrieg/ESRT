using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ESRT.Entities;

namespace ESRT.Rendering
{
    unsafe class RenderSectionThread
    {
        byte* rawData;
        int startX, endX, startY, endY;

        private Raytracer raytracer;

        Vector3 sidewaysVector;
        float distance;

        public RenderSectionThread(byte* rawData, int startX, int endX, int startY, int endY, Raytracer raytracer)
        {
            this.rawData = rawData;
            this.startX = startX;
            this.endX = endX;
            this.startY = startY;
            this.endY = endY;
            this.raytracer = raytracer;

            distance = (float)Math.Tan((Math.PI / 360) * raytracer.CurrentScene.MainCamera.FieldOfView) * raytracer.Settings.Resolution.width / 2;
            raytracer.CurrentScene.MainCamera.Up.Normalize();
            raytracer.CurrentScene.MainCamera.ViewDirection.Normalize();
            sidewaysVector = ( -1 * raytracer.CurrentScene.MainCamera.ViewDirection ).CrossProduct( raytracer.CurrentScene.MainCamera.Up );
            sidewaysVector.Normalize();
        }

        public void Execute()
        {
            
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    Vector3 rayDirection = generateRay(x, y);
                    Color color = raytracer.Trace(raytracer.CurrentScene.MainCamera.Position, rayDirection);
                    setPixel(x, y, color);
                }
            }
        }

        private void setPixel(int x, int y, Color color)
        {
            int offset = (3 * raytracer.Settings.Resolution.width * y) + 3 * x;
            (byte r, byte g, byte b) tristimulus = color.ToTristimulus();
            *(rawData + offset) = tristimulus.b;
            *(rawData + offset + 1) = tristimulus.g;
            *(rawData + offset + 2) = tristimulus.r;
        }

        private Vector3 generateRay(int x, int y)
        {
            int xSteps = x - (int)(raytracer.Settings.Resolution.width / 2);
            int ySteps = y - (int)(raytracer.Settings.Resolution.height / 2);
            Vector3 result = -1 * distance * raytracer.CurrentScene.MainCamera.ViewDirection + xSteps * sidewaysVector + ySteps * raytracer.CurrentScene.MainCamera.Up;
            result.Normalize();
            return result;
        }
    }
}
