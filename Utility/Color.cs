using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    public class Color
    {
        /// <summary>
        /// The Red component as value in interval [0, 1]. Can be outside this interval.
        /// </summary>
        public float r { get; set; }
        /// <summary>
        /// The Green component as value in interval [0, 1]. Can be outside this interval.
        /// </summary>
        public float g { get; set; }
        /// <summary>
        /// The Blue component as value in interval [0, 1]. Can be outside this interval.
        /// </summary>
        public float b { get; set; }

        /// <summary>
        /// Creates an RGB Color instance using values [0, 1]. Can be outside this interval for usage in some edge cases (like bloom).
        /// </summary>
        /// <param name="r">Red component</param>
        /// <param name="g">Green component</param>
        /// <param name="b">Blue component</param>
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        /// <summary>
        /// Creates an RGB Color instance using values [0, 1]. Can be outside this interval for usage in some edge cases (like bloom).
        /// </summary>
        /// <param name="values">The Vector containing the values</param>
        public Color(Vector3 values)
        {
            this.r = values.x;
            this.g = values.y;
            this.b = values.z;
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

        /// <summary>
        /// Limits all values between 0 and 1.
        /// </summary>
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

        /// <summary>
        /// Converts the Color to a tuple of components, usually in intervals [0, 255]. The Values can ocassionaly be higher than 255.
        /// </summary>
        /// <returns>The tuple of components, usually in intervals [0, 255]</returns>
        public (byte r, byte g, byte b) To24BitRepresentation()
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
