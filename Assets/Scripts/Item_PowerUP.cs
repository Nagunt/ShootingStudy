using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 충돌하면 플레이어의 Power를 증가시키는 Item입니다.
// Item에 관련해서는 딱히 공통되는 속성을 만들지 않았기 때문에 Item이라는 클래스를 만들고 이를 상속하는 식으로 구조를 짜지는 않았습니다.
public class Item_PowerUP : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform target = MyFunc.GetParent(collision);
        switch (target.tag)
        {
            case "Wall":
                Destroy(gameObject);
                break;
            case "Player":
                Player player = target.GetComponent<Player>();
                if (player != null) player.Power++;
                Destroy(gameObject);
                break;
        }
    }
}
