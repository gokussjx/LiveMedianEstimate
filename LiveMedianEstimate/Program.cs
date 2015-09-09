using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LiveMedianEstimate
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            ProgramTester programTester = new ProgramTester();

            try
            {
                StreamReader stream = new StreamReader(@"C:\Users\Bidyut\Documents\Visual Studio 2015\Projects\LiveMedianEstimate\TestFile.txt");

                float median = program.estimateMedian(stream, 5);
                Console.WriteLine(median);
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Sorry, File not found!");
            }

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

            // Read the file stream one line at a time
            while((line = stream.ReadLine()) != null)
            {
                count++;
                if(count <= k)
                {
                    sampleArray[count - 1] = float.Parse(line);
                }
                
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

            sortedSampleArray = (float[]) sampleArray.Clone();
            QuickSort(sortedSampleArray, 0, k - 1);
            return kIsEven ? (sortedSampleArray[(k/2)-1] + sortedSampleArray[(k/2)])/2.0f : sortedSampleArray[(k-1)/2];
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


// Test Program
    public class ProgramTester
    {
        public void MedianTestMethod()
        {
            Console.WriteLine(" ");
            Console.WriteLine(" ");

            Program program = new Program();
            string line;
            var listOfInboundStream = new List<float>();

            string path = @"C:\Users\Bidyut\Documents\Visual Studio 2015\Projects\LiveMedianEstimate\TestFile.txt";
            string pathAsc = @"C:\Users\Bidyut\Documents\Visual Studio 2015\Projects\LiveMedianEstimate\TestFileAsc.txt";
            string pathDesc = @"C:\Users\Bidyut\Documents\Visual Studio 2015\Projects\LiveMedianEstimate\TestFileDesc.txt";

            try
            {
                StreamReader stream = new StreamReader(path);

                // Store the stream in a List
                while ((line = stream.ReadLine()) != null)
                {
                    listOfInboundStream.Add(float.Parse(line));
                }

                //if (!File.Exists(pathAsc) || !File.Exists(pathDesc))
                //{
                //    using (FileStream fs = File.Create(pathAsc))
                //    {
                //        for (byte i = 0; i < 100; i++)
                //        {
                //            fs.WriteByte(i);
                //        }
                //    }

                //    using (FileStream fs = File.Create(pathDesc))
                //    {
                //        for (byte i = 0; i < 100; i++)
                //        {
                //            fs.WriteByte(i);
                //        }
                //    }
                //}

                if (!File.Exists(pathAsc) || (!File.Exists(pathDesc)))
                {
                    File.Create(pathAsc).Close();
                    File.Create(pathDesc).Close();
                }

                StreamWriter writerAsc = new StreamWriter(pathAsc);
                StreamWriter writerDesc = new StreamWriter(pathDesc);

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

                stream = new StreamReader(path);
                StreamReader streamAsc = new StreamReader(pathAsc);
                StreamReader streamDesc = new StreamReader(pathDesc);

                //float[] streamArray = listOfInboundStream.ToArray();

                float median = program.estimateMedian(stream, 5);
                float medianAsc = program.estimateMedian(streamAsc, 5);
                float medianDesc = program.estimateMedian(streamDesc, 5);
                Console.WriteLine("TEST Original: " + median);
                Console.WriteLine("TEST Ascending: " + medianAsc);
                Console.WriteLine("TEST Descending: " + medianDesc);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("Sorry, File not found!");
            }
        }

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
    }
}
