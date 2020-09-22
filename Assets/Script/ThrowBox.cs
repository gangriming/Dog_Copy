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


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (createBox)
        {
            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            createBox.transform.eulerAngles = new Vector3(0f, 0f, angle);
        }

    }

    public void PigThrowBox()
    {
        throwPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        DesPos = throwPos;
        DesPos.x += 2f;
        DesPos.y += Random.Range(1f, 5f);

        createBox = Instantiate(boxObject, throwPos, Quaternion.identity);
        createBox.transform.right = new Vector3(DesPos.x - throwPos.x, DesPos.y - throwPos.y, 0f);

        // 낼 자연스럽게 손보기,.
        createBox.GetComponent<Rigidbody2D>().velocity = createBox.transform.right * 8f;
    }
}
