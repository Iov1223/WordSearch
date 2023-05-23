using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace WordSearch
{
    public class SearchAndWrite
    {
        public string filePathToWrite = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\report.txt";
        private static readonly object lockObject = new object();
        public void SearchWrite(string path, string word_search, string fileForSearch)
        {
            int count = 0;  
            Regex regex = new Regex("(?i)" + word_search);
            MatchCollection matches = regex.Matches(fileForSearch);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    count++;
                }
            }

            Write(path, word_search, count);
        }

        private void Write(string path, string word_search, int count)
        {
            lock (lockObject)
            {
                File.AppendAllText(filePathToWrite, path + ":\n" + word_search + " - " + count + "\n\n");
            }
        }

        public void WriteToFile(string path, string word_search, string fileForSearch)
        {
            
            List<Thread> threads = new List<Thread>();
            Thread thread = new Thread(() => SearchWrite(path, word_search, fileForSearch));
            threads.Add(thread);
            thread.Start();
            thread.Join();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите слово для поиска: ");
            string word_search = Console.ReadLine();

            SearchAndWrite searchAndWrite = new SearchAndWrite();

            string path = "Джордж Оруэлл - 1984.txt";
            string file = File.ReadAllText(path);
            searchAndWrite.WriteToFile(path, word_search, file);

            Console.WriteLine("Ждите, идёт процесс поиска и записи...");

            path = "Божественная комедия.txt";
            file = File.ReadAllText(path);
            searchAndWrite.WriteToFile(path, word_search, file);

            path = "Молодые львы.txt";
            file = File.ReadAllText(path);
            searchAndWrite.WriteToFile(path, word_search, file);

            path = "На западно фронте без перемен.txt";
            file = File.ReadAllText(path);
            searchAndWrite.WriteToFile(path, word_search, file);

            Console.Clear();
            Console.WriteLine("Файл сформирован, путь к файлу: " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\report.txt");

        }
    }
}
