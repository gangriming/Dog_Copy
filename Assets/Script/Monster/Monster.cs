using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { IDLE, RUN, ATT, DEAD };
public enum MonsterName { PIG, BOX_PIG, BOMB_PIG, KING_PIG, HUMAN };

public class Monster : MonoBehaviour
{
    [Range(1.0f, 2.0f)]
    public float speed = 1.0f;
    public int monsterMaxHP = 50;
    public Transform hpFillImageTrans;

    // -읽을 수 있는 변수들-
    public bool IsPlayerSummon
    {
        get { return isPlayerSummon; }
        set
        {
            isPlayerSummon = value;

            if (!spriteRenderer)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (isPlayerSummon)
                spriteRenderer.flipX = true;
            else
            {
                spriteRenderer.flipX = false;
                spriteRenderer.color = new Color(1f, 0.3977f, 0.3977f);
                hpFillImageTrans.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
    protected bool isPlayerSummon = true;      // 플레이어가 소환했는지.


    // 내부 함수
    protected MonsterState monsterState = MonsterState.IDLE;


    // -사용 변수-
    protected SummonMgr summonMgr;
    protected Rigidbody2D myRigidbody;
    protected SpriteRenderer spriteRenderer;
    protected MonsterName monsterName;

    protected int monsterCurHp = 50;

    // -애니메이터 변수-
    protected Animator animator;
    protected float attTime = 0.2f;


    public MonsterState GetState() { return monsterState; }
    public void SetState(MonsterState state) { monsterState = state; }  // 임시


    protected void AnimationSetting(MonsterState state)
    {
        monsterState = state;

        // Animator 변경
        switch (monsterState)
        {
            case MonsterState.IDLE:
                animator.SetBool("isMoving", false);
                animator.SetBool("isAtt", false);
                break;
            case MonsterState.RUN:
                animator.SetBool("isMoving", true);
                animator.SetBool("isAtt", false);
                break;
            case MonsterState.ATT:
                animator.SetBool("isMoving", false);
                animator.SetBool("isAtt", true);
                break;
            case MonsterState.DEAD:
                break;
        }
    }

    public void SetHp(int damage)
    {
        monsterCurHp -= damage;

        Vector3 imageScale = hpFillImageTrans.localScale;
        if (monsterCurHp <= 0)
        {
            AnimationSetting(MonsterState.DEAD);
            hpFillImageTrans.localScale = new Vector3(0f, imageScale.y);

            summonMgr.monsterDead(isPlayerSummon);
            // 일단 죽여놓자
            Destroy(gameObject);        // 나중에 오브젝트 풀 연동해서 setactive바꾸기
            return;
        }

        float temp = ((float)monsterCurHp / (float)monsterMaxHP);
        hpFillImageTrans.localScale = new Vector3(temp, imageScale.y);
    }

    protected void setting()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (isPlayerSummon)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        AnimationSetting(MonsterState.RUN);
        monsterCurHp = monsterMaxHP;

        summonMgr = SummonMgr.instance;
    }
}
