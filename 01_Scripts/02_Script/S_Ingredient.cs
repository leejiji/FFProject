using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_Ingredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SO_IngredientData so_IngredientData;
    [SerializeField] CanvasGroup canvasGroup;

    private RectTransform rectTrans;
    private RectTransform parentRectTrans;
    private Canvas rootCanvas;
    private Vector2 offset;

    private void Awake()
    {
        rectTrans = this.GetComponent<RectTransform>();
        parentRectTrans = this.rectTrans.parent as RectTransform;
        this.rootCanvas = this.GetComponentInParent<Canvas>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(6);

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

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(this.parentRectTrans);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }
}
