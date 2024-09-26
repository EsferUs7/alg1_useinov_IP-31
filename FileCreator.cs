using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_1_useinov_ip31
{
    public class FileCreator
    {
        public static void CreateFile(string name, long size = 1024 * 1024 * 1024)
        {
            size /= sizeof(Int32);
            using (var bw = new BinaryWriter(File.Create(name)))
            {
                var rand = new Random();

                for (int i = 0; i < size; i++)
                {
                    bw.Write(rand.Next(-10000, 10000));
                }
            }
        }
    }
}
