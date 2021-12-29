using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyScript : MonoBehaviour
{

    private GameObject moneyObject;
    private ResourceSystem moneyHandler;
    public TMP_Text moneyText;

    void Start()
    {
        moneyObject = GameObject.Find("ResourceSystem");
        moneyHandler = moneyObject.GetComponent<ResourceSystem>();
    }


    public void AddMoneyButton()
    {
        moneyHandler.AddMoney(10);
    }

    public void RemoveMoneyButton()
    {

        moneyHandler.RemoveMoney(10);
        
     //    if(moneyHandler.CurrentMoney() >= 0)
     //   {

    //     }
    }


    // Update is called once per frame
    void Update()
    {
        moneyText.text = moneyHandler.CurrentMoney().ToString() + "$";
    }


}
