using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
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
            internal bool nonCancelable { get; set; }
            internal string acceptObjectType { get; set; }
            internal string[] acceptObjectIds { get; set; } // nullable
            internal string questType { get; set; }
            internal string tableName { get; set; }
        }
        struct SpawnData
        {
            internal string spawnID { get; set; }
            internal string spawnLayerID { get; set; } // nullable
            internal float[] postion { get; set; }
            internal List<string> npcList { get; set; }
            internal bool isSpawnOnStartup { get; set; }
            internal string tableName { get; set; }
        }
        struct NpcData
        {
            internal string npcID { get; set; }
            internal bool showHeadInfo { get; set; }
            internal string faction { get; set; }
            internal string[] questList { get; set; } // nullable
            internal bool hideNpc { get; set; }
            internal bool isCapturable { get; set; }
            internal string tableName { get; set; }
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

        public enum ReacceptableWarningCode
        {
            NotNpcAccpetTypeInGeneral, // General 퀘스트인데 Npc 수락이 아님
            NotHaveAcceptNpc, // 수락할 Npc가 없음
            DesyncNpcQuestList, // 수락 Npc의 QuestList에 해당 퀘스트가 없음
            FalseInShowHeadInfo, // 수락 Npc에게서 수락 아이콘이 보이지 않음
            AcceptNpcCanDeath, // 수락 Npc가 죽을 수 있음
            AcceptNpcCanCombatWithFieldMonster, // 수락 Npc가 몬스터와 교전할 수 있음
            AcceptNpcCanShowInFieldPermanently, // 수락 Npc가 필드에 상시 보일 수 있음
            NotExistAcceptNpcSpawnData, // 수락 Npc에 대한 스폰 데이터가 없음
            AcceptNpcNotSpawnInPermanentField, // 수락 Npc가 스폰되지 않음
            AcceptNpcNotSpawnInPermanentField_SpawnLayer, // 수락 Npc가 스폰되지 않음 (스폰레이어)
            LongDistanceBetweenAcceptNpcInInstanceAndPermanentField // 인스턴스 <-> 필드 Npc 사이가 너무 떨어져 있음 (Config 값 참조)
        }
        public struct ReacceptableWarningQuestData
        {
            internal ReacceptableWarningQuestData(string _questID, string _questTableName, ReacceptableWarningCode _code)
            {
                questID = _questID;
                questTableName = _questTableName;

                npcID = null;
                npcTableName = null;
                
                spawnID = null;
                spawnTableName = null;

                code = _code;
            }
            internal ReacceptableWarningQuestData(string _questID, string _questTableName, string _npcID, string _npcTableName, ReacceptableWarningCode _code)
            {
                questID = _questID;
                questTableName = _questTableName;

                npcID = _npcID;
                npcTableName = _npcTableName;

                spawnID= null;
                spawnTableName = null;

                code = _code;
            }
            internal ReacceptableWarningQuestData(string _questID, string _questTableName, string _npcID, string _npcTableName, string _spawnID, string _spawnTableName, ReacceptableWarningCode _code)
            {
                questID = _questID;
                questTableName = _questTableName;

                npcID = _npcID;
                npcTableName = _npcTableName;

                spawnID = _spawnID;
                spawnTableName = _spawnTableName;

                code = _code;
            }

            public string questID { get; }
            public string questTableName { get; }

            public string npcID { get; }
            public string npcTableName { get; }

            public string spawnID { get; }
            public string spawnTableName { get; }

            public ReacceptableWarningCode code { get; }
        }
        #endregion

        public static List<ReacceptableWarningQuestData> reacceptableWarningQuestDataList;

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
            List<SpawnData> spawnDatas = ReadSpawnData();
            List<NpcData> npcsDatas = ReadNpcData();
            List<InstanceFieldData> instanceFieldsData = ReadInstanceFieldData();
            //List<SpawnLayerData> spawnLayerData = ReadSpawnLayerData();

            reacceptableWarningQuestDataList = new List<ReacceptableWarningQuestData>();

            // 에러 판정
            foreach (var questData in questDatas)
            {
                if (questData.nonCancelable == true)
                    continue;

                if(questData.acceptObjectType != "Npc")
                {
                    if (questData.questType != "General")
                        continue;
                    else
                        AddReacceptableWarningQuest(questData.questID, questData.tableName, ReacceptableWarningCode.NotNpcAccpetTypeInGeneral);
                }
                else
                {
                    if (questData.acceptObjectIds == null || questData.acceptObjectIds.Length == 0)
                        AddReacceptableWarningQuest(questData.questID, questData.tableName, ReacceptableWarningCode.NotHaveAcceptNpc);
                    else
                    {
                        foreach (string npcId in questData.acceptObjectIds)
                        {
                            NpcData npcData = npcsDatas.Find(e => e.npcID.Equals(npcId));

                            if (npcData.questList == null || !npcData.questList.Contains(questData.questID))
                                AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, ReacceptableWarningCode.DesyncNpcQuestList);
                            
                            if(npcData.showHeadInfo == false)
                                AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, ReacceptableWarningCode.FalseInShowHeadInfo);

                            if(npcData.faction != "NonAttackable" && npcData.isCapturable == true)
                                AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, ReacceptableWarningCode.AcceptNpcCanDeath);

                            if (npcData.faction == "QuestHelper" && npcData.isCapturable == true)
                                AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, ReacceptableWarningCode.AcceptNpcCanCombatWithFieldMonster);

                            if (npcData.hideNpc == false)
                                AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, ReacceptableWarningCode.AcceptNpcCanShowInFieldPermanently);


                            // spawn Data
                            List<SpawnData> spawnDatas_HasNpcId = spawnDatas.FindAll(e => e.npcList.Find(s => s.Equals(npcId)) != null);

                            if(spawnDatas_HasNpcId == null || spawnDatas_HasNpcId.Count == 0)
                            {
                                AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, ReacceptableWarningCode.NotExistAcceptNpcSpawnData);
                            }
                            else
                            {
                                foreach(SpawnData spawnData in spawnDatas_HasNpcId)
                                {
                                    if (spawnData.isSpawnOnStartup == false)
                                        AddReacceptableWarningQuest(questData.questID, questData.tableName, npcId, npcData.tableName, spawnData.spawnID, spawnData.tableName, ReacceptableWarningCode.AcceptNpcNotSpawnInPermanentField);

                                    if(spawnData.spawnLayerID != null)
                                    {
                                        // spawnLayer 테이블 가져와서, 데이터 구조화 한 다음 에러 구성...
                                        // 스폰 되는가? 체크 -> 인스턴스 스폰레이어면 체크 X
                                        // 인스턴스 스폰레이어면 => 위치 체크해야 하는데...
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 예외 퀘스트, Npc는 에러 판정 대상에서 제외할 수 있음 (빌드 폴더 내에 Config 항목 추가하기)
            foreach(var reacceptableWarningQuest in reacceptableWarningQuestDataList)
            {

            }
        }
        void AddReacceptableWarningQuest(string _questID, string _questTableName, ReacceptableWarningCode _code)
        {
            ReacceptableWarningQuestData questData = new ReacceptableWarningQuestData(_questID, _questTableName, _code);

            reacceptableWarningQuestDataList.Add(questData);
        }
        void AddReacceptableWarningQuest(string _questID, string _questTableName, string _npcID, string _npcTableName, ReacceptableWarningCode _code)
        {
            ReacceptableWarningQuestData questData = new ReacceptableWarningQuestData(_questID, _questTableName, _npcID, _npcTableName, _code);

            reacceptableWarningQuestDataList.Add(questData);
        }
        void AddReacceptableWarningQuest(string _questID, string _questTableName, string _npcID, string _npcTableName, string _spawnID, string _spawnTableName, ReacceptableWarningCode _code)
        {
            ReacceptableWarningQuestData questData = new ReacceptableWarningQuestData(_questID, _questTableName, _npcID, _npcTableName, _spawnID, _spawnTableName, _code);

            reacceptableWarningQuestDataList.Add(questData);
        }

        #region ReadData
        List<QuestData> ReadQuestData()
        {
            List<string> questFiles = DataHandler.DataDictHandler.DataDict["Quest"].files;
            if (questFiles == null)
                return null;

            List<QuestData> questDatas = new List<QuestData>();
            int colValue1, colValue2, colValue3, colValue4, colValue5;
            colValue1 = colValue2 = colValue3 = colValue4 = colValue5 = 0;
            string fileName;

            foreach (string file in questFiles)
            {
                fileName = System.IO.Path.GetFileName(file);

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
                            if (questSheet.Cells[5, col].Value.ToString() == "Type")
                                colValue5 = col;
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
                            questData.nonCancelable = bool.Parse(questSheet.Cells[row, colValue2].Value.ToString());
                        else
                            questData.nonCancelable = false;

                        questData.acceptObjectType = questSheet.Cells[row, colValue3].Value.ToString();

                        if (questSheet.Cells[row, colValue4].Value != null)
                            questData.acceptObjectIds = questSheet.Cells[row, colValue4].Value.ToString().Split('\n');
                        else
                            questData.acceptObjectIds = null;

                        questData.questType = questSheet.Cells[row, colValue5].Value.ToString();

                        questData.tableName = fileName;


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
            string fileName;

            foreach (string file in spawnFiles)
            {
                fileName = System.IO.Path.GetFileName(file);

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

                        spawnData.tableName = fileName;


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
            string fileName;

            foreach (string file in npcFiles)
            {
                fileName = System.IO.Path.GetFileName(file);

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

                        npcData.tableName = fileName;


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
