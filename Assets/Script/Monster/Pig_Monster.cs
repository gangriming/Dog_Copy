using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig_Monster : Monster
{
    public Transform summonAttackPos;
    public Transform enemyAttackPos;
    public Vector2 boxSize;

    AudioSource hitAudio;

    private void Start()
    {
        base.setting();     // 기존 awake를 옮김.

        monsterName = MonsterName.PIG;
        hitAudio = GetComponent<AudioSource>();
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
        attTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (onceDead)
            return;
        // 이동말고도 Att, Dead, 다른 몬스터에 의해 멈출때 분기.
        switch (monsterState)
        {
            case MonsterState.RUN:
                {
                    if (isPlayerSummon)      // 오른쪽으로 가는 Summon
                    {
                        myRigidbody.MovePosition(new Vector2(myRigidbody.position.x + (speed * Time.fixedDeltaTime), myRigidbody.position.y));

                    }
                    else                        // 왼쪽으로 오는 Enemy
                    {
                        myRigidbody.MovePosition(new Vector2(myRigidbody.position.x - (speed * Time.fixedDeltaTime), myRigidbody.position.y));
                    }
                    break;
                }
            case MonsterState.ATT:
                {
                    if (attTime <= 0f
                         && GetComponent<SpriteRenderer>().sprite.name == "Attack (34x28)_3")      // 특정 프레임에 공격 다른 방법이 있을까?
                    {
                        attTime = 0.6f;
                        hitAudio.volume = UIMgr.instance.Get_Volume();
                        hitAudio.Play();

                        Collider2D[] collider2Ds;
                        if (IsPlayerSummon)
                            collider2Ds = Physics2D.OverlapBoxAll(summonAttackPos.position, boxSize, 0f);
                        else
                            collider2Ds = Physics2D.OverlapBoxAll(enemyAttackPos.position, boxSize, 0f);

                        foreach (Collider2D item in collider2Ds)
                        {
                            if (tag == "SummonMonster" && item.gameObject.tag == "EnemyMonster")
                            {
                                Debug.Log("내 근접 몹이 상대를 공격");
                                item.gameObject.GetComponent<Monster>().SetHp(10);
                               // SummonMgr.instance.Make_FloatingTextOnHead(10.ToString(), new Vector2(item.transform.position.x, item.transform.position.y + 0.6f), false);
                            }
                            else if (tag == "EnemyMonster" && item.gameObject.tag == "SummonMonster")
                            {
                                Debug.Log("적 근접 몹이 나를 공격");
                                item.gameObject.GetComponent<Monster>().SetHp(10);
                                //  SummonMgr.instance.Make_FloatingTextOnHead(10.ToString(), new Vector2(transform.position.x - 0.2f, transform.position.y + 0.6f), true);
                                //hitAudio.Play();
                            }
                        }
                    }
                    break;
                }
            case MonsterState.HIT:
                {
                    attTime = 0.6f;
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
                    {
                        if(targetMonster)
                            AnimationSetting(MonsterState.ATT);
                        else
                            AnimationSetting(MonsterState.RUN);
                    }
                    break;
                }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (onceDead)
            return;
        if (collision.gameObject.tag == "SummonMonster" || collision.gameObject.tag == "EnemyMonster")
        {
            AnimationSetting(MonsterState.ATT);
            targetMonster = collision.gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //// 충돌체가 떨어질 때
        //if (collision.gameObject.tag == "EnemyMonster" || collision.gameObject.tag == "SummonMonster")
        //{
        //    AnimationSetting(MonsterState.RUN);
        //}
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(IsPlayerSummon)
            Gizmos.DrawWireCube(summonAttackPos.position, boxSize);
        else
            Gizmos.DrawWireCube(enemyAttackPos.position, boxSize);
    }

}
