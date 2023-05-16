using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataHandler
{
    public class DataDictHandler
    {
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
        
        public Dictionary<string, DataInfo> DataDict;

        public DataDictHandler()
        {
            this.DataDict = new Dictionary<string, DataInfo>();
        }
        public DataDictHandler(Dictionary<string, DataInfo> _dataDict)
        {
            this.DataDict = _dataDict;
        }

        public void SetData(FileDictHandler _fileDictHandler)
        {
            this.DataDict = null;
            this.DataDict = new Dictionary<string, DataInfo>();

            bool hasData;
            string representSheet = "";

            foreach (var file in _fileDictHandler.FileDict.Keys)
            {
                hasData = false;

                // 해당 file의 시트 순회
                foreach (var sheet in _fileDictHandler.FileDict[file].Keys)
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
                    foreach (var fileSheet in _fileDictHandler.FileDict[file].Keys)
                    {
                        if (!DataDict[representSheet].sheetList.Contains(fileSheet))
                        {
                            DataDict[representSheet].AddSheet(fileSheet);
                        }
                    }

                    // File 리스트에 해당 file 추가
                    this.DataDict[representSheet].AddFiles(file);
                }
                else
                {
                    this.DataDict.Add(_fileDictHandler.FileDict[file].Keys.ToArray().First(), new DataInfo(_fileDictHandler.FileDict[file].Keys.ToList(), file));
                }
            }
        }

        // 물리적으로 분리된 테이블 찾기
        public List<string> FindDivisionData()
        {
            List<string> tmpList = new List<string>();

            foreach (string e in this.DataDict.Keys)
            {
                if (this.DataDict[e].files.Count > 1)
                {
                    tmpList.Add(e);
                }
            }

            return tmpList;
        }
    }

    public class FileDictHandler
    {
        // 개별 Table File 기준으로 Path, 시트, 컬럼 정보 보유
        // 머지 시, 개별 테이블 접근용도로 사용
        public Dictionary<string, Dictionary<string, List<string>>> FileDict;

        public FileDictHandler()
        {
            this.FileDict = new Dictionary<string, Dictionary<string, List<string>>>();
        }
        public FileDictHandler(Dictionary<string, Dictionary<string, List<string>>> _fileDict)
        {
            this.FileDict = _fileDict;
        }

        public void AddFile(string _filePath, string _sheet, List<string> _columns)
        {
            if (!FileDict.ContainsKey(_filePath))
            {
                Dictionary<string, List<string>> tmpDict = new Dictionary<string, List<string>>();
                tmpDict.Add(_sheet, _columns.ToList());

                this.FileDict.Add(_filePath, tmpDict);
            }
            else
            {
                this.FileDict[_filePath].Add(_sheet, _columns.ToList());
            }
        }
    }
}