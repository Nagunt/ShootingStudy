using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; // 플레이어가 움직이는 속도입니다.
    public float cooldown; // 총알이 1프레임마다 나가는 것을 방지하기 위한 cooldown 변수입니다.

    GameObject bullet; // 총알을 Instantiate하기 위해 총알의 원본을 저장하는 변수입니다.
    Transform firePos;      // 파워에 따라서 총알이 많이 나가게 하기 위해 총알이 나가는 부분을 저장한 변수입니다.

    Vector3 limitPosition; // 플레이어가 화면 밖으로 나가는 것을 방지하기 위한 한계점을 저장하기 위한 변수입니다.

    public const int MAXHP = 3;             // 플레이어의 최대 체력값을 저장한 상수 변수입니다.
    public const int MAXPOWER = 3;          // 플레이어의 최대 파워값을 저장한 상수 변수입니다.
    public const float MAXCOOLDOWN = 0.2f;  // 플레이어의 총알 간 발사 간격 값을 저장한 상수 변수입니다.

    // SerializedField는 변수가 public이 아니더라고 inspector에 노출되게 만들어 줍니다.
    // 유니티는 변수를 public으로 선언하지 않고 이처럼 private 또는 protected이더라도 SerializedFieid를 통해 사용하는 것을 권장하고 있습니다.
    [SerializeField]        
    private int hp;         // 플레이어의 체력.
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp > MAXHP) hp = MAXHP;
            if (hp <= 0) Dead();
        }
    }

    private int power;      // 플레이어의 파워.
    public int Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
            if (power >= MAXPOWER) power = MAXPOWER - 1;
            if (power < 0) power = 0; 
        }
    }
    // 이처럼 Get Set을 활용하면 최대값과 최소값을 정해서, 변수의 값이 이를 벗어나지 않게 할 수도 있습니다.

    // 플레이어 또한 체력이 0 이하가 되면 죽어야 합니다.
    // 이 함수에 게임 오버에 관련된 기능을 구현하면 되겠죠?
    void Dead()
    {
        MyFunc.Log("게임 오버!");
        Destroy(gameObject);
    }

    void Awake()
    {
        HP = MAXHP; // 체력을 최대 체력으로 초기화
        Power = 0;
        cooldown = 0;
        firePos = transform.Find("FirePosition");
    }

    private void Start()
    {
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

    // 추가로 파워업을 구현하였습니다. ( 토요일 반에서만 하였습니다. 금요일 반도 이 부분을 참고해 주세요.)
    // Player GameObject를 보시면 FirePosition이라는 하위 오브젝트가 추가된 것을 볼 수 있습니다.
    // 이 FirePosition에다가 총알이 나갈 부분들을 빈 GameObject들로 지정해 놓았습니다.
    void Fire()
    {
        // childCount는 해당하는 Transform이 자식으로 가지고 있는 Transform의 개수입니다.
        // 하위의 하위는 따지지 않고, 하위만 따집니다.
        for (int i = 0; i <= Power && i < firePos.childCount; ++i)
        {
            // GetChild(int num)은 자식 오브젝트를 Hierarchy의 순서에 따라 가져오는 함수입니다.
            // Hierarchy의 위에 위치할 수록 순서가 빨라집니다.
            // 비슷한 기능으로는 Find(string name)이 있는데, 이 함수는 자식 오브젝트를 오브젝트의 이름으로 가져오는 함수입니다.
            Transform nTransform = firePos.GetChild(i);
            for (int j = 0; j < nTransform.childCount; ++j)
            {
                Vector3 nPos = nTransform.GetChild(j).position;
                GameObject newObject = Instantiate(bullet);
                Bullet newBullet = newObject.GetComponent<Bullet>();
                newBullet.Fire(nPos, Vector3.up, 5f);
            }
        }
        // 위의 반복문에서, power가 0이라면 FirePosition의 하위 오브젝트 Pos_1의 하위 오브젝트들을 가져와서 그 위치에 각각 총알을 생성할 것입니다.
        // power가 1이라면 FirePosition의 하위 오브젝트 Pos_1와 Pos_2의 하위 오브젝트들을 가져와서 그 위치에 각각 총알을 생성할 것입니다.
        // 마지막으로 power가 2이라면 FirePositon의 하위 오브젝트 Pos_1, Pos_2, Pos_3의 하위 오브젝트들을 가져와서 그 위치에 각각 총알을 생성할 것입니다.
        // Pos_n의 하위 오브젝트의 수에 따라서 총알을 생성하기에, 에디터에서 이들 오브젝트에 빈 오브젝트를 추가해서 위치를 조정하기만 하면 쉽게 파워업의 기능을 수정할 수 있을 것입니다.

        // sfx라는 이름의 효과음을 재생합니다.
        SoundManager.Instance.PlaySFX("sfx");
        cooldown = MAXCOOLDOWN;
        /*
        // Instantiate(원본 게임오브젝트)는 게임오브젝트를 복제하는 함수입니다.
        // 아래와 같이 하면 복제한 오브젝트를 또다른 변수에 저장할 수 있습니다.
        // prefab은 아래와 같이 복제하여 사용하는 것을 추천합니다.
        GameObject newObject = Instantiate(bullet);
        // GetComponent<타입>()은 해당하는 Object에서 원하는 타입의 Component를 가져오는 함수입니다.
        // 아래와 같이 하면 가져온 Component를 또다른 변수에 저장할 수 있습니다.
        Bullet newBullet = newObject.GetComponent<Bullet>();*/
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
