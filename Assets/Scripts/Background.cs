using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // 배경 이미지를 Loop하는 스크립트 입니다.

    public float speed;

    Vector3 firstPosition;
    Vector3 limitPosition;

    // Start is called before the first frame update
    void Start()
    {
        firstPosition = transform.position; // 초기 위치를 저장합니다.
        limitPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            0, Screen.height)) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -limitPosition.y)
        {
            transform.position = firstPosition; // 임계점을 넘으면 다시 초기 위치로 되돌립니다.(루프합니다.)
        }
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
