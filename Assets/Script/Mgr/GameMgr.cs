using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageSeq { ST1,ST2,ST3,ENDST };
public class GameMgr : MonoBehaviour
{
    public static GameMgr instance = null;
    
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
    
   public void Stage_Pass(StageSeq stage)
    {
        SummonMgr.instance.NextStage_Prepare(stage);
        UIMgr.instance.NextStage_Prepare(stage);
    }
}
