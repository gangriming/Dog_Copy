using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextEffect : MonoBehaviour
{

    TextMesh textMesh;
    float initSize;

    public void FloatingText_Setting(string str, bool isSummon)
    {
        if(!textMesh)
            textMesh = GetComponent<TextMesh>();

        textMesh.text = str;
        if (isSummon)
            textMesh.color = new Color(0.5551115f, 0.9254902f, 0.345098f);
        else
            textMesh.color = new Color(1f, 0.1568628f, 0.1568628f);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (!textMesh)
            textMesh = GetComponent<TextMesh>();

        textMesh.fontSize = Random.Range(20, 40);
        initSize = (float)textMesh.fontSize;

        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-20f, 20f)));

        StartCoroutine("textAnimation");      // 별로인듯?
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator textAnimation()
    {
        while (textMesh.fontSize > 0)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + Time.deltaTime );
            initSize -= Time.deltaTime * 10f;

            if (initSize < 0f)
                textMesh.fontSize = 0;
            else
             textMesh.fontSize = (int)initSize;

            yield return null;
        }

        Destroy(gameObject);
    }
}
