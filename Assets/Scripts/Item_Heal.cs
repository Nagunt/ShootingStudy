using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 충돌하면 플레이어의 체력을 회복해주는 Item입니다.
public class Item_Heal : MonoBehaviour
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
                if (player != null) player.HP++;
                Destroy(gameObject);
                break;
        }
    }
}
