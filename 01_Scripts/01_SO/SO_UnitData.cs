using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SO_UnitData", menuName = "SO/Unit", order = 0)]

public class SO_UnitData : ScriptableObject
{
    [SerializeField] string unitName;
    public string UnitName => unitName;

    [SerializeField] int number;
    public int Number => number;
}
