using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterMove :  Monster
{
    
    private void Awake()
    {
        base.setting();     // 기존 awake를 옮김.
    }

    // Update is called once per frame
    void Update()
    {
        if(monsterState == MonsterState.ATT)
        {
            if (isPlayerSummon && !summonMgr.isEnemyMonsterExist())
                AnimationSetting(MonsterState.RUN);
            else if(!isPlayerSummon && !summonMgr.isSummonMonsterExist())
                AnimationSetting(MonsterState.RUN);
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
                    if (isPlayerSummon)      // 오른쪽으로 가는 Summon
                    {
                        myRigidbody.MovePosition(new Vector2(myRigidbody.position.x + (speed * Time.fixedDeltaTime), myRigidbody.position.y));

                        if (summonMgr.isEnemyMonsterExist()
                            && Vector2.Distance(myRigidbody.position, summonMgr.NearestMonsterPos().localPosition) < 5f
                            && monsterName == MonsterName.BOX_PIG || monsterName == MonsterName.BOMB_PIG)       // 원거리 몬스터 이면서, 공격에 맞게 일정 이상 가까워지면
                        {
                            AnimationSetting(MonsterState.ATT);     // 공격.
                        }
                    }
                    else                        // 왼쪽으로 오는 Enemy
                    {
                        myRigidbody.MovePosition(new Vector2(myRigidbody.position.x - (speed * Time.fixedDeltaTime), myRigidbody.position.y));

                        if (summonMgr.isSummonMonsterExist()
                            && Vector2.Distance(myRigidbody.position, summonMgr.NearestSummonPos().localPosition) < 5f 
                            && monsterName == MonsterName.BOX_PIG || monsterName == MonsterName.BOMB_PIG) // 원거리 몬스터 이면서, 공격에 맞게 일정 이상 가까워지면
                        {
                            AnimationSetting(MonsterState.ATT);     // 공격.
                        }
                    }
                    break;
                }
            case MonsterState.ATT:
                {
                    if (monsterName == MonsterName.BOX_PIG
                      && attTime <= 0f
                      && GetComponent<SpriteRenderer>().sprite.name == "Throwing Box (26x30)_3")      // 특정 프레임에 공격 다른 방법이 있을까?
                    {
                        attTime = 1f;
                        GetComponent<ThrowBox>().PigThrowBox();
                    }
                    else if (monsterName == MonsterName.BOMB_PIG
                        && attTime <= 0f
                        && GetComponent<SpriteRenderer>().sprite.name == "Throwing Boom (26x26)_3")
                    {
                        attTime = 2f;
                        GetComponent<ThrowBox>().PigThrowBox();
                    }


                    if (monsterName == MonsterName.BOX_PIG || monsterName == MonsterName.BOMB_PIG)
                    {
                        if (!summonMgr.isSummonMonsterExist())
                            AnimationSetting(MonsterState.RUN);
                        //else
                        //{
                        //    if (Vector2.Distance(myRigidbody.position, summonMgr.NearestSummonPos()) < 5f)
                        //        AnimationSetting(MonsterState.IDLE);
                        //}
                    }
                    break;
                }
            case MonsterState.IDLE:
                {
                    if (summonMgr.isEnemyMonsterExist()
                          && Vector2.Distance(myRigidbody.position, summonMgr.NearestMonsterPos().localPosition) > 5f
                            && monsterName == MonsterName.BOX_PIG || monsterName == MonsterName.BOMB_PIG)       // 원거리 몬스터 이면서 멀면
                    {
                        AnimationSetting(MonsterState.RUN);
                    }
                        break;
                }
        }
    }

              // dynamic상태일때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체에 들어올 때
        // 아마 근거리 몬스터(PIG)밖에 쓸 일이 없을 것 같다.

        if (collision.gameObject.tag == "SummonMonster" || collision.gameObject.tag == "EnemyMonster")
        {
            // 1. list에 맨 앞에있는 summonMonster가 enemy 소환 영역 끝까지 갔다면 ( 맵 끝까지 갔다면 )
            // 2. 혹은 summonMonster의 맨 앞에있는 몬스터가 enemyMonster의 맨 앞과 만났다면

            // 1,2를 해결 전에 일단 중앙까지 오면, 뒤의 리스트들은 이동을 멈춘다. <- layer를 통해 같은 몬스터끼리는 충돌 못하게 함. 필요없어짐.
            // 그리고 맨 앞이 죽거나, 혹은 쓰러트려서 앞으로 전진한다면 같이 이동
            // 위의 내용을 SummonMgr에 구현 <- 리스트를 가지고 있어야 하니까. 
            // 그렇다면 맨 앞의 몬스터는 자기가 1번이라는것을 알고 있어야 한다. 별도 변수 필요?

            //Debug.Log("부딪침");
            AnimationSetting(MonsterState.ATT);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌체가 떨어질 때
        if (collision.gameObject.tag == "EnemyMonster" || collision.gameObject.tag == "SummonMonster" )
        {
            AnimationSetting(MonsterState.RUN);
        }
    }
    

    //void AnimationSetting(MonsterState state)
    //{
    //    monsterState = state;

    //    // Animator 변경
    //    switch (monsterState)
    //    {
    //        case MonsterState.IDLE:
    //            animator.SetBool("isMoving", false);
    //            animator.SetBool("isAtt", false);
    //            break;
    //        case MonsterState.RUN:
    //            animator.SetBool("isMoving", true);
    //            animator.SetBool("isAtt", false);
    //            break;
    //        case MonsterState.ATT:
    //            animator.SetBool("isMoving", false);
    //            animator.SetBool("isAtt", true);
    //            break;
    //        case MonsterState.DEAD:
    //            break;
    //    }
    //}
}


