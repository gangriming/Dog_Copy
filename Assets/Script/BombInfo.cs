using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombInfo : MonoBehaviour
{
    Animator animator;
    int state;
    AudioSource hitAudio;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        hitAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
        if (state == 0)
        {
            // 돌리지말자
            //Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            //float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            //transform.eulerAngles = new Vector3(0f, 0f, angle);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)     // Trigger로 처리해야, 몬스터가 밀리지않음.
    {
        // 충돌체에 들어올 때

        if (collision.gameObject.tag == "GroundCol")
        {
            Debug.Log("바닥-폭탄 충돌");

            GetComponent<Rigidbody2D>().simulated = false;
            animator.SetInteger("BombState", 1);            // 폭탄 꿈틀꿈틀
            state = 1;

            Invoke("booooooom", 2f);                          // 2초후 터진다!
            // Destroy(gameObject, 2f);        // 2초후 터짐.
        }
    }

    private void booooooom()
    {
        animator.SetInteger("BombState", 2);
        state = 2;

        ////////////////////// 폭탄 반경
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        foreach (Collider2D hit in colliders)
        {
            // 반경에 있는것들 다 데미지를 준다.
            if (gameObject.tag == "SummonMissile"
    && hit.gameObject.tag == "EnemyMonster")     // 내가 적을 맞췄을 때
            {
                hit.GetComponentInParent<Monster>().SetHp(25);
                //SummonMgr.instance.Make_FloatingTextOnHead(25.ToString(), new Vector2(hit.transform.position.x, hit.transform.position.y + 0.6f), false);

            }
            else if (gameObject.tag == "EnemyMissile"
    && hit.gameObject.tag == "SummonMonster")   // 적이 나를 맞췄을 때 
            {
                hit.GetComponentInParent<Monster>().SetHp(25);
               // SummonMgr.instance.Make_FloatingTextOnHead(25.ToString(), new Vector2(hit.transform.position.x, hit.transform.position.y + 0.6f), true);
            }
        }

        hitAudio.volume = UIMgr.instance.Get_Volume();
        hitAudio.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        //  Gizmos.DrawWireCube(transform.position boxSize);
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

}
