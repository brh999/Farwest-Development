using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : MonoBehaviour
{

    public int money = 0;
    public int wood;


    void Start()
    {
        
    }

    public void SetMoney(int amt)
    {
        money = amt;
    }

    public void AddMoney(int amt)
    {
        money = money + amt;
    }

    public void RemoveMoney(int amt)
    {
        money = money - amt;
    }

    public int CurrentMoney()
    {
        return money;
    }

}
