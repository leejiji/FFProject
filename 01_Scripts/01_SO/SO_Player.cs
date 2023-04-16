using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Player", menuName = "SO/SO_Player", order = 3)]
public class SO_Player : SO_UnitData
{
    [SerializeField] int money;
    public int Money => money;

    [SerializeField] bool isDrag;
    public bool IsDrag => isDrag;

    [SerializeField] bool isGameOver;
    public bool IsGameOver => isGameOver;

    public string[] InText;
    public string[] HappyText;
    public string[] SadText;

    public void GetMoney(int Money) { money += Money; }
    public void SetMoney(int Money) { money = Money; }
    public void Drag(bool _type) { isDrag = _type; }
    public void GameOver(bool _type) { isGameOver = _type; }
}
