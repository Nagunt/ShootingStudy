using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 적들이 상속받아 사용할 클래스입니다.
public class Enemy : MonoBehaviour
{
    // 체력
    protected int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (hp <= 0) Dead();
        }
    }
    // Get Set을 사용하여 체력을 만들었습니다.
    // Get Set을 사용할 때에는 함수와 같이 사용하면 됩니다.
    // Get Set에 관련한 자세한 설명은 아래의 링크를 참조해 주세요.
    // https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/classes-and-structs/how-to-declare-and-use-read-write-properties

    // 초기화를 수행할 함수를 protected virtual로 선언하였습니다.
    // protected 예약어는 이 함수가 상위 클래스와 하위 클래스에서만 접근 가능하도록 설정합니다.
    // virtual 예약어는 이 함수가 하위 클래스에서 override 하여 재정의가 가능하도록 설정합니다.
    // 초기화를 해야할 경우, 하위 클래스는 이 Init() 함수를 override 하여 선언해 주기만 하면 됩니다.
    // 상위 클래스의 Start()에서 이 Init() 함수를 호출하고 있기 때문입니다.
    protected virtual void Init()
    {

    }

    private void Start()
    {
        Init();
    }

    // HP가 0 이하가 되었을 때 파괴 동작을 수행할 함수를 선언하였습니다.
    // Init()와 마찬가지로, 상위 클래스에서 hp가 0이 될 시 Dead()를 호출하고 있기 때문에,
    // 하위 클래스는 이 Dead() 함수를 override하여 선언해 주기한 하면 됩니다.
    protected virtual void Dead()
    {
        Destroy(gameObject);
    }

    // Init() 함수와 Dead() 함수를 실제로 어떻게 사용하고 있는지는 Enemy_Jaco 클래스를 참조해 주세요.
    // Enemy_Jaco 클래스의 Dead() 함수에서 사용한 base는, 상위 클래스를 의미합니다.
    // 즉, base.Dead() 는 이 상위 클래스의 Dead() 함수를 실행한다는 의미입니다.
}
