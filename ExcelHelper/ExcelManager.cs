using DataHandler;
using Perforce.P4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using static DataHandler.DataDictHandler;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Windows.Interop;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2016.Excel;
using OfficeOpenXml;

namespace ExcelManager
{
    public class ExcelManager
    {
        DataDictHandler dataDictHandler;
        FileDictHandler fileDictHandler;

        #region Get Methods
        public List<string> GetAllRepresentDataList()
        {
            if (dataDictHandler == null) { return null; }
            return dataDictHandler.DataDict.Keys.ToList();
        }
        public List<string> GetAllDividedData()
        {
            return dataDictHandler.FindDivisionData();
        }
        public List<string> GetFilesInSpecificData(string _data)
        {
            return dataDictHandler.DataDict[_data].files;
        }
        #endregion

        #region Read Full Files
        // 루트 따라 파일 읽고, DataDict, FileDict 초기화
        public void ReadFilesFromFullio(string _rootPath, string _savePath)
        {

            dataDictHandler = new DataDictHandler();
            fileDictHandler = new FileDictHandler();

            string[] files = Directory.GetFiles(_rootPath, "*.xlsx", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                const string exceptionFilePrefix = "Merged_";
                const string exceptionDir = "BotSetting";
                if (file.Contains(exceptionFilePrefix))
                    continue;
                if (file.Contains(exceptionDir))
                    continue;

                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (SpreadsheetDocument workbook = SpreadsheetDocument.Open(stream, false))
                    {
                        string _fileFullName = stream.Name;

                        WorkbookPart workbookPart = workbook.WorkbookPart;
                        IEnumerable<Sheet> sheets = workbookPart.Workbook.Descendants<Sheet>();
                        foreach (Sheet sheet in sheets)
                        {
                            const string exceptionPrefix = ".";
                            if (sheet.Name.Value.StartsWith(exceptionPrefix))
                                continue;

                            string _sheetName = sheet.Name.Value;
                            List<string> _columns = new List<string>();

                            WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                            Cell cell;

                            // B5 기준, column들 전부 list에 집어넣기
                            for (uint _col = 2; _col <= 1000; _col++)
                            {
                                cell = sheetData.Descendants<Cell>().Where(c => c.CellReference == GetCellReference(5, _col)).FirstOrDefault();

                                if (cell == null)
                                    break;

                                string cellValue = GetCellValue(cell, workbookPart);

                                if (cellValue == null || cellValue == "")
                                {
                                    break;
                                }

                                _columns.Add(cellValue);
                            }

                            fileDictHandler.AddFile(_fileFullName, _sheetName, _columns);
                        }
                    }
                }
            }
            
            // Set Data Dict
            dataDictHandler.SetData(fileDictHandler);

            SaveDicts(_savePath);
        }
        #endregion

