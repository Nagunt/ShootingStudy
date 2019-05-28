using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // 싱글톤 패턴을 위해 정적 클래스 변수 하나를 만들었습니다.
    // 싱글톤 패턴이란, 어떤 클래스가 단 하나만 필요할 때 사용하는 프로그래밍 패턴입니다.
    // 이 SoundManager 클래스의 경우, 소리를 관리하는 기능을 수행합니다.
    // 이러한 Manager 클래스 들의 경우, 두 개 이상의 객체가 있을 경우 오히려 곤란해지기 때문에 싱글톤 패턴을 사용하기 매우 적합합니다.
    // SoundManager.Instance.~ 로 SoundManager의 기능을 수행할 수 있습니다.
    public static SoundManager Instance { get; private set; }

    // Dictionary는 Key와 Value 한 쌍으로 이루어진 데이터 형식입니다.
    // Key값으로 Value에 접근할 수 있습니다.
    // 이 isPause 의 경우, string이 키 값으로 (자식 오브젝트의 이름을 키 값으로 사용했습니다) 사용됩니다.
    Dictionary<string, bool> isPause =
        new Dictionary<string, bool>();

    // 싱글톤 패턴을 사용함에 따라, Awake와 Start에서 수행해야 할 부분을 엄격하게 구분해야 합니다.
    // 만약 또 다른 클래스의 Awake에서 이 SoundManager를 호출할 경우, 그 코드가 동작하는 것을 보장할 수 없습니다.
    // Awake 함수도 실행되는 순서가 있기 때문입니다.
    // 따라서 Awake에서는 클래스 내부에서 수행하는 초기화 작업을 수행합니다.
    // Start에서는 다른 클래스를 참조해서 수행하는 초기화 작업을 수행합니다.
    private void Awake()
    {
        Instance = this;
    }

    // Unity에서 소리를 발생시키려면 AudioListener, AudioClip, AudioSource가 필요합니다.
    // AudioListener 컴포넌트는 이름 그대로 소리를 듣는 컴포넌트입니다. 설정을 건드리지 않는 한, Main Camera에 자동으로 부여되어 있습니다.
    // Unity는 mp3, wav 등의 소리 파일들을 AudioClip으로 인식합니다. 
    // AudioSource는 위에서 설명한 AudioClip을 실질적으로 실행하는 컴포넌트입니다.
    // 이 프로젝트에서 효과음은 GameObject에 AudioSource 컴포넌트를 부여하고, 재생이 끝나면 제거하는 방식으로 구현되었습니다.
    // 이 프로젝트에서 배경음은 하나마다 자식 오브젝트를 만들고, 그 오브젝트에 AudioSource 컴포넌트를 부여하여 이름을 통해 접근할 수 있도록 구현되었습니다.

    // 배경음을 구현한 함수입니다,
    // 이 함수를 호출하면 배경음이 재생됩니다.
    // Resources/Sounds 폴더 안에 있는 효과음 파일의 이름을 인자로 넣으면 기능합니다.

    public void PlayBGM(string bgm)
    {
        // transform.Find(string name)은 name을 이름으로 갖는 자식 오브젝트를 가져오는 함수입니다.
        if (transform.Find(bgm))
        {
            // 이미 인자를 이름으로 갖는 자식 오브젝트가 있다는 것은 해당하는 배경음이 이미 재생 중이거나, 일시정지 중이라는 뜻입니다.
            // 이 프로젝트에서는 isPause에다가 인자를 Key 값으로 하여 일시정지를 하고 있는가 여부를 저장할 것입니다.
            // isPause 값이 참일 경우, 일시정지를 해제해 줍니다.
            if(isPause[bgm])
            {
                AudioSource audioSource =
                    transform.Find(bgm).GetComponent<AudioSource>();
                audioSource.UnPause();
                isPause[bgm] = false;
            }
            return;
        }

        AudioClip newClip =
            Resources.Load<AudioClip>("Sounds/" + bgm);
        if (newClip == null) return;
        // Resources/Sounds 폴더 안에 있는 배경음 파일을 불러오고, null 값이면 (존재하지 않으면) 함수를 종료합니다.

        GameObject newObject = new GameObject(bgm);
        newObject.transform.SetParent(transform);
        // 인자를 이름으로 갖는 새로운 게임 오브젝트를 생성하고, 부모로 이 컴포넌트가 부여된 오브젝트를 설정합니다.

        AudioSource newSource =
            newObject.AddComponent<AudioSource>();
        // AudioSource 컴포넌트를 자식 게임 오브젝트에 부여합니다.

        newSource.clip = newClip;

        // AudioSource 컴포넌트가 재생할 파일을 설정합니다.

        newSource.loop = false;

        // loop는 무한반복을 할 것인가를 결정합니다.

        newSource.volume = 1f;

        // volume은 효과음의 소리 크기를 결정합니다.

        newSource.playOnAwake = false;

        // playOnAwake는 AudioSource가 Scene 안에 게임 오브젝트의 컴포넌트로 부여되어 있을 때, play 버튼을 누르면 바로 재생할 것인지 결정합니다.

        newSource.Play();

        // Play()를 호출하는 것으로 AudioSource를 재생할 수 있습니다.

        isPause.Add(bgm, false);

        // isPause에다가 인자를 키 값으로 하는 저장공간을 부여하고, 그 값에 false를 저장합니다.

        StartCoroutine(RemoveBGM(newSource, bgm));// 재생이 끝나거나 중지가 되면 해당 오브젝트를 제거하도록 (순차적 기능 수행) 코루틴을 사용했습니다.
    }

    // 배경음을 일시정지하는 함수입니다.

    public void PauseBGM(string bgm)
    {
        if (transform.Find(bgm))
        {
            // 이미 인자를 이름으로 갖는 자식 오브젝트가 있다는 것은 해당하는 배경음이 이미 재생 중이거나, 일시정지 중이라는 뜻입니다.
            // 이 프로젝트에서는 isPause에다가 인자를 Key 값으로 하여 일시정지를 하고 있는가 여부를 저장할 것이라고 했습니다.
            // 따라서 isPause에서 Key 값을 통해 가져온 Value가 false인지, 즉 일시정지가 되어 있지 않은지 판별합니다.
            if (isPause[bgm] == false)
            {
                AudioSource audioSource =
                    transform.Find(bgm).GetComponent<AudioSource>();
                audioSource.Pause();
                isPause[bgm] = true;
                // 일시정지 되어 있지 않다면 일시정지를 하고, isPause의 Key - Value 값도 true로 바꾸어 줍니다.
            }
            return;
        }
    }

    // 배경음을 중지하는 함수입니다.

    public void StopBGM(string bgm)
    {
        if (transform.Find(bgm))
        {
            // 이미 인자를 이름으로 갖는 자식 오브젝트가 있다는 것은 해당하는 배경음이 이미 재생 중이거나, 일시정지 중이라는 뜻입니다.
            // 중지를 함에 있어서 배경음이 재생 중인지, 일시정지 중인지는 중요하지 않습니다.
            AudioSource audioSource = transform.Find(bgm).GetComponent<AudioSource>();
            audioSource.Stop();
            isPause[bgm] = false;
            // 재생을 중지하고, isPause의 Key - Value 값을 false로 설정합니다.
            // 이는 자연적으로 재생이 끝났을 경우와 조건을 똑같이 맞추어주기 위함입니다.
            // 자연적으로 재생이 끝났다면 isPause의 Key - Value 값이 false로 되어 있을 것입니다.
            return;
        }
    }

    // 효과음을 구현한 함수입니다,
    // 이 함수를 호출하면 효과음이 재생됩니다.
    // Resources/Sounds 폴더 안에 있는 효과음 파일의 이름을 인자로 넣으면 기능합니다.

    // PlaySFX함수의 경우 Player 클래스의 Fire 함수에서 사용하고 있습니다.

    public void PlaySFX(string sfx)
    {
        AudioClip newClip =
            Resources.Load<AudioClip>("Sounds/" + sfx);
        
        if (newClip == null) return;

        // Resources/Sounds 폴더 안에 있는 효과음 파일을 불러오고, null 값이면 (존재하지 않으면) 함수를 종료합니다.

        AudioSource newSource =
            gameObject.AddComponent<AudioSource>();

        // AudioSource 컴포넌트를 게임 오브젝트에 부여합니다.

        newSource.clip = newClip;

        // AudioSource 컴포넌트가 재생할 파일을 설정합니다.

        newSource.loop = false;

        // loop는 무한반복을 할 것인가를 결정합니다.

        newSource.volume = 1f;

        // volume은 효과음의 소리 크기를 결정합니다.

        newSource.playOnAwake = false;

        // playOnAwake는 AudioSource가 Scene 안에 게임 오브젝트의 컴포넌트로 부여되어 있을 때, play 버튼을 누르면 바로 재생할 것인지 결정합니다.

        newSource.Play();

        // Play()를 호출하는 것으로 AudioSource를 재생할 수 있습니다.

        StartCoroutine(RemoveSFX(newSource)); // 재생이 끝나면 해당 컴포넌트를 제거하도록 (순차적 기능 수행) 코루틴을 사용했습니다.
    }

    IEnumerator RemoveSFX(AudioSource audioSource)
    {
        if (audioSource == null) yield break;
        // AudioSource 컴포넌트의 isPlaying 으로 이 컴포넌트가 재생 중인지 아니면 끝났는지 판별할 수 있습니다.
        // 다만, isPlaying을 통해 일시정지와 중지를 구별할 수 없습니다. 따라서 배경음을 재생할 때는 약간 다르게 코드를 짜야합니다.
        yield return new WaitWhile(() => audioSource.isPlaying);
        Destroy(audioSource);
        // 인자로 주어진 audioSource의 isPlaying이 거짓이 될 때 까지 기다린 후, audioSource를 제거합니다.
    }

    IEnumerator RemoveBGM(AudioSource audioSource, string key)
    {
        if (audioSource == null) yield break;
        // AudioSource 컴포넌트의 isPlaying 으로 이 컴포넌트가 재생 중인지 아니면 끝났는지 판별할 수 있습니다.
        // 다만, isPlaying을 통해 일시정지와 중지를 구별할 수 없습니다. 따라서 배경음을 재생할 때는 약간 다르게 코드를 짜야합니다.
        // isPlaying과 isPause의 Key - Value 값을 통해 이 컴포넌트가 재생이 종료 되었는지 판별합니다.
        yield return new WaitUntil(() =>
        audioSource.isPlaying == false && isPause[key] == false);
        Destroy(audioSource.gameObject);
        isPause.Remove(key);
        // 종료가 되었다면 isPause의 Key - Value 값도 더 이상 사용하지 않을 것이므로 메모리 할당을 해제해 줍니다.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Q를 누르면 bgm이라는 이름의 배경음을 재생합니다.
        {
            SoundManager.Instance.PlayBGM("bgm");
        }
        if (Input.GetKeyDown(KeyCode.W))// W를 누르면 bgm이라는 이름의 배경음을 일시정지합니다.
        {
            SoundManager.Instance.PauseBGM("bgm");
        }
        if (Input.GetKeyDown(KeyCode.E))// E를 누르면 bgm이라는 이름의 배경음을 중지합니다.
        {
            SoundManager.Instance.StopBGM("bgm");
        }
    }
}
