using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class DataDictHandler
    {
        #region DataInfo: Class for DataDict
        // 테이블을 이루고 있는 전체 data(시트) 리스트, 분리된 파일 리스트 정보를 보유할 class
        public class DataInfo
        {
            public List<string> sheetList;
            public List<string> files;

            public DataInfo(List<string> _sheets, string _file)
            {
                sheetList = new List<string>();
                sheetList.AddRange(_sheets);

                files = new List<string> { _file };
            }
            public DataInfo(List<string> _sheets, List<string> _files)
            {
                sheetList = new List<string>();
                sheetList.AddRange(_sheets);

                files = new List<string>();
                files.AddRange(_files);
            }

            public void AddSheet(string _sheet)
            {
                this.sheetList.Add(_sheet);
            }

            public void AddFiles(string _file)
            {
                this.files.Add(_file);
            }

            public void ChangeDataList(List<string> _newDataList)
            {
                this.sheetList.Clear();
                this.sheetList.AddRange(_newDataList);
            }
        }
        #endregion

        public static Dictionary<string, DataInfo> DataDict;

        public DataDictHandler()
        {
            DataDict = new Dictionary<string, DataInfo>();
        }
        public DataDictHandler(string _rootPath)
        {
            DataDict = ReadSavedDataDict(_rootPath);
        }

        public void SetData()
        {
            DataDict = null;
            DataDict = new Dictionary<string, DataInfo>();

            bool hasData;
            string representSheet = "";

            foreach (var file in FileDictHandler.FileDict.Keys)
            {
                hasData = false;

                // 해당 file의 시트 순회
                foreach (var sheet in FileDictHandler.FileDict[file].Keys)
                {
                    if (DataDict.ContainsKey(sheet))
                    {
                        hasData = true;
                        representSheet = sheet;
                        break;
                    }
                }

                if (hasData)
                {
                    // 특정 파일이 중간에 다른 시트 들고 있는 경우
                    foreach (var fileSheet in FileDictHandler.FileDict[file].Keys)
                    {
                        if (!DataDict[representSheet].sheetList.Contains(fileSheet))
                        {
                            DataDict[representSheet].AddSheet(fileSheet);
                        }
                    }

                    // File 리스트에 해당 file 추가
                    DataDict[representSheet].AddFiles(file);
                }
                else
                {
                    DataDict.Add(FileDictHandler.FileDict[file].Keys.ToArray().First(), new DataInfo(FileDictHandler.FileDict[file].Keys.ToList(), file));
                }
            }
        }

        #region Util
        // 물리적으로 분리된 테이블 찾기
        public List<string> FindDivisionData()
        {
            List<string> tmpList = new List<string>();
            List<string> tmpForDistinguishFileName = new List<string>();

            foreach (string e in DataDict.Keys)
            {
                tmpForDistinguishFileName.Clear();
                DataDict[e].files.ForEach(s => tmpForDistinguishFileName.Add(s));

                tmpForDistinguishFileName.RemoveAll(s => s.Contains("Replace") || s.Contains("Override"));

                if (tmpForDistinguishFileName.Count > 1)
                {
                    tmpList.Add(e);
                }
            }

            return tmpList;
        }
        public bool IsDataContains(string _data)
        {
            return DataDict.ContainsKey(_data);
        }
        #endregion

        #region Read, Save
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
                    representData = line.Split(new string[] { ">>" }, StringSplitOptions.RemoveEmptyEntries)[1];
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
        public static void SaveDataDict(string _rootPath)
        {
            using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "DataDict.txt"))
            {
                string tmpstr = "";

                foreach (string e in DataDict.Keys)
                {
                    tmpstr += "RepresentData>>" + e + "\n"
                        + "SheetList>>...";

                    foreach (string d in DataDict[e].sheetList)
                    {
                        tmpstr += d + "...";
                    }

                    tmpstr += "\n"
                        + "FileList>>...";

                    foreach (string v in DataDict[e].files)
                    {
                        tmpstr += v + "...";
                    }

                    tmpstr += "\nNext...\n";
                }

                newSaveFile.Write(tmpstr);

                newSaveFile.Close();
            }
        }
        #endregion
    }

    public class FileDictHandler
    {
        // 개별 Table File 기준으로 Path, 시트, 컬럼 정보 보유
        // 머지 시, 개별 테이블 접근용도로 사용
        public static Dictionary<string, Dictionary<string, List<string>>> FileDict;

        public FileDictHandler()
        {
            FileDict = new Dictionary<string, Dictionary<string, List<string>>>();
        }
        public FileDictHandler(string _rootPath)
        {
            FileDict = ReadSavedFileDict(_rootPath);
        }

        public void AddFile(string _filePath, string _sheet, List<string> _columns)
        {
            if (!FileDict.ContainsKey(_filePath))
            {
                Dictionary<string, List<string>> tmpDict = new Dictionary<string, List<string>>();
                tmpDict.Add(_sheet, _columns.ToList());

                FileDict.Add(_filePath, tmpDict);
            }
            else
            {
                FileDict[_filePath].Add(_sheet, _columns.ToList());
            }
        }

        #region Read, Save
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
        public static void SaveFileDict(string _rootPath)
        {
            using (var newSaveFile = System.IO.File.CreateText(_rootPath + "\\" + "FileDict.txt"))
            {
                string tmpstr = "";

                foreach (string e in FileDict.Keys)
                {
                    tmpstr += "File>>" + e + "\n";

                    foreach (string v in FileDict[e].Keys)
                    {
                        tmpstr += "Sheet>>" + v + "\n"
                            + "ColumnList>>...";
                        foreach (string col in FileDict[e][v])
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
        #endregion
    }
}