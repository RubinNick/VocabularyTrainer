using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using VocabularyTrainer.Services.Models;

namespace VocabularyTrainer.Services.Utils
{
    public class WordProcessingService
    {
        public List<VocabularyRowModel> ProcessDocumentTable(string filePath)
        {
            
            using var document = WordprocessingDocument.Open(filePath, false);
            
            if (document.MainDocumentPart == null) throw new NullReferenceException("Main document part is null");
            if (document.MainDocumentPart.Document == null) throw new NullReferenceException("Document part is null");
            if (document.MainDocumentPart.Document.Body == null) throw new NullReferenceException("Document Body is null");
            
            var documentBody = document.MainDocumentPart.Document.Body;
 
            var docFirstTable = documentBody.Elements<Table>().FirstOrDefault();

            if (docFirstTable == null) throw new NullReferenceException("Table not found");
            
            var tableRows = docFirstTable.Elements<TableRow>().ToArray();

            //If first row is header - it'll be a text in other case - number
            var firstCellText = tableRows.FirstOrDefault().Elements<TableCell>().FirstOrDefault().InnerText;
            var isFirstRowHeading = !int.TryParse(firstCellText, out _);

            var vocabularyList = new List<VocabularyRowModel>();
            var rowModelProperties = typeof(VocabularyRowModel).GetProperties();
            
            for (var i = isFirstRowHeading ? 1 : 0; i < tableRows.Length; i++)
            {
                var row = tableRows[i];
                var rowModel = new VocabularyRowModel();

                var rowCellsArr = row.Elements<TableCell>().ToArray();
                
                for (var g = 0; g < rowCellsArr.Length; g++)
                {
                    var cellInnerText = rowCellsArr[g].InnerText;
                    var cellFormattedValue = string.IsNullOrEmpty(cellInnerText) ? g.ToString() : cellInnerText;

                    var property = rowModelProperties[g];
                    var propType = property.PropertyType;
                    
                    var converter = TypeDescriptor.GetConverter(propType);
                    var convertedValue = converter.ConvertFromString(cellFormattedValue);
                    
                    property.SetValue(rowModel, convertedValue);
                }
                
                vocabularyList.Add(rowModel);
            }
            
            return vocabularyList;
        }
    }
}