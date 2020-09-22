using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    public static SceneMgr instance = null;
    public enum sceneName { TITLE, MAIN, END };

    private sceneName curScene = sceneName.TITLE;

    private void Awake()
    {
        if(instance )
        {
            // 씬에 존재하면 소멸시킨다.
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void nextScene()
    {
        SceneManager.LoadScene((int)curScene + 1);
    }
}
