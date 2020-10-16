using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 2f;
    // Start is called before the first frame update
    public bool isHaveSound;

    void Start()
    {
        if (isHaveSound)
        {
            GetComponent<AudioSource>().volume = UIMgr.instance.Get_Volume();
            GetComponent<AudioSource>().Play();
        }
        Destroy(gameObject, destroyTime);
    }
}
