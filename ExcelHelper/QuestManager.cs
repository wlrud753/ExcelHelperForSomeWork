using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace QuestManager
{
    public class QuestManager
    {
        public QuestManager()
        {

        }

        public void FindQuestReacceptable()
        {
            /* 
             * QuestTable.Quest 시트 → AcceptObjectType, AcceptObjectCuid List
             * QuestCuid 마다 저장
             * Episode.Contains("Phase1")은 제외
             * NpcSpawnerTable.NpcCandidateData → 동일한 NpcCuid를 지닌 NpcSpawnerCuid 일괄 참조
             * NpcSpawnerTable.NpcSpawner → IsSpawnOnStartup 체크, SpawnLayer 체크
             * SpawnLayer.SpawnLayer → IsActivateonStartup인지 확인 / InstanceFieldTable에 값 존재하는지 확인
             * NpcTable.QuestList에 해당 QuestCuid 존재하는지 확인 / ShowHeadInfo == True 인지 확인 / IsCapturable == False인지 확인
             * 컬럼 명은 별도 txt 파일에서 긁어올 수 있게 (이후 추가)
             */ 
        }
        List<string> FindFiles()
        {
            List<string> fileLit = new List<string>();


            return fileLit;
        }
        // DivisionData들 → Merged_ 테이블 존재하는지 확인
        // 순회해야 하는 File들 리스트 일괄 정리해서 던져주는 함수
    }
}
