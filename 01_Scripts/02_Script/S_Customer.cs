using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class S_Customer : MonoBehaviour, IDropHandler
{
    [SerializeField] SO_Player so_player;
    [SerializeField] Sprite[] FeelingSprite;
    [SerializeField] Sprite[] OrderSprite;
    [SerializeField] Image FeelingImage;
    [SerializeField] Image OrderImage;
    [SerializeField] Image TimerImage;
    [SerializeField] Text SaidText;
    S_CustomerSpawn spawnManager;

    public int OrderCode = 0;
    int charcterCode = 0;
    float MaxTime = 20f;
    float Timer = 20f;
    bool isOver = false;

    private void Awake()
    {
        OrderCode = Random.Range(1, 6);
        charcterCode = Random.Range(0, 6);
        OrderImage.sprite = OrderSprite[OrderCode - 1];
        FeelingImage.sprite = FeelingSprite[0 + (3 * charcterCode)];

        int saidText = Random.Range(0, so_player.InText.Length);
        SaidText.text = so_player.InText[saidText];
        spawnManager = GameObject.Find("CustomerPanel").GetComponent<S_CustomerSpawn>();
    }
    private void Update()
    {
        if(!isOver)
        {
            Timer -= Time.deltaTime;

            TimerImage.fillAmount = (MaxTime - Timer) / MaxTime;

            if (Timer <= 0)
            {
                FeelingImage.sprite = FeelingSprite[1 + (3 * charcterCode)];
                StartCoroutine(GameOver());
            }
        }

        if(so_player.IsGameOver)
            StartCoroutine(GameOver());

    }
    #region Drop
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Pot")
        {
            var food = eventData.pointerDrag.gameObject.GetComponent<S_Pot>();

            if (!food.so_stoveData.isCooking && food.so_stoveData.isDone)
            {
                if (food.so_stoveData.FFCode == OrderCode)
                {
                    StartCoroutine(Submit(food, true));
                }
                else if (food.so_stoveData.FFCode == 0)
                {
                    Debug.Log("Uncompleted");
                }
                else
                {
                    StartCoroutine(Submit(food, false));
                }
            }      
            
        }
    }

    IEnumerator Submit(S_Pot food, bool isAlright)
    {
        food.so_stoveData.Cooking(false);
        food.UI_Setting(false);
        yield return new WaitForSeconds(0.1f);
        if (isAlright)
        {
            FeelingImage.sprite = FeelingSprite[2 + (3* charcterCode)];

            Said();
            int saidText = Random.Range(0, so_player.HappyText.Length);
            SaidText.text = so_player.HappyText[saidText];

            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(3);
            food.GetCoin();
        }
        else
        {
            FeelingImage.sprite = FeelingSprite[1 + (3 * charcterCode)];

            Said();
            int saidText = Random.Range(0, so_player.SadText.Length);
            SaidText.text = so_player.SadText[saidText];

            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(4);
        }

        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        isOver = true;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<Animator>().SetBool("isOver", true);
        yield return new WaitForSeconds(1f);
        switch (this.gameObject.name)
        {
            case "Customer1":
                spawnManager.isFull[0] = false;
                break;
            case "Customer2":
                spawnManager.isFull[1] = false;
                break;
            case "Customer3":
                spawnManager.isFull[2] = false;
                break;
        }
        Destroy(this.gameObject);
    }
    #endregion

    #region Said
    private void Said()
    {
        this.gameObject.GetComponent<Animator>().SetBool("isSaid", true);
    }
    #endregion
}
