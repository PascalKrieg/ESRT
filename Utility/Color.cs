using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    public class Color
    {
        public float r { get; set; }
        public float g { get; set; }
        public float b { get; set; }

        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        public Color(Vector3 tristimulus)
        {
            this.r = tristimulus.x;
            this.g = tristimulus.y;
            this.b = tristimulus.z;
        }
        public Color(string hexRepresentation)
        {
            throw new NotImplementedException();
        }

        public static Color operator +(Color color1, Color color2)
        {
            return new Color(color1.r + color2.r, color1.g + color2.g, color1.b + color2.b);
        }
        public static Color operator -(Color color1, Color color2)
        {
            return new Color(color1.r - color2.r, color1.g - color2.g, color1.b - color2.b);
        }

        public static Color operator *(float a, Color color)
        {
            return new Color(a * color.r, a * color.g, a * color.b);
        }
        public static Color operator *(Color color, float a)
        {
            return new Color(a * color.r, a * color.g, a * color.b);
        }

        public void Clamp()
        {
            if (r > 1f)
            {
                r = 1;
            } 
            else if (r < 0)
            {
                r = 0;
            }
            if (g > 1f)
            {
                g = 1;
            }
            else if (g < 0)
            {
                g = 0;
            }
            if (b > 1f)
            {
                b = 1;
            }
            else if (b < 0)
            {
                b = 0;
            }
        }

        public (byte r, byte g, byte b) ToTristimulus()
        {
            return ((byte)Math.Round(r * 255), (byte)Math.Round(g * 255), (byte)Math.Round(b * 255));
        }

        public static Color Black { get => new Color(0, 0, 0); }
        public static Color White { get => new Color(1, 1, 1); }
        public static Color Red { get => new Color(1, 0, 0); }
        public static Color Green { get => new Color(0, 1, 0); }
        public static Color Blue { get => new Color(0, 0, 1); }
    }
}
