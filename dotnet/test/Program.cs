using System;
using System.Collections.Generic;
using System.Linq;

namespace TestIntelliSense
{
    public class Person
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }

        public string GetDescription()
        {
            return $"Name: {Name}, Age: {Age}";
        }

        public bool IsAdult => Age >= 18;
    }

    public static class MathHelper
    {
        public static double CalculateCircleArea(double radius)
        {
            return Math.PI * radius * radius;
        }

        public static int AddNumbers(int a, int b)
        {
            return a + b;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing C# IntelliSense Features");
            Console.WriteLine("================================");

            // Test 1: Object creation and property IntelliSense
            var person = new Person();
            // Type 'person.' here to test property IntelliSense
            person.Name = "John Doe";
            person.Age = 25;
            person.BirthDate = DateTime.Now.AddYears(-25);

            // Test 2: Method IntelliSense
            // Type 'person.' here to see methods
            Console.WriteLine(person.GetDescription());
            Console.WriteLine($"Is adult: {person.IsAdult}");

            // Test 3: Static method IntelliSense
            // Type 'MathHelper.' here to see static methods
            double area = MathHelper.CalculateCircleArea(5.0);
            int sum = MathHelper.AddNumbers(10, 20);

            Console.WriteLine($"Circle area: {area:F2}");
            Console.WriteLine($"Sum: {sum}");

            // Test 4: LINQ IntelliSense
            var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            // Type 'numbers.' here to see LINQ extension methods
            var evenNumbers = numbers.Where(n => n % 2 == 0).ToList();
            var sumOfEvens = evenNumbers.Sum();

            Console.WriteLine($"Even numbers: {string.Join(", ", evenNumbers)}");
            Console.WriteLine($"Sum of even numbers: {sumOfEvens}");

            // Test 5: DateTime IntelliSense
            var now = DateTime.Now;
            // Type 'now.' here to see DateTime methods and properties
            Console.WriteLine($"Current time: {now.ToString("yyyy-MM-dd HH:mm:ss")}");
            Console.WriteLine($"Day of week: {now.DayOfWeek}");

            // Test 6: String IntelliSense
            string testString = "Hello, World!";
            // Type 'testString.' here to see string methods
            Console.WriteLine($"Length: {testString.Length}");
            Console.WriteLine($"Uppercase: {testString.ToUpper()}");
            Console.WriteLine($"Contains 'World': {testString.Contains("World")}");

            // Test 7: Generic collections IntelliSense
            var dictionary = new Dictionary<string, int>();
            // Type 'dictionary.' here to see Dictionary methods
            dictionary.Add("apple", 5);
            dictionary["banana"] = 3;

            foreach (var kvp in dictionary)
            {
                // Type 'kvp.' here to see KeyValuePair properties
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }

            Console.WriteLine("\nIntelliSense Test Instructions:");
            Console.WriteLine("1. Place cursor after 'person.' and press Ctrl+Space");
            Console.WriteLine("2. Place cursor after 'MathHelper.' and press Ctrl+Space");
            Console.WriteLine("3. Place cursor after 'numbers.' and press Ctrl+Space");
            Console.WriteLine("4. Place cursor after 'now.' and press Ctrl+Space");
            Console.WriteLine("5. Place cursor after 'testString.' and press Ctrl+Space");
            Console.WriteLine("6. Place cursor after 'dictionary.' and press Ctrl+Space");
            Console.WriteLine("7. Try typing new variable names to test auto-completion");
        }
    }
}
