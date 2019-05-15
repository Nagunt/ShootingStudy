using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 플레이어가 움직이는 속도입니다.
    public float cooldown; // 총알이 1프레임마다 나가는 것을 방지하기 위한 cooldown 변수입니다.

    GameObject bullet; // 총알을 Instantiate하기 위해 총알의 원본을 저장하는 변수입니다.

    Vector3 limitPosition; // 플레이어가 화면 밖으로 나가는 것을 방지하기 위한 한계점을 저장하기 위한 변수입니다.
    public const float MAXCOOLDOWN = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        limitPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            Screen.width, Screen.height));
        // Camera.main은 기본적으로 Scene에 배치되어 있는 Main Camera를 가리킵니다.
        // SceenToWorldPoint 함수는 화면의 좌표를 유니티 Scene의 world 좌표로 변환하는 함수입니다.
        // Screen.width와 Screen.height는 게임 화면의 너비와 높이를 가리킵니다.
        // 위의 코드는 화면 우측 상단의 좌표를 Scene의 world 좌표로 변환합니다.
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
        // 유니티에서는 위와 같이 Resources.Load<타입>(경로) 함수를 통해 특정 폴더 안의 파일을 불러올 수 있습니다.
        // Resoures.Load 함수는 Project 화면에서 Resources 라는 이름의 폴더 안의 파일을 불러옵니다.
        // 위의 함수는 Prefabs 폴더 안의 Bullet이라는 Prefab을 불러와 bullet이라는 변수에 저장하는 코드입니다.
        // Prefab이란, 미리 만들어 둔 GameObject를 복제하여 사용할 수 있도록 파일로 만든 것입니다.
        // Hierarchy에서 확인하면 일반적인 흰색 아이콘이 아닌, 푸른 색 아이콘으로 되어 있는 GameObject를 볼 수 있는데, 이는 Prefab화 되었다는 의미입니다.
    }

    void Fire()
    {
        // Instantiate(원본 게임오브젝트)는 게임오브젝트를 복제하는 함수입니다.
        // 아래와 같이 하면 복제한 오브젝트를 또다른 변수에 저장할 수 있습니다.
        // prefab은 아래와 같이 복제하여 사용하는 것을 추천합니다.
        GameObject newObject = Instantiate(bullet);
        // GetComponent<타입>()은 해당하는 Object에서 원하는 타입의 Component를 가져오는 함수입니다.
        // 아래와 같이 하면 가져온 Component를 또다른 변수에 저장할 수 있습니다.
        Bullet newBullet = newObject.GetComponent<Bullet>();
        newBullet.Fire(transform.position, Vector3.up, 5f);
        cooldown = MAXCOOLDOWN;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate는 현재 위치를 원점으로 하여 인자 만큼 움직이는 함수입니다.
        // Input.GetKey(KeyCode)는 인자로 넣은 키보드의 버튼이 눌려 있다면 True를, 그렇지 않으면 False를 반환하는 함수입니다.
        // Input 클래스에는 GetKey이외에도 많은 입력에 관련된 함수들이 있습니다.
        if (transform.position.y < limitPosition.y && Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (transform.position.y > -limitPosition.y && Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        if (transform.position.x < limitPosition.x && Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (transform.position.x > -limitPosition.x && Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (cooldown <= 0 &&Input.GetKey(KeyCode.Space))
        {
            Fire();
        }

        if (cooldown > 0) cooldown -= Time.deltaTime;

    }
}
