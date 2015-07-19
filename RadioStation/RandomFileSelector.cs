using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RadioStation
{
    class RandomFileSelector
    {
        private List<string> fileList;
        private Random random;
        private int currentIndex = 0;

        public RandomFileSelector(string initFolder)
        {
            fileList = Directory.EnumerateFiles(initFolder, "*.mp3", SearchOption.AllDirectories).ToList();
            if (!fileList.Any())
                throw new Exception("No .mp3 files found in this folder!");
            fileList = RandomSort(fileList);
        }

        public string NextFile()
        {
            return fileList[currentIndex == fileList.Count - 1 ? currentIndex = 0 : ++currentIndex];
        }

        public static List<T> RandomSort<T>(List<T> input)
        {
            var randomizer = new Random();
            var index = 0;
            
            var list = input.Select(s => new { Index = index++, Number = randomizer.Next() }).OrderBy(t => t.Number).ToList();

            var outputList = new List<T>();
            list.ForEach(item => outputList.Add(input[item.Index]));

            return outputList;
        }
    }
}
