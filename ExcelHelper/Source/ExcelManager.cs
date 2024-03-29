﻿using DataHandler;
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
        static DataDictHandler dataDictHandler;
        static FileDictHandler fileDictHandler;

        #region Get Methods
        public List<string> GetAllRepresentDataList()
        {
            if (dataDictHandler == null) { return null; }
            return DataDict.Keys.ToList();
        }
        public List<string> GetAllDividedData()
        {
            return dataDictHandler.FindDivisionData();
        }
        public List<string> GetFilesInSpecificData(string _data)
        {
            return DataDict[_data].files;
        }
        public bool IsDataDictContain(string _data)
        {
            return dataDictHandler.IsDataContains(_data);
        }
        #endregion

        #region Delete File
        public void DeleteFiles(string _savePath, List<string> _deletedFiles)
        {
            dataDictHandler.RemoveFiles(_deletedFiles);
            fileDictHandler.RemoveFiles(_deletedFiles);

            DataDictHandler.SaveDataDict(_savePath);
            FileDictHandler.SaveFileDict(_savePath);
        }
        #endregion

        #region Read Files
        // 루트 따라 파일 읽고, DataDict, FileDict 초기화
        public void ReadFilesFromFullio(string _rootPath, string _savePath)
        {
            dataDictHandler = new DataDictHandler();
            fileDictHandler = new FileDictHandler();

            const string exceptionFilePrefix = "Merged_";
            const string exceptionFileName_Replace = "Replace";
            const string exceptionFileName_Override = "Override";
            const string exceptionDir = "BotSetting";

            string[] files = Directory.GetFiles(_rootPath, "*.xlsx", SearchOption.AllDirectories);

            FileStream stream = null;

            foreach (string file in files)
            {
                // 파일 이름, Dir 예외처리
                if (file.Contains(exceptionFilePrefix))
                    continue;
                if (file.Contains(exceptionFileName_Replace))
                    continue;
                if (file.Contains(exceptionFileName_Override))
                    continue;
                if (file.Contains(exceptionDir))
                    continue;

                try
                {
                    stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

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
                catch(Exception e)
                {
                    using (var debugLogFile = System.IO.File.CreateText(_savePath + "\\" + "ReadDebugLog.txt"))
                    {
                        string tmpstr = "Error in <" + file + "> " + e.Message + "\n";

                        debugLogFile.WriteLine(tmpstr);
                    }
                }
                finally
                {
                    stream.Dispose();
                }
            }
            
            // Set Data Dict
            dataDictHandler.SetData();

            SaveDicts(_savePath);
        }

        public void ReadFilesFromSpecificIO(string _rootPath, string _savePath, List<string> _files)
        {
            if (dataDictHandler == null || fileDictHandler == null)
                return;

            const string exceptionFilePrefix = "Merged_";
            const string exceptionFileName_Replace = "Replace";
            const string exceptionFileName_Override = "Override";
            const string exceptionDir = "BotSetting";

            List<string> files = _files;

            FileStream stream = null;

            foreach (string file in files)
            {
                // 파일 이름, Dir 예외처리
                if (file.Contains(exceptionFilePrefix))
                    continue;
                if (file.Contains(exceptionFileName_Replace))
                    continue;
                if (file.Contains(exceptionFileName_Override))
                    continue;
                if (file.Contains(exceptionDir))
                    continue;

                try
                {
                    stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);

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
                catch (Exception e)
                {
                    using (var debugLogFile = System.IO.File.CreateText(_savePath + "\\" + "ReadDebugLog.txt"))
                    {
                        string tmpstr = "Error in <" + file + "> " + e.Message + "\n";

                        debugLogFile.WriteLine(tmpstr);
                    }
                }
                finally
                {
                    stream.Dispose();
                }
            }

            // Set Data Dict
            dataDictHandler.SetData();

            SaveDicts(_savePath);
        }
        #endregion

        #region Read DictData
        // SaveDicts로 저장했던 데이터 내역 읽어오기
        public void ReadFilesFromDictData(string _rootPath)
        {
            dataDictHandler = new DataDictHandler(_rootPath);
            fileDictHandler = new FileDictHandler(_rootPath);
        }
        #endregion

        #region Save DictData
        // 최적화용... 전체 IO 내역 저장해두면 -> 다음엔 해당 파일만 읽어도 됨.
        void SaveDicts(string _rootPath)
        {
            if (dataDictHandler != null)
            {
                DataDictHandler.SaveDataDict(_rootPath);
            }
            else
            {
                using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "NoData.txt"))
                {
                    newSaveFile.Close();
                }
            }

            if (fileDictHandler != null)
            {
                FileDictHandler.SaveFileDict(_rootPath);                
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
            string mergedTablePath = _rootPath + "\\" + "Merged_" + _data + "_Table.xlsx";

            // 라이센스 에러
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage mergedFile = new ExcelPackage(new FileInfo(mergedTablePath)))
            {
                Dictionary<string, int> mergingRowIdxDict = new Dictionary<string, int>();

                // 기존 MergedFile 처리
                string referFile = "";
                foreach (var sheet in DataDictHandler.DataDict[_data].sheetList)
                {
                    foreach (var file in DataDictHandler.DataDict[_data].files)
                    {
                        if (FileDictHandler.FileDict[file].Keys.Contains(sheet))
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

                    InitSheet(mergedFile.Workbook.Worksheets[sheet], FileDictHandler.FileDict[referFile][sheet]);
                    mergingRowIdxDict.Add(sheet, 6);
                }
                // 저장해줘야 이후 Dimension을 정상적으로 체크 가능
                mergedFile.Save();

                ExcelPackage sourceFile = null;
                ExcelWorksheet mergingSheet = null;
                int _sourceRow, _sourceCol;
                // Merge
                foreach (string sourceFilePath in DataDictHandler.DataDict[_data].files)
                {
                    try
                    {
                        sourceFile = new ExcelPackage(new FileInfo(sourceFilePath));

                        // 소스 파일의 시트를 순회하며 -> B6부터 Row를 한 칸씩 내려가며 MergedFile에 복붙
                        foreach (var sourceSheet in sourceFile.Workbook.Worksheets)
                        {
                            // "."으로 시작하는 시트는 Merge하지 않음
                            if (sourceSheet.Name.StartsWith("."))
                                continue;

                            mergingSheet = mergedFile.Workbook.Worksheets[sourceSheet.Name];

                            for (_sourceRow = 6; ; _sourceRow++)
                            {
                                if (sourceSheet.Cells[_sourceRow, 2].Value == null || sourceSheet.Cells[_sourceRow, 2].Value.ToString() == "")
                                    break;


                                for (_sourceCol = 2; ; _sourceCol++)
                                {
                                    if (sourceSheet.Cells[5, _sourceCol].Value == null || sourceSheet.Cells[5, _sourceCol].Value.ToString() == "")
                                        break;

                                    mergingSheet.Cells[mergingRowIdxDict[sourceSheet.Name], _sourceCol].Value = sourceSheet.Cells[_sourceRow, _sourceCol].Value;
                                }
                                // 1열에 해당 데이터가 포함된 파일 이름 추가
                                mergingSheet.Cells[mergingRowIdxDict[sourceSheet.Name], 1].Value = Path.GetFileName(sourceFilePath);

                                mergingRowIdxDict[sourceSheet.Name]++;
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        using (var debugLogFile = System.IO.File.CreateText(_rootPath + "\\" + "MergeDebugLog.txt"))
                        {
                            string tmpstr = "Error in <" + sourceFile + "> " + e.Message + "\n";

                            debugLogFile.WriteLine(tmpstr);
                        }
                    }
                    finally
                    {
                        sourceFile.Dispose();
                    }
                }

                foreach(var sheet in mergedFile.Workbook.Worksheets)
                {
                    int _col;
                    for(_col = 2; ; _col++)
                    {
                        if (sheet.Cells[5, _col].Value == null || sheet.Cells[5, _col].Value.ToString() == "")
                            break;
                    }
                    // 컬럼 사이즈 러프하게 조정 (첫 번째 데이터 길이에 맞춰서 조정)
                    sheet.Cells[5, 1, 6, _col].AutoFitColumns();
                }
                mergedFile.Save();
            }
        }

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
        #endregion
    }
}