using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum summonMonsterName { PIG, BOX_PIG, BOMB_PIG, KING_PIG, HUMAN };

public class SummonMgr : MonoBehaviour
{
    public static SummonMgr instance = null;
    
    public GameObject[] summonMonster ; // 소환할 몬스터 목록.

    public int maxSummonCount = 10;
    public float enemySummonTime = 3f;

    // SummonMgr에서는 소환한 몬스터의 리스트를 가지고 있는다.
    List<GameObject> summonMonsterList = new List<GameObject>();        // 버튼으로 소환한 내 몬스터
    List<GameObject> enemyMonsterList = new List<GameObject>();          // 일정시간마다 나오는 적의 몬스터

    // 간단한 GetterSetter
    public int Get_CurSummonMonster() { return summonMonsterList.Count; }


     //-----------------------------------------------
    private void Awake()
    {
        if (instance)
        {
            // 씬에 존재하면 소멸시킨다.
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine("enemySummon");
    }


    public bool SummonMonster(int index) // 임시.. 구조 고민하자.
    {
        // 리스트로 소환 목록 가지고있자

        if (summonMonsterList.Count >= maxSummonCount)
            return false;

        UIMgr uiMgr = UIMgr.instance;

        switch ((summonMonsterName)index)
        {
            case summonMonsterName.PIG:
                if (UIMgr.instance.PlayerMoney - 10 >= 0)       // 나중에 상수 구조체로 묶어버리기 걍 대충 해놓ㅇ므
                {   // 맘ㅇ ㅔ안드는데 코드좀 줄이자
                    var temp = Instantiate(summonMonster[index], new Vector3(-7.0f, -1.3f), Quaternion.identity);
                    UIMgr.instance.PlayerMoney = UIMgr.instance.PlayerMoney - 10;
                    summonMonsterList.Add(temp);

                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return true;
                }
                return false;
            case summonMonsterName.BOX_PIG:
                if (UIMgr.instance.PlayerMoney - 20 >= 0)    
                {
                    var temp = Instantiate(summonMonster[index], new Vector3(-7.0f, -1.3f), Quaternion.identity);
                    UIMgr.instance.PlayerMoney = UIMgr.instance.PlayerMoney - 20;
                    summonMonsterList.Add(temp);

                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return true;
                }
                return false;
            case summonMonsterName.BOMB_PIG:
                if (UIMgr.instance.PlayerMoney - 30 >= 0)
                {
                    var temp = Instantiate(summonMonster[index], new Vector3(-7.0f, -1.3f), Quaternion.identity);
                    UIMgr.instance.PlayerMoney = UIMgr.instance.PlayerMoney - 30;
                    summonMonsterList.Add(temp);

                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return true;
                }
                return false;
            default:
                break;
        }
        return false;
    }


    IEnumerator enemySummon()
    {
        while (true)
        {
            var temp = Instantiate(summonMonster[(int)Random.Range(0f, 1.5f)], new Vector3(8f, -1.3f), Quaternion.identity);
            temp.GetComponent<MonsterMove>().IsPlayerSummon = false;
            temp.tag = "EnemyMonster";      // 적으로 태그 변경
            enemyMonsterList.Add(temp);

            Debug.Log("적 소환");

            yield return new WaitForSeconds(enemySummonTime);
        }
    }

}
