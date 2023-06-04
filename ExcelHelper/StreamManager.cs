using System;
using System.Collections.Generic;
using System.Text;

namespace StreamManager
{
    public class StreamManager
    {
        static string saveStreamPath = ExcelHelper.ExcelHelperMain.saveDataPath + "\\StreamPath.txt";
        static List<string[]> streamPath;

        public List<string[]> GetStreamData()
        {
            //첨에 Stream을 정해야겠죵

            //테이블 확인 > 관련 Form 뜨게? >> Stream 마다 일치/ 불일치 여부 알려주기...

            // StreamPath.txt 파일 없으면 > 스트림 설정 안 된 것으로 간주 / 실수로 삭제했더라도, 무결성 깨진 것으로 간주

            /*
             * StreamPath 양식
             * Dev: C\~~
             * Stable: C\~~
             * 요런 식으로 구성...
             */

            string[] splitStr = new string[] { ": " };

            foreach (var line in System.IO.File.ReadLines(saveStreamPath, Encoding.UTF8))
            {
                if (line.Equals(""))
                    break;

                string[] streamPathData = new string[2];

                streamPathData[0] = line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries)[0];
                streamPathData[1] = line.Split(splitStr, StringSplitOptions.RemoveEmptyEntries)[1];

                streamPath.Add(streamPathData);

                streamPathData = null;
            }

            return streamPath;
        }

        public string GetStreamTablePath(string _stream)
        {
            if(streamPath == null)
                return null;

            string tmpStr = streamPath.Find(s => s[0].Equals(_stream))[1];

            if(!tmpStr.Contains("GameDesign") || !tmpStr.Contains("Table"))
                return null;

            return tmpStr;
        }

        public void SaveStreamData(List<string[]> _streamPath)
        {
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
    }
}
