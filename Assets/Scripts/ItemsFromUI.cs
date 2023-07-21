using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsFromUI : MonoBehaviour
{
    [SerializeField] private List<Transform> Items;
    [SerializeField] private List<Transform> ItemsFinalPos;

    private List<int> itemIdList = new List<int>();

    [SerializeField] private Items itemsScript;

    [SerializeField] private GameObject wings;

    public void AddID(int itemId)
    {
        Debug.Log(itemId);
        itemIdList.Add(itemId);
        
    }
    
    public void AnimateItem()
    {
        for (int i = 0; i < itemIdList.Count; i++)
        {
            if (itemIdList[i] < 5)
            {
                // color
                itemsScript.ChangeColor();
                Debug.Log("Farbaj");
            }
            else if (itemIdList[i] == 6)
            {
                itemsScript.ShowItem(Items[itemIdList[i]].gameObject);
            }
            else if (itemIdList[i] >= 5 && itemIdList[i] < 10 && itemIdList[i] != 6)
            {
                itemsScript.ShowItem(Items[itemIdList[i]], ItemsFinalPos[itemIdList[i]]);
                Debug.Log("ISKACU ITEMI");
            }
            else
            {
                // magic
                itemsScript.Magic();
                Debug.Log("MAGIJA");
            }
        }

        itemIdList.Clear();
    }
}

// animacija kako jede, saceka da pojede, FUNKCIJA ZA IZRASLINU, vrati se kamera i iskoci sledeci UI;
// posle treceg pojedenog item-a sta raditi sa kamerom
// kako pratiti koji je item stavljen u zdjelu da bi se kasnije zvala funckija za izraslinu
// Iteme u zdjelu destrojati kad krene da se melje

// napravi se prazna lista<int>
// u nju stavljaju ID-ijevi (svaki item da ima razlicit id)
// funkcija koja puni tu listu da se poziva na objectfollow kad pogodis zdjelu
// ako imas jos 2 liste, jedna sa objektima koji izrastaju a drugim gdje treba da se pomjeri
// da svaki id item-a ubacenog uzdjelu odgovara na mjesta iz druge 2 liste
// a za iteme koji ce imati animaciju za njihove id-e staviti if condition
// svaki od njih kaciti na armaturu modela