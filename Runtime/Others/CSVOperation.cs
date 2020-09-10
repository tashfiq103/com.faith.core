namespace com.faith.core
{
    using UnityEngine;
    using System.Collections.Generic;
    public static class CSVOperation
    {
        public static int GetNumberOfLineInCSV(TextAsset csvFile)
        {

            return csvFile.text.Split('\n').Length - 1;
        }

        public static List<List<string>> GetCSVData(TextAsset csvFile, List<int> colums)
        {

            string rawCSVText = csvFile.text;
            string[] csvDataSplitedByNewLine = rawCSVText.Split('\n');
            int numberOfLineInCSV = csvDataSplitedByNewLine.Length - 1;
            int numberOfColumn = colums.Count;

            List<List<string>> resultData = new List<List<string>>();
            for (int i = 0; i < numberOfColumn; i++)
                resultData[i] = new List<string>();

            for (int i = 0; i < numberOfLineInCSV; i++)
            {

                string[] csvDataSplitedByComa = csvDataSplitedByNewLine[i].Split(',');
                for (int j = 0; j < numberOfColumn; j++)
                {

                    resultData[j].Add(csvDataSplitedByComa[colums[j]]);
                }
            }

            return resultData;
        }
    }
}

