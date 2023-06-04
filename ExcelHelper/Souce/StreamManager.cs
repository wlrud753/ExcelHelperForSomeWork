using System;
using System.Collections.Generic;
using System.Text;

namespace StreamManager
{
    public class StreamManager
    {
        static string saveStreamPath = ExcelHelper.ExcelHelperMain.saveDataPath + "\\StreamPath.txt";
        static List<string[]> streamPathList;

        public List<string[]> GetStreamData()
        {
            string[] splitStr = new string[] { ": " };
            streamPathList = new List<string[]>();

            foreach (var line in System.IO.File.ReadLines(saveStreamPath, Encoding.UTF8))
            {
                if (line.Equals(""))
                    break;

                string[] streamPathData = new string[2];

                streamPathData[0] = line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries)[0];
                streamPathData[1] = line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries)[1];

                streamPathList.Add(streamPathData);

                streamPathData = null;
            }

            return streamPathList;
        }
        public List<string[]> GetStreamPathList()
        {
            return streamPathList;
        }

        public string GetStreamTablePath(string _stream)
        {
            if(streamPathList == null)
                return null;

            string tmpStr = streamPathList.Find(s => s[0].Equals(_stream))[1];

            if(!tmpStr.Contains("GameDesign") || !tmpStr.Contains("Table"))
                return null;

            return tmpStr;
        }

        public void SaveStreamData(List<string[]> _streamPath)
        {
            streamPathList = _streamPath;

            using (var newSaveFile = System.IO.File.CreateText(saveStreamPath))
            {
                string tmpStr = "";

                foreach (var streamPath in _streamPath)
                {
                    tmpStr += streamPath[0] + ": " + streamPath[1] + "\n";
                }

                newSaveFile.Write(tmpStr);
            }
        }

        public bool HasSavedStreamData()
        {
            if (System.IO.File.Exists(saveStreamPath))
                return true;
            return false;
        }

        // 관련 없는 폴더 지우는 건 > 수동으로 폴더만 지우는 게 깔끔할 듯...
        // 중간에 Stream 이름이 수정될 수도 있고 하니까... 이런 경우에는 수동으로 폴더 명만 바꿔주거나 해야 함
    }
}
