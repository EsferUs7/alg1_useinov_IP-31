using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1_useinov_ip31
{
    public class ModExternalSorting
    {
        public static void StartSorting(string path, long chunkSize)
        {
            List<string> namesOfChunks = GetChunks(path, chunkSize);
            MergeChunks(path, namesOfChunks);

            int i = Convert.ToInt32(chunkSize / sizeof(Int32));
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


        private static List<string> GetChunks(string fileName, long chunkSize)
        {
            int amountOfChunks;
            var namesOfChunks = new List<string>();

            using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                long fileLength = reader.BaseStream.Length;

                amountOfChunks = Convert.ToInt32(fileLength / chunkSize) + Convert.ToInt32(fileLength % chunkSize != 0);

                int currentChunk = 0;
                int chunkLength = Convert.ToInt32(fileLength / amountOfChunks);

                Int32[] buffer = new Int32[chunkSize / sizeof(Int32)];

                while (currentChunk < amountOfChunks - 1)
                {
                    namesOfChunks.Add($"{Environment.CurrentDirectory}\\{currentChunk + 1}.bin");
                    for (int i = 0; i < chunkSize / sizeof(Int32); i++)
                    {
                        buffer[i] = reader.ReadInt32();
                    }
                    Array.Sort(buffer);

                    using (var chunk = new BinaryWriter(File.Open(namesOfChunks[currentChunk], FileMode.Create)))
                    {
                        foreach (var value in buffer)
                        {
                            chunk.Write(value);
                        }
                    }

                    currentChunk++;
                }

                namesOfChunks.Add($"{Environment.CurrentDirectory}\\{currentChunk + 1}.bin");


                int lastChunkLength = Convert.ToInt32(fileLength - reader.BaseStream.Position) / sizeof(Int32);
                buffer = new Int32[lastChunkLength];
                for (int i = 0; i < lastChunkLength; i++)
                {
                    buffer[i] = reader.ReadInt32();
                }
                Array.Sort(buffer);

                using (var chunk = new BinaryWriter(File.Open(namesOfChunks[currentChunk], FileMode.Create)))
                {
                    foreach (var value in buffer)
                    {
                        chunk.Write(value);
                    }
                }
            }

            return namesOfChunks;
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

        private static void MergeChunks(string outputFile, List<string> chunks)
        {
            using (var writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                foreach (var chunk in chunks)
                {
                    using (var reader = new BinaryReader(File.Open(chunk, FileMode.Open)))
                    {
                        long chunkLength = reader.BaseStream.Length;
                        while (reader.BaseStream.Position != chunkLength)
                        {
                            var value = reader.ReadInt32();
                            writer.Write(value);
                        }
                    }
                }
            }

            foreach (var chunk in chunks)
            {
                File.Delete(chunk);
            }
        }
    }
}
