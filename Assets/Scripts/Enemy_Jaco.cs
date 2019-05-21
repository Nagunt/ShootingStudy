using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jaco : Enemy
{
    Transform player;
    GameObject bullet;

    // 이 Enemy_Jaco 의 Init() 함수에서는 체력을 설정해주고, Player의 정보를 저장해 놓고, 총알의 정보를 저장해 놓습니다.
    // GameObject 객체의 정적 함수 FindGameObjectWithTag(string tag)는, 인자로 넣은 tag를 tag로 가지고 있는 GameObject를 가져오는 함수입니다.
    // 이 Enemy_Jaco라는 적은 Player의 위치로 3초마다 총알을 쏘아내는 적입니다.
    // 이처럼 하나의 적마다 Enemy를 상속받게 하여 서로 다른 동작을 수행하도록 할 것입니다.
    // 하지만 Bullet 클래스를 보면 알 수 있듯이, Bullet에서는 충돌한 Object에서 Enemy를 가져와 체력을 줄어들게 할 뿐입니다.
    // 이런 방식이 가능한 이유는, 클래스의 다형성이라는 속성 때문입니다.
    // 클래스의 다형성에 대한 자세한 설명은 아래의 링크를 참조해 주세요.
    // https://pacs.tistory.com/entry/C-%EC%83%81%EC%86%8D%EA%B3%BC-%EB%8B%A4%ED%98%95%EC%84%B1-Inheritance-Polymorphism
    protected override void Init()
    {
        HP = 5;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bullet = Resources.Load<GameObject>("Prefabs/eBullet");
        StartCoroutine(Attack());
    }

    // 3초마다 플레이어의 위치를 참조하여 그 위치를 향해 총알을 쏘아내는 코루틴입니다.
    IEnumerator Attack()
    {
        WaitForSeconds waitForThreeSeconds = new WaitForSeconds(3f);
        while (true)
        {
            yield return waitForThreeSeconds;
            if (player == null) yield break;
            Vector3 pPos = player.position;
            Vector3 nPos = transform.position;
            Vector3 direction = (pPos - nPos).normalized; // normalized는 해당 Vector의 단위벡터를 가리킵니다.
            Bullet newBullet = Instantiate(bullet).GetComponent<Bullet>();
            // 이 Enemy_Jaco라는 클래스가 bullet에 저장한 Resources/Prefabs/eBullet 프리팹은 Bullet_Enemy라는 스크립트를 가지고 있습니다.
            // 그리고 Bullet_Enemy라는 클래스는 Bullet 클래스를 상속받고 있습니다.
            // Fire라는 함수는 Bullet에 선언되어있는 함수이기 때문에, 이런 식으로 사용할 수 있습니다.
            newBullet.Fire(nPos, direction, 3f);
        }
    }

    protected override void Dead()
    {
        MyFunc.Log("자코가 죽었습니다.");
        base.Dead();
    }
}
