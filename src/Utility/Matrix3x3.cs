using System;

namespace ESRT
{
    public class Matrix3x3
    {
        public float[,] entries = new float[3, 3];

        public Matrix3x3(Vector3 firstRow, Vector3 secondRow, Vector3 thirdRow)
        {
            entries[0, 0] = firstRow.x;   //0 a
            entries[0, 1] = firstRow.y;   //1 b
            entries[0, 2] = firstRow.z;   //2 c

            entries[1, 0] = secondRow.x;  //3 d
            entries[1, 1] = secondRow.y;  //4 e
            entries[1, 2] = secondRow.z;  //5 f

            entries[2, 0] = thirdRow.x;   //6 g
            entries[2, 1] = thirdRow.y;   //7 h
            entries[2, 2] = thirdRow.z;   //8 i
        }

        public Matrix3x3(float a, float b, float c, float d, float e, float f, float g, float h, float i)
        {
            entries[0, 0] = a;  //0 a
            entries[0, 1] = b;  //1 b
            entries[0, 2] = c;  //2 c

            entries[1, 0] = d;  //3 d
            entries[1, 1] = e;  //4 e
            entries[1, 2] = f;  //5 f

            entries[2, 0] = g;  //6 g
            entries[2, 1] = h;  //7 h
            entries[2, 2] = i;  //8 i
        }

        public Matrix3x3(float[,] entries)
        {
            if (entries.Length != 9)
                throw new ArgumentException("Entries must contain 9 elements.");

            Array.Copy(entries, 0, this.entries, 0, 9);
        }

        public Matrix3x3 ExchangeColumn(int columnIndex, Vector3 newColumn)
        {
            Matrix3x3 result = new Matrix3x3(this.entries);
            result.entries[0, columnIndex] = newColumn.x;
            result.entries[1, columnIndex] = newColumn.y;
            result.entries[2, columnIndex] = newColumn.z;
            return result;
        }

        public float Determinant
        {
            get
            {
                return entries[0, 0] * entries[1, 1] * entries[2, 2] +
                    entries[0, 1] * entries[1, 2] * entries[2, 0] +
                    entries[0, 2] * entries[1, 0] * entries[2, 1] -
                    entries[2, 0] * entries[1, 1] * entries[0, 2] -
                    entries[2, 1] * entries[1, 2] * entries[0, 0] -
                    entries[2, 2] * entries[1, 0] * entries[0, 1];
            }
        }
    }
}
