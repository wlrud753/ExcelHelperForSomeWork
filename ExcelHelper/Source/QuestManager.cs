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

        #region Data Structure
        struct QuestData
        {
            internal QuestData(string _questID, string[] _acceptNpcIDs)
            {
                questID = _questID;
                npcAndSpawnDataList = new Dictionary<string, List<SpawnData>>();

                foreach (string npc in _acceptNpcIDs)
                {
                    // 이거 찾기로 바꿔야 함
                    List<SpawnData> spawnList = new List<SpawnData>();
                    npcAndSpawnDataList.Add(npc, spawnList);
                }
            }

            string questID;
            Dictionary<string, List<SpawnData>> npcAndSpawnDataList;
        }

        struct SpawnData
        {
            internal string spawnID { get; set; }
            internal string spawnLayerID { get; set; }
            internal float[] postion { get; set; }
            internal List<string> npcList { get; set; }

            internal bool isInstanceNpc { get; set; }
            internal bool isSpawnOnStartup { get; set; }
        }

        public enum CannotReacceptableCode { NotHaveAcceptNpc, DesyncNpcQuestList, NullFieldSpawnData, DistantInstanceFieldNpc }

        public struct CannotReacceptableQuestData
        {
            internal CannotReacceptableQuestData(string _questID, CannotReacceptableCode _code)
            {
                questID = _questID;
                code = _code;
            }

            public string questID { get; }
            public CannotReacceptableCode code { get; }
        }
        #endregion

        static List<QuestData> questDataList;
        public static List<CannotReacceptableQuestData> cannotReacceptableQuestDataList;

        public void FindQuestReacceptable()
        {
            /* 
             * QuestTable.Quest 시트 → AcceptObjectType, AcceptObjectCuid List
             * QuestCuid 마다 저장
             * 나머지 데이터들은, 파일 한 번씩 열고 그냥 List<string> 으로 일괄 저장 (IO 최소로 하고, 편하게 메모리를 더 많이 쓰자)
             * 
             * ========================이하 파기========================
             * NpcSpawnerTable.NpcCandidateData → 동일한 NpcCuid를 지닌 NpcSpawnerCuid 일괄 참조
             * NpcSpawnerTable.NpcSpawner → IsSpawnOnStartup 체크, SpawnLayer 체크
             * SpawnLayer.SpawnLayer → IsActivateonStartup인지 확인 / InstanceFieldTable에 값 존재하는지 확인
             * NpcTable.QuestList에 해당 QuestCuid 존재하는지 확인 / ShowHeadInfo == True 인지 확인 / IsCapturable == False인지 확인
             * 컬럼 명은 별도 txt 파일에서 긁어올 수 있게 (이후 추가)
             */

            questDataList = new List<QuestData>();
            cannotReacceptableQuestDataList = new List<CannotReacceptableQuestData>();

            List<string> questFiles = DataHandler.DataDictHandler.DataDict["Quest"].files;
            List<string> npcFiles = DataHandler.DataDictHandler.DataDict["Npc"].files;

            if (questFiles == null)
                return;
            if (npcFiles == null)
                return;

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
                    }

                    // 실제 데이터 판별
                    for(int row = 6; ; row++)
                    {
                        if (questSheet.Cells[row, colValue1].Value == null || questSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        // 취소 불가능이면 판별할 필요 없음
                        if (questSheet.Cells[row, colValue2].Value.ToString().ToLower().Equals("true"))
                            continue;

                        // Npc 대상 수락이 아닌 경우는 별도 판단:: 매우 특이 케이스라, 자동화 필요 없음
                        if (!questSheet.Cells[row, colValue3].Value.ToString().ToLower().Equals("npc"))
                            continue;

                        if(questSheet.Cells[row, colValue4].Value == null || questSheet.Cells[row, colValue4].Value.ToString() == "")
                        {
                            // 재수락 불가 : 수락 NPC 없음
                            CannotReacceptableQuestData cannotQuset = new CannotReacceptableQuestData(questSheet.Cells[row, colValue1].Value.ToString(), CannotReacceptableCode.NotHaveAcceptNpc);
                            cannotReacceptableQuestDataList.Add(cannotQuset);
                        }
                        else
                        {
                            QuestData newQuestData;

                            newQuestData = new QuestData(questSheet.Cells[row, colValue1].Value.ToString(), questSheet.Cells[row, colValue4].Value.ToString().Split('\n'));

                            questDataList.Add(newQuestData);
                        }
                    }
                }
            }

            // SpawnerID, IsSpawnOnStartup, SpawnerNpcCuidList, SpawneLayerID, SpawnerPosition
            List<SpawnData> spawnDatas = FindSpawnDatas();

            // NpcData Setting: QuestList, ShowHeadInfo, Faction, IsCapturable
        }

        // SpawnerData 세팅
        List<SpawnData> FindSpawnDatas()
        {
            List<SpawnData> spawnDatas = new List<SpawnData>();

            List<string> spawnFiles = DataHandler.DataDictHandler.DataDict["NpcSpawner"].files;
            
            if (spawnFiles == null || spawnFiles.Count == 0)
                return null;

            foreach (string file in spawnFiles)
            {
                using(ExcelPackage spawnFile = new ExcelPackage(file))
                {
                    ExcelWorksheet spawnerSheet = spawnFile.Workbook.Worksheets.Single(s => s.Name.Equals("NpcSpawner"));
                    ExcelWorksheet spawnCandidateSheet = spawnFile.Workbook.Worksheets.Single(s => s.Name.Equals("NpcSpawnCandidate"));

                    // 필요한 Col Idx 세팅
                    int spawnerColValue1 = 2, spawnerColValue2 = 8, spawnerColValue3 = 12, spawnerColValue4 = 13, spawnerColValue5 = 14, spawnerColValue6 = 15;
                    for (int col = 0; ; col++)
                    {
                        if (spawnerSheet.Cells[5, col].Value == null || spawnerSheet.Cells[5, col].Value.ToString() == "")
                            break;

                        if (spawnerSheet.Cells[5, col].Value.ToString() == "Cuid")
                            spawnerColValue1 = col;
                        if (spawnerSheet.Cells[5, col].Value.ToString() == "IsSpawnOnStartup")
                            spawnerColValue2 = col;
                        if (spawnerSheet.Cells[5, col].Value.ToString() == "SpawnLayerCuid")
                            spawnerColValue3 = col;
                        if (spawnerSheet.Cells[5, col].Value.ToString() == "LocationX_cm")
                            spawnerColValue4 = col;
                        if (spawnerSheet.Cells[5, col].Value.ToString() == "LocationY_cm")
                            spawnerColValue5 = col;
                        if (spawnerSheet.Cells[5, col].Value.ToString() == "LocationZ_cm")
                            spawnerColValue6 = col;
                    }

                    int spawnCandidateColValue1 = 2, spawnCandidateColValue2 = 3, spawnCandidateColValue3 = 4;
                    for (int col = 0; ; col++)
                    {
                        if (spawnCandidateSheet.Cells[5, col].Value == null || spawnCandidateSheet.Cells[5, col].Value.ToString() == "")
                            break;

                        if (spawnCandidateSheet.Cells[5, col].Value.ToString() == "NpcSpawnerCuid")
                            spawnCandidateColValue1 = col;
                        if (spawnCandidateSheet.Cells[5, col].Value.ToString() == "SpawnPointIndex")
                            spawnCandidateColValue2 = col;
                        if (spawnCandidateSheet.Cells[5, col].Value.ToString() == "NpcCuid")
                            spawnCandidateColValue3 = col;
                    }

                    // 스포너 id 기반 Npc Cuid 탐색용으로, spawnCandidate 관련 데이터 우선 구조화
                    Dictionary<string, string> spawnCandidateData = new Dictionary<string, string>();
                    for (int row = 6; ; row++)
                    {
                        if (spawnCandidateSheet.Cells[row, spawnCandidateColValue1].Value == null || spawnCandidateSheet.Cells[row, spawnCandidateColValue1].Value.ToString() == "")
                            break;

                        // SpawnPoint > 1 인 경우, 같은 SpawnerID로 여러 NpcCuid가 연결될 수 있어서 해당 내용 보강
                        // 이후 NpcCuid 기반 탐색 시, Value에서 내부 문자열 Find 하는 방식 (Value를 '\n'으로 split하는 것도 방법일듯)
                        if(int.Parse(spawnCandidateSheet.Cells[row, spawnCandidateColValue2].Value.ToString()) == 1)
                        {
                            spawnCandidateData.Add(spawnCandidateSheet.Cells[row, spawnCandidateColValue1].Value.ToString(), spawnCandidateSheet.Cells[row, spawnCandidateColValue3].Value.ToString());
                        }
                        else
                        {
                            spawnCandidateData[spawnCandidateSheet.Cells[row, spawnCandidateColValue1].Value.ToString()] = spawnCandidateData[spawnCandidateSheet.Cells[row, spawnCandidateColValue1].Value.ToString()] + "/n" + spawnCandidateSheet.Cells[row, spawnCandidateColValue3].Value.ToString();
                        }
                    }

                    // Spawner 데이터 구조화
                    for (int row = 6; ; row++)
                    {
                        if (spawnerSheet.Cells[row, spawnerColValue1].Value == null || spawnerSheet.Cells[row, spawnerColValue1].Value.ToString() == "")
                            break;

                        SpawnData spawnData = new SpawnData();
                        spawnData.spawnID = spawnerSheet.Cells[row, spawnerColValue1].Value.ToString();
                        spawnData.isSpawnOnStartup = bool.Parse(spawnerSheet.Cells[row, spawnerColValue1].Value.ToString());
                        spawnData.spawnLayerID = spawnerSheet.Cells[row, spawnerColValue3].Value.ToString();

                        float[] pos = {float.Parse(spawnerSheet.Cells[row, spawnerColValue4].Value.ToString()),
                                        float.Parse(spawnerSheet.Cells[row, spawnerColValue5].Value.ToString()),
                                        float.Parse(spawnerSheet.Cells[row, spawnerColValue6].Value.ToString())};

                        spawnData.postion = pos;

                        List<string> npcList = spawnCandidateData[spawnerSheet.Cells[row, spawnerColValue1].Value.ToString()].Split('\n').ToList();
                        spawnData.npcList = npcList;

                        spawnDatas.Add(spawnData);
                    }
                }
            }

            return spawnDatas;
        }
        void FindInstanceFieldSpawnData(string _npcSpawnData)
        {
            List<string> spawnLayerDataInInstanceField = GetInstanceFieldSpawnLayer();

            if(spawnLayerDataInInstanceField == null)
                return;

            foreach (var questData in questDataList)
            {

            }
        }
        
        List<string> GetInstanceFieldSpawnLayer()
        {
            List<string> spawnLayerDataInInstanceField;

            List<string> instanceFieldFiles = DataHandler.DataDictHandler.DataDict["InstanceField"].files;

            if (instanceFieldFiles == null)
                return null;

            spawnLayerDataInInstanceField = new List<string>();

            foreach (string file in instanceFieldFiles)
            {
                using(ExcelPackage instanceFieldFile = new ExcelPackage(file))
                {
                    ExcelWorksheet instanceFieldSheet = instanceFieldFile.Workbook.Worksheets.Single(s => s.Name.Equals("InstanceField"));

                    int colValue = 6;

                    for (int col = 2; ; col++)
                    {
                        if (instanceFieldSheet.Cells[5, col].Value == null || instanceFieldSheet.Cells[5, col].Value.ToString() == "")
                            break;

                        if (instanceFieldSheet.Cells[5, col].Value.ToString() == "SpawnLayerCuids")
                        {
                            colValue = col;
                            break;
                        }
                    }

                    for(int row = 6; ; row++)
                    {
                        if (instanceFieldSheet.Cells[row, 2].Value == null || instanceFieldSheet.Cells[row, 2].Value.ToString() == "")
                            break;

                        spawnLayerDataInInstanceField.AddRange(instanceFieldSheet.Cells[row, colValue].Value.ToString().Split('\n'));
                    }
                }
            }

            return spawnLayerDataInInstanceField;
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
