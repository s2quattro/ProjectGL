using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CityStage;

namespace CityStage
{
    public class BlockManager : MonoSingletonBase<BlockManager>
    {
        private LocalProperties localProperties;
        public GameObject playerCharacter;
        public Transform centerPos;   // 블럭집합의 정중앙 위치
        public GameObject block;

        [Header("Option")]
        [SerializeField] private int blockHorNum; // 블럭 가로 개수
        [SerializeField] private int blockVerNum; // 블럭 세로 개수
        [SerializeField] private int blockEntitySize;   // 블럭 개체 크기
        [SerializeField] private bool gizmoOn;

        private BlockEntity blockEntity;    // 블록 개체를 임시로 담아놓을 곳
        private EntityBase[] objectEntities;  // 블록 개체 스크립트 컴포넌트들을 모두 불러와 담을 배열
        private Dictionary<int, BlockEntity> blockNumToEntityDic = new Dictionary<int, BlockEntity>();   // 블록 번호를 key로 블록 개체 스크립트를 저장
        private Dictionary<Vector2, GameObject> blockVectorToObjectDic = new Dictionary<Vector2, GameObject>();
        private List<int> blockNumList = new List<int>();           // 모든 블록의 번호를 list로 관리    
        private List<Vector2> blockPosList = new List<Vector2>();   // 모든 블록의 위치를 list로 관리

        // 최종적으로 넘겨줘야할 리스트이며 플레이어와 가까운 주변블럭들의 번호를 저장하는 리스트
        [HideInInspector] public List<int> nextNearBlockNumList = new List<int>();

        private int prevBlockNum = -1;  // 이전에 가장 가까웠던 블록의 번호    
        private float maxDistOfBlocks;
        
        private void Start()
        {
            localProperties = LinkContainer.Instance.localProperties;

            prevBlockNum = -1;
        }

        void Update()
        {
            CheckNearBlock();
        }

        public void SetUp()
        {
            // 블럭 자동생성
            // 블럭 생성후 사이즈에 맞게 기즈모 생성
            // 오브젝트 매니저로부터 List<GameObject>를 받으면 해당 GameObject의 vector2값을 양자화해서 딕셔너리의 key값으로 바로 block을 얻어서 해당 block의 자식으로 GameObject를 넣는다.              
			
            maxDistOfBlocks = blockEntitySize * 1.7f;   // 블럭 사이즈에따라 최대거리 설정  

            Vector2 blockInstantiatePos;

            int blockNum = 1;

            for (int i = 0; i < blockVerNum; i++)
            {
                for (int j = 0; j < blockHorNum; j++)
                {
                    GameObject instantiateBlock = Instantiate(block);

                    // 좌상단부터 블럭 생성

                    blockInstantiatePos.x = ((int)centerPos.position.x / blockEntitySize) * blockEntitySize - (blockHorNum - 1 - 2 * j) * (blockEntitySize / 2f);
                    blockInstantiatePos.y = ((int)centerPos.position.y / blockEntitySize) * blockEntitySize + (blockVerNum - 1 - 2 * i) * (blockEntitySize / 2f);

                    if (blockHorNum % 2 == 0)
                    {
                        blockInstantiatePos.x -= blockEntitySize / 2f;
                    }

                    if (blockVerNum % 2 == 0)
                    {
                        blockInstantiatePos.y -= blockEntitySize / 2f;
                    }

                    instantiateBlock.GetComponent<BlockEntity>().blockNum = blockNum;
                    instantiateBlock.transform.position = blockInstantiatePos;
                    instantiateBlock.transform.SetParent(gameObject.transform);

                    blockNum++;
                }
            }

            SetBlockDic();  // 블럭 생성후 초기에 한번 모든 블록을 블록 딕셔너리에 등록해줌

            // 각각의 블럭을 블러와서 블록을 딕셔너리에 등록
            for (int i = 0; i < transform.childCount; i++)
            {
                blockEntity = transform.GetChild(i).GetComponent<BlockEntity>();    // 각각의 블록을 불러옴

                Vector2 v;
                v.x = ((int)blockEntity.transform.position.x / blockEntitySize) * blockEntitySize;
                v.y = ((int)blockEntity.transform.position.y / blockEntitySize) * blockEntitySize;

                blockVectorToObjectDic.Add(v, blockEntity.gameObject);

                blockNumToEntityDic.Add(blockEntity.blockNum, blockEntity);
                blockPosList.Add(blockEntity.transform.position);
                blockNumList.Add(blockEntity.blockNum);
            }

            List<int> closeBlockNumList = new List<int>();

            // 최대 블럭범위(15)보다 짧은 거리 범위내에서 해당 블럭과 가장 가까운 순으로 최대 8개까지 블럭을 찾으면 next block 배열에 저장시키고 다음 배열로 넘어간다.

            for (int i = 0; i < blockNumList.Count; i++)
            {
                float mostCloseBlockDist;
                int mostCloseBlockIndex;
                float distBlockToBlock;

                blockEntity = blockNumToEntityDic[blockNumList[i]];

                closeBlockNumList.Clear();

                for (int k = 0; k < 8; k++) // 최대 8개의 가장 가까운 블럭을 찾는다
                {
                    mostCloseBlockDist = maxDistOfBlocks;
                    mostCloseBlockIndex = -1;

                    for (int j = 0; j < blockNumList.Count; j++)
                    {
                        distBlockToBlock = Vector2.Distance(blockPosList[i], blockPosList[j]);

                        if (!closeBlockNumList.Contains(blockNumList[j])    // 이미 등록돼있는 블럭인지 검사
                            && (i != j)                                     // 비교하는 블럭이 같은블럭이 아닌게 맞는지 검사
                            && (distBlockToBlock < maxDistOfBlocks)         // 블럭과의 거리가 최대 거리 안에 있는지 검사
                            && (distBlockToBlock < mostCloseBlockDist))       // 현재 블럭이 최단 거리를 갱신했는지 검사
                        {
                            mostCloseBlockDist = distBlockToBlock;
                            mostCloseBlockIndex = j;
                        }
                    }

                    if (mostCloseBlockIndex != -1)
                    {
                        closeBlockNumList.Add(blockNumList[mostCloseBlockIndex]);
                    }
                }

                for (int a = 0; a < closeBlockNumList.Count; a++)
                {
                    blockNumToEntityDic[blockNumList[i]].nextBlock[a] = closeBlockNumList[a];
                }
            }

            for (int i = 0; i < blockNumList.Count; i++)
            {
                blockNumToEntityDic[blockNumList[i]].gameObject.SetActive(false);
            }
        }

