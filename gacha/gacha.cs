using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Localization;
using System.Collections.Generic;
using System;

namespace Warudo.Plugins.Core.Nodes
{

    // ガチャアイテム構造体
    public readonly struct GachaItem
    {
        public GachaItem(string itemName, float weight, int rare)
        {
            ITEM_NAME = itemName;
            WEIGHT = weight;
            RARE = rare;
        }

        public string ITEM_NAME { get; }
        public float WEIGHT { get; }
        public int RARE { get; }
    }


    [NodeType(
        Id = "0ac03086-79f1-d79a-1578-0cd1f6e696f1",
        Title = "RegisterGachaItem",
        Category = "Gacha"
    )]
    public class RegisterGachaItem : Node
    {
        [DataInput]
        [Label("Item1")]
        public string Item1;

        [DataInput]
        [Label("Item2")]
        public string Item2;

        [DataInput]
        [Label("Item3")]
        public string Item3;

        [DataInput]
        [Label("Item4")]
        public string Item4;

        [DataInput]
        [Label("Item5")]
        public string Item5;

        [DataInput]
        [Label("Item6")]
        public string Item6;

        [DataInput]
        [Label("Item7")]
        public string Item7;

        [DataInput]
        [Label("Item8")]
        public string Item8;

        [DataInput]
        [Label("Item9")]
        public string Item9;

        [DataOutput]
        [Label("GachaItemList")]
        public string[] OutputRegisterGahcaItemList()
        {
            return RegisterGahcaItemList.ToArray();
        }
        public List<string> RegisterGahcaItemList = new List<string>();

        [DataOutput]
        [Label("Counter")]
        public int ShowCounter() { return Counter; }
        public int Counter;

        protected override void OnCreate()
        {
            base.OnCreate();
            Watch(nameof(Item1), Makeup);
            Watch(nameof(Item2), Makeup);
            Watch(nameof(Item3), Makeup);
            Watch(nameof(Item4), Makeup);
            Watch(nameof(Item5), Makeup);
            Watch(nameof(Item6), Makeup);
            Watch(nameof(Item7), Makeup);
            Watch(nameof(Item8), Makeup);
            Watch(nameof(Item9), Makeup);
            Makeup();
        }

        public void Makeup()
        {
            Counter++;
            RegisterGahcaItemList.Clear();
            AddList(Item1);
            AddList(Item2);
            AddList(Item3);
            AddList(Item4);
            AddList(Item5);
            AddList(Item6);
            AddList(Item7);
            AddList(Item8);
            AddList(Item9);
        }

        public void AddList(string item)
        {
            if (item == null)
            {
                return;
            }
            string[] propList = item.Split(',');
            if (propList.Length >= 4)
            {
                int num = 0;
                string name = propList[1];
                float weight = 0.0f;
                int rare = 0;
                if (
                    int.TryParse(propList[0], out num) &&
                    float.TryParse(propList[2], out weight) &&
                    int.TryParse(propList[3], out rare) &&
                    weight > 0
                    )
                {
                    RegisterGahcaItemList.Add(item);
                }
            }
        }
    }

    [NodeType(
        Id = "5b4080a5-9e9d-4f22-87fe-ac69536d5360",
        Title = "MakeGachaItem",
        Category = "Gacha"
    )]
    public class MakeGachaItem : Node
    {
        [DataInput]
        [Label("ItemNumber")]
        public int ItemNumber = 1;

        [DataInput]
        [Label("ItemName")]
        public string ItemName = "SR";

        [DataInput]
        [Label("Weight")]
        public float Weight = 30;

        [DataInput]
        [Label("Rare")]
        public int Rare = 1;

        [DataOutput]
        [Label("GachaItem")]
        public string OutputGachaItem() { return GachaItem; }
        public string GachaItem;

        protected override void OnCreate()
        {
            base.OnCreate();
            Watch(nameof(ItemNumber), Makeup);
            Watch(nameof(ItemName), Makeup);
            Watch(nameof(Weight), Makeup);
            Watch(nameof(Rare), Makeup);
            Makeup();
        }