        #region Read DictData
        // SaveDicts로 저장했던 데이터 내역 읽어오기
        public void ReadFilesFromDictData(string _rootPath)
        {
            dataDictHandler = new DataDictHandler(ReadSavedDataDict(_rootPath));
            fileDictHandler = new FileDictHandler(ReadSavedFileDict(_rootPath));
        }
        Dictionary<string, DataInfo> ReadSavedDataDict(string _rootPath)
        {
            string representData = "";
            Dictionary<string, DataInfo> dataDict = new Dictionary<string, DataInfo>();
            List<string> sheetList = new List<string>();
            List<string> fileList = new List<string>();
            string[] splitStr = new string[] { "..." };
            foreach (var line in System.IO.File.ReadLines(_rootPath + "\\" + "DataDict.txt", Encoding.UTF8))
            {
                if (line.Contains("Next..."))
                {
                    DataInfo dataInfo = new DataInfo(sheetList, fileList);
                    dataDict.Add(representData, dataInfo);
                    continue;
                }

                if (line.Contains("RepresentData>>"))
                {
                    representData = line.Split(new string[] {">>"}, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                if (line.Contains("SheetList>>"))
                {
                    sheetList = new List<string>();
                    foreach (var sheet in line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (sheet.Contains("SheetList>>"))
                            continue;

                        sheetList.Add(sheet);
                    }
                }
                if (line.Contains("FileList>>"))
                {
                    fileList = new List<string>();
                    foreach (var file in line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (file.Contains("FileList>>"))
                            continue;

                        fileList.Add(file);
                    }
                }
            }

            return dataDict;
        }
        Dictionary<string, Dictionary<string, List<string>>> ReadSavedFileDict(string _rootPath)
        {
            string file = "";
            Dictionary<string, Dictionary<string, List<string>>> fileDict = new Dictionary<string, Dictionary<string, List<string>>>();
            string sheet = "";
            Dictionary<string, List<string>> sheetDict = new Dictionary<string, List<string>>();
            List<string> columnList = new List<string>();
            string[] splitStr = new string[] { "..." };
            foreach (var line in System.IO.File.ReadLines(_rootPath + "\\" + "FileDict.txt", Encoding.UTF8))
            {
                if (line.Contains("Next..."))
                {
                    fileDict.Add(file, sheetDict);
                    continue;
                }

                if (line.Contains("File>>"))
                {
                    file = line.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    sheetDict = new Dictionary<string, List<string>>();
                }
                if (line.Contains("Sheet>>"))
                {
                    sheet = line.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                if (line.Contains("ColumnList>>"))
                {
                    columnList = new List<string>();
                    foreach (var column in line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (column.Contains("ColumnList>>"))
                            continue;

                        columnList.Add(column);
                    }

                    sheetDict.Add(sheet, columnList);
                }
            }

            return fileDict;
        }
        #endregion

        #region Save DictData
        // 최적화용... 전체 IO 내역 저장해두면 -> 다음엔 해당 파일만 읽어도 됨.
        void SaveDicts(string _rootPath)
        {
            if (this.dataDictHandler != null)
            {
                using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "DataDict.txt"))
                {
                    string tmpstr = "";

                    foreach (string e in this.dataDictHandler.DataDict.Keys)
                    {
                        tmpstr += "RepresentData>>" + e + "\n"
                            + "SheetList>>...";

                        foreach (string d in this.dataDictHandler.DataDict[e].sheetList)
                        {
                            tmpstr += d + "...";
                        }

                        tmpstr += "\n"
                            + "FileList>>...";

                        foreach (string v in this.dataDictHandler.DataDict[e].files)
                        {
                            tmpstr += v + "...";
                        }

                        tmpstr += "\nNext...\n";
                    }

                    newSaveFile.Write(tmpstr);

                    newSaveFile.Close();
                }
            }
            else
            {
                using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "NoData.txt"))
                {
                    newSaveFile.Close();
                }
            }

