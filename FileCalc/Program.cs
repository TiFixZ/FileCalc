using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace FileCalc
{
    internal class Program
    {
        static string Calculate(string line)
        {
            string left = line.Split('=')[0].Trim();
            string[] parts = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 5 || parts[3] != "=" || parts[4] != "?")
            {
                return left + " = \"Ошибка: неверный формат!\"";
            }

            left = parts[0] + " " + parts[1] + " " + parts[2];
            decimal a;
            decimal b;
            if (!decimal.TryParse(parts[0].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out a)
                || !decimal.TryParse(parts[2].Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out b))
            {
                return left + " = \"Ошибка: неверное число!\"";
            }

            decimal result;
            try
            {
                switch (parts[1])
                {
                    case "+":
                        result = a + b;
                        break;
                    case "-":
                        result = a - b;
                        break;
                    case "*":
                        result = a * b;
                        break;
                    case "/":
                        if (b == 0) return left + " = \"Ошибка: на нуль делить нельзя!\"";
                        result = a / b;
                        break;
                    default:
                        return left + " = \"Ошибка: неизвестная операция!\"";
                }
            }
            catch (OverflowException)
            {
                return left + " = \"Ошибка: переполнение!\"";
            }
            return left + " = " + result.ToString("G29", CultureInfo.InvariantCulture);
        }

        static void Main(string[] args)
        {
            string input = "input.txt";
            string output = "output.txt";
            if (args.Length == 2)
            {
                input = args[0];
                output = args[1];
            }
            else if (args.Length != 0)
            {
                Console.WriteLine("Пример: FileCalc.exe input.txt output.txt");
                return;
            }

            try
            {
                if (Path.GetFullPath(input).Equals(Path.GetFullPath(output), StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Ошибка: входной и выходной файлы должны быть разными.");
                    return;
                }

                string text = FileIO.ReadFromFile(input);
                string customTime = DateTime.Now.ToString("HH:mm:ss.fff");
                Console.WriteLine(customTime);
                if (File.Exists(output)) Console.WriteLine(FileIO.ReadFromFile(output));
                Console.WriteLine(text);

                StringBuilder result = new StringBuilder();
                string[] lines = text.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    result.AppendLine(Calculate(line));
                }
                FileIO.WriteToFile(output, result.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
