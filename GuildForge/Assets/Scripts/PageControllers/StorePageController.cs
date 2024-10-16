using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class StorePageController
{
    VisualElement storePage;
    List<Item> storeInventory;
    Item selectedItem;

    public void InitializeStorePage(VisualElement root)
    {
        storePage = root;

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        storeInventory = new List<Item>(GameManager.IsRestockTime());

        FillStoreUI();

    }
    private void FillStoreUI()
    {
        int index = 0;
        for (int i = 1; i <= 5; i++)
        {
            
            VisualElement row = storePage.Q<VisualElement>("Row" + i);
            for (int j = 1; j <= 3; j++)
            {
                VisualElement buttonBox = row.Q<VisualElement>("Button" + j);
                Button icon = buttonBox.Q<Button>("IconButton");
                Label cost = buttonBox.Q<Label>("Cost");
                
                if (index < storeInventory.Count)
                {
                    icon.style.backgroundImage = (StyleBackground)storeInventory[index].Icon;
                    icon.text = null;
                    icon.userData = index;
                    icon.RegisterCallback<ClickEvent>(e => ItemDetails((int)icon.userData));

                    cost.text = storeInventory[index].Cost.ToString();
                }
                else
                {
                    icon.style.backgroundImage = null;
                    icon.style.backgroundColor = Color.white;
                    icon.text = "SOLD!";
                    cost.text = "0";
                }
                index++;
            }
        }
    }



    void ItemDetails(int index)
    {
        if(index < storeInventory.Count)
        {
            selectedItem = storeInventory[index];

            GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
            SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
            soundManager.ButtonSound();

            GameObject UIMaster = GameObject.FindGameObjectWithTag("UI Manager");
            UIMaster.GetComponent<UIManager>().ShowItemDetailsPopUp();
        }
    }
    #region Item Details PopUp
    VisualElement detailsPopUp;
    bool doOnce = true;
    public void InitializeDetailsPopUp(VisualElement popUp)
    {
        detailsPopUp = popUp;

        
        Button exitButton = detailsPopUp.Q<Button>("ExitButton");
        Button buyButton = detailsPopUp.Q<Button>("BuyButton");

        VisualElement detailsIcon = detailsPopUp.Q<VisualElement>("Icon");
        detailsIcon.style.backgroundImage = (StyleBackground)selectedItem.Icon;

        Label itemName = detailsPopUp.Q<Label>("ItemName");
        itemName.text = selectedItem.Name;


        Label itemHealth = detailsPopUp.Q<Label>("Health");
        itemHealth.text = selectedItem.HPBonus.ToString();

        Label itemMana = detailsPopUp.Q<Label>("Mana");
        itemMana.text = selectedItem.ManaBonus.ToString();

        Label itemAttack = detailsPopUp.Q<Label>("Attack");
        itemAttack.text = selectedItem.AtkBonus.ToString();

        Label itemDefense = detailsPopUp.Q<Label>("Defense");
        itemDefense.text = selectedItem.DefBonus.ToString();

        Label itemMagic = detailsPopUp.Q<Label>("Magic");
        itemMagic.text = selectedItem.MagBonus.ToString();

        Label itemSpeed = detailsPopUp.Q<Label>("Speed");
        itemSpeed.text = selectedItem.SpdBonus.ToString();

        Label itemCost = detailsPopUp.Q<Label>("CostLabel");
        itemCost.text = selectedItem.Cost.ToString();

        if(doOnce)
        {
            exitButton.clicked += ExitPopUp;
            buyButton.clicked += BuyItem;
        }

    }
    void BuyItem()
    {
        GameObject soundMaster = GameObject.FindGameObjectWithTag("SoundManager");
        SoundManager soundManager = soundMaster.GetComponent<SoundManager>();
        soundManager.MoneySound();

        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        if (GameManager.getCurGold() >= selectedItem.Cost)
        {
            GameManager.BuyItem(selectedItem);
            storeInventory.Remove(selectedItem);
            FillStoreUI();

            ExitPopUp();
        }
    }

    void ExitPopUp()
    {
        detailsPopUp.style.display = DisplayStyle.None;
    }
    #endregion
}
