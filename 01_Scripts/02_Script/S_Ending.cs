using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_Ending : MonoBehaviour
{

    [SerializeField] GameObject[] Credit;
    [SerializeField] Image image;
    [SerializeField] Sprite[] endSprite;
    [SerializeField] Text endText;
    [SerializeField] string[] ending;
    SO_Player playerData;
    bool TouchTime = false;
    int coin = 0;

    private void Awake()
    {
        playerData = GameObject.Find("SoundManager").GetComponent<SoundManager>().playerData;
        coin = playerData.Money;
    }
    private void Start()
    {        
        if (coin >= 300)
        {
            StartCoroutine(EndCredit(0));
        }
        else if(coin < 300 && coin >= 100)
        {
            StartCoroutine(EndCredit(1));
        }
        else if (coin < 100)
        {
            StartCoroutine(EndCredit(2));
        }
    }
    private void OnMouseDown()
    {
        if(TouchTime)
            SceneController.LoadScene("MainScene");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && TouchTime)
            SceneController.LoadScene("MainScene");
    }
    IEnumerator EndCredit(int num)
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlayBGMSound(1,num);
        image.sprite = endSprite[num];
        endText.text = ending[num];

        yield return new WaitForSeconds(2f);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(6);
        Credit[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(6);
        Credit[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(6);
        Credit[2].SetActive(true);
        TouchTime = true;
    }
}
