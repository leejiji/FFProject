using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class S_Pot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
{
    public SO_StoveData so_stoveData;
    [SerializeField] SO_Player so_PlayerData;
    
    [SerializeField] GameObject UI_CountPanel;
    [SerializeField] GameObject UI_TimerPanel;
    [SerializeField] GameObject UI_Ingredient;
    [SerializeField] GameObject UI_Trash;
    [SerializeField] GameObject FX_Bubble;
    [SerializeField] Text UI_CountText;
    [SerializeField] Text UI_CoinText;
    [SerializeField] Image[] UI_Icon = new Image[2];
    [SerializeField] Image UI_Slider;
    [SerializeField] Sprite[] IconSprite = new Sprite[7];
    [SerializeField] Sprite[] FoodSprite = new Sprite[7];
    [SerializeField] CanvasGroup canvasGroup;

    float Timer = 5f;

    private RectTransform rectTrans;
    private RectTransform parentRectTrans;
    private Canvas rootCanvas;
    private Vector2 offset;


    private void Awake()
    {
        so_stoveData.Init();
        UI_Setting(false);

        rectTrans = this.GetComponent<RectTransform>();
        parentRectTrans = this.rectTrans.parent as RectTransform;
        this.rootCanvas = this.GetComponentInParent<Canvas>();
    }
    private void Update()
    {
        if (so_stoveData.isCooking)
        {
            UI_Setting(true);
            Timer -= Time.deltaTime;
            UI_Slider.fillAmount = Timer / 5;
            FX_Bubble.SetActive(true);
            this.gameObject.GetComponent<Image>().sprite = FoodSprite[7];
            if (Timer <= 0)
            {
                so_stoveData.Done();
                CookCompletion();
                Timer = 5f;
            }
        }   
    }

    #region Touch
    public void OnPointerClick(PointerEventData eventData)
    {
        if (so_stoveData.isCooking == false && so_stoveData.nowCount() == 2)
        {
            so_stoveData.Cooking(true);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(1);
            Debug.Log("Pot" + so_stoveData.Number + "'s Cooking Start!");
        }
    }
    public void UI_Setting(bool _Cook)
    {
        if (_Cook)
        {
            UI_CountPanel.SetActive(false);
            UI_TimerPanel.SetActive(true);
            UI_Icon[0].enabled = false;
            UI_Icon[1].enabled = false;
            UI_Icon[0].sprite = IconSprite[0];
            UI_Icon[1].sprite = IconSprite[0];
        }
        else if (!_Cook)
        {
            this.gameObject.GetComponent<Image>().sprite = FoodSprite[0];
            UI_CountPanel.SetActive(true);
            UI_TimerPanel.SetActive(false);
            UI_Icon[0].enabled = false;
            UI_Icon[1].enabled = false;
            UI_CountText.text = so_stoveData.nowCount().ToString() + " / 2";
        }
        else { return; }
    }
    
    void CookCompletion()
    {
        FX_Bubble.SetActive(false);
        if (so_stoveData.isFood)
        {
            this.gameObject.GetComponent<Image>().sprite = FoodSprite[so_stoveData.FFCode];
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = FoodSprite[6];
        }
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(2);
    }
    
    #endregion

    #region Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(6);
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        canvasGroup.blocksRaycasts = false;

        this.transform.SetParent(rootCanvas.transform);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.parentRectTrans,
                eventData.position,
                (this.rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this.rootCanvas.worldCamera,
                out offset))
        {
            this.offset.x = this.offset.x - this.transform.localPosition.x;
            this.offset.y = this.offset.y - this.transform.localPosition.y;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (so_stoveData.isDone)
        {
            so_PlayerData.Drag(true);
            UI_Ingredient.SetActive(false);
            UI_Trash.SetActive(true);

            Vector2 outLocalPos = Vector2.zero;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                  this.parentRectTrans,
                  eventData.position,
                  (this.rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this.rootCanvas.worldCamera,
                  out outLocalPos))
            {
                this.transform.localPosition = outLocalPos - offset;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(this.parentRectTrans);
        transform.localPosition = new Vector3(0,16.25f,0);
        UI_Ingredient.SetActive(true);
        UI_Trash.SetActive(false);
        canvasGroup.blocksRaycasts = true;
    }

    #endregion

    #region Drop
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.gameObject.tag == "Ingredient" &&  so_stoveData.isCooking == false && so_stoveData.isDone == false)
        {
            var ingredient = eventData.pointerDrag.gameObject.GetComponent<S_Ingredient>();
            so_stoveData.pushIngredient(ingredient.so_IngredientData.Number);
            UI_CountText.text = so_stoveData.nowCount().ToString() + " / 2";

            for(int i = 0; i < so_stoveData.nowCount(); i++)
            {
                UI_Icon[i].enabled = true;
                UI_Icon[i].sprite = IconSprite[so_stoveData.Ingredients[i]];
            }
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(7);
        }
    }
    #endregion

    #region Coin
    public void GetCoin()
    {
        StartCoroutine(Count(so_PlayerData.Money + 10, so_PlayerData.Money));
        so_PlayerData.GetMoney(10);
    }
    IEnumerator Count(float target, float current)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정
        float offset = (target - current) / duration;

        yield return new WaitForSeconds(0.5f);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(5);

        while (current < target)
        {
            current += offset * Time.deltaTime;
            UI_CoinText.text = ((int)current).ToString();
            yield return null;
        }

        current = target;
        UI_CoinText.text = ((int)current).ToString();
    }
    #endregion
}