            if (this.fileDictHandler != null)
            {
                using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "FileDict.txt"))
                {
                    string tmpstr = "";

                    foreach (string e in this.fileDictHandler.FileDict.Keys)
                    {
                        tmpstr += "File>>" + e + "\n";

                        foreach (string v in this.fileDictHandler.FileDict[e].Keys)
                        {
                            tmpstr += "Sheet>>" + v + "\n"
                                + "ColumnList>>...";
                            foreach (string col in this.fileDictHandler.FileDict[e][v])
                            {
                                tmpstr += col + "...";
                            }
                            tmpstr += "\n";
                        }
                        tmpstr += "\nNext...\n";
                    }

                    newSaveFile.Write(tmpstr);

                    newSaveFile.Close();
                }
            }
            else
            {
                using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "NoFile.txt"))
                {
                    newSaveFile.Close();
                }
            }
        }
        #endregion

        #region Merge
        public void Merge(string _rootPath, string _data)
        {
            // 라이센스 에러
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string mergedTablePath = _rootPath + "\\" + "Merged_" + _data + "_Table.xlsx";

            using (ExcelPackage mergedFile = new ExcelPackage(new FileInfo(mergedTablePath)))
            {
                // 기존 MergedFile 처리
                string referFile = "";
                foreach (var sheet in dataDictHandler.DataDict[_data].sheetList)
                {
                    foreach (var file in dataDictHandler.DataDict[_data].files)
                    {
                        if (fileDictHandler.FileDict[file].Keys.Contains(sheet))
                        {
                            referFile = file;
                            break;
                        }
                    }
                    if (referFile == "")
                    {
                        // 뭔가 이상한 거임. 중단해야 함
                        // 관련해서 에러 코드 잡아줘야 하긴 할 듯...
                        return;
                    }

                    if (mergedFile.Workbook.Worksheets[sheet] == null)
                    {
                        mergedFile.Workbook.Worksheets.Add(sheet);
                    }
                    else
                    {
                        mergedFile.Workbook.Worksheets[sheet].Cells[1, 2, mergedFile.Workbook.Worksheets[sheet].Rows.EndRow, mergedFile.Workbook.Worksheets[sheet].Columns.EndColumn].Clear();
                    }

                    InitSheet(mergedFile.Workbook.Worksheets[sheet], fileDictHandler.FileDict[referFile][sheet]);
                }
                // 저장해줘야 이후 Dimension을 정상적으로 체크 가능
                mergedFile.Save();


                // Merge
                foreach (string sourceFilePath in dataDictHandler.DataDict[_data].files)
                {
                    using (var sourceFile = new ExcelPackage(new FileInfo(sourceFilePath)))
                    {
                        int _mergingRowIdxInMergedFile;
                        ExcelWorksheet mergingSheet;

                        // 소스 파일의 시트를 순회하며 -> B6부터 Row를 한 칸씩 내려가고, DimensionColumn (데이터 있는 컬럼 개수) 개만큼 MergedFile에 복붙
                        foreach (var sourceSheet in sourceFile.Workbook.Worksheets)
                        {
                            // "."으로 시작하는 시트는 Merge하지 않음
                            if (sourceSheet.Name.StartsWith("."))
                                continue;

                            mergingSheet = mergedFile.Workbook.Worksheets[sourceSheet.Name];

                            _mergingRowIdxInMergedFile = mergingSheet.Dimension.End.Row + 1;

                            for (int _sourceRow = 6; _sourceRow <= sourceSheet.Dimension.End.Row; _sourceRow++)
                            {
                                // 빈 셀인데 표서식은 존재하는 경우 -> Dimension은 존재하는 것으로 취급함
                                // 복사할 row의 첫 행에 Value 없으면 -> 상기 케이스로 간주하여 복사하지 않고 다음 시트로 넘어감 (Cuid는 중간에 공란이 생길 수 없음)
                                if (sourceSheet.Cells[_sourceRow, 2].Value == null || sourceSheet.Cells[_sourceRow, 2].Value.ToString() == "")
                                    break;


                                // 데이터 복붙
                                for (int _sourceCol = 2; _sourceCol <= sourceSheet.Dimension.End.Column; _sourceCol++)
                                {
                                    mergingSheet.Cells[_mergingRowIdxInMergedFile, _sourceCol].Value = sourceSheet.Cells[_sourceRow, _sourceCol].Value;
                                }


                                _mergingRowIdxInMergedFile++;
                            }
                        }

                        // Merge 파일 저장. 저장하면 자동 Close 됨
                        mergedFile.Save();
                    }
                }
            }

            #region Open XML 버전 (실패)
            // MergedFile 열기
            //SpreadsheetDocument mergeTargetFile = SpreadsheetDocument.Open(mergedTablePath, true);

            //WorkbookPart mergedFileWorkbookPart = mergeTargetFile.WorkbookPart;
            //WorksheetPart mergedFileWorksheetPart = mergedFileWorkbookPart.WorksheetParts.First();
            //Sheets mergedFileSheets;
            //Sheet mergedFileSheet;

            //if (!System.IO.File.Exists(mergedTablePath))
            //{
            //    using (SpreadsheetDocument mergeTargetFile = SpreadsheetDocument.Create(mergedTablePath, SpreadsheetDocumentType.Workbook))
            //    {
            //        WorkbookPart mergedWorkbookPart = mergeTargetFile.AddWorkbookPart();
            //        mergedWorkbookPart.Workbook = new Workbook();

            //        WorksheetPart mergedWorksheetPart = mergedWorkbookPart.AddNewPart<WorksheetPart>();
            //        mergedWorksheetPart.Worksheet = new Worksheet(new SheetData());

            //        Sheets mergedSheets = mergeTargetFile.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            //        Sheet mergedSheet;

            //        foreach (var sheet in dataDictHandler.DataDict[_data].sheetList)
            //        {
            //            mergedSheet = new Sheet() { Id = mergeTargetFile.WorkbookPart.GetIdOfPart(mergedWorksheetPart), Name = sheet };
            //            mergedSheets.Append(mergedSheet);

            //            using (SpreadsheetDocument sourceWorkbook = SpreadsheetDocument.Open(dataDictHandler.DataDict[_data].files[0], false))
            //            {
            //                Worksheet sourceWorksheet = sourceWorkbook.WorkbookPart.WorksheetParts.First(wp => wp.Worksheet.LocalName == sheet).Worksheet;
            //                Row sourceRow = sourceWorksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault(r => r.RowIndex == 5);
            //                Row destinationRow = mergedSheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault(r => r.RowIndex == 5);

            //                if (sourceRow == null)
            //                {
            //                    // Exit if either row does not exist
            //                    return;
            //                }

            //                Row newRow = (Row)sourceRow.CloneNode(true);
            //                newRow.RowIndex = 5;

            //                foreach (Cell cell in newRow.Elements<Cell>())
            //                {
            //                    string cellReference = cell.CellReference.Value;
            //                    cell.CellReference = new StringValue(cellReference);
            //                }

            //                // Insert the new row into the destination worksheet
            //                mergedSheet.GetFirstChild<SheetData>().InsertBefore(newRow, destinationRow);
            //            }
            //        }
            //    }
            //}

            //string referFile = "";
            //foreach (var sheet in dataDictHandler.DataDict[_data].sheetList)
            //{
            //    foreach (var file in dataDictHandler.DataDict[_data].files)
            //    {
            //        if (fileDictHandler.FileDict[file].Keys.Contains(sheet))
            //        {
            //            referFile = file;

            //            CopyRow(referFile, sheet, 6, mergedTablePath, sheet, GetRowCount(mergedTablePath, sheet) + 1, GetRowCount(referFile, sheet));

            //            //break;
            //        }
            //    }
            //}

            #endregion
        }
        #region Open XML 버전에 쓰는 함수들
        /*
        public void CopyRow(string sourceFilePath, string sourceSheetName, uint sourceRowIndex, string destinationFilePath, string destinationSheetName, uint destinationRowIndex, uint numberOfRows)
        {
            // Open the source workbook and worksheet
            using (SpreadsheetDocument sourceWorkbook = SpreadsheetDocument.Open(sourceFilePath, true))
            {
                Worksheet sourceWorksheet = sourceWorkbook.WorkbookPart.WorksheetParts.First(wp => wp.Worksheet.LocalName == sourceSheetName).Worksheet;

                // Open the destination workbook and worksheet
                using (SpreadsheetDocument destinationWorkbook = SpreadsheetDocument.Open(destinationFilePath, true))
                {
                    Worksheet destinationWorksheet = destinationWorkbook.WorkbookPart.WorksheetParts.First(wp => wp.Worksheet.LocalName == destinationSheetName).Worksheet;

                    // Get the source and destination rows
                    Row sourceRow = sourceWorksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault(r => r.RowIndex == sourceRowIndex);
                    Row destinationRow = destinationWorksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault(r => r.RowIndex == destinationRowIndex);

                    if (sourceRow == null || destinationRow == null)
                    {
                        // Exit if either row does not exist
                        return;
                    }

                    // Copy the source row to the destination row
                    for (uint i = 0; i < numberOfRows; i++)
                    {
                        Row newRow = (Row)sourceRow.CloneNode(true);
                        newRow.RowIndex = destinationRowIndex + i;

                        // Adjust the cell references in the new row
                        foreach (Cell cell in newRow.Elements<Cell>())
                        {
                            string cellReference = cell.CellReference.Value;
                            cell.CellReference = new StringValue(cellReference.Replace(sourceRowIndex.ToString(), (destinationRowIndex + i).ToString()));
                        }

                        // Insert the new row into the destination worksheet
                        destinationWorksheet.GetFirstChild<SheetData>().InsertBefore(newRow, destinationRow);

                        // Increment the destination row index
                        destinationRowIndex++;
                    }

                    // Save the changes to the destination workbook
                    destinationWorkbook.WorkbookPart.Workbook.Save();
                }
            }
        }
        public uint GetRowCount(string filePath, string sheetName)
        {
            uint rowCount = 0;
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name == sheetName);
                if (sheet != null)
                {
                    WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id);
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    rowCount = (uint)sheetData.Elements<Row>().Count();
                }
            }
            return rowCount;
        }
        */
        #endregion

        void InitSheet(ExcelWorksheet _mergeSheet, List<string> _sourceColumns)
        {
            int _col;

            for (_col = 2; _col < _sourceColumns.Count + 2; _col++)
            {
                _mergeSheet.Cells[5, _col].Value = _sourceColumns[_col - 2].ToString();
                _mergeSheet.Cells[5, _col].Style.Font.Bold = true;
                _mergeSheet.Cells[5, _col].Style.Font.Color.SetColor(System.Drawing.Color.White);
                _mergeSheet.Cells[5, _col].Style.Fill.SetBackground(System.Drawing.Color.FromArgb(68, 114, 196));
                _mergeSheet.Columns[_col].AutoFit();
            }
            // 범위 찾아서 필터 걸어주기
            _mergeSheet.Cells[5, 2, 5, _sourceColumns.Count + 1].AutoFilter = true;
        }
        #endregion

        #region Utility
        public string GetMergedTableName(string _savePath, string _data)
        {
            return _savePath + "\\" + "Merged_" + _data + "_Table.xlsx";
        }

        private string GetCellValue(Cell _cell, WorkbookPart _workbookPart)
        {
            string cellValue = _cell.InnerText;

            // 이게 뭐지..? 왜 되지...?
            if (_cell.DataType != null)
            {
                switch (_cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        var stringTable = _workbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                        if (stringTable != null)
                        {
                            cellValue = stringTable.SharedStringTable.ElementAt(int.Parse(cellValue)).InnerText;
                        }
                        break;
                    default:
                        break;
                }
            }
            return cellValue;
        }
        private string GetCellReference(uint _row, uint _column)
        {
            // Get the column letter.
            string columnLetter = GetColumnLetter(_column);

            // Return the cell reference.
            return columnLetter + _row.ToString();
        }
        private string GetColumnLetter(uint _column)
        {
            uint dividend = _column;
            string columnLetter = string.Empty;

            while (dividend > 0)
            {
                uint modulo = (dividend - 1) % 26;
                columnLetter = Convert.ToChar(65 + modulo).ToString() + columnLetter;
                dividend = (uint)((dividend - modulo) / 26);
            }

            return columnLetter;
        }

        private static bool IsFileOpend(FileInfo _fileInfo)
        {
            try
            {
                using (FileStream fs = _fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    fs.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}