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

        struct QuestData
        {
            internal QuestData(string _questID, string[] _acceptNpcIDs, string _instanceFieldID)
            {
                questID = _questID;
                acceptNpcIDs = _acceptNpcIDs;
                instanceFieldID = _instanceFieldID;
            }

            string questID;
            string[] acceptNpcIDs;
            string instanceFieldID;
        }

        struct SpawnData
        {
            SpawnData(string _spawnID, string _spawnLayerID, float[] _position)
            {
                spawnID = _spawnID;
                spawnLayerID = _spawnLayerID;
                postion = _position;
            }

            string spawnID;
            string spawnLayerID;
            float[] postion;
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

            List<string> questFiles = DataHandler.DataDictHandler.DataDict["Quest"].files;
            List<string> npcFiles = DataHandler.DataDictHandler.DataDict["Npc"].files;
            List<string> spawnFiles = DataHandler.DataDictHandler.DataDict["NpcSpawner"].files;

            int colValue1, colValue2, colValue3, colValue4, colValue5;

            foreach (string file in questFiles)
            {
                colValue1 = colValue2 = colValue3 = colValue4 = colValue5 = 0;

                using(ExcelPackage questFile = new ExcelPackage(file))
                {
                    ExcelWorksheet questSheet = questFile.Workbook.Worksheets.Single(s => s.Name.Equals("Quest"));

                    // 필요한 컬럼 Idx 값 세팅
                    for(int col = 2; ; col++)
                    {
                        if (questSheet.Cells[5, col].Value == null || questSheet.Cells[5, col].Value.ToString() == "")
                            break;

                        if (questSheet.Cells[5, col].Value.ToString() == "Cuid")
                            colValue1 = col;
                        if (questSheet.Cells[5, col].Value.ToString() == "IsNoncancelable")
                            colValue2 = col;
                        if (questSheet.Cells[5, col].Value.ToString() == "AcceptObject")
                            colValue3 = col;
                        if (questSheet.Cells[5, col].Value.ToString() == "AcceptObjectCuidList")
                            colValue4 = col;
                        if (questSheet.Cells[5, col].Value.ToString() == "InstanceFieldCuid")
                            colValue5 = col;
                    }

                    // 실제 데이터 판별
                    for(int row = 6; ; row++)
                    {
                        if (questSheet.Cells[row, colValue1].Value == null || questSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        // 취소 불가능은 판별할 필요 없음
                        if (questSheet.Cells[row, colValue2].Value.ToString().ToLower().Equals("true"))
                            continue;

                        // Npc 대상 수락이 아닌 경우는 별도 판단:: 매우 특이 케이스라, 자동화 필요 없음
                        if (!questSheet.Cells[row, colValue3].Value.ToString().ToLower().Equals("npc"))
                            continue;

                        if(questSheet.Cells[row, colValue4].Value == null || questSheet.Cells[row, colValue4].Value.ToString() == "")
                        {
                            // 재수락 불가
                        }
                        else
                        {
                            QuestData newQuestData;

                            if (questSheet.Cells[row, colValue5].Value != null)
                                newQuestData = new QuestData(questSheet.Cells[row, colValue1].Value.ToString(), questSheet.Cells[row, colValue4].Value.ToString().Split('\n'), questSheet.Cells[row, colValue5].Value.ToString());
                            else
                                newQuestData = new QuestData(questSheet.Cells[row, colValue1].Value.ToString(), questSheet.Cells[row, colValue4].Value.ToString().Split('\n'), "");

                            // newQuestData를... 저장해놔야죵

                            // 다 저장해 놓고 -> QuestData 저장된 거 훑으면서 Npc, SpawnerData 구조화
                        }

                    }
                }
            }
        }
        List<string> FindFiles()
        {
            List<string> fileList = new List<string>();


            return fileList;
        }
        // DivisionData들 → Merged_ 테이블 존재하는지 확인
        // 순회해야 하는 File들 리스트 일괄 정리해서 던져주는 함수
    }
}
