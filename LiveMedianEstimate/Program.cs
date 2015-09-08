using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LiveMedianEstimate
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            try
            {
                StreamReader stream = new StreamReader(@"C:\Users\Bidyut\Documents\Visual Studio 2015\Projects\LiveMedianEstimate\TestFile.txt");

                int median = program.estimateMedian(stream, 4);
                Console.WriteLine(median);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(" ");
            }
        }

        // Continuous File stream read
        private int estimateMedian(StreamReader stream, int k)
        {
            int count = 0;
            string line;

            // Initialize sample array of size k
            int[] sampleArray = new int[k];
            int[] sortedSampleArray = new int[k];

            // Read the file stream one line at a time
            while((line = stream.ReadLine()) != null)
            {
                count++;
                if(count <= k)
                {
                    sampleArray[count - 1] = Convert.ToInt32(line);
                }
                
                else if(count > k)
                {
                    Random randomizer = new Random(Guid.NewGuid().GetHashCode());

                    int randomIndex = randomizer.Next(0, count);

                    if(randomIndex < k)
                    {
                        sampleArray[randomIndex] = Convert.ToInt32(line);
                    }
                }
            }
            sortedSampleArray = (int[])sampleArray.Clone();
            QuickSort(sortedSampleArray, 0, k-1);
            return sortedSampleArray[(k - 1) / 2];
        }
        // TODO: Consider receiving data stream as byte-array

// QuickSort code starts here
        private void QuickSort(int[] inputArray, int low, int high)
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
        private int Partition(int[] inputArray, int low, int high)
        {
            int pivot = inputArray[high];
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
        private static void Swap(int[] inputArray, int elementA, int elementB)
        {
            int temp = inputArray[elementA];
            inputArray[elementA] = inputArray[elementB];
            inputArray[elementB] = temp;
        }
// QuickSort code ends here
    }
}
