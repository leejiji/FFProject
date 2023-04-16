using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public SO_Player playerData;
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }

            return instance;
        }
    } // Sound를 관리해주는 스크립트는 하나만 존재해야하고 instance프로퍼티로 언제 어디에서나 불러오기위해 싱글톤 사용

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeSFX = 1f;
    public float masterVolumeBGM = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip; //메인화면에서 사용할 BGM
    [SerializeField]
    private AudioClip adventureBgmAudioClip; //어드벤쳐씬에서 사용할 BGM
    [SerializeField]
    private AudioClip[] EndBgmAudioClip; //에서 사용할 BGM

    [SerializeField]
    private AudioClip[] sfxAudioClips; //효과음들 지정

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); //여러 씬에서 사용할 것.

        bgmPlayer = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        PlayBGMSound();
    }

    // 효과 사운드 재생 : 이름을 필수 매개변수, 볼륨을 선택적 매개변수로 지정
    public void PlaySFXSound(int ID, float volume = 1f)
    {
        sfxPlayer.PlayOneShot(sfxAudioClips[ID], volume * masterVolumeSFX);
    }

    //BGM 사운드 재생 : 볼륨을 선택적 매개변수로 지정
    public void PlayBGMSound(float volume = 1f, int type = 0)
    {
        bgmPlayer.loop = true; //BGM 사운드이므로 루프설정
        bgmPlayer.volume = volume * masterVolumeBGM;

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            bgmPlayer.clip = mainBgmAudioClip;
            bgmPlayer.Play();
        }
        else if (SceneManager.GetActiveScene().name == "GameScene")
        {
            bgmPlayer.clip = adventureBgmAudioClip;
            bgmPlayer.Play();
        }
        else if (SceneManager.GetActiveScene().name == "EndScene")
        {
            bgmPlayer.clip = EndBgmAudioClip[type];
            bgmPlayer.Play();
        }
        //현재 씬에 맞는 BGM 재생
    }

}
