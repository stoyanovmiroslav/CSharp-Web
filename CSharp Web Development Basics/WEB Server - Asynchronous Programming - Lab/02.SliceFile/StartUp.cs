using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace _02.SliceFile
{
    public class Program
    {
        public const int BufferLength = 4096;

        static void Main(string[] args)
        {
            string sourceFile = "../../../movie.mp4";
            string destinationDirectory = "../../../Resource/";
            int parts = 5;

            SliceAsync(sourceFile, destinationDirectory, parts);

            Console.WriteLine("Anything else?");

            while (true)
            {
                string command = Console.ReadLine();

                if (command == "END")
                {
                    break;
                }
            }
        }

        private static async void SliceAsync(string sourceFile, string destinationDirectory, int parts)
        {
            await Task.Run(() => Slice(sourceFile, destinationDirectory, parts));
            Console.WriteLine("Done.");
        }

        private static void Slice(string sourceFile, string destinationDirectory, int parts)
        {
            using (var streamReadFile = new FileStream(sourceFile, FileMode.Open))
            {
                string extension = sourceFile.Substring(sourceFile.LastIndexOf(".") + 1);
                long pieceSize = (long)Math.Ceiling((double)streamReadFile.Length / parts);

                for (int i = 0; i < parts; i++)
                {
                    long currentPieceSize = 0;
                    string currentPath = destinationDirectory + $"Part-{i}.{extension}";

                    using (var streamCreateFile = new FileStream(currentPath, FileMode.Create))
                    {
                        byte[] buffer = new byte[BufferLength];

                        while ((streamReadFile.Read(buffer, 0, buffer.Length)) == buffer.Length)
                        {
                            currentPieceSize += buffer.Length;

                            streamCreateFile.Write(buffer, 0, buffer.Length);
                            if (currentPieceSize >= pieceSize)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}