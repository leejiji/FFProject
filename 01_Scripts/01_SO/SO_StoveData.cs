using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SO_StoveData", menuName = "SO/StoveData", order = 1)]
public class SO_StoveData : SO_UnitData
{
    enum Ingredient { Potato = 1, Fish, Pork, Tteok, Rice, Doenjang }

    [SerializeField] int[] ingredients = new int[2];
    public int[] Ingredients => ingredients;

    [HideInInspector] public bool isCooking = false;
    [HideInInspector] public bool isFood = false;
    [HideInInspector] public bool isDone = false;

    public int FFCode = 0;
    private int[,] recipes = new int[5, 2];
    private int pushnum = 0;
    
    public void Init()
    { 
        ingredients[0] = 0; ingredients[1] = 0;
        isCooking = false; isDone = false;
        pushnum = 0;
        FFCode = 0;

        recipes[0, 0] = (int)Ingredient.Tteok;
        recipes[0, 1] = (int)Ingredient.Fish;

        recipes[1, 0] = (int)Ingredient.Tteok;
        recipes[1, 1] = (int)Ingredient.Pork;

        recipes[2, 0] = (int)Ingredient.Potato;
        recipes[2, 1] = (int)Ingredient.Rice;

        recipes[3, 0] = (int)Ingredient.Pork;
        recipes[3, 1] = (int)Ingredient.Rice;

        recipes[4, 0] = (int)Ingredient.Fish;
        recipes[4, 1] = (int)Ingredient.Doenjang;
    }
    public void pushIngredient(int num)
    {
        if(ingredients[pushnum] != num)
        {
            ingredients[pushnum] = num;

            if (pushnum == 0)
                pushnum++;
            else if (pushnum == 1)
                pushnum--;
        }        
    }
    public virtual void Cooking(bool _isCook)
    {
        isCooking = _isCook;
        if(isCooking)
        {
            for (int i = 0; i < 5; i++)
            {
                if ((ingredients[0] == recipes[i, 0] && ingredients[1] == recipes[i, 1])
                    || (ingredients[0] == recipes[i, 1] && ingredients[1] == recipes[i, 0]))
                {
                    isFood = true;
                    FFCode = i + 1;
                    break;
                }
                else
                {
                    FFCode = 6;
                    isFood = false;
                }
            } //Checking Recipe
            ingredients[0] = 0; ingredients[1] = 0;
        }
        else { pushnum = 0; FFCode = 0; isFood = false; isDone = false; }
    }
    public virtual void Done() { isCooking = false; isDone = true; }
    public int nowCount() {
        if (ingredients[0] == 0 && ingredients[1] == 0) { return 0; }
        else if (ingredients[0] != 0 && ingredients[1] == 0) { return 1; }
        else { return 2; }
    }
}