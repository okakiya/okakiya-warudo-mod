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
                float weight = Convert.ToSingle(propList[2]);
                if (weight > 0)
                {
                    Add(
                        Convert.ToInt32(propList[0]),
                        new GachaItem(
                            propList[1],
                            weight,
                            Convert.ToInt32(propList[3])
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