using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using OfficeOpenXml;

namespace QuestManager
{
    public class QuestManager
    {

        #region Data Structure
        struct QuestData
        {
            internal string questID { get; set; }
            internal bool cancelable { get; set; }
            internal string acceptObjectType { get; set; }
            internal string[] acceptObjectIds { get; set; } // nullable
        }
        struct SpawnData
        {
            internal string spawnID { get; set; }
            internal string spawnLayerID { get; set; } // nullable
            internal float[] postion { get; set; }
            internal List<string> npcList { get; set; }
            internal bool isSpawnOnStartup { get; set; }
        }
        struct NpcData
        {
            internal string npcID { get; set; }
            internal bool showHeadInfo { get; set; }
            internal string faction { get; set; }
            internal string[] questList { get; set; } // nullable
            internal bool hideNpc { get; set; }
            internal bool isCapturable { get; set; }
        }
        struct InstanceFieldData
        {
            internal string instanceFieldID { get; set; }
            internal string[] spawnLayerIds { get; set; } // nullable
        }
        struct SpawnLayerData
        {
            internal string spawnLayerID { get; set; }
            internal bool isActivateOnStartup{ get; set; }
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

        public static List<CannotReacceptableQuestData> cannotReacceptableQuestDataList;

        public void FindQuestReacceptable()
        {
            /* 
             * QuestTable.Quest 시트 → AcceptObjectType, AcceptObjectCuid List
             * QuestCuid 마다 저장
             * 나머지 데이터들은, 파일 한 번씩 열고 그냥 List<string> 으로 일괄 저장 (IO 최소로 하고, 편하게 메모리를 더 많이 쓰자)
             * 컬럼 명은 별도 txt 파일에서 긁어올 수 있게 (이후 추가)
             */
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            List<QuestData> questDatas = ReadQuestData();
            List<SpawnData> spawnData = ReadSpawnData();
            List<NpcData> npcsData = ReadNpcData();
            List<InstanceFieldData> instanceFieldsData = ReadInstanceFieldData();
            //List<SpawnLayerData> spawnLayerData = ReadSpawnLayerData();

            // 에러 판정
        }

