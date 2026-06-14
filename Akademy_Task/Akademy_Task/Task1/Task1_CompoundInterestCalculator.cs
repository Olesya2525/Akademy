using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akademy_Task
{
    internal class Task1_CompoundInterestCalculator
    {
        /// <summary>
        /// Формирует и возвращает строку с расчетом сложных процентов по годам.
        /// </summary>
        /// <param name="initialDeposit">Начальный вклад (положительное число).</param>
        /// <param name="years">Количество лет (положительное целое число).</param>
        /// <param name="interestRate">Годовая процентная ставка (положительное число).</param>
        /// <returns>Строка с расчетом по годам.</returns>
        public static string CalculateCompoundInterest(double initialDeposit, int years, double interestRate)
        {
            if (initialDeposit <= 0)
            {
                throw new ArgumentException("Начальный вклад должен быть положительным числом.", nameof(initialDeposit));
            }

            if (years <= 0)
            {
                throw new ArgumentException("Количество лет должно быть положительным целым числом.", nameof(years));
            }

            if (interestRate <= 0)
            {
                throw new ArgumentException("Процентная ставка должна быть положительным числом.", nameof(interestRate));
            }

            var result = new System.Text.StringBuilder();
            double currentAmount = initialDeposit;
            double rateMultiplier = 1 + (interestRate / 100);

            for (int year = 1; year <= years; year++)
            {
                currentAmount *= rateMultiplier;
                result.AppendLine($"Год {year}: {currentAmount:F2} руб.");
            }

            return result.ToString().TrimEnd();
        }
    }

    
}
