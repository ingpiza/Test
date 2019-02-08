using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kitzbuehel.SkiResort {

    class Program {


        static void Main(string[] args) {
            Console.WriteLine("Enter the path of the file");
            var pathFile = Console.ReadLine();
            Console.WriteLine("Processing...");

            var lines = (from line in File.ReadAllLines(pathFile)
                         select (from field in line.Split(' ')
                                 select Convert.ToInt32(field)).ToList()).ToList();

            lines.RemoveAt(0);

            var lists = new List<List<int>>();
            for(var rowIndex = 0; rowIndex < lines.Count; rowIndex++) {
                for(var colIndex = 0; colIndex < lines[rowIndex].Count; colIndex++) {
                    if(
                        (rowIndex == 0 || lines[rowIndex][colIndex] > lines[rowIndex - 1][colIndex]) &&
                        (rowIndex == lines.Count - 1 || lines[rowIndex][colIndex] > lines[rowIndex + 1][colIndex]) &&
                        (colIndex == 0 || lines[rowIndex][colIndex] > lines[rowIndex][colIndex - 1]) &&
                        (colIndex == lines[rowIndex].Count - 1 || lines[rowIndex][colIndex] > lines[rowIndex][colIndex + 1])
                        ) {
                        lists.Add(GetPath(lines, rowIndex, colIndex));
                        //Console.WriteLine(String.Join("-", GetPath(lines, rowIndex, colIndex)));
                    }
                }
            }

            var list = lists.OrderBy(l => l.Count).ThenBy(l => l.First() - l.Last()).Last();

            Console.WriteLine($"Length of calculated path: {list.Count}");
            Console.WriteLine($"Drop of calculated path: {list.First() - list.Last()}");
            Console.WriteLine($"Calculated path: {String.Join("-", list)}");
            Console.ReadKey();
        }

        static List<int> GetPath(List<List<int>> list, int rowIndex, int colIndex) {
            var path = new List<int>() { list[rowIndex][colIndex] };
            var paths = new List<List<int>>();

            if(rowIndex > 0 && path.First() > list[rowIndex - 1][colIndex]) { paths.Add(GetPath(list, rowIndex - 1, colIndex)); }
            if(rowIndex < list.Count - 1 && path.First() > list[rowIndex + 1][colIndex]) { paths.Add(GetPath(list, rowIndex + 1, colIndex)); }
            if(colIndex > 0 && path.First() > list[rowIndex][colIndex - 1]) { paths.Add(GetPath(list, rowIndex, colIndex - 1)); }
            if(colIndex < list[rowIndex].Count - 1 && path.First() > list[rowIndex][colIndex + 1]) { paths.Add(GetPath(list, rowIndex, colIndex + 1)); }

            if(paths.Count > 0) { path.AddRange(paths.OrderByDescending(l => l.Count).ThenBy(l => l.Last()).FirstOrDefault()); }

            return path;
        }

    }

}
