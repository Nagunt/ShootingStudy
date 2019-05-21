using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 자주 사용하는 함수들을 정적 클래스를 통해 따로 묶어놓았습니다.
// GetParent의 경우 금요일반에서는 각 클래스마다 모두 선언을 해 주었지만,
// 이처럼 정적 클래스를 선언하여 각 클래스마다 선언을 하지 않아도 사용할 수 있도록 만들었습니다.
public static class MyFunc
{
    // Debug.Log 함수의 경우 디버깅에서는 매우 편리하지만,
    // Build를 하여 우리가 실제로 플레이하는 게임에서는 별도의 Asset을 사용하지 않는 이상 쓸모가 없습니다.
    // 하지만 유니티에서는 빌드를 하더라도 각 스크립트에 흩어져 있는 Debug.Log 함수를 제거하지 않습니다.
    // 따라서 이처럼 별도의 Log 함수를 만들어서 사용하되, 유니티 에디터일 때에만 Debug.Log를 호출하도록 만들었습니다.
    public static void Log(object obj)
    {
        //유니티 에디터일 때에만 실행
        if(Application.platform == RuntimePlatform.WindowsEditor)
            Debug.Log(obj);
    }

    // 충돌한 오브젝트의 최상위 오브젝트를 가져옵니다.
    // Collider의 부모 - 자식 관계에 관련 없이 최상위 오브젝트만을 활용.
    public static Transform GetParent(Collider2D obj)
    {
        Transform target = obj.transform;
        while (target.parent != null)
        {
            target = target.parent;
        }
        return target;
    }
}
