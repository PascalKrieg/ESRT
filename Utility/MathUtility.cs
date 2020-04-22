using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESRT
{
    /// <summary>
    /// Utility class for various math related tasks.
    /// </summary>
    public static class MathUtility
    {
        /// <summary>
        /// Solves a linear equation in the form A * x = v, with A being a 3x3 matrix and v being a given vector. 
        /// </summary>
        /// <param name="matrix">The 3x3 matrix A.</param>
        /// <param name="equals">The Vector v.</param>
        /// <returns></returns>
        public static (float a, float b, float c) SolveLinearEquationSystem(Matrix3x3 matrix, Vector3 equals)
        {
            float determinant = matrix.Determinant;
            (float a, float b, float c) result;
            result.a = matrix.ExchangeColumn(0, equals).Determinant / determinant;
            result.b = matrix.ExchangeColumn(1, equals).Determinant / determinant;
            result.c = matrix.ExchangeColumn(2, equals).Determinant / determinant;
            return result;
        }
    }
}
