using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NEA_Matrix_Tester
{
    internal class SumOfResiduals
    {
        private List<(long, double)> data;
        private List<double> curve;

        public SumOfResiduals(List<(long, double)> data, List<double> curve)
        {
            this.data = data;
            this.curve = curve;
        }

        public double Residuals()
        {
            double sum = 0;

            for (int i = 0; i < data.Count; i++)
            {
                double predicted = FOfX(data[i].Item1);
                sum += data[i].Item2 - predicted * predicted;
            }

            double variance = sum / (data.Count - curve.Count);

            return variance;
        }

        private double FOfX(long x)
        {
            double sum = 0;

            for (int i = 0; i < curve.Count; i++)
            {
                sum += curve[i] * double.Parse(Math.Pow(x, i).ToString());
            }

            return sum;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            List<BigFloat[,]> listOfMatrices = new List<BigFloat[,]>();


            BigFloat[,] matrix2x2 = { { 3, 2 }, { 5, 4 } } ;
            BigFloat[,] matrix3x3 = { { 8, 5, 2 }, { 3, 6, 3 }, { 4, 6, 8 } };
            BigFloat[,] matrix4x4 = { { 3, 6, 9, 5 }, {4,9,6,6},{7,3,0,1}, {2,3,2,5}};
            BigFloat[,] matrix5x5 = { { 8,0,6,6,4}, {6,8,8,3,7}, {0,3,2,1,4}, {6,5,4,9,0},{ 1,4,5,4,5} };

            listOfMatrices.Add(matrix2x2); listOfMatrices.Add(matrix3x3); listOfMatrices.Add(matrix4x4);listOfMatrices.Add(matrix5x5);




            BigFloat coloum = 1;
            BigFloat row = 1;

            foreach (BigFloat[,] matrix in listOfMatrices)
            {
                Console.WriteLine("The matrix is:");
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        Console.Write(matrix[i,j] + " ");
                    }
                    Console.WriteLine();
                }
                
                BigFloat[,] inverseMatix = Inverse(matrix);
                Console.ReadKey();
                Console.WriteLine("The inverse matrix is:");
                for (int i = 0; i < inverseMatix.GetLength(0); i++)
                {
                    for (int j = 0; j < inverseMatix.GetLength(0); j++)
                    {
                        string num = inverseMatix[i, j].ToString();
                        
                        if (num[0] == '-')
                        {
                            Console.Write(num[0]);
                            num = num.Remove(0,1);
                        }
                        string[] list = num.Split('-');
                        num = string.Concat(list);
                        for (int k = 0; k < 6 && k<num.Length; k++)
                        {
                            if (k == 5 && int.Parse(num[k + 1].ToString()) > 4)
                            {
                                int buffer = int.Parse(num[k].ToString());
                                Console.Write(buffer+1);
                            }
                            else
                            {
                                Console.Write(num[k]);
                            }
                        }

                        Console.Write(" ");
                        
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.ReadKey();
                Console.WriteLine("The determinant is:");
                Console.WriteLine(Determinant(matrix));
                Console.WriteLine();

                if (matrix.GetLength(0) > 2)
                {
                    BigFloat[,] cofactorMatrix = Cofactor(matrix, row, coloum);
                    BigFloat[,] transposedMatrix = Transpose(matrix);
                    Console.ReadKey();
                    Console.WriteLine("The transposed matrix is:");
                    for (int i = 0; i < transposedMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < transposedMatrix.GetLength(0); j++)
                        {
                            Console.Write(transposedMatrix[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    Console.ReadKey();
                    Console.WriteLine("The cofactor matrix of (1,1) is:");
                    for (int i = 0; i < cofactorMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < cofactorMatrix.GetLength(0); j++)
                        {
                            Console.Write(cofactorMatrix[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.ReadKey();
                Console.Clear();
                
            }
            
            List<(long,double)> linear = new List<(long,double)>();
            List<(long, double)> quadratic = new List<(long, double)>();
            List<(long, double)> cubic = new List<(long, double)>();
            List<(long, double)> quartic = new List<(long, double)>();


            for(int i = 0; i <=15; i ++)
            {
                linear.Add((i,-i+6 ));
                quadratic.Add((i, 4 * Math.Pow(i, 2) - 3 * i + 5));
                cubic.Add((i, -Math.Pow(i, 3) - 7 * Math.Pow(i, 2) - 5 * i - 3));
                quartic.Add((i, 8 * Math.Pow(i, 4) - 6 * Math.Pow(i, 3) - 9 * Math.Pow(i, 2) + 3 * i + 3));
            }
            List<List<(long, double)>> listOfCurves = new List<List<(long, double)>>();
            listOfCurves.Add(linear);
            listOfCurves.Add(quadratic);
            listOfCurves.Add(cubic);
            listOfCurves.Add(quartic);

            for(int i = 0; i < listOfCurves.Count; i++)
            {
                List<double> coeffcients = DoPolynomialRegressionForSpecificOrder(listOfCurves[i], i + 1);

            }

            Console.ReadKey();
        }
        public static List<double> DoPolynomialRegressionForSpecificOrder(List<(long, double)> points,int degree)
        {
            List<List<double>> ListOfCoefficients = new List<List<double>>();

            for (int x = degree; x <= degree; x++)
            {
                List<double> coefficients = new List<double>();

                BigFloat[,] matrixA = new BigFloat[x + 1, x + 1];
                BigFloat[] matrixB = new BigFloat[x + 1];

                bool outOFRange = false;

                for (int i = 0; i < matrixA.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixA.GetLength(0); j++)
                    {
                        double sumOfx = 0;

                        foreach ((long, double) coordinate in points)
                        {
                            sumOfx += Math.Pow(coordinate.Item1, i + j);
                        }

                        matrixA[i, j] = sumOfx;
                    }

                    double sumOfxy = 0;

                    foreach ((long, double) coordinate in points)
                    {
                        sumOfxy += Math.Pow(coordinate.Item1, i) * coordinate.Item2;
                    }

                    matrixB[i] = sumOfxy;
                }

                BigFloat[,] inverseMatrixA = Inverse(matrixA);

                for (int i = 0; i < inverseMatrixA.GetLength(0); i++)
                {
                    double sum = 0;

                    for (int j = 0; j < inverseMatrixA.GetLength(0); j++)
                    {
                        if (inverseMatrixA[i, j] * matrixB[j] < double.MinValue)
                        {
                            outOFRange = true;
                        }
                        else
                        {
                            sum += (double)(inverseMatrixA[i, j] * matrixB[j]);
                        }

                    }

                    coefficients.Add(sum);
                }

                if (!outOFRange)
                {
                    Console.WriteLine("Degree " + x + " polynomial has been succesfully generated");
                    ListOfCoefficients.Add(coefficients);
                }
            }

            int bestLine = 0;
            double bestVariance = double.MaxValue;

            for (int i = 0; i < ListOfCoefficients.Count; i++)
            {
                SumOfResiduals s = new SumOfResiduals(points, ListOfCoefficients[i]);
                double variance = s.Residuals();

                if (bestVariance > variance)
                {
                    bestVariance = variance;
                    bestLine = i;
                }
            }

            return ListOfCoefficients[bestLine];
        }
        public static List<double> DoPolynomialRegression(List<(long, double)> points)
        {
            List<List<double>> ListOfCoefficients = new List<List<double>>();

            for (int x = 1; x <= 4; x++)
            {
                List<double> coefficients = new List<double>();

                BigFloat[,] matrixA = new BigFloat[x + 1, x + 1];
                BigFloat[] matrixB = new BigFloat[x + 1];

                bool outOFRange = false;

                for (int i = 0; i < matrixA.GetLength(0); i++)
                {
                    for (int j = 0; j < matrixA.GetLength(0); j++)
                    {
                        double sumOfx = 0;

                        foreach ((long, double) coordinate in points)
                        {
                            sumOfx += Math.Pow(coordinate.Item1, i + j);
                        }

                        matrixA[i, j] = sumOfx;
                    }

                    double sumOfxy = 0;

                    foreach ((long, double) coordinate in points)
                    {
                        sumOfxy += Math.Pow(coordinate.Item1, i) * coordinate.Item2;
                    }

                    matrixB[i] = sumOfxy;
                }

                BigFloat[,] inverseMatrixA = Inverse(matrixA);

                for (int i = 0; i < inverseMatrixA.GetLength(0); i++)
                {
                    double sum = 0;

                    for (int j = 0; j < inverseMatrixA.GetLength(0); j++)
                    {
                        if (inverseMatrixA[i, j] * matrixB[j] < double.MinValue)
                        {
                            outOFRange = true;
                        }
                        else
                        {
                            sum += (double)(inverseMatrixA[i, j] * matrixB[j]);
                        }

                    }

                    coefficients.Add(sum);
                }

                if (!outOFRange)
                {
                    Console.WriteLine("Degree " + x + " polynomial has been succesfully generated");
                    ListOfCoefficients.Add(coefficients);
                }
            }

            int bestLine = 0;
            double bestVariance = double.MaxValue;

            for (int i = 0; i < ListOfCoefficients.Count; i++)
            {
                SumOfResiduals s = new SumOfResiduals(points, ListOfCoefficients[i]);
                double variance = s.Residuals();

                if (bestVariance > variance)
                {
                    bestVariance = variance;
                    bestLine = i;
                }
            }

            return ListOfCoefficients[bestLine];
        }

        public static BigFloat[,] Inverse(BigFloat[,] matrix)
        {
            BigFloat[,] inverse = new BigFloat[matrix.GetLength(0), matrix.GetLength(0)];

            if (matrix.GetLength(0) == 2)
            {
                BigFloat det = Determinant(matrix);
                inverse[0, 0] = matrix[1, 1] / det;
                inverse[1, 1] = matrix[0, 0] / det;
                inverse[0, 1] = -matrix[0, 1] / det;
                inverse[1, 0] = -matrix[1, 0] / det;
                return inverse;
            }
            else
            {

                BigFloat det = Determinant(matrix);
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        if (i % 2 == j % 2)
                        {
                            inverse[i, j] = Determinant(Cofactor(matrix, i, j)) / det;
                        }
                        else
                        {
                            inverse[i, j] = (-1) * Determinant(Cofactor(matrix, i, j)) / det;
                        }
                    }
                }

                inverse = Transpose(inverse);
            }

            return inverse;
        }
        public static BigFloat Determinant(BigFloat[,] matrix)
        {
            BigFloat det = 0;

            if (matrix.GetLength(0) == 2)
            {
                det += matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            }
            else
            {
                for (int i = 0; i < matrix.GetLength(0); ++i)
                {
                    BigFloat[,] cofactorMatrix = Cofactor(matrix, 0, i);

                    if (i % 2 == 0)
                    {
                        det += matrix[0, i] * Determinant(cofactorMatrix);
                    }
                    else if (i % 2 == 1)
                    {
                        det -= matrix[0, i] * Determinant(cofactorMatrix);
                    }
                }
            }

            return det;
        }
        public static BigFloat[,] Cofactor(BigFloat[,] matrix, BigFloat row, BigFloat coloum)
        {
            BigFloat[,] cofactorMatrix = new BigFloat[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];

            bool checkRow = false;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (i == row)
                {
                    checkRow = true;
                }
                else
                {
                    bool checkColoum = false;

                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        if (j == coloum)
                        {
                            checkColoum = true;
                        }
                        else if (checkColoum && checkRow)
                        {
                            cofactorMatrix[i - 1, j - 1] = matrix[i, j];
                        }
                        else if (checkRow)
                        {
                            cofactorMatrix[i - 1, j] = matrix[i, j];
                        }
                        else if (checkColoum)
                        {
                            cofactorMatrix[i, j - 1] = matrix[i, j];
                        }
                        else
                        {
                            cofactorMatrix[i, j] = matrix[i, j];
                        }
                    }
                }
            }

            return cofactorMatrix;
        }
        public static BigFloat[,] Transpose(BigFloat[,] matrix)
        {
            BigFloat[,] transposed = new BigFloat[matrix.GetLength(0), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    transposed[i, j] = matrix[j, i];
                }
            }

            return transposed;
        }
    }
}

