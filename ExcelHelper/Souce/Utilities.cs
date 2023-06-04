using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class Utilities
    {
        public void SaveTablePath(string _utilPath, string _tablePath)
        {
            using (var saveTablePathFile = System.IO.File.CreateText(_utilPath + "\\TablePath.txt"))
            {
                saveTablePathFile.Write(_tablePath);

                saveTablePathFile.Close();
            }
        }

        public void SaveTablecount(string _utilPath, int _tableCount)
        {
            using (var saveTablePathFile = System.IO.File.CreateText(_utilPath + "\\TableCount.txt"))
            {
                saveTablePathFile.Write(_tableCount);

                saveTablePathFile.Close();
            }
        }

        public bool CompareTableCount(string _utilPath, int _nowTableCount)
        {
            int savedTableCount = Convert.ToInt32(System.IO.File.ReadLines(_utilPath + "\\TableCount.txt").ToArray()[0].ToString());

            return savedTableCount == _nowTableCount;
        }
    }
}
