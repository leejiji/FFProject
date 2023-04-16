using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_MainTouch : MonoBehaviour
{
    private void OnMouseDown()
    {
        SceneController.LoadScene("GameScene");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            SceneController.LoadScene("GameScene");
    }

}
