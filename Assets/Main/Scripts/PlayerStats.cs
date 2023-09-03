using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI chipsText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI researchText;
    [SerializeField] TextMeshProUGUI genText;

    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI backpackSizeText;

    [SerializeField] List<RectTransform> trans;

    public static PlayerStats instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        newGenerationGoal = FactoryBuilding.GetNewGenerationGoal(1);

        Money = money;
        Chips = chips;
        MaxGeneration = maxGeneration;
        ChipInventorySize = chipInventorySize;
        CurrentResearch = currentResearch;
        Speed = speed;
        ToolMode = ChipToolMode.None;
    }

    private int money = 284; //000
    private int chips;
    private int chipInventorySize = 10;
    private int maxGeneration = 1;
    private int currentResearch = 0;
    private int speed = 50;
    public int newGenerationGoal;
    private ChipToolMode toolMode;

    public int Money { get => money; set { money = value; moneyText.text = $"{money}"; } }
    public int Chips { get => chips; set { chips = value; chipsText.text = $"{chips}/{chipInventorySize}"; } }
    public int MaxGeneration { get => maxGeneration; set { maxGeneration = value; genText.text = $"{maxGeneration}"; } }
    public int ChipInventorySize { get => chipInventorySize; 
        set { 
            chipInventorySize = value;
            chipsText.text = $"{chips}/{chipInventorySize}";
            backpackSizeText.text = $"{chipInventorySize}";
        } 
    }

    public int CurrentResearch { get => currentResearch; 
        set { 
            currentResearch = value; 
            while (currentResearch > newGenerationGoal)
            {
                currentResearch -= newGenerationGoal;
                MaxGeneration += 1;
                newGenerationGoal = FactoryBuilding.GetNewGenerationGoal(maxGeneration);

            }
            researchText.text = $"{currentResearch}/{newGenerationGoal}"; 
        } 
    }

    public int Speed { get => speed; set { speed = value; speedText.text = $"{speed}"; } }

    public ChipToolMode ToolMode { get => toolMode; 
        set { 
            toolMode = value;
            for(int i = 0; i < trans.Count; i++)
            {
                var tran = trans[(i + (int)toolMode) % trans.Count];
                var pos = tran.localPosition;
                pos.x = -160 + 160 * i;
                tran.localPosition = pos;
            }
        } 
    }

    public enum ChipToolMode
    {
        None,
        In,
        Out,
    }

}
