﻿using System;
using System.Collections.Generic;
using System.IO;

namespace LiveMedianEstimate
{
    class Program
    {
        private float[] array = {};

        static void Main(string[] args)
        {
            Program program = new Program();
            ProgramTester programTester = new ProgramTester();

            // Execution module to read stream, estimate median and report in console
            try
            {
                StreamReader stream = new StreamReader(Path.Combine(Environment.CurrentDirectory, "TestFile.txt"));
                float median = program.estimateMedian(stream, 5);
                Console.WriteLine("Estimated Median: " + median);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Sorry, File not found!");
            }

            // Call the Test method
            programTester.MedianTestMethod();
        }

        // PRIMARY MODULE : estimateMedian
        // For the purpose of testing, the method has been declared with 'public' access
        // Case needs to be k <= n, for unbiased results
        public float estimateMedian(StreamReader stream, int k)
        {
            int count = 0;
            string line;
            bool kIsEven = k % 2 == 0;  // Check whether k is Even or Odd

            // Initialize sample array of size k
            float[] sampleArray = new float[k];
            float[] sortedSampleArray = new float[k];

            // Read the file stream one line at a time until EOF
            while((line = stream.ReadLine()) != null)
            {
                count++;

                // Store stream data to sample array of size k
                if (count <= k)
                {
                    sampleArray[count - 1] = float.Parse(line);
                }
                
                // When stream size exceeds size k, randomly replace existing data in sample array with new stream data, with equal probability
                // Using pseudo-random generator, GUID as SEED to randomizer
                else if(count > k)
                {
                    Random randomizer = new Random(Guid.NewGuid().GetHashCode());

                    int randomIndex = randomizer.Next(0, count);

                    if(randomIndex < k)
                    {
                        sampleArray[randomIndex] = float.Parse(line);
                    }   
                }
            }

            // Make a copy of final sample array, sort using QuickSort and return estimated median to parent method
            sortedSampleArray = (float[]) sampleArray.Clone();
            QuickSort(sortedSampleArray, 0, k - 1);
            array = sortedSampleArray;
            return kIsEven ? (sortedSampleArray[(k/2)-1] + sortedSampleArray[(k/2)])/2.0f : sortedSampleArray[(k-1)/2];
        }

        public float[] getSampleArray()
        {
            return array;
        }

// QuickSort code starts here
        private void QuickSort(float[] inputArray, int low, int high)
        {
            int pivotPosition = 0;

            if(low < high)
            {
                try
                {
                    pivotPosition = Partition(inputArray, low, high);
                    QuickSort(inputArray, low, pivotPosition - 1);
                    QuickSort(inputArray, pivotPosition + 1, high);
                }
                catch(IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(" ");
                }
            }
        }

        // QuickSort : Partition module
        private int Partition(float[] inputArray, int low, int high)
        {
            float pivot = inputArray[high];
            int i = low - 1;

            for(int j = low; j < high; j++)
            {
                if (inputArray[j] > pivot) continue;
                i++;
                Swap(inputArray, i, j);
            }

            Swap(inputArray, i + 1, high);
            return i + 1;
        }

        // QuickSort : Swap module
        private static void Swap(float[] inputArray, int elementA, int elementB)
        {
            float temp = inputArray[elementA];
            inputArray[elementA] = inputArray[elementB];
            inputArray[elementB] = temp;
        }
// QuickSort code ends here
    }

//***********************************************************************************************************************************
//***********************************************************************************************************************************
// Test Program
    public class ProgramTester
    {
        public void MedianTestMethod()
        {
            Console.WriteLine("");
            Console.WriteLine("********************************** TEST **********************************");
            Console.WriteLine("k/n = 5/8 = " + 5.0f/8.0f);
            Program program = new Program();
            string line;
            var listOfInboundStream = new List<float>();

            // Define file paths to use
            string path = Path.Combine(Environment.CurrentDirectory, "TestFile.txt"); ;
            string pathAsc = Path.Combine(Environment.CurrentDirectory, "TestFileAsc.txt");
            string pathDesc = Path.Combine(Environment.CurrentDirectory, "TestFileDesc.txt");
            string pathRand = Path.Combine(Environment.CurrentDirectory, "TestFileRand.txt");

            try
            {
                StreamReader stream = new StreamReader(path);

                // Store the stream in a List
                while ((line = stream.ReadLine()) != null)
                {
                    listOfInboundStream.Add(float.Parse(line));
                }

                // Create new files to store ascending, descending and randomized data sets, if they don't already exist
                if ((!File.Exists(pathAsc)) || (!File.Exists(pathDesc)) || (!File.Exists(pathRand)))
                {
                    File.Create(pathAsc).Close();
                    File.Create(pathDesc).Close();
                    File.Create(pathRand).Close();
                }

                // Declare StreamWriter to later write into recently created files
                StreamWriter writerAsc = new StreamWriter(pathAsc);
                StreamWriter writerDesc = new StreamWriter(pathDesc);
                StreamWriter writerRand = new StreamWriter(pathRand);

                // Sort List in Ascending order and write to new file stream
                listOfInboundStream.Sort();
                foreach (var variable in listOfInboundStream)
                {
                    writerAsc.WriteLine(variable);
                }
                writerAsc.Close();

                // Sort List in Descending order and write to new file stream
                listOfInboundStream.Reverse();
                foreach (var variable in listOfInboundStream)
                {
                    writerDesc.WriteLine(variable);
                }
                writerDesc.Close();

                // Randomize List and write to new file stream
                listOfInboundStream.Reverse();
                foreach (var variable in listOfInboundStream)
                {
                    writerRand.WriteLine(variable);
                }
                writerRand.Close();

                // Report test results 5 times
                for (int i = 0; i < 5; i++)
                {
                    // Declare StreamReader to read from recently populated files
                    stream = new StreamReader(path);
                    StreamReader streamAsc = new StreamReader(pathAsc);
                    StreamReader streamDesc = new StreamReader(pathDesc);
                    StreamReader streamRand = new StreamReader(pathRand);

                    // Make a copy of the Stream List as an Array
                    // Shuffle to generate new randomized sequence of the input stream
                    float[] streamArray = new float[listOfInboundStream.Count];
                    listOfInboundStream.CopyTo(streamArray);
                    Shuffle(new Random(), streamArray);

                    Console.WriteLine("");

                    // Estimate median in all test cases and report in console
                    float median = program.estimateMedian(stream, 5);
                    Console.WriteLine("TEST Original: " + median);
                    DisplayArray(program.getSampleArray());

                    float medianAsc = program.estimateMedian(streamAsc, 5);
                    Console.WriteLine("TEST Ascending: " + medianAsc);
                    DisplayArray(program.getSampleArray());

                    float medianDesc = program.estimateMedian(streamDesc, 5);
                    Console.WriteLine("TEST Descending: " + medianDesc);
                    DisplayArray(program.getSampleArray());

                    float medianRand = program.estimateMedian(streamRand, 5);
                    Console.WriteLine("TEST Random: " + medianRand);
                    DisplayArray(program.getSampleArray());
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Sorry, File not found!");
            }
        }

        // Array Shuffler
        public static void Shuffle<T>(Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }

        public void DisplayArray(float[] array)
        {
            Console.Write("[ ");
            foreach (var i in array)
            {
                Console.Write(i + " ");
            }
            Console.Write("]");
            Console.WriteLine("");
            
        }
    }
}
