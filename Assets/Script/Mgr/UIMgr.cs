using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIMgr : MonoBehaviour
{
    public static UIMgr instance = null;

    // -얻어올 것들-
    Text playerMoneyText;
    Text maxSummonCount;
    Text curSummonCount;
    Image playerHpImage;
    Image enemyHpImage;

    AudioSource bgmAudio;
    public GameObject unitCreateGroup;
    public GameObject menuGroup;
    public GameObject pauseButton;
    public GameObject endGroup;
    public GameObject startGroup;

    public AudioClip winAudio;
    public AudioClip loseAudio;

    // 간단한 Setter
    public void Set_CursummonCountUI(int count)
    {
        curSummonCount.text = count.ToString();
        // 적용과 애니메이션 1번 재생
        curSummonCount.gameObject.GetComponent<Animator>().Rebind();        // 되감긔
    }
    public void Set_MaxSummonCountUI(int maxCount) { maxSummonCount.text = "/ " + maxCount.ToString(); }
    // -읽을 수 있는 변수들-
    public int PlayerMoney
    {
        get { return playerMoney; }
        set
        {
            playerMoney = value;
            playerMoneyText.text = playerMoney.ToString();  // ui에 update
        }
    }
    public float Get_Volume()
    {
        return bgmAudio.volume;
    }

    public void StartStage(string startAniStr)
    {
        startGroup.SetActive(true);
        // stage시작 애니메이션을 다시 돌려준다.
        startGroup.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = startAniStr;
        startGroup.GetComponent<Animator>().Rebind();
    }

    public void NextStage_Prepare(StageSeq stage)
    {
        Set_MaxSummonCountUI(SummonMgr.instance.maxSummonCount);
        Set_CursummonCountUI(0);
        PlayerMoney = 0;

        playerHp = playerMaxHp;
        enemyHp = enemyMaxHp;
        playerHpImage.transform.localScale = new Vector3(1f, 1f);
        enemyHpImage.transform.localScale = new Vector3(1f, 1f);

        switch (stage)
        {
            case StageSeq.ST2:
                playerMoneyTime = 0.13f;
                StartStage("STAGE 2");
                break;
            case StageSeq.ST3:
                playerMoneyTime = 0.15f;
                StartStage("STAGE 3");
                break;
            case StageSeq.ENDST:
                playerMoneyTime = 0.18f;
                StartStage("FINAL STAGE");
                break;
            default:
                break;
        }
    }

    // -사용할 변수들-
    int playerMoney = 0;
    float playerMoneyTime = 0.1f;       // playerMoneyTime초에 playerMoneyDegree씩
    int playerMoneyDegree = 1;


    int playerHp = 100;
    int enemyHp = 100;      // 이 두개는 UIMgr보단 GameMgr에 가는게 더 적절할듯?
    int playerMaxHp;
    int enemyMaxHp;

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

        playerHpImage = GameObject.Find("Player_FullHP").GetComponent<Image>();
        enemyHpImage = GameObject.Find("Enemy_FullHP").GetComponent<Image>();

        if (playerMoneyText)
            StartCoroutine("PlayerMakeMoney");


        playerMaxHp = playerHp;
        enemyMaxHp = enemyHp;


        // UI 적용
        Set_CursummonCountUI(0);
        Set_MaxSummonCountUI(SummonMgr.instance.maxSummonCount);
        bgmAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(startGroup.activeSelf)
        {
            if (startGroup.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                startGroup.SetActive(false);
            }
        }
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


    public void SetTotalHp(int damage, bool isPlayer)
    {
        if (isPlayer)
        {
            playerHp -= damage;

            Vector3 imageScale = playerHpImage.transform.localScale;
            if (playerHp <= 0)
            {
                playerHp = 0;
                //  EndGame("GAME OVER",false);
                // Debug
                GameMgr.instance.Stage_Pass(StageSeq.ST2);
            }

            float temp = ((float)playerHp / (float)playerMaxHp);

            playerHpImage.transform.localScale = new Vector3(temp, imageScale.y);
            StartCoroutine(playerHpImage.transform.parent.GetComponent<UIShake>().Shake(10f, 0.5f));        // UIShake
        }
        else
        {
            enemyHp -= damage;

            Vector3 imageScale = enemyHpImage.transform.localScale;
            if (enemyHp <= 0)
            {
                enemyHp = 0;
                EndGame("GAME CLEAR", true);
            }

            float temp = ((float)enemyHp / (float)enemyMaxHp);

            enemyHpImage.transform.localScale = new Vector3(temp, imageScale.y);
            StartCoroutine(enemyHpImage.transform.parent.GetComponent<UIShake>().Shake(10f, 0.5f));
        }

    }


    //////////////////

    public void activeMenu()
    {
        if(!menuGroup.activeSelf)       // 메뉴가 꺼져있는 상태면
        {
            Time.timeScale = 0.15f;      // 시간을 느리게
            menuGroup.SetActive(true);      // 메뉴 활성화.
            pauseButton.GetComponent<Button>().interactable = false;  // 버튼을 꺼준다. 

            for (int i = 0; i < unitCreateGroup.transform.childCount; ++i)
            {
                if(unitCreateGroup.transform.GetChild(i).gameObject.activeSelf)
                {
                    unitCreateGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public void menuExit()
    {
        Time.timeScale = 1f;
        menuGroup.SetActive(false);  
        pauseButton.GetComponent<Button>().interactable = true; 
        for (int i = 0; i < unitCreateGroup.transform.childCount; ++i)
        {
            if (unitCreateGroup.transform.GetChild(i).gameObject.activeSelf)
            {
                unitCreateGroup.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }

    void EndGame(string str, bool win)
    {
        endGroup.SetActive(true);
        Time.timeScale = 0f;

        endGroup.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = str;
        if (win)
        {
            bgmAudio.clip = winAudio;
            bgmAudio.loop = false;
            bgmAudio.Play();
        }
        else
        {
            bgmAudio.clip = loseAudio;
            bgmAudio.loop = false;
            bgmAudio.Play();
        }
    }
}
