using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알을 발사하려면 무엇이 필요할까요?
    // 초기 위치, 나아갈 방향, 나아갈 속도가 필요합니다.
    // 인자값으로 위의 세 가지 요소를 받습니다.
    // 아래의 함수를 통해 총알을 초기화하고, 발사할 수 있습니다.(Player script의 Fire() 함수 참조.)
    public void Fire(Vector3 position, Vector3 direction, float speed)
    {
        transform.position = position;
        StartCoroutine(Move(direction, speed));
    }

    // 코루틴은 yield 라는 특수한 예약어를 사용할 수 있는 기능입니다.
    // 함수처럼 사용하되, IEnumerator라고 써야 합니다.
    // yield를 쓴다면 다음에 들어갈 기능이 수행될때 까지 기다린다고 생각하면 됩니다.
    IEnumerator Move(Vector3 direction, float speed)
    {
        // yield break; // Coroutine을 중지합니다,
        // yield return new WaitUntil(() => 조건문); // 조건문이 참이 될때까지 기다립니다.
        // yield return new WaitWhile(() => 조건문); // 조건문이 거짓이 될때까지 기다립니다.
        // yield return null; // 1 프레임을 기다립니다.
        // yield return new WaitForSeconds(시간); // 일정 시간이 지날때 까지 기다립니다.
        // yield return new WaitForSecondsRealTime(시간); // 현실 시간 기준으로 일정 시간이 지날때 까지 기다립니다.
        // yield return StartCoroutine(다른 코루틴); // 다른 코루틴이 실행되고 끝날때 까지 기다립니다.
        while (true)
        {
            yield return new WaitForEndOfFrame(); // 한 프레임이 끝날때까지 기다립니다.
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    // OnTriggerEnter2D는 Collider2D Component를 가지고 있는 오브젝트가, IsTrigger 속성이 참일 때, Collider2D 범위에 다른 Collider가 Enter 했을 때 호출됩니다.
    // 인자로 주어지는 Collider2D collision을 통해 충돌한 오브젝트의 정보를 가져올 수 있습니다.
    // Tag를 통해 collision을 구별할 수 있습니다.
    // 아래의 코드는 Wall 이라는 Tag를 가진 Object와 충돌하면 총알을 삭제하는 코드입니다. 총알이 화면 밖으로 나가도 Scene안에 남아 있는 문제를 해결하기 위한 것입니다.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform target = MyFunc.GetParent(collision);
        MyFunc.Log("충돌하였습니다");
        switch (target.tag)
        {
            case "Wall":
                Destroy(gameObject);
                break;
            case "Enemy":
                Enemy enemy = target.GetComponent<Enemy>();
                if (enemy != null) enemy.HP--;
                Destroy(gameObject);
                break;
        }
    }


}
