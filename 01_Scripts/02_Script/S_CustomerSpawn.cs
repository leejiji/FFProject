using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_CustomerSpawn : MonoBehaviour
{
    [SerializeField] SO_Player so_player;
    [SerializeField] GameObject customer;
    [SerializeField] Transform[] Parent;
    public bool[] isFull = new bool[3];
    public float MaxTime = 10f;
    public float MinTime = 8f;
    float Timer = 10f;
    int _num;

    void Start()
    {
        Timer = 2f;
    }

    void Update()
    {
        if(!so_player.IsGameOver)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
            {
                Timer = Random.Range(MinTime, MaxTime);
                if (!isFull[0] || !isFull[1] || !isFull[2])
                {
                    while (true)
                    {
                        _num = Random.Range(0, 3);
                        if (isFull[_num] == false)
                        {
                            GameObject customerObject = Instantiate(customer) as GameObject;
                            customerObject.name = "Customer" + (_num + 1);
                            customerObject.transform.SetParent(Parent[_num], false);
                            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySFXSound(0);
                            isFull[_num] = true;
                            break;
                        }
                        else
                            continue;
                    }
                }
                else
                    Timer = 10f;
            }
        }
    }
}
