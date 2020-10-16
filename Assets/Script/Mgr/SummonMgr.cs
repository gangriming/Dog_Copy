using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SummonMgr : MonoBehaviour
{
    public static SummonMgr instance = null;
    
    public GameObject[] summonMonster ; // 소환할 몬스터 목록.
    public GameObject floatingText;

    public int maxSummonCount = 10;
    public float enemySummonTime = 3f;

    // SummonMgr에서는 소환한 몬스터의 리스트를 가지고 있는다.
    List<GameObject> summonMonsterList = new List<GameObject>();        // 버튼으로 소환한 내 몬스터
    List<GameObject> enemyMonsterList = new List<GameObject>();          // 일정시간마다 나오는 적의 몬스터
    // 타워 어떻게 관리하지?
    //List<int>

    // 간단한 GetterSetter
    public int Get_CurSummonMonster() { return summonMonsterList.Count; }

    public bool isEnemyMonsterExist()
    {
        if (enemyMonsterList.Count > 0)
            return true;
        return false;
    }
    public Transform NearestMonsterPos()      
    {
        if (enemyMonsterList.Count > 0)
        {

            float near = 99999f;
            int index = 0;
            for(int i = 0; i < enemyMonsterList.Count; ++i)
            {
                if (near > enemyMonsterList[i].transform.localPosition.x)
                {
                    near = enemyMonsterList[i].transform.localPosition.x;
                    index = i;
                }
            }
            return enemyMonsterList[index].transform;/* new Vector2(near, enemyMonsterList[index].transform.localPosition.y);*/
        }
        Debug.Log("적 몬스터가 없는데 위치를 찾으려고함 <- 뜨면 안됨ㅠ");    // 꼭 isEnemyMonsterExist 체크후 사용.
        return transform;
    }


    public bool isSummonMonsterExist()
    {
        if (summonMonsterList.Count > 0)
            return true;
        return false;
    }
    public Transform NearestSummonPos()
    {
        if (summonMonsterList.Count > 0)
        {
            float near = -99999f;
            int index = 0;
            for (int i = 0; i < summonMonsterList.Count; ++i)
            {
                if (near < summonMonsterList[i].transform.localPosition.x)
                {
                    near = summonMonsterList[i].transform.localPosition.x;
                    index = i;
                }
            }
            return summonMonsterList[index].transform;/*new Vector2(near, summonMonsterList[index].transform.localPosition.y)*/;
        }
        Debug.Log("소환 몬스터가 없는데 위치를 찾으려고함 <- 뜨면 안됨ㅠ"); 
        return transform;
    }

    public void monsterDead(bool isSummon)
    {
        if(isSummon)
        {
            for (int i = 0; i < summonMonsterList.Count; ++i)
            {
                if (summonMonsterList[i].GetComponent<Monster>().GetState() == MonsterState.DEAD)
                {
                    summonMonsterList.RemoveAt(i);
                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return;
                }
             }   
        }
        else
        {
            for (int i = 0; i < enemyMonsterList.Count; ++i)
            {
                if (enemyMonsterList[i].GetComponent<Monster>().GetState() == MonsterState.DEAD)
                {
                    enemyMonsterList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void debugSummonlistAdd(GameObject obj)
    {
        summonMonsterList.Add(obj);
    }

    public void Make_FloatingTextOnHead(string str, Vector2 pos, bool isSummon)
    {
        var floating = Instantiate(floatingText, new Vector2(pos.x, pos.y), Quaternion.identity);
        floating.GetComponent<FloatingTextEffect>().FloatingText_Setting(str, isSummon);
    }

    public void NextStage_Prepare(StageSeq stage)
    {
        foreach (var item in summonMonsterList)
            Destroy(item);
        foreach (var item in enemyMonsterList)
            Destroy(item);
        summonMonsterList.Clear();
        enemyMonsterList.Clear();
        
        switch (stage)
        {
            case StageSeq.ST2:
                maxSummonCount = 12;
                enemySummonTime = 3f;
                break;
            case StageSeq.ST3:
                maxSummonCount = 15;
                enemySummonTime = 2.5f;
                break;
            case StageSeq.ENDST:
                maxSummonCount = 17;
                enemySummonTime = 2f;
                break;
            default:
                break;
        }
    }

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

        switch ((MonsterName)index)
        {
            case MonsterName.PIG:
                if (UIMgr.instance.PlayerMoney - 10 >= 0)       // 나중에 상수 구조체로 묶어버리기 걍 대충 해놓ㅇ므
                {   // 맘ㅇ ㅔ안드는데 코드좀 줄이자
                    var temp = Instantiate(summonMonster[index], new Vector3(-13f, -1.25f), Quaternion.identity);
                    UIMgr.instance.PlayerMoney = UIMgr.instance.PlayerMoney - 10;
                    summonMonsterList.Add(temp);

                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return true;
                }
                return false;
            case MonsterName.BOX_PIG:
                if (UIMgr.instance.PlayerMoney - 20 >= 0)    
                {
                    var temp = Instantiate(summonMonster[index], new Vector3(-13f, -1.25f), Quaternion.identity);
                    UIMgr.instance.PlayerMoney = UIMgr.instance.PlayerMoney - 20;
                    summonMonsterList.Add(temp);

                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return true;
                }
                return false;
            case MonsterName.BOMB_PIG:
                if (UIMgr.instance.PlayerMoney - 30 >= 0)
                {
                    var temp = Instantiate(summonMonster[index], new Vector3(-13f, -1.25f), Quaternion.identity);
                    UIMgr.instance.PlayerMoney = UIMgr.instance.PlayerMoney - 30;
                    summonMonsterList.Add(temp);

                    UIMgr.instance.Set_CursummonCountUI(summonMonsterList.Count);
                    return true;
                }
                return false;
            case MonsterName.TOWER:
                if (UIMgr.instance.PlayerMoney - 50 >= 0)
                {
                    //타워
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
            var temp = Instantiate(summonMonster[(int)Random.Range(0f, 1.5f)], new Vector3(22f, -1.25f), Quaternion.identity);
            temp.GetComponent<Monster>().IsPlayerSummon = false;
            temp.tag = "EnemyMonster";      // 적으로 태그 변경
            temp.layer = 9;     // 레이어도 적으로 변경
            enemyMonsterList.Add(temp);

            Debug.Log("적 소환");

            yield return new WaitForSeconds(enemySummonTime);
        }
    }

}
