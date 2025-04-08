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
        public GachaItem(int num, string itemName, float weight, int rare)
        {
            ITEM_NUM = num;
            ITEM_NAME = itemName;
            WEIGHT = weight;
            RARE = rare;
        }

        public int ITEM_NUM { get; }
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

        public int ItemNumber;
        public string ItemName;
        public float ItemWeight;
        public int Rare;

        public string DebugStr; // debug

        [FlowInput]
        public Continuation Enter()
        {
            gachagacha();
            return Exit;
        }

        [DataOutput]
        [Label("ITEM_NUMBER")] // You can customize the port label
        public int ShowItemNumber() { return ItemNumber; }
        [DataOutput]
        [Label("ITEM_NAME")] // You can customize the port label
        public string ShowItemName() { return ItemName; }
        [DataOutput]
        [Label("ITEM_WEIGHT")] // You can customize the port label
        public float ShowItemWeight() { return ItemWeight; }
        [DataOutput]
        [Label("RARE")] // You can customize the port label
        public int ShowRare() { return Rare; }

        [DataOutput]
        [Label("DEBUG")] // You can customize the port label
        public string ShowDebug() { return DebugStr; } // debug

        // Usually, we name the default flow input "Exit".
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
            Watch(nameof(GachaItemList), SetupGachaDatabase);
            SetupGachaDatabase();
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

        public void SetupGachaDatabase()
        {
            // TODO 値検査
            foreach (string item in GachaItemList)
            {
                string[] propList = item.Split(',');
                int num = Convert.ToInt32(propList[0]);
                Add(
                    num,
                    new GachaItem(
                        num,
                        propList[1],
                        Convert.ToSingle(propList[2]),
                        Convert.ToInt32(propList[3])
                    )
                );
                // 0,testname,0.5,2
                // GachaDatabaseManage.data.Add(0, 0, new GachaItem(0, 0, "testname", 0.5f, 2));
            }
        }

        protected void gachagacha()
        {
            Dictionary<int, GachaItem> itemList = Get();

            float sumWeight = 0.0f;
            // TODO Dictionaryのforeachの書き方見直す
            foreach (GachaItem item in itemList) {
                sumWeight = sumWeight + item.WEIGHT;
            }

            float res = UnityEngine.Random.Range(0.0f, sumWeight);

            float nowWeight = 0.0f;
            foreach (GachaItem item in itemList) {
                nowWeight = nowWeight + item.WEIGHT;
                if (res <= nowWeight) {
                    ItemNumber = num;
                    ItemName = item.ITEM_NAME;
                    ItemWeight = item.WEIGHT;
                    Rare = item.RARE;
                }
            }

            GachaItem item = GetByNum(0);
            // TODO 値参照例外処理
        }
    }
}