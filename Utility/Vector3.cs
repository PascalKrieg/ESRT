using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    /// <summary>
    /// Represents a three-dimensional float vector. Contains all neccessary utility methods.
    /// </summary>
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
        public float Length
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

        /// <summary>
        /// Shortens the vector to length 1 while still pointing in the same direction.
        /// </summary>
        /// <returns>Returns the normalized version of the vector.</returns>
        public Vector3 Normalize()
        {
            float currentLength = Length;
            if (currentLength < 0.001f)
            {
                return this;
            }
            x /= currentLength;
            y /= currentLength;
            z /= currentLength;
            lengthValue = 1f;
            isLengthDirty = false;
            return this;
        }

        /// <summary>
        /// Creates a normalized version of the given vector.
        /// </summary>
        /// <param name="vector">The vector to be normalized.</param>
        /// <returns>Returns the normalized version of the vector.</returns>
        public static Vector3 Normalize(Vector3 vector)
        {
            Vector3 vec = new Vector3(vector.x, vector.y, vector.z);
            vec.Normalize();
            return vec;
        }

        /// <summary>
        /// Calculates the angle between this vector and another one.
        /// </summary>
        /// <param name="vec">The vector the angle to will be calculated.</param>
        /// <returns>Returns the angle between the vectors as radian.</returns>
        public float Angle(Vector3 vec)
        {
            return (float)Math.Acos(this.CosAngle(vec));
        }

        /// <summary>
        /// Calculates the cosine of the angle between this vector and another one.
        /// </summary>
        /// <param name="vec">The vector the cosine angle to will be calculated.</param>
        /// <returns>Returns the cosine of the angle between the vectors as radian.</returns>
        public float CosAngle(Vector3 vec)
        {
            return (this * vec) / (vec.Length * this.Length);
        }

        /// <summary>
        /// Calculates the azimut out of the Vector.
        /// </summary>
        /// <returns>The azimut as float tuple (azimut, elevation) - ([0, 2*Pi], [0, Pi])</returns>
        public (float azimut, float elevation) CalculateAzimut()
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

        /// <summary>
        /// Reflects the Vector on a surface given by it's normal.
        /// </summary>
        /// <param name="normal">The normal of the survace the vector will be reflected on.</param>
        /// <returns>Returns the direction of the reflection.</returns>
        public Vector3 Reflect(Vector3 normal)
        {
            normal.Normalize();
            var result = this - 2 * (this * normal) * normal;
            result.Normalize();
            return result;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="vec1">First Vector.</param>
        /// <param name="vec2">Second Vector.</param>
        /// <returns>Returns the cross product of the two vectors.</returns>
        public static Vector3 CrossProduct(Vector3 vec1, Vector3 vec2)
        {
            float x = vec1.y * vec2.z - vec1.z * vec2.y;
            float y = vec1.z * vec2.x - vec1.x * vec2.z;
            float z = vec1.x * vec2.y - vec1.y * vec2.x;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Calculates the cross product of this vector and another.
        /// </summary>
        /// <param name="vec">The vector the cross product will be calculated with.</param>
        /// <returns>Returns the cross product of this vector and another.</returns>
        public Vector3 CrossProduct(Vector3 vec)
        {
            return CrossProduct(this, vec);
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="vec1">First Vector.</param>
        /// <param name="vec2">Second Vector.</param>
        /// <returns>Returns the distance between the vectors.</returns>
        public static float Distance(Vector3 vec1, Vector3 vec2)
        {
            return vec1.Distance(vec2);
        }

        /// <summary>
        /// Calculates the distance between this vector and another.
        /// </summary>
        /// <param name="vec">Second Vector.</param>
        /// <returns>Returns the distance between the vectors.</returns>
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

        /// <summary>
        /// Shortcut for the Vector (0, 0, 0).
        /// </summary>
        public static Vector3 Zero { get => new Vector3(0, 0, 0); }
        /// <summary>
        /// Shortcut for the Vector (1, 1, 1).
        /// </summary>
        public static Vector3 One { get => new Vector3(1, 1, 1); }
        /// <summary>
        /// Shortcut for the Vector (0, 1, 0).
        /// </summary>
        public static Vector3 Up { get => new Vector3(0, 1, 0); }
        /// <summary>
        /// Shortcut for the Vector (0, -1, 0).
        /// </summary>
        public static Vector3 Down { get => new Vector3(0, -1, 0); }
        /// <summary>
        /// Shortcut for the Vector (0, 0, 1).
        /// </summary>
        public static Vector3 Forward { get => new Vector3(0, 0, 1); }
        /// <summary>
        /// Shortcut for the Vector (0, 0, -1).
        /// </summary>
        public static Vector3 Backward { get => new Vector3(0, 0, -1); }
        /// <summary>
        /// Shortcut for the Vector (-1, 0, 0).
        /// </summary>
        public static Vector3 Left { get => new Vector3(-1, 0, 0); }
        /// <summary>
        /// Shortcut for the Vector (1, 0, 0).
        /// </summary>
        public static Vector3 Right { get => new Vector3(1, 0, 0); }
    }
}
