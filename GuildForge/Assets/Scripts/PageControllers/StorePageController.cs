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
        if (GameManager.IsRestockTime())
        {
            FillStoreInventory();
        }

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

    private void FillStoreInventory()
    {
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();

        //get all items
        List<Item> allItems  = new List<Item>();
        allItems.AddRange(Resources.LoadAll<Item>("Items"));

        //filter items based on rank
        List<Item> filteredItems = new List<Item>();
        foreach (Item item in allItems)
        {
            switch(GameManager.GetGuildRank())
            {
                case GameState.GuildRank.S:
                    {
                        if(item.itemRank == Item.ItemRank.S || item.itemRank == Item.ItemRank.A)
                        {
                            filteredItems.Add(item);
                        }
                    }break;
                case GameState.GuildRank.A:
                    {
                        if (item.itemRank == Item.ItemRank.S || item.itemRank == Item.ItemRank.S || item.itemRank == Item.ItemRank.B)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
                case GameState.GuildRank.B:
                    {
                        if (item.itemRank == Item.ItemRank.A || item.itemRank == Item.ItemRank.B || item.itemRank == Item.ItemRank.C)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
                case GameState.GuildRank.C or GameState.GuildRank.D:
                    {
                        if (item.itemRank == Item.ItemRank.B || item.itemRank == Item.ItemRank.D || item.itemRank == Item.ItemRank.C)
                        {
                            filteredItems.Add(item);
                        }
                    }
                    break;
            }
        }

        //if there are more than 15 items random selection sample down to 15 if not just add them all to sotre inventory
        if (filteredItems.Count > 15)
        {
            storeInventory = new List<Item>();
            for (int i = 0; i < filteredItems.Count; i++)
            {
                if (storeInventory.Count < 15)
                {
                    if (UnityEngine.Random.Range(0,filteredItems.Count - i) < 15 - storeInventory.Count)
                    {
                        storeInventory.Add(filteredItems[i]);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            storeInventory = new List<Item>(filteredItems);
        }
        
        //if we are short duplicate items to fill out the list
        if (storeInventory.Count < 15)
        {
           for(int i = 0;i < 15 - storeInventory.Count;i++)
            {
                storeInventory.Add(storeInventory[i]);
            }
        }
    }

    void ItemDetails(int index)
    {
        if(index < storeInventory.Count)
        {
            selectedItem = storeInventory[index];

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
        GameObject GameMaster = GameObject.FindGameObjectWithTag("GameController");
        GameManager GameManager = GameMaster.GetComponent<GameManager>();
        if (GameManager.getCurGold() >= selectedItem.Cost)
        {
            selectedItem.BuyItem();
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
