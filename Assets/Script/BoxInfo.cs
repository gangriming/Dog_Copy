using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BoxInfo : MonoBehaviour
{
    public GameObject boxPieces;

    bool onceAtt = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }
    


    private void OnTriggerEnter2D(Collider2D collision)     // Trigger로 처리해야, 몬스터가 밀리지않음.
    {
        // 충돌체에 들어올 때

        if (collision.gameObject.tag == "GroundCol")
        {
            Debug.Log("바닥-상자 충돌");
            // 충돌 판정을 만들자.
            CreateBoxPieces();
            Destroy(gameObject);
            return;
        }


        if (gameObject.tag == "SummonMissile" 
            && collision.gameObject.tag == "EnemyMonster")     // 내가 적을 맞췄을 때
        {
            if (!onceAtt)
            {
                collision.gameObject.GetComponent<Monster>().SetHp(20);
                onceAtt = true;
               // SummonMgr.instance.Make_FloatingTextOnHead(20.ToString(), new Vector2(collision.transform.position.x, collision.transform.position.y + 0.6f), false);
                CreateBoxPieces();
                Destroy(gameObject);
            }
        }
        else if (gameObject.tag == "EnemyMissile" 
            && collision.gameObject.tag == "SummonMonster")   // 적이 나를 맞췄을 때 
        {
            if (!onceAtt)
            {
                collision.gameObject.GetComponent<Monster>().SetHp(20);
                onceAtt = true;
              //  SummonMgr.instance.Make_FloatingTextOnHead(20.ToString(), new Vector2(collision.transform.position.x, collision.transform.position.y + 0.6f), true);
                CreateBoxPieces();
                Destroy(gameObject);
            }

        }
    }


    private void CreateBoxPieces()
    {

        var pieces = Instantiate(boxPieces, transform.position, Quaternion.identity); // 조각을 만든다.

        for (int i = 0; i < pieces.transform.childCount; ++i)
        {
            // 조각들에 힘을 준다.
            pieces.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f)), new Vector2(0f, 0f), ForceMode2D.Impulse);
        }

    }
}
