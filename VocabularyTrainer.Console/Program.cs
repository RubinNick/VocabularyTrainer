using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using VocabularyTrainer.Services.Services;
using VocabularyTrainer.Services.Utils;

namespace VocabularyTrainer
{
    class Program
    {
        private static string vocabularyDocumentPath = "D:\\日本語\\satorijp\\N5\\Словарь.docx";
        
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            
            Console.WriteLine("Parsing text...");
            var wordProcessingService = new WordProcessingService();
            
            var vocabularyList =  wordProcessingService.ProcessDocumentTable(vocabularyDocumentPath);
            Console.WriteLine("Text parsed");

            Console.WriteLine($"Select mode (1 - from rus, 2 - to rus)");
            int.TryParse(Console.ReadLine(), out var selectedMode);

            if (!(selectedMode == 1 || selectedMode == 2))
            {
                Console.WriteLine("Invalid mode selected");
            }

            var bllService = new VocabularyBusinessLogicService();
            if (selectedMode == 1)
            {
                
            }else if (selectedMode == 2)
            {
                bllService.ToRussian(vocabularyList);
            }
            else
            {
                Console.WriteLine("Bye-Bye");
            }


            System.Console.ReadKey();
        }
    }
}