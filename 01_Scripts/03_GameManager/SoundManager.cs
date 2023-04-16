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
    } // Sound�� �������ִ� ��ũ��Ʈ�� �ϳ��� �����ؾ��ϰ� instance������Ƽ�� ���� ��𿡼��� �ҷ��������� �̱��� ���

    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeSFX = 1f;
    public float masterVolumeBGM = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip; //����ȭ�鿡�� ����� BGM
    [SerializeField]
    private AudioClip adventureBgmAudioClip; //��庥�ľ����� ����� BGM
    [SerializeField]
    private AudioClip[] EndBgmAudioClip; //���� ����� BGM

    [SerializeField]
    private AudioClip[] sfxAudioClips; //ȿ������ ����

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject); //���� ������ ����� ��.

        bgmPlayer = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        PlayBGMSound();
    }

    // ȿ�� ���� ��� : �̸��� �ʼ� �Ű�����, ������ ������ �Ű������� ����
    public void PlaySFXSound(int ID, float volume = 1f)
    {
        sfxPlayer.PlayOneShot(sfxAudioClips[ID], volume * masterVolumeSFX);
    }

    //BGM ���� ��� : ������ ������ �Ű������� ����
    public void PlayBGMSound(float volume = 1f, int type = 0)
    {
        bgmPlayer.loop = true; //BGM �����̹Ƿ� ��������
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
        //���� ���� �´� BGM ���
    }

}
