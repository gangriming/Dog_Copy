using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeButton : MonoBehaviour
{
    public float coolTime = 2f;

    // 내부 사용 변수
    GameObject coolTimeObject;
    Image coolImage;
    Text coolText;
    SummonMgr summonMgr;
    float remainCoolTime = 0f;

    void Start()
    {
        coolTimeObject = transform.Find("CoolTimeActive").gameObject;

        if (coolTimeObject)
        {
            coolImage = coolTimeObject.transform.Find("CoolImage").gameObject.GetComponent<Image>();
            coolText = coolTimeObject.transform.Find("CoolText").gameObject.GetComponent<Text>();
        }

        // 할당 후, Active 해제.
        coolTimeObject.SetActive(false);

        summonMgr = SummonMgr.instance;
    }

    public void CoolTimeActive(int summonIndex)
    {
        if (coolTimeObject.activeSelf)      // 활성화 중이면 X
            return;
        else
        {
            if (summonMgr.SummonMonster(summonIndex))       // 소환 성공 하면 쿨타임 활성화.
            {
                coolTimeObject.SetActive(true);
                remainCoolTime = coolTime;

                StartCoroutine("activeCoolTime");
            }
        }
    }


    IEnumerator activeCoolTime()
    {
        while (remainCoolTime > 0f)
        {
            remainCoolTime -= Time.deltaTime;

            float temp = (Mathf.Floor(remainCoolTime * 10) / 10);       // 1자리
            if (temp < 0f)
                temp = 0f;

            coolText.text = temp.ToString();

            float ratio = (remainCoolTime / coolTime);
            coolImage.fillAmount = ratio;

            yield return null;
        }

        // 쿨 타임이 끝나면 액티브 활성화를 풀어준다.
        coolTimeObject.SetActive(false);
        remainCoolTime = 0f;
    }
}

