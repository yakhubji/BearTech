using System;
using System.Drawing;

class Program
{
    static void Main(string[] args)
    {
        // Get the current working directory
        string currentDirectory = Directory.GetCurrentDirectory();

        // Navigate up to the BearTech directory
        string bearTechPath = Path.Combine(currentDirectory, @"..\..\", "TextFile1.txt");

        // Resolve the full path
        string fullPath = Path.GetFullPath(bearTechPath);

        // Check if the file exists before reading
        if (!File.Exists(fullPath))
        {
            Console.WriteLine($"File not found: {fullPath}");
            return;
        }

        // Read all lines from the file
        var input = File.ReadAllLines(fullPath);

        int height = input.Length;          // Number of rows
        int width = input[0].Length;        // Number of columns

        // Create a grid to store the map
        var map = new char[height, width]; // [rows, columns]
        for (var y = 0; y < height; y++)   // Correct mapping of grid
        {
            for (var x = 0; x < width; x++)
            {
                map[y, x] = input[y][x];
            }
        }

        int runningTotal = 0;       // Total sum of valid numbers
        int currentNumber = 0;      // Current number being read
        bool hasNeighboringSymbol = false; // Flag to check if a symbol is adjacent

        // Traverse the grid row by row
        for (var y = 0; y < height; y++)
        {
            void EndCurrentNumber()
            {
                if (currentNumber != 0 && hasNeighboringSymbol)
                {
                    // Print the number if it's valid
                    Console.WriteLine($"Valid Number: {currentNumber}");
                    runningTotal += currentNumber;
                }
                currentNumber = 0;
                hasNeighboringSymbol = false;
            }

            for (var x = 0; x < width; x++) // Fixed inner loop to traverse `width`
            {
                var character = map[y, x];
                // Check if we are reading a number
                if (char.IsDigit(character))
                {
                    int value = character - '0'; // Convert character to integer
                    currentNumber = currentNumber * 10 + value;

                    // Print the current number being read
                    Console.WriteLine($"Reading Number: {currentNumber}");

                    // Check all 8 neighbors for symbols
                    foreach (var direction in WithDiagonals)
                    {
                        int neighborX = x + direction.X;
                        int neighborY = y + direction.Y;

                        // Skip out-of-bounds neighbors
                        if (neighborX < 0 || neighborX >= width || neighborY < 0 || neighborY >= height)
                            continue;

                        // Check if the neighbor is a symbol (not a digit and not '.')
                        var neighborCharacter = map[neighborY, neighborX];
                        if (!char.IsDigit(neighborCharacter) && neighborCharacter != '.')
                        {
                            hasNeighboringSymbol = true; // Found a valid symbol
                        }
                    }
                }
                else
                {
                    // If the character is not a digit, process the current number
                    EndCurrentNumber();
                }
            }

            // End the current number at the end of the row
            EndCurrentNumber();
        }

        // Output the total sum
        Console.WriteLine($"Total Sum of Valid Numbers: {runningTotal}");
    }

    // Directions for adjacent cells with diagonals
    public static Point[] WithDiagonals { get; } = new Point[]
    {
        new Point(0, 1),  // Right
        new Point(1, 0),  // Down
        new Point(0, -1), // Left
        new Point(-1, 0), // Up
        new Point(1, 1),  // Bottom-right
        new Point(-1, 1), // Bottom-left
        new Point(1, -1), // Top-right
        new Point(-1, -1) // Top-left
    };
}
