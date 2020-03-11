using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    public class Vector3
    {
        private bool isLengthDirty = false;

        private float xValue;
        public float x
        {
            get
            {
                return xValue;
            }
            set
            {
                isLengthDirty = true;
                xValue = value;
            }
        }

        private float yValue;
        public float y
        {
            get
            {
                return yValue;
            }
            set
            {
                isLengthDirty = true;
                yValue = value;
            }
        }

        private float zValue;
        public float z
        {
            get
            {
                return zValue;
            }
            set
            {
                isLengthDirty = true;
                zValue = value;
            }
        }

        private float lengthValue;
        float Length
        {
            get
            {
                if (isLengthDirty)
                {
                    lengthValue = (float)Math.Sqrt((x * x) + (y * y) + (z * z));
                    isLengthDirty = false;
                }
                return lengthValue;
            }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Normalize()
        {
            float currentLength = Length;
            if (currentLength < 0.001f)
            {
                return;
            }
            x /= currentLength;
            y /= currentLength;
            z /= currentLength;
            lengthValue = 1f;
            isLengthDirty = false;
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            Vector3 vec = new Vector3(vector.x, vector.y, vector.z);
            vec.Normalize();
            return vec;
        }

        public float Angle(Vector3 vec)
        {
            return (float)Math.Acos(this.CosAngle(vec));
        }

        public float CosAngle(Vector3 vec)
        {
            return (this * vec) / (vec.Length * this.Length);
        }

        public (float u, float v) CalculateAzimut()
        {
            Vector3 xzProjection = new Vector3(x, 0, z);

            float u = 0;
            float v = 0;
            if (x == 0 && z == 0)
            {
                u = 0;
                v = Math.Sign(y) * (float)Math.PI / 2;
                
            } 
            else
            {
                u = xzProjection.Angle(Vector3.Forward);
                if (x > 0)
                {
                    u = 2 * (float)Math.PI - u;
                }
                if (y > 0)
                {
                    v = xzProjection.Angle(this);
                } else
                {
                    v = -xzProjection.Angle(this);
                }
                
            }

            return (u, v);
        }

        public Vector3 Reflect(Vector3 normal)
        {
            normal.Normalize();
            var result = this - 2 * (this * normal) * normal;
            result.Normalize();
            return result;
        }

        public static Vector3 CrossProduct(Vector3 vec1, Vector3 vec2)
        {
            float x = vec1.y * vec2.z - vec1.z * vec2.y;
            float y = vec1.z * vec2.x - vec1.x * vec2.z;
            float z = vec1.x * vec2.y - vec1.y * vec2.x;
            return new Vector3(x, y, z);
        }

        public Vector3 CrossProduct(Vector3 vec)
        {
            return CrossProduct(this, vec);
        }

        public static float Distance(Vector3 vec1, Vector3 vec2)
        {
            return vec1.Distance(vec2);
        }

        public float Distance(Vector3 vec)
        {
            return (this - vec).Length;
        }

        public static Vector3 operator+ (Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
        }

        public static Vector3 operator- (Vector3 vec1, Vector3 vec2)
        {
            return new Vector3(vec1.x - vec2.x, vec1.y - vec2.y, vec1.z - vec2.z);
        }

        public static float operator* (Vector3 vec1, Vector3 vec2)
        {
            return vec1.x * vec2.x + vec1.y * vec2.y + vec1.z * vec2.z;
        }
        public static Vector3 operator* (float a, Vector3 vec)
        {
            return new Vector3(a * vec.x, a * vec.y, a * vec.z);
        }
        public static Vector3 operator *(Vector3 vec, float a)
        {
            return new Vector3(a * vec.x, a * vec.y, a * vec.z);
        }

        public static Vector3 Zero { get => new Vector3(0, 0, 0); }
        public static Vector3 One { get => new Vector3(1, 1, 1); }
        public static Vector3 Up { get => new Vector3(0, 1, 0); }
        public static Vector3 Down { get => new Vector3(0, -1, 0); }
        public static Vector3 Forward { get => new Vector3(0, 0, 1); }
        public static Vector3 Backward { get => new Vector3(0, 0, -1); }
        public static Vector3 Left { get => new Vector3(-1, 0, 0); }
        public static Vector3 Right { get => new Vector3(1, 0, 0); }
    }
}
