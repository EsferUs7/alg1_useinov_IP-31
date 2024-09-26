using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1_useinov_ip31
{
    public class ExternalSorting
    {
        public static void StartSorting(string path)
        {
            int i = 1;
            string firstTempFile = Environment.CurrentDirectory + "B.bin";
            string secondTempFile = Environment.CurrentDirectory + "C.bin";
            var stopSorting = false;

            while (!stopSorting)
            {
                stopSorting = Spliting(i, path, firstTempFile, secondTempFile);
                if (!stopSorting)
                {
                    Merging(i, path, firstTempFile, secondTempFile);
                    i *= 2;
                }
            }

            File.Delete(firstTempFile);
            File.Delete(secondTempFile);
        }


        private static bool Spliting(int i, string mainFile, string firstTempFile, string secondTempFile)
        {
            bool firstIsEmpty = true;
            bool secondIsEmpty = true;
            using (var reader = new BinaryReader(File.Open(mainFile, FileMode.Open)))
            using (var firstWriter = new BinaryWriter(File.Open(firstTempFile, FileMode.Create)))
            using (var secondWriter = new BinaryWriter(File.Open(secondTempFile, FileMode.Create)))
            {
                bool addToFirst = true;
                int addedNumbers = 0;
                Int32 number;

                long fileLength = reader.BaseStream.Length;
                while (reader.BaseStream.Position != fileLength)
                {
                    number = reader.ReadInt32();

                    if (addedNumbers >= i)
                    {
                        addToFirst = !addToFirst;
                        addedNumbers = 0;
                    }

                    if (addToFirst)
                    {
                        firstWriter.Write(number);
                        firstIsEmpty = false;
                    }
                    else
                    {
                        secondWriter.Write(number);
                        secondIsEmpty = false;
                    }

                    addedNumbers++;
                }
            }

            return firstIsEmpty || secondIsEmpty;
        }


        private static void Merging(int i, string mainFile, string firstTempFile, string secondTempFile)
        {
            using (var writer = new BinaryWriter(File.Open(mainFile, FileMode.Create)))
            using (var firstReader = new BinaryReader(File.Open(firstTempFile, FileMode.Open)))
            using (var secondReader = new BinaryReader(File.Open(secondTempFile, FileMode.Open)))
            {
                long firstLength = firstReader.BaseStream.Length;
                long secondLength = secondReader.BaseStream.Length;

                while (firstReader.BaseStream.Position != firstLength && secondReader.BaseStream.Position != secondLength)
                {
                    var valuesAddFirst = 0;
                    var valuesAddSecond = 0;

                    var firstValue = firstReader.ReadInt32();
                    var secondValue = secondReader.ReadInt32();

                    while (valuesAddFirst < i && valuesAddSecond < i)
                    {
                        if (firstValue < secondValue)
                        {
                            writer.Write(firstValue);
                            valuesAddFirst++;
                            if (firstReader.BaseStream.Position == firstLength)
                            {
                                valuesAddFirst = i;
                            }
                            if (valuesAddFirst < i && firstReader.BaseStream.Position != firstLength)
                            {
                                firstValue = firstReader.ReadInt32();
                            }
                        }
                        else
                        {
                            writer.Write(secondValue);
                            valuesAddSecond++;
                            if (secondReader.BaseStream.Position == secondLength)
                            {
                                valuesAddSecond = i;
                            }
                            if (valuesAddSecond < i && secondReader.BaseStream.Position != secondLength)
                            {
                                secondValue = secondReader.ReadInt32();
                            }
                        }
                    }

                    while (valuesAddFirst < i)
                    {
                        writer.Write(firstValue);
                        valuesAddFirst++;
                        if (firstReader.BaseStream.Position == firstLength)
                        {
                            valuesAddFirst = i;
                        }
                        if (valuesAddFirst < i && firstReader.BaseStream.Position != firstLength)
                        {
                            firstValue = firstReader.ReadInt32();
                        }
                    }

                    while (valuesAddSecond < i)
                    {
                        writer.Write(secondValue);
                        valuesAddSecond++;
                        if (secondReader.BaseStream.Position == secondLength)
                        {
                            valuesAddSecond = i;
                        }
                        if (valuesAddSecond < i && secondReader.BaseStream.Position != secondLength)
                        {
                            secondValue = secondReader.ReadInt32();
                        }
                    }
                }

                while (firstReader.BaseStream.Position != firstLength)
                {
                    writer.Write(firstReader.ReadInt32());
                }

                while (secondReader.BaseStream.Position != secondLength)
                {
                    writer.Write(secondReader.ReadInt32());
                }
            }
        }
    }
}