        public void Makeup()
        {
            // TODO バリデーション Weight>0
            string[] words = { ItemNumber.ToString(), ItemName, Weight.ToString(), Rare.ToString() };
            GachaItem = string.Join(",", words);
        }
    }


    // TODO カテゴリーをなんとかする
    [NodeType(
        Id = "1761f98b-2703-a91a-e094-8da9a2641180",
        Title = "doGacha",
        Category = "Gacha"
    )]
    public class DoGacha : Node
    {

        [DataInput]
        [Label("GACHA_ITEM_LIST")]
        public string[] GachaItemList;

        [FlowInput]
        public Continuation Gacha()
        {
            GachaGacha();
            return Exit;
        }

        [FlowInput]
        public Continuation Update()
        {
            SetupGachaDatabase();
            return null;
        }

        public int ItemNumber;
        public string ItemName;
        public float ItemWeight;
        public int Rare;

        [DataOutput]
        [Label("ITEM_NUMBER")]
        public int ShowItemNumber() { return ItemNumber; }
        [DataOutput]
        [Label("ITEM_NAME")]
        public string ShowItemName() { return ItemName; }
        [DataOutput]
        [Label("ITEM_WEIGHT")]
        public float ShowItemWeight() { return ItemWeight; }
        [DataOutput]
        [Label("RARE")]
        public int ShowRare() { return Rare; }

        [FlowOutput]
        public Continuation Exit;

        public Dictionary<int, GachaItem> GACHA_ITEM_LIST;

        public DoGacha()
        {
            this.GACHA_ITEM_LIST = new Dictionary<int, GachaItem>();
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            // StringリストだとWatchが回り続けるので一旦やめる
            // Watch(nameof(GachaItemList), SetupGachaDatabase);
            // SetupGachaDatabase();
        }

        public void InitDb()
        {
            this.GACHA_ITEM_LIST = new Dictionary<int, GachaItem>();
        }

        public void Add(int num, GachaItem gachaItem)
        {
            this.GACHA_ITEM_LIST.Add(num, gachaItem);
        }

        public Dictionary<int, GachaItem> Get()
        {
            return this.GACHA_ITEM_LIST;
        }

        public GachaItem GetByNum(int num)
        {
            return this.GACHA_ITEM_LIST[num];
        }

        public void InitGachaResult()
        {
            ItemNumber = 0;
            ItemName = null;
            ItemWeight = 0.0f;
            Rare = 0;
        }

        public void SetupGachaDatabase()
        {
            InitDb();
            foreach (string item in GachaItemList)
            {
                string[] propList = item.Split(',');
                int num = 0;
                string name = propList[1];
                float weight = 0.0f;
                int rare = 0;
                if (
                    int.TryParse(propList[0], out num) &&
                    float.TryParse(propList[2], out weight) &&
                    int.TryParse(propList[3], out rare) &&
                    weight > 0
                    )
                {
                    Add(
                        num,
                        new GachaItem(
                            name,
                            weight,
                            rare
                        )
                    );
                }
            }
        }

        protected void GachaGacha()
        {
            InitGachaResult();

            Dictionary<int, GachaItem> itemList = Get();

            float sumWeight = 0.0f;
            foreach (KeyValuePair<int, GachaItem> item in itemList)
            {
                sumWeight = sumWeight + item.Value.WEIGHT;
            }

            float res = UnityEngine.Random.Range(0.0f, sumWeight);

            float nowWeight = 0.0f;
            foreach (KeyValuePair<int, GachaItem> item in itemList)
            {
                nowWeight = nowWeight + item.Value.WEIGHT;
                if (res <= nowWeight)
                {
                    ItemNumber = item.Key;
                    ItemName = item.Value.ITEM_NAME;
                    ItemWeight = item.Value.WEIGHT;
                    Rare = item.Value.RARE;
                    break;
                }
            }
        }
    }
}