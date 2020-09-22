using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public static UIMgr instance = null;


    // -얻어올 것들-
    Text playerMoneyText; 
    Text maxSummonCount;
    Text curSummonCount;

    // 간단한 Setter
    public void Set_CursummonCountUI (int count) {
        curSummonCount.text = count.ToString();
        // 적용과 애니메이션 1번 재생
        curSummonCount.gameObject.GetComponent<Animator>().Rebind();        // 되감긔
    }
    public void Set_MaxSummonCountUI (int maxCount) { maxSummonCount.text = "/ " + maxCount.ToString(); }
    // -읽을 수 있는 변수들-
    public int PlayerMoney
    {
        get { return playerMoney; }
        set { playerMoney = value;
            playerMoneyText.text = playerMoney.ToString();  // ui에 update
        }
    }


    // -사용할 변수들-
    int playerMoney = 0;
    float playerMoneyTime = 0.1f;       // playerMoneyTime초에 playerMoneyDegree씩
    int playerMoneyDegree = 1;
    
    private void Awake()
    {
        if (instance)
        {
            // 씬에 존재하면 소멸시킨다.
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerMoneyText = GameObject.Find("PlayerMoney_Text").GetComponent<Text>();
        curSummonCount = GameObject.Find("CurSummonCount_Text").GetComponent<Text>();
        maxSummonCount = GameObject.Find("MaxSummonCount_Text").GetComponent<Text>();

        if (playerMoneyText)
            StartCoroutine("PlayerMakeMoney");


        // UI 적용
        Set_CursummonCountUI(0);
        Set_MaxSummonCountUI(SummonMgr.instance.maxSummonCount);
    }

    IEnumerator PlayerMakeMoney()
    {
        while (true)
        {
            playerMoney += playerMoneyDegree;
            playerMoneyText.text = playerMoney.ToString();
            
            yield return new WaitForSeconds(playerMoneyTime);
        }
    }
}
