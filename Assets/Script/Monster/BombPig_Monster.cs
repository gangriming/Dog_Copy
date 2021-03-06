﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPig_Monster : Monster
{

    private void Start()
    {
        base.setting();     // 기존 awake를 옮김.

        monsterName = MonsterName.BOMB_PIG;
    }


    // Update is called once per frame
    void Update()
    {
        if (monsterState == MonsterState.ATT)
        {
            if (!targetMonster)
                AnimationSetting(MonsterState.RUN);     // 타겟된게 없으면 RUN
        }
        else if (monsterState == MonsterState.DEAD)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)    // 죽는 모션 끝나면
            {
            }
            else
                Destroy(gameObject);        // 나중에 오브젝트 풀 연동해서 setactive바꾸기
        }
    }

    private void FixedUpdate()
    {
        if (onceDead)
            return;
        attTime -= Time.deltaTime;
        
        switch (monsterState)
        {
            case MonsterState.RUN:
                {
                    if (isPlayerSummon)      // 오른쪽으로 가는 Summon
                    {
                        myRigidbody.MovePosition(new Vector2(myRigidbody.position.x + (speed * Time.fixedDeltaTime), myRigidbody.position.y));

                        if (summonMgr.isEnemyMonsterExist()
                             && Vector2.Distance(myRigidbody.position, summonMgr.NearestMonsterPos().localPosition) < 5f)
                        {
                            AnimationSetting(MonsterState.ATT);     // 공격.
                            targetMonster = summonMgr.NearestMonsterPos();
                        }
                    }
                    else                        // 왼쪽으로 오는 Enemy
                    {
                        myRigidbody.MovePosition(new Vector2(myRigidbody.position.x - (speed * Time.fixedDeltaTime), myRigidbody.position.y));

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
                    if (attTime <= 0f
                        && GetComponent<SpriteRenderer>().sprite.name == "Throwing Boom (26x26)_3")
                    {
                        attTime = 2f;
                        GetComponent<ThrowBox>().PigThrowBox();
                    }
                    
                    if (!targetMonster)
                        AnimationSetting(MonsterState.RUN);

                    break;
                }
            case MonsterState.HIT:
                {
                    attTime = 2f;
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    {
                        if (targetMonster)
                            AnimationSetting(MonsterState.ATT);
                        else
                            AnimationSetting(MonsterState.RUN);
                    }
                    break;
                }

            case MonsterState.IDLE:
                {
                    if (summonMgr.isEnemyMonsterExist()
                         && Vector2.Distance(myRigidbody.position, summonMgr.NearestMonsterPos().localPosition) > 5f)
                    {
                        AnimationSetting(MonsterState.RUN);
                    }
                    break;
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "SummonMonster" || collision.gameObject.tag == "EnemyMonster")
        //{
        //    AnimationSetting(MonsterState.ATT);
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //// 충돌체가 떨어질 때
        //if (collision.gameObject.tag == "EnemyMonster" || collision.gameObject.tag == "SummonMonster")
        //{
        //    AnimationSetting(MonsterState.RUN);
        //}
    }
}
