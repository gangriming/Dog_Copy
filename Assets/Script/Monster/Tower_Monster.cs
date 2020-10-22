using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Monster : Monster
{
    public int towerIndex;
    public bool isAttach = false;       // 처음에는 attach상태가 아님.

    public void Tower_Attach(int index)
    {
        isAttach = true;
        if(!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = Color.white;
        towerIndex = index;

        // 태그 달아서 인식하게
        gameObject.tag = "SummonMonster";
        gameObject.layer = 8;
    }

    private void Start()
    {
        monsterName = MonsterName.TOWER;

        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isPlayerSummon)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        AnimationSetting(MonsterState.RUN);
        monsterCurHp = monsterMaxHP;

        summonMgr = SummonMgr.instance;

    }


    // Update is called once per frame
    void Update()
    {
        if (monsterState == MonsterState.ATT)
        {
            if (!targetMonster)
                AnimationSetting(MonsterState.RUN);     // 타겟된게 없으면 RUN
        }

    }

    private void FixedUpdate()
    {
        attTime -= Time.deltaTime;
        // 이동말고도 Att, Dead, 다른 몬스터에 의해 멈출때 분기.
        switch (monsterState)
        {
            case MonsterState.RUN:
                {
                    if (isPlayerSummon) 
                    {
                        if (summonMgr.isEnemyMonsterExist()
                             && Vector2.Distance(myRigidbody.position, summonMgr.NearestMonsterPos().localPosition) < 5f)
                        {
                            AnimationSetting(MonsterState.ATT);     // 공격.
                            targetMonster = summonMgr.NearestMonsterPos();
                        }
                    }
                    else                        // 왼쪽으로 오는 Enemy
                    {
                        if (summonMgr.isSummonMonsterExist()
                            && Vector2.Distance(myRigidbody.position, summonMgr.NearestSummonPos().localPosition) < 5f)
                        {
                            AnimationSetting(MonsterState.ATT);     // 공격.
                            targetMonster = summonMgr.NearestSummonPos();
                        }
                    }
                    break;
                }
            case MonsterState.ATT:
                {
                    if (attTime <= 0f)   
                    {
                        attTime = 1f;
                    }

                    if (!targetMonster)
                        AnimationSetting(MonsterState.RUN);

                    break;
                }
        }
    }
}
