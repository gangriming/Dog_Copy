using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBox : MonoBehaviour
{
    public GameObject boxObject;


    // 던질때의 위치 조절
    GameObject createBox;
    Vector3 throwPos;       // 던지는 위치
    Vector3 DesPos;         // 목표 위치

    Color color;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public void PigThrowBox()
    {
        throwPos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);

        DesPos = throwPos;
        DesPos.x += 2f;
        DesPos.y += Random.Range(1.3f, 3.5f);

        createBox = Instantiate(boxObject, throwPos, Quaternion.identity);
        if (!GetComponent<MonsterMove>().IsPlayerSummon)
            createBox.tag = "EnemyMissile";

        if(!GetComponent<MonsterMove>().IsPlayerSummon)
            createBox.GetComponent<SpriteRenderer>().color = new Color(1f, 0.3977f, 0.3977f);
        //   createBox.transform.right = new Vector3(DesPos.x - throwPos.x, DesPos.y - throwPos.y, 0f);

        // 낼 자연스럽게 손보기,.
        if (GetComponent<MonsterMove>().IsPlayerSummon)
            createBox.GetComponent<Rigidbody2D>().velocity = createBox.transform.right * 8f;
        else
            createBox.GetComponent<Rigidbody2D>().velocity = -createBox.transform.right * 8f;

        createBox.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
    }
    
}
