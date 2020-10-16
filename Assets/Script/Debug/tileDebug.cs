using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class tileDebug : MonoBehaviour
{
    public Tilemap collTile;
    public Grid grid;
    public GameObject tempTower;
    Camera mainCam;

    Vector3 debugPos;

    void Start()
    {
        mainCam = Camera.main;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))   //마우스 좌측 버튼을 누름.
        {
            // Debug.Log(Input.GetTouch(0));       // ? 왜않되 
            // 아마 InputGetTouch 는 핸드폰 터치에서만 인식되는건가?

            Vector3 touchPos = mainCam.ScreenToWorldPoint(Input.mousePosition);     // PC용 Debug
            Vector3 cellPos = grid.WorldToCell(touchPos);
            //debugPos = cellPos;
            //collTile.

            Debug.Log(cellPos);

            // 그짝으로 타워 생성
            if (-3f >= cellPos.y - Mathf.Epsilon && -3f <= cellPos.y + Mathf.Epsilon)
            {
                Vector2 towerCol = tempTower.GetComponent<BoxCollider2D>().size;
                SummonMgr.instance.debugSummonlistAdd(Instantiate(tempTower, new Vector3(cellPos.x + towerCol.x / 2f, cellPos.y + towerCol.y), Quaternion.identity));
            }
        }
    }
   
}
