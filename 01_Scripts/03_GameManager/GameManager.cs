using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Slider TimeSlider;
    [SerializeField] SO_Player so_player;
    [SerializeField] GameObject OverPanel;
    [SerializeField] GameObject EffectPanel;
    [SerializeField] GameObject PadePanel;

    float GameTime = 180f;

    private void Awake()
    {
        so_player.GameOver(false);
        so_player.Drag(false);
        so_player.SetMoney(0);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBGMSound();
        TimeSlider.maxValue = GameTime;
    }
    private void Start()
    {
        Pade(true);
    }
    private void Update()
    {     
        if(GameTime <= 0f)
        {
            so_player.GameOver(true);
            OverPanel.SetActive(true);
            EffectPanel.SetActive(false);
            Pade(false);
        }
        else
        {
            GameTime -= Time.deltaTime;
            TimeSlider.value = GameTime;
        }

        if(GameTime <= 120f && GameTime >= 60f)
        {
            GameObject.Find("CustomerPanel").GetComponent<S_CustomerSpawn>().MaxTime = 8f;
            GameObject.Find("CustomerPanel").GetComponent<S_CustomerSpawn>().MinTime = 5f;
        }
        else if(GameTime < 60f)
        {
            GameObject.Find("CustomerPanel").GetComponent<S_CustomerSpawn>().MaxTime = 5f;
            GameObject.Find("CustomerPanel").GetComponent<S_CustomerSpawn>().MinTime = 3f;
        }
    }

    private void Pade(bool paidin)
    {
        if (paidin)
        {
            PadePanel.GetComponent<Animator>().SetBool("PADE", false);
        }        
        else
        {
            PadePanel.GetComponent<Animator>().SetBool("PADE", true);
            StartCoroutine(GameOver());
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        SceneController.LoadScene("EndScene");
    }
}
