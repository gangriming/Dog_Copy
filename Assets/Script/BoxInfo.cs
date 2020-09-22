using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInfo : MonoBehaviour
{
    public GameObject boxPieces;

    // Start is called before the first frame update
    void Start()
    {


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌체에 들어올 때

        if (collision.gameObject.tag == "GroundCol")
        {
            Debug.Log("바닥-상자 충돌");
            // 충돌 판정을 만들자.

            var pieces = Instantiate(boxPieces, transform.position, Quaternion.identity); // 조각을 만든다.

            for (int i = 0; i < pieces.transform.childCount; ++i)
            {
                // 조각들에 힘을 준다.
                pieces.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                    new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 4f)), new Vector2(0f, 0f), ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }

        if (gameObject.tag == "SummonMissile" 
            && collision.gameObject.tag == "EnemyMonster")     // 내가 적을 맞췄을 때
        {

        }
        else if (gameObject.tag == "EnemyMissile" 
            && collision.gameObject.tag == "SummonMonster")   // 적이 나를 맞췄을 때 
        {

        }
    }
}
