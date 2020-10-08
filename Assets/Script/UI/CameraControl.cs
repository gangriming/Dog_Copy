using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

using UnityEngine.EventSystems;


public class CameraControl : MonoBehaviour
{
    int screenHeight;
    int screenWidth;
    Vector3 preCamPos;
    CinemachineVirtualCamera cineCam;
    Camera mainCam;

    public float moveSpeed = 0.5f;
    public float zoomSpeed = 0.2f;
        
    void Start()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        preCamPos = transform.position;
        cineCam = GetComponent<CinemachineVirtualCamera>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // zoom의 경우
        if(Input.touchCount ==2)
        {
            //  mainCam.orthographicSize
            Touch touchZero = Input.GetTouch(0);        // 첫번째 좌표
            Touch touchOne = Input.GetTouch(1);         // 두번째 좌표

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // 움직임 크기 구함
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // 두 값의 차
            float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;

            cineCam.m_Lens.OrthographicSize += deltaMagDiff * zoomSpeed * Time.deltaTime;
            float temp = cineCam.m_Lens.OrthographicSize + deltaMagDiff * zoomSpeed * Time.deltaTime;

            if (temp >= 7.4f)
                temp = 7.4f;
            else if (temp <= 3.5f)
                temp = 3.5f;
            cineCam.m_Lens.OrthographicSize = temp;
        }
        else if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)   // 하나의 손가락으로 움직이는 상태일 때
        {
            Vector3 touchPos = Input.GetTouch(0).deltaPosition;/* mainCam.ScreenToWorldPoint(Input.touches[0].position);*/
            transform.Translate(-touchPos.x * moveSpeed * Time.deltaTime, -touchPos.y * moveSpeed * Time.deltaTime, 0);
            mainCam.fieldOfView += Time.deltaTime;

        }
    }
}
