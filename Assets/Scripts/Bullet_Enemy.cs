using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : Bullet
{

    // OnTriggerEnter2D가 private로 선언되어 있기 때문에, Bullet의 OnTriggerEnter2D는 이 Bullet_Enemy에 영향을 주지 않을 것입니다.
    // 따라서 이처럼 따로 선언을 해주어야 합니다.
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
                if (player != null) player.HP--;
                Destroy(gameObject);
                break;
        }
    }
}
