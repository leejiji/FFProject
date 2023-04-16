using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_Trash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {        
        if (eventData.pointerDrag.gameObject.tag == "Pot")
        {
            var food = eventData.pointerDrag.gameObject.GetComponent<S_Pot>();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(7);
            food.so_stoveData.Cooking(false);
            food.UI_Setting(false);
        }
    }
}
