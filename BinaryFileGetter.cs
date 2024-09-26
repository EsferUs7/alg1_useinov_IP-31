using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1_useinov_ip31
{
    public class BinaryFileGetter
    {
        public static void GetData(string firstTempFile, string secondTempFile)
        {
            using (var first = new BinaryReader(File.Open(firstTempFile, FileMode.Open)))
            using (var second = new BinaryReader(File.Open(secondTempFile, FileMode.Open)))
            {
                while (first.BaseStream.Position != first.BaseStream.Length)
                {
                    var value = first.ReadInt32();
                    Console.WriteLine(value + " ");
                }

                Console.WriteLine();

                while (second.BaseStream.Position != second.BaseStream.Length)
                {
                    var value = second.ReadInt32();
                    Console.WriteLine(value + " ");
                }
            }
        }

        public static void GetData(string fileName)
        {
            using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (reader.BaseStream.Position != reader.BaseStream.Length)
                {
                    var number = reader.ReadInt32();
                    Console.WriteLine(number);
                }

                Console.WriteLine();
            }
        }

        public static void GetData(string fileName, long count)
        {
            using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                for (int i = 0; i < count; i++)
                {
                    var number = reader.ReadInt32();
                    Console.WriteLine(number);
                }

                Console.WriteLine();
            }
        }

        public static void GetData(string fileName, long start, long count)
        {
            using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                reader.BaseStream.Seek(start, SeekOrigin.Begin);

                for (long i = 0; i < count; i++)
                {
                    var number = reader.ReadInt32();
                    Console.WriteLine(number);
                }

                Console.WriteLine();
            }
        }
    }
}
