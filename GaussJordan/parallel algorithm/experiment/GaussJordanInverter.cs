using System;
using System.Threading.Tasks;

public class GaussJordanInverter
{

    // Послідовна реалізація алгоритму Гауса-Жордана (еталон).
    public double[,] Invert(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        if (n != matrix.GetLength(1)) throw new ArgumentException("Матриця має бути квадратною.");
        
        double[,] augmentedMatrix = new double[n, 2 * n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) augmentedMatrix[i, j] = matrix[i, j];
            augmentedMatrix[i, i + n] = 1;
        }

        for (int i = 0; i < n; i++) {
            int maxRow = i;
            for (int k = i + 1; k < n; k++) {
                if (Math.Abs(augmentedMatrix[k, i]) > Math.Abs(augmentedMatrix[maxRow, i])) maxRow = k;
            }
            for (int k = i; k < 2 * n; k++) {
                double temp = augmentedMatrix[i, k];
                augmentedMatrix[i, k] = augmentedMatrix[maxRow, k];
                augmentedMatrix[maxRow, k] = temp;
            }

            if (Math.Abs(augmentedMatrix[i, i]) < 1e-9) return null;

            double pivot = augmentedMatrix[i, i];
            for (int j = i; j < 2 * n; j++) augmentedMatrix[i, j] /= pivot;

            for (int j = 0; j < n; j++) {
                if (i != j) {
                    double factor = augmentedMatrix[j, i];
                    for (int k = i; k < 2 * n; k++) augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                }
            }
        }
        
        double[,] inverse = new double[n, n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) inverse[i, j] = augmentedMatrix[i, j + n];
        }
        return inverse;
    }

    // Паралельна реалізація алгоритму Гауса-Жордана з використанням TPL.
    public double[,] InvertParallel(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        if (n != matrix.GetLength(1)) throw new ArgumentException("Матриця має бути квадратною.");
        
        double[,] augmentedMatrix = new double[n, 2 * n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) augmentedMatrix[i, j] = matrix[i, j];
            augmentedMatrix[i, i + n] = 1;
        }

        for (int i = 0; i < n; i++)
        {
            int maxRow = i;
            for (int k = i + 1; k < n; k++) {
                if (Math.Abs(augmentedMatrix[k, i]) > Math.Abs(augmentedMatrix[maxRow, i])) maxRow = k;
            }
            for (int k = i; k < 2 * n; k++) {
                double temp = augmentedMatrix[i, k];
                augmentedMatrix[i, k] = augmentedMatrix[maxRow, k];
                augmentedMatrix[maxRow, k] = temp;
            }

            if (Math.Abs(augmentedMatrix[i, i]) < 1e-9) return null;

            double pivot = augmentedMatrix[i, i];
            for (int j = i; j < 2 * n; j++) augmentedMatrix[i, j] /= pivot;

            Parallel.For(0, n, j =>
            {
                if (i != j) {
                    double factor = augmentedMatrix[j, i];
                    for (int k = i; k < 2 * n; k++) {
                        augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                    }
                }
            });
        }
        
        double[,] inverse = new double[n, n];
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) inverse[i, j] = augmentedMatrix[i, j + n];
        }
        return inverse;
    }
}