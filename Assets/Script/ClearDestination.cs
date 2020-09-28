using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDestination : MonoBehaviour
{

    public bool isPlayerDestination = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isPlayerDestination &&
            collision.tag == "EnemyMonster")         // 소환 몬스터 생성 지점에 enemyMonster가 들어오게 된다면.
        {
            collision.gameObject.GetComponent<MonsterMove>().SetState(MonsterState.DEAD);
            SummonMgr.instance.monsterDead(false);
            Destroy(collision.gameObject);              // 파괴 -> 후에 오브젝트 풀링으로 대체 
            // 개맘에안든다
            // 오브젝트 풀링할떄 리스트 지우는것도 넣어야댐
            UIMgr.instance.SetTotalHp(10, true);
        }
        else if(!isPlayerDestination &&
            collision.tag == "SummonMonster")
        {
            collision.gameObject.GetComponent<MonsterMove>().SetState(MonsterState.DEAD);
            SummonMgr.instance.monsterDead(false);
            Destroy(collision.gameObject);              // 파괴 -> 후에 오브젝트 풀링으로 대체
            UIMgr.instance.SetTotalHp(10, false);
        }
    }
}
