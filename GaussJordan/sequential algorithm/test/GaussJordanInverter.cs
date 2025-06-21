using System;

public class GaussJordanInverter
{
    public double[,] Invert(double[,] matrix)
    {
        int n = matrix.GetLength(0);

        // Перевірка, чи матриця є квадратною
        if (n != matrix.GetLength(1))
        {
            throw new ArgumentException("Матриця має бути квадратною.", nameof(matrix));
        }

        // Створення розширеної матриці [A | I]
        double[,] augmentedMatrix = new double[n, 2 * n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                augmentedMatrix[i, j] = matrix[i, j];
            }
            // Додаємо одиничну матрицю справа
            augmentedMatrix[i, i + n] = 1;
        }

        // Прямий хід (приведення до верхньої трикутної матриці) та обернений хід (діагоналізація)
        for (int i = 0; i < n; i++)
        {
            // 1. Пошук ведучого елемента (частковий вибір для чисельної стійкості)
            int maxRow = i;
            for (int k = i + 1; k < n; k++)
            {
                if (Math.Abs(augmentedMatrix[k, i]) > Math.Abs(augmentedMatrix[maxRow, i]))
                {
                    maxRow = k;
                }
            }

            // Перестановка рядків
            for (int k = i; k < 2 * n; k++)
            {
                double temp = augmentedMatrix[i, k];
                augmentedMatrix[i, k] = augmentedMatrix[maxRow, k];
                augmentedMatrix[maxRow, k] = temp;
            }

            // 2. Перевірка на сингулярність
            // Якщо ведучий елемент близький до нуля, матриця не має оберненої
            if (Math.Abs(augmentedMatrix[i, i]) < 1e-9)
            {
                return null; // Матриця є сингулярною
            }

            // 3. Нормалізація ведучого рядка
            double pivot = augmentedMatrix[i, i];
            for (int j = i; j < 2 * n; j++)
            {
                augmentedMatrix[i, j] /= pivot;
            }

            // 4. Елімінація елементів у стовпці
            for (int j = 0; j < n; j++)
            {
                if (i != j)
                {
                    double factor = augmentedMatrix[j, i];
                    for (int k = i; k < 2 * n; k++)
                    {
                        augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                    }
                }
            }
        }

        // 5. Вилучення оберненої матриці з правої частини розширеної матриці
        double[,] inverse = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                inverse[i, j] = augmentedMatrix[i, j + n];
            }
        }

        return inverse;
    }
}