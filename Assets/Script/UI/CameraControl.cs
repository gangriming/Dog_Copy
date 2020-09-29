using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraControl : MonoBehaviour
{
    //ScreenHeight = Screen.height;
    //    ScreenWidth = Screen.width; 

    int screenHeight;
    int screenWidth;
    //public Camera myCamera;
    Vector3 preCamPos;

    public float moveSpeed = 10f;
        
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        preCamPos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            float xinput = Input.GetAxisRaw("Mouse X");
            float yinput = Input.GetAxisRaw("Mouse Y");

            float x = transform.position.x;
            float y = transform.position.y;

            preCamPos = new Vector3(x - xinput, y - yinput, -10f);
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, preCamPos, moveSpeed * Time.fixedDeltaTime);

    }

}
