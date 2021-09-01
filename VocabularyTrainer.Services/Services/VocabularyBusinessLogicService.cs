using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VocabularyTrainer.Services.Models;

namespace VocabularyTrainer.Services.Services
{
    public class VocabularyBusinessLogicService
    {
        private Random random = new Random();
        private int GetUniqueRandomNumber(int maxValue, List<int> usedValues)
        {
            int randomNumber;

            do
            {
                randomNumber = random.Next(maxValue);
            } while (usedValues.Contains(randomNumber));

            return randomNumber;
        }

        private void ConsoleStringBuilderHelper(PropertyInfo[] props, VocabularyRowModel currentRow,
            int startPropIndex = 1, int lastPropIndex = 0)
        {
            var strBuilder = new StringBuilder();

            var loopLength = lastPropIndex == 0 ? props.Length - 1 : lastPropIndex;

            for (var i = startPropIndex; i <= loopLength; i++)
            {
                var propName = props[i].Name;
                var propValue = currentRow.GetType().GetProperty(propName)?.GetValue(currentRow);

                if (i == startPropIndex)
                {
                    strBuilder.Append(propValue);
                }
                else
                {
                    strBuilder.Append($" {propValue}");
                }
            }

            Console.WriteLine(strBuilder.ToString());
        }

        public void ToRussian(List<VocabularyRowModel> vocabularyList)
        {
            var usedIndexes = new List<int>();
            var vocabularyListCount = vocabularyList.Count;
            var stop = false;
            var props = typeof(VocabularyRowModel).GetProperties();
            
            do
            {
                var currentPhase = 1;
                var index = GetUniqueRandomNumber(vocabularyListCount - 1, usedIndexes);
                
                var selectedRow = vocabularyList[index];

                ConsoleStringBuilderHelper(props, selectedRow, currentPhase, currentPhase);

                do
                {
                    if (Console.ReadLine() == "-")
                    {
                        currentPhase++;
                        ConsoleStringBuilderHelper(props, selectedRow, 1, currentPhase);
                    }
                    else
                    {
                        currentPhase = props.Length;
                        ConsoleStringBuilderHelper(props, selectedRow, 1, 0);
                        Console.WriteLine();
                    }
                } while (currentPhase != props.Length);
                usedIndexes.Add(index);
            } while (usedIndexes.Count != vocabularyListCount || stop);
        }
    }
}