        void Legecy()
        {
            List<QuestData> questDataList = new List<QuestData>();
            cannotReacceptableQuestDataList = new List<CannotReacceptableQuestData>();

            List<string> questFiles = DataHandler.DataDictHandler.DataDict["Quest"].files;
            List<string> npcFiles = DataHandler.DataDictHandler.DataDict["Npc"].files;

            if (questFiles == null)
                return;
            if (npcFiles == null)
                return;

            int colValue1, colValue2, colValue3, colValue4;

            foreach (string file in questFiles)
            {
                colValue1 = colValue2 = colValue3 = colValue4 = 0;

                using (ExcelPackage questFile = new ExcelPackage(file))
                {
                    ExcelWorksheet questSheet = questFile.Workbook.Worksheets.Single(s => s.Name.Equals("Quest"));

                    // 필요한 컬럼 Idx 값 세팅
                    for (int col = 2; ; col++)
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
                    for (int row = 6; ; row++)
                    {
                        if (questSheet.Cells[row, colValue1].Value == null || questSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        // 취소 불가능이면 판별할 필요 없음
                        if (questSheet.Cells[row, colValue2].Value.ToString().ToLower().Equals("true"))
                            continue;

                        // Npc 대상 수락이 아닌 경우는 별도 판단:: 매우 특이 케이스라, 자동화 필요 없음
                        if (!questSheet.Cells[row, colValue3].Value.ToString().ToLower().Equals("npc"))
                            continue;

                        if (questSheet.Cells[row, colValue4].Value == null || questSheet.Cells[row, colValue4].Value.ToString() == "")
                        {
                            // 재수락 불가 : 수락 NPC 없음
                            CannotReacceptableQuestData cannotQuset = new CannotReacceptableQuestData(questSheet.Cells[row, colValue1].Value.ToString(), CannotReacceptableCode.NotHaveAcceptNpc);
                            cannotReacceptableQuestDataList.Add(cannotQuset);
                        }
                        else
                        {
                            QuestData newQuestData = new QuestData();

                            //newQuestData = new QuestData(questSheet.Cells[row, colValue1].Value.ToString(), questSheet.Cells[row, colValue4].Value.ToString().Split('\n'));

                            questDataList.Add(newQuestData);
                        }
                    }
                }
            }

            // SpawnerID, IsSpawnOnStartup, SpawnerNpcCuidList, SpawneLayerID, SpawnerPosition
            List<SpawnData> spawnDatas = ReadSpawnData();

            // NpcData Setting: QuestList, ShowHeadInfo, Faction, IsCapturable
        }

        #region ReadData
        List<QuestData> ReadQuestData()
        {
            List<string> questFiles = DataHandler.DataDictHandler.DataDict["Quest"].files;
            if (questFiles == null)
                return null;

            List<QuestData> questDatas = new List<QuestData>();
            int colValue1, colValue2, colValue3, colValue4;
            colValue1 = colValue2 = colValue3 = colValue4 = 0;

            foreach (string file in questFiles)
            {
                using (ExcelPackage questFile = new ExcelPackage(file))
                {
                    ExcelWorksheet questSheet = questFile.Workbook.Worksheets.Single(s => s.Name.Equals("Quest"));

                    // 필요한 컬럼 Idx 값 세팅
                    if (file.Equals(questFiles.First()))
                    {
                        for (int col = 2; ; col++)
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
                    }

                    // 데이터 Read
                    for (int row = 6; ; row++)
                    {
                        if (questSheet.Cells[row, colValue1].Value == null || questSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        QuestData questData = new QuestData();

                        questData.questID = questSheet.Cells[row, colValue1].Value.ToString();

                        if(questSheet.Cells[row, colValue2].Value != null)
                            questData.cancelable = bool.Parse(questSheet.Cells[row, colValue2].Value.ToString());
                        else
                            questData.cancelable = false;

                        questData.acceptObjectType = questSheet.Cells[row, colValue3].Value.ToString();

                        if (questSheet.Cells[row, colValue4].Value != null)
                            questData.acceptObjectIds = questSheet.Cells[row, colValue4].Value.ToString().Split('\n');
                        else
                            questData.acceptObjectIds = null;

                        questDatas.Add(questData);
                    }
                }
            }

            return questDatas;
        }
        List<SpawnData> ReadSpawnData()
        {
            List<string> spawnFiles = DataHandler.DataDictHandler.DataDict["NpcSpawner"].files;
            
            if (spawnFiles == null || spawnFiles.Count == 0)
                return null;

            List<SpawnData> spawnDatas = new List<SpawnData>();

            int spawnerColValue1 = 2, spawnerColValue2 = 8, spawnerColValue3 = 12, spawnerColValue4 = 13, spawnerColValue5 = 14, spawnerColValue6 = 15;
            int spawnCandidateColValue1 = 2, spawnCandidateColValue2 = 3, spawnCandidateColValue3 = 4;

            foreach (string file in spawnFiles)
            {
                using(ExcelPackage spawnFile = new ExcelPackage(file))
                {
                    ExcelWorksheet spawnerSheet = spawnFile.Workbook.Worksheets.Single(s => s.Name.Equals("NpcSpawner"));
                    ExcelWorksheet spawnCandidateSheet = spawnFile.Workbook.Worksheets.Single(s => s.Name.Equals("NpcSpawnCandidate"));

                    // 필요한 Col Idx 세팅
                    if (file.Equals(spawnFiles.First()))
                    {
                        for (int col = 2; ; col++)
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

                        for (int col = 2; ; col++)
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
                        spawnData.isSpawnOnStartup = bool.Parse(spawnerSheet.Cells[row, spawnerColValue2].Value.ToString());

                        if (spawnerSheet.Cells[row, spawnerColValue3].Value != null)
                            spawnData.spawnLayerID = spawnerSheet.Cells[row, spawnerColValue3].Value.ToString();
                        else
                            spawnData.spawnLayerID = null;

                        float[] pos = {float.Parse(spawnerSheet.Cells[row, spawnerColValue4].Value.ToString()),
                                        float.Parse(spawnerSheet.Cells[row, spawnerColValue5].Value.ToString()),
                                        float.Parse(spawnerSheet.Cells[row, spawnerColValue6].Value.ToString())};

                        spawnData.postion = pos;

                        // spawnCandidateData의 Value 값을 Split하면 List 나옴
                        List<string> npcList = spawnCandidateData[spawnerSheet.Cells[row, spawnerColValue1].Value.ToString()].Split('\n').ToList();
                        spawnData.npcList = npcList;


                        spawnDatas.Add(spawnData);
                    }
                }
            }

            return spawnDatas;
        }
        List<NpcData> ReadNpcData()
        {
            List<string> npcFiles = DataHandler.DataDictHandler.DataDict["Npc"].files;
            if (npcFiles == null)
                return null;

            List<NpcData> npcDatas = new List<NpcData>();
            int colValue1, colValue2, colValue3, colValue4, colValue5, colValue6;
            colValue1 = colValue2 = colValue3 = colValue4 = colValue5 = colValue6 = 0;

            foreach (string file in npcFiles)
            {
                using (ExcelPackage npcFile = new ExcelPackage(file))
                {
                    ExcelWorksheet npcSheet = npcFile.Workbook.Worksheets.Single(s => s.Name.Equals("Npc"));

                    // 필요한 컬럼 Idx 값 세팅
                    if (file.Equals(npcFiles.First()))
                    {
                        for (int col = 2; ; col++)
                        {
                            if (npcSheet.Cells[5, col].Value == null || npcSheet.Cells[5, col].Value.ToString() == "")
                                break;

                            if (npcSheet.Cells[5, col].Value.ToString() == "Cuid")
                                colValue1 = col;
                            if (npcSheet.Cells[5, col].Value.ToString() == "ShowHeadInfo")
                                colValue2 = col;
                            if (npcSheet.Cells[5, col].Value.ToString() == "QuestList")
                                colValue3 = col;
                            if (npcSheet.Cells[5, col].Value.ToString() == "HideNpc")
                                colValue4 = col;
                            if (npcSheet.Cells[5, col].Value.ToString() == "Faction")
                                colValue5 = col;
                            if (npcSheet.Cells[5, col].Value.ToString() == "IsCapturable")
                                colValue6 = col;
                        }
                    }

                    // 데이터 Read
                    for (int row = 6; ; row++)
                    {
                        if (npcSheet.Cells[row, colValue1].Value == null || npcSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        NpcData npcData = new NpcData();

                        npcData.npcID = npcSheet.Cells[row, colValue1].Value.ToString();

                        if(npcSheet.Cells[row, colValue2].Value != null)
                            npcData.showHeadInfo = bool.Parse(npcSheet.Cells[row, colValue2].Value.ToString());
                        else
                            npcData.showHeadInfo = false;

                        if (npcSheet.Cells[row, colValue3].Value != null)
                            npcData.questList = npcSheet.Cells[row, colValue3].Value.ToString().Split('\n');
                        else
                            npcData.questList = null;

                        if(npcSheet.Cells[row, colValue4].Value != null)
                            npcData.hideNpc = bool.Parse(npcSheet.Cells[row, colValue4].Value.ToString());
                        else
                            npcData.hideNpc = false;

                        npcData.faction = npcSheet.Cells[row, colValue5].Value.ToString();

                        if(npcSheet.Cells[row, colValue6].Value != null)
                            npcData.isCapturable = bool.Parse(npcSheet.Cells[row, colValue6].Value.ToString());
                        else
                            npcData.isCapturable = false;

                        npcDatas.Add(npcData);
                    }
                }
            }

            return npcDatas;
        }
        List<InstanceFieldData> ReadInstanceFieldData()
        {
            List<string> instanceFieldFiles = DataHandler.DataDictHandler.DataDict["InstanceField"].files;
            if (instanceFieldFiles == null)
                return null;

            List<InstanceFieldData> instanceFieldDatas = new List<InstanceFieldData>();
            int colValue1, colValue2;
            colValue1 = colValue2 = 0;

            foreach (string file in instanceFieldFiles)
            {
                using (ExcelPackage instanceFieldFile = new ExcelPackage(file))
                {
                    ExcelWorksheet instanceFieldSheet = instanceFieldFile.Workbook.Worksheets.Single(s => s.Name.Equals("InstanceField"));

                    // 필요한 컬럼 Idx 값 세팅
                    if (file.Equals(instanceFieldFiles.First()))
                    {
                        for (int col = 2; ; col++)
                        {
                            if (instanceFieldSheet.Cells[5, col].Value == null || instanceFieldSheet.Cells[5, col].Value.ToString() == "")
                                break;

                            if (instanceFieldSheet.Cells[5, col].Value.ToString() == "Cuid")
                                colValue1 = col;
                            if (instanceFieldSheet.Cells[5, col].Value.ToString() == "SpawnLayerCuids")
                                colValue2 = col;
                        }
                    }

                    // 데이터 Read
                    for (int row = 6; ; row++)
                    {
                        if (instanceFieldSheet.Cells[row, colValue1].Value == null || instanceFieldSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        InstanceFieldData instanceFieldData = new InstanceFieldData();

                        instanceFieldData.instanceFieldID = instanceFieldSheet.Cells[row, colValue1].Value.ToString();

                        if(instanceFieldSheet.Cells[row, colValue2].Value != null)
                            instanceFieldData.spawnLayerIds = instanceFieldSheet.Cells[row, colValue2].Value.ToString().Split('\n');
                        else
                            instanceFieldData.spawnLayerIds = null;
                        ;
                        instanceFieldDatas.Add(instanceFieldData);
                    }
                }
            }

            return instanceFieldDatas;
        }
        List<SpawnLayerData> ReadSpawnLayerData()
        {
            List<string> spawnLayerFiles = DataHandler.DataDictHandler.DataDict["SpawnLayer"].files;
            if (spawnLayerFiles == null)
                return null;

            List<SpawnLayerData> spawnLayerDatas = new List<SpawnLayerData>();
            int colValue1, colValue2;
            colValue1 = colValue2 = 0;

            foreach (string file in spawnLayerFiles)
            {
                using (ExcelPackage spawnLayerFile = new ExcelPackage(file))
                {
                    ExcelWorksheet spawnLayerSheet = spawnLayerFile.Workbook.Worksheets.Single(s => s.Name.Equals("SpawnLayer"));

                    // 필요한 컬럼 Idx 값 세팅
                    if (file.Equals(spawnLayerFiles.First()))
                    {
                        for (int col = 2; ; col++)
                        {
                            if (spawnLayerSheet.Cells[5, col].Value == null || spawnLayerSheet.Cells[5, col].Value.ToString() == "")
                                break;

                            if (spawnLayerSheet.Cells[5, col].Value.ToString() == "Cuid")
                                colValue1 = col;
                            if (spawnLayerSheet.Cells[5, col].Value.ToString() == "IsActivateOnStartup")
                                colValue2 = col;
                        }
                    }

                    // 데이터 Read
                    for (int row = 6; ; row++)
                    {
                        if (spawnLayerSheet.Cells[row, colValue1].Value == null || spawnLayerSheet.Cells[row, colValue1].Value.ToString() == "")
                            break;

                        SpawnLayerData spawnLayerData = new SpawnLayerData();

                        spawnLayerData.spawnLayerID = spawnLayerSheet.Cells[row, colValue1].Value.ToString();
                        spawnLayerData.isActivateOnStartup = bool.Parse(spawnLayerSheet.Cells[row, colValue2].Value.ToString());

                        spawnLayerDatas.Add(spawnLayerData);
                    }
                }
            }

            return spawnLayerDatas;
        }
        #endregion
    }
}
