using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //회전 속도 변수
    private float rotSpeed;
    //회전 값 변수
    float mx = 0;
    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = GameManager.gm.rotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
         // 게임 중일때만 동작 
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
       
        float mouse_X = Input.GetAxis("Mouse X");

        mx += mouse_X * rotSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0,mx,0);
        
    }
}