        public bool IsEntityInBlock(EntityBase entity)
        {
            CheckNearBlock();

            int entityBlockNum = GetBlockEntityOnObjectPos(entity.transform.position).blockNum;

            foreach (int blockNum in nextNearBlockNumList)
            {
                if(blockNum == entityBlockNum)
                {
                    return true;
                }
            }

            return false;
        }

        public void SetObjectParent(List<EntityBase> objectList)
        {
            // 블록안의 오브젝트들을 블록안에 자식으로 넣는 과정
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].transform.SetParent(GetBlockEntityOnObjectPos(objectList[i].transform.position).transform);
            }

            SetBlockDic();
        }

        private void SetBlockDic()
        {
            List<EntityBase> EntityBaseList = new List<EntityBase>();

            localProperties.blockDic.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                blockEntity = transform.GetChild(i).GetComponent<BlockEntity>();

                objectEntities = blockEntity.GetComponentsInChildren<EntityBase>();   // 해당 블록의 오브젝트 개체 스크립트들을 불러옴

                EntityBaseList.Clear();   // 추가하기전 비움

                for (int j = 0; j < objectEntities.Length; j++)  // 리스트에 모든 오브젝트 개체 스크립트들을 등록
                {
                    EntityBaseList.Add(objectEntities[j]);
                }

                localProperties.blockDic.Add(blockEntity.blockNum, new List<EntityBase>(EntityBaseList));   // 딕셔너리에 블록번호를 key로 리스트 등록
            }
        }

        public void AddObjectToBlock(EntityBase entity)
        {
            BlockEntity block;

            if (block = GetBlockEntityOnObjectPos(entity.transform.position))
            {
                localProperties.blockDic[block.blockNum].Add(entity);

                entity.transform.SetParent(block.transform);
            }
            else
            {
                Debug.Log("AddObjectToBlock Failed");
            }
        }

        public void DeleteObjectToBlock(EntityBase entity)
        {
            BlockEntity block;

            if (block = GetBlockEntityOnObjectPos(entity.transform.position))
            {
                localProperties.blockDic[block.blockNum].Remove(entity);
            }
            else
            {
                Debug.Log("DeleteObjectToBlock Failed");
            }
        }

        private BlockEntity GetBlockEntityOnObjectPos(Vector2 objectPos)
        {
            Vector2 v;

            if (objectPos.x >= 0f)
            {
                v.x = ((int)objectPos.x / blockEntitySize) * blockEntitySize;
            }
            else
            {
                v.x = (((int)objectPos.x - blockEntitySize) / blockEntitySize) * blockEntitySize;
            }

            if (objectPos.y >= 0f)
            {
                v.y = ((int)objectPos.y / blockEntitySize) * blockEntitySize;
            }
            else
            {
                v.y = (((int)objectPos.y - blockEntitySize) / blockEntitySize) * blockEntitySize;
            }

            if (blockVectorToObjectDic.ContainsKey(v))
            {
                return blockVectorToObjectDic[v].GetComponent<BlockEntity>();
            }
            else
            {
                Debug.Log("block is not found");
                return null;
            }
        }

        private void CheckNearBlock()
        {
            int closeBlockNum = -1;  // 플레이어와 가장 가까운 블록의 번호

            closeBlockNum = GetBlockEntityOnObjectPos(playerCharacter.transform.position).blockNum;

            if (prevBlockNum != closeBlockNum)
            {
                prevBlockNum = closeBlockNum;

                if (nextNearBlockNumList.Count == 0)
                {
                    for (int i = 0; i < blockNumList.Count; i++)
                    {
                        blockNumToEntityDic[blockNumList[i]].gameObject.SetActive(false);
                    }
                }
                else
                {
                    for (int i = 0; i < nextNearBlockNumList.Count; i++)
                    {
                        blockNumToEntityDic[nextNearBlockNumList[i]].gameObject.SetActive(false);

                        foreach (EntityBase entity in blockNumToEntityDic[nextNearBlockNumList[i]].GetComponentsInChildren<EntityBase>())
                        {
                            MiniMapManager.Instance.DisenableIcon(entity);
                        }
                    }
                }

                // 가장 가까운 블록을 찾았을경우 해당 블록의 next블록들을 nextBlockList에 등록하는 과정 //
                if (blockNumToEntityDic.ContainsKey(closeBlockNum))
                {
                    nextNearBlockNumList.Clear();
                    nextNearBlockNumList.Add(closeBlockNum);

                    blockEntity = blockNumToEntityDic[closeBlockNum];

                    for (int i = 0; i < blockEntity.nextBlock.Length; i++)
                    {
                        if (blockEntity.nextBlock[i] > 0)
                        {
                            nextNearBlockNumList.Add(blockEntity.nextBlock[i]);
                        }
                    }
                }

                for (int i = 0; i < nextNearBlockNumList.Count; i++)
                {
                    blockNumToEntityDic[nextNearBlockNumList[i]].gameObject.SetActive(true);

                    foreach (EntityBase entity in blockNumToEntityDic[nextNearBlockNumList[i]].GetComponentsInChildren<EntityBase>())
                    {
                        MiniMapManager.Instance.EnableIcon(entity);
                    }
                }
            }

            // 최종적으로 nextBlockNumList 에는 플레이어와 가장 가까운 블럭의 next 블럭들의 번호들이 저장된다.
            // 따라서 해당 리스트를 전달받으면 하나씩 리스트의 내용물(블록번호)을 꺼내 storage.blockDic에 key로 등록하여 오브젝트 리스트를 꺼내면 된다.
        }

        private void OnDrawGizmos()
        {
            Vector2 gizmoLinePosFor;
            Vector2 gizmoLinePosTo;

            Gizmos.color = new Color(240f / 255f, 20f / 255f, 50f / 255f);

            if (gizmoOn)
            {
                gizmoLinePosFor.x = ((int)centerPos.position.x / blockEntitySize) * blockEntitySize - ((blockHorNum - 1) * (blockEntitySize / 2f));
                gizmoLinePosTo.x = ((int)centerPos.position.x / blockEntitySize) * blockEntitySize + ((blockHorNum + 1) * (blockEntitySize / 2f));

                if (blockHorNum % 2 == 0)
                {
                    gizmoLinePosFor.x -= blockEntitySize / 2f;
                    gizmoLinePosTo.x -= blockEntitySize / 2f;
                }

                for (int i = 0; i <= blockVerNum; i++)
                {
                    gizmoLinePosFor.y = ((int)centerPos.position.y / blockEntitySize) * blockEntitySize + (blockVerNum + 1 - 2 * i) * (blockEntitySize / 2f);
                    gizmoLinePosTo.y = ((int)centerPos.position.y / blockEntitySize) * blockEntitySize + (blockVerNum + 1 - 2 * i) * (blockEntitySize / 2f);

                    if (blockVerNum % 2 == 0)
                    {
                        gizmoLinePosFor.y -= blockEntitySize / 2f;
                        gizmoLinePosTo.y -= blockEntitySize / 2f;
                    }

                    Gizmos.DrawLine(gizmoLinePosFor, gizmoLinePosTo);
                }

                gizmoLinePosFor.y = ((int)centerPos.position.y / blockEntitySize) * blockEntitySize + ((blockVerNum + 1) * (blockEntitySize / 2f));
                gizmoLinePosTo.y = ((int)centerPos.position.y / blockEntitySize) * blockEntitySize - ((blockVerNum - 1) * (blockEntitySize / 2f));

                if (blockVerNum % 2 == 0)
                {
                    gizmoLinePosFor.y -= blockEntitySize / 2f;
                    gizmoLinePosTo.y -= blockEntitySize / 2f;
                }

                for (int i = 0; i <= blockHorNum; i++)
                {
                    gizmoLinePosFor.x = ((int)centerPos.position.x / blockEntitySize) * blockEntitySize + (blockHorNum + 1 - 2 * i) * (blockEntitySize / 2f);
                    gizmoLinePosTo.x = ((int)centerPos.position.x / blockEntitySize) * blockEntitySize + (blockHorNum + 1 - 2 * i) * (blockEntitySize / 2f);

                    if (blockHorNum % 2 == 0)
                    {
                        gizmoLinePosFor.x -= blockEntitySize / 2f;
                        gizmoLinePosTo.x -= blockEntitySize / 2f;
                    }

                    Gizmos.DrawLine(gizmoLinePosFor, gizmoLinePosTo);
                }
            }
        }
    }
}