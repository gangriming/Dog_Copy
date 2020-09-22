using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { IDLE, RUN, ATT, DEAD };

public class MonsterMove : MonoBehaviour
{
    [Range(1.0f, 2.0f)]
    public float speed = 1.0f;

    // -읽을 수 있는 변수들-
    public bool IsPlayerSummon
    {
        get { return isPlayerSummon; }
        set
        {
            isPlayerSummon = value;
            if (isPlayerSummon)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }
    public bool isPlayerSummon = true;      // 플레이어가 소환했는지.


    // -사용 변수-
    Rigidbody2D rigidbody;
    SpriteRenderer spriteRenderer;
    MonsterState monsterState = MonsterState.IDLE;
    public summonMonsterName monsterName;

    // -애니메이터 변수-
    Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (isPlayerSummon)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        monsterState = MonsterState.RUN;



        // debug용
        if (monsterName == summonMonsterName.BOX_PIG)
            StartCoroutine("DebugBoxPig");

    }
    // Update is called once per frame
    void Update()
    {
        // Animator 변경
        if (monsterState == MonsterState.RUN)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

    }

    private void FixedUpdate()
    {
        // 이동말고도 Att, Dead, 다른 몬스터에 의해 멈출때 분기.
        // 지금은 임시로
        if (monsterState == MonsterState.RUN)
        {
            if (isPlayerSummon)      // 오른쪽
            {
                rigidbody.MovePosition(new Vector2(rigidbody.position.x + (speed * Time.deltaTime), rigidbody.position.y));
            }
            else                        // 왼쪽
            {
                rigidbody.MovePosition(new Vector2(rigidbody.position.x - (speed * Time.deltaTime), rigidbody.position.y));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체에 들어올 때

        if (collision.gameObject.tag == "SummonMonster")
        {
            //   Debug.Log("충돌");
            // 1. list에 맨 앞에있는 summonMonster가 enemy 소환 영역 끝까지 갔다면 ( 맵 끝까지 갔다면 )
            // 2. 혹은 summonMonster의 맨 앞에있는 몬스터가 enemyMonster의 맨 앞과 만났다면

            // 1,2를 해결 전에 일단 중앙까지 오면, 뒤의 리스트들은 이동을 멈춘다.
            // 그리고 맨 앞이 죽거나, 혹은 쓰러트려서 앞으로 전진한다면 같이 이동
            // 위의 내용을 SummonMgr에 구현 <- 리스트를 가지고 있어야 하니까. 
            // 그렇다면 맨 앞의 몬스터는 자기가 1번이라는것을 알고 있어야 한다. 별도 변수 필요?

            // 일단 멈춰놓자. ㅇㅅㅇ
            monsterState = MonsterState.IDLE;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌체가 떨어질 때
        if (collision.gameObject.tag == "SummonMonster")
        {
            monsterState = MonsterState.RUN;
        }
    }

    IEnumerator DebugBoxPig()
    {
        while(true)
        {
            GetComponent<ThrowBox>().PigThrowBox();
            yield return new WaitForSeconds(2f);
        }
    }
}