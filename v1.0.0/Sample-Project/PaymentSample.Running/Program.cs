using System;
using System.Collections.Generic;
using System.Globalization;
using PaymentSample.Common;
using PaymentSample.Common.Actions;

namespace PaymentSample.Running
{
    internal class Program
    {
        private static void Main()
        {
            var command = "help";
            do
            {
                var strings = command.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var results = (AppsOnContainer.Instance.GetExportedValueOrDefault<IAction>(strings[0])
                               ?? AppsOnContainer.Instance.GetExportedValueOrDefault<IAction>("empty"))
                    .Act(command);
                WriteResults(results);
            } while (!(command = Console.ReadLine()?.ToLower(CultureInfo.InvariantCulture)
                                 ?? "empty")
                .Trim()
                .Equals("quit", StringComparison.InvariantCultureIgnoreCase));
        }

        private static void WriteResults(List<string> results)
        {
            Console.WriteLine("-------------------------");
            results.ForEach(Console.WriteLine);
            Console.WriteLine();
            Console.Write(">");
        }
    }
}