
namespace ESRT
{
    public static class MathUtility
    {
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
