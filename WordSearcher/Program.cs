using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace WordSearcher
{
    class Program
    {
        // .txt files was genereted here -> http://nietzsche-ipsum.com/
        private static string words1000 = File.ReadAllText(@"..\..\..\source\1000words.txt");
        private static string words1500 = File.ReadAllText(@"..\..\..\source\1500words.txt");
        private static string words3000 = File.ReadAllText(@"..\..\..\source\3000words.txt");

        private static List<String> result = new List<string>();

        private static string[] source1000 = SplitByWords(words1000);
        private static string[] source1500 = SplitByWords(words1500);
        private static string[] source3000 = SplitByWords(words3000);

        private static String[] sorted1000;
        private static String[] sorted1500;
        private static String[] sorted3000;

        private static bool sorted = false;

        /// <summary>
        /// MAIN
        /// </summary>
        static void Main(string[] args)
        {
            StartApp();
        }

        /// <summary>
        /// Menu and menu controller
        /// </summary>
        private static void StartApp()
        {
            int input = 0;
            
            do
            {
                Console.Clear();
                Console.WriteLine("1. Count word occurences in the text.");
                Console.WriteLine("2. Print all results.");
                Console.WriteLine("3. Print first [X] words from sorted text.");
                Console.WriteLine("4. Sort text");
                Console.WriteLine("5. Exit");
                Console.WriteLine();

                input = AskForInput();

                switch (input)
                {
                    case 1:
                        Console.Write("Please, enter a word: ");
                        string stringInput = Console.ReadLine();
                        CountOccurences(stringInput);
                        break;
                    case 2:
                        PrintAllResults();
                        break;
                    case 3:
                        HandlePrinting();
                        break;
                    case 4:
                        HandleSortringMenu();
                        break;
                    case 5:
                        Exit();
                        break;
                    default:
                        break;
                }

            } while (input != 5);
        }

        /// <summary>
        /// handle sorting menu case (menu number 4)
        /// </summary>
        private static void HandleSortringMenu()
        {
            Console.Clear();
            Console.WriteLine("Removing duplicates ... ");
            sorted1000 = RemoveDuplicates(source1000);
            sorted1500 = RemoveDuplicates(source1500);
            sorted3000 = RemoveDuplicates(source3000);
            Console.WriteLine("Done. Duplicates have been removed.");
            Console.WriteLine("Sorting started ...");
            Quicksort(sorted1000, 0, sorted1000.Length - 1);
            Quicksort(sorted1500, 0, sorted1500.Length - 1);
            Quicksort(sorted3000, 0, sorted3000.Length - 1);
            Console.WriteLine("Sorting ended.");
            Console.WriteLine("All files was sorted.");
            sorted = true;
            PressToContinue();
        }

        /// <summary>
        /// handle printing menu case (menu number 3)
        /// </summary>
        private static void HandlePrinting()
        {
            Console.Clear();
            if (sorted)
            {
                int amountToPrint = AskForInput();
                Console.WriteLine(amountToPrint + " first words in the 1000 words file: " + '\n' 
                    + PrintFirstWords(sorted1000, amountToPrint) + '\n' 
                    + "-----------------------------------------------------------------------------" + '\n');
                Console.WriteLine(amountToPrint + " first words in the 1500 words file: " + '\n'
                    + PrintFirstWords(sorted1500, amountToPrint) + '\n'
                    + "-----------------------------------------------------------------------------" + '\n');
                Console.WriteLine(amountToPrint + " first words in the 3000 words file: " + '\n'
                    + PrintFirstWords(sorted3000, amountToPrint) + '\n'
                    + "-----------------------------------------------------------------------------" + '\n');
                PressToContinue();
            }
            else
            {
                Console.WriteLine("Please sort text first.");
                PressToContinue();
            }
        }

        /// <summary>
        /// print first X words from every sorted array
        /// O(1 + n)
        /// </summary>
        /// <param name="arr">array to use</param>
        /// <param name="amountToPrint">amount to print</param>
        private static string PrintFirstWords(string[] arr, int amountToPrint)
        {
            String optionalResult = "";
            for (int i = 0; i < amountToPrint; i++)
            {
                optionalResult += arr[i] + " ";
            }
            return optionalResult;
        }


        /// <summary>
        /// Printing all stored results
        /// </summary>
        private static void PrintAllResults()
        {
            Console.Clear();
            foreach(string res in result)
            {
                Console.WriteLine(res);
            }
            PressToContinue();
        }

        /// <summary>
        /// Convert the string into an array of words 
        /// </summary>
        private static String[] SplitByWords(String input)
        {
            return input.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',', '\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// count occurences of the word
        /// </summary>
        /// <param name="searchTerm">word to search in the text</param>
        private static void CountOccurences(String searchTerm)
        {
            String optionalResult = "";

            // Create the query. Use ToLowerInvariant to match "data" and "Data"
            var matchQuery1000 = from word in source1000
                                 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                 select word;
            var matchQuery1500 = from word in source1500
                                 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                 select word;
            var matchQuery3000 = from word in source3000
                                 where word.ToLowerInvariant() == searchTerm.ToLowerInvariant()
                                 select word;

            // create a string to print
            optionalResult = matchQuery1000.Count() + " occurrences(s) of the word \"" + searchTerm + "\" were found in the 1000words.txt file." + '\n' +
                matchQuery1500.Count() + " occurrences(s) of the word \"" + searchTerm + "\" were found in the 1500words.txt file." + '\n' +
                matchQuery3000.Count() + " occurrences(s) of the word \"" + searchTerm + "\" were found in the 3000words.txt file." + '\n' +
                "-----------------------------------------------------------------------------";

            // printing
            Console.WriteLine(optionalResult);

            result.Add(optionalResult);
            PressToContinue();
        }


        /// <summary>
        /// arrange and split the list into two list based on pivot element
        /// </summary>
        static int Partition(string[] arr, int start, int end)
        {
            int pivot = end;
            int i = start, j = end;
            string temp;
            while (i < j)
            {
                while (i < end && string.Compare(arr[i], arr[pivot]) < 0)
                    i++;
                while (j > start && string.Compare(arr[j], arr[pivot]) > 0)
                    j--;

                if (i < j)
                {
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
            temp = arr[pivot];
            arr[pivot] = arr[j];
            arr[j] = temp;
            return j;
        }

        /// <summary>
        /// Best case: O(n logn) 
        /// Worst case: 0(n^2)
        /// </summary>
        /// <param name="arr">array to search</param>
        /// <param name="start">where to start</param>
        /// <param name="end">where to end</param>
        static void Quicksort(string[] arr, int start, int end)
        {
            if (start < end)
            {
                int pivotIndex = Partition(arr, start, end);
                Quicksort(arr, start, pivotIndex - 1);
                Quicksort(arr, pivotIndex + 1, end);
            }
        }

        /// <summary>
        /// Remove duplicates from an Array of strings
        /// O(n)
        /// </summary>
        /// <param name="input">Array with strings</param>
        /// <returns>New <ArrayList> without duplicates</returns>
        public static string[] RemoveDuplicates(string[] input)
        {
            ArrayList newList = new ArrayList();

            foreach (string str in input)
            {
                if (!newList.Contains(str.ToLower())) {  newList.Add(str.ToLower()); }
            }
               
            return (string[])newList.ToArray(typeof(string));
        }

        /// <summary>
        /// Ask for user input
        /// </summary>
        private static int AskForInput()
        {
            Console.Write("Please, enter a number: ");
            string stringInput = Console.ReadLine();
            int numericalInput;
            bool isNumber;
            do
            {
                isNumber = int.TryParse(stringInput, out numericalInput);
                if (!isNumber)
                {
                    Console.Write("[" + stringInput + "]" + " - is not a number or empty. Please try again: ");
                    stringInput = Console.ReadLine();
                }
            } while (!isNumber);

            return numericalInput;
        }

        /// <summary>
        /// Press any button to continue
        /// </summary>
        private static void PressToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Press any button to continue ... ");
            Console.ReadKey();
            StartApp();
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private static void Exit()
        {
            Console.Clear();
            Console.WriteLine("App is closed!");
            Environment.Exit(0);
        }
    }
}