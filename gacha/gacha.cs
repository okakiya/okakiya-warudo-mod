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
        public GachaItem(int gachaId, int itemNumber, string itemName, float weight, int rare)
        {
            GACHA_ID = gachaId;
            ITEM_NUMBER = itemNumber;
            ITEM_NAME = itemName;
            WEIGHT = weight;
            RARE = rare;
        }

        public int GACHA_ID { get; }
        public int ITEM_NUMBER { get; }
        public string ITEM_NAME { get; }
        public float WEIGHT { get; }
        public int RARE { get; }
    }

    // ガチャデータベース
    public class GachaDatabase
    {
        public GachaDatabase()
        {
            this.GACHA_ITEM_LIST = new Dictionary<int, GachaItem[]>();
        }

        public void Add(int gachaId, GachaItem gachaItem)
        {
            GachaItem[] source = Get(gachaId);
            GachaItem[] dest = new GachaItem[source.Length + 1];
            Array.Copy(source, dest, source.Length);
            dest[dest.Length - 1] = gachaItem;
            this.GACHA_ITEM_LIST.Add(gachaId, dest);
        }

        public GachaItem[] Get(int id)
        {
            return this.GACHA_ITEM_LIST[id];
        }

        public Dictionary<int, GachaItem[]> GACHA_ITEM_LIST { get; set; }
    }

    public static class GachaDatabaseManage
    {
        public static GachaDatabase data = new GachaDatabase();
    }

    // TODO カテゴリーをなんとかする
    [NodeType(
        Id = "684E705F-3C14-45F8-8A5C-E6666873BF93",
        Title = "setGachaItemList",
        Category = "Gacha"
    )]
    public class SetGachaItemList : Node
    {

        [DataInput]
        [Label("GACHA_ITEM_LIST")]
        public string[] GachaItemList;

        protected override void OnCreate()
        {
            base.OnCreate();
            Watch(nameof(GachaItemList), SetupGachaDatabase);
            SetupGachaDatabase();
        }

        public void SetupGachaDatabase()
        {
            foreach (string item in GachaItemList)
            {
                string[] propList = item.Split(',');
                int id = Convert.ToInt32(propList[0]);
                int num = Convert.ToInt32(propList[1]);
                GachaDatabaseManage.data.Add(
                    id,
                    new GachaItem(
                        id,
                        num,
                        propList[2],
                        Convert.ToSingle(propList[3]),
                        Convert.ToInt32(propList[4])
                    )
                );
                // 0,0,testname,0.5,2
                // GachaDatabaseManage.data.Add(0, 0, new GachaItem(0, 0, "testname", 0.5f, 2));
            }
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
        [Label("GACHA_ID")]
        public int GachaId;

        public int ItemNumber;
        public string ItemName;
        public float ItemWeight;
        public int Rare;

        [FlowInput]
        public Continuation Enter()
        {
            gachagacha(this.GachaId);
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

        // Usually, we name the default flow input "Exit".
        [FlowOutput]
        public Continuation Exit;

        protected void gachagacha(int id)
        {
            // GachaItem[] itemList = GachaDatabaseManage.data.Get(id);
            // foreach (GachaItem item in itemList) {
            //     // TODO
            // }
            GachaDatabaseManage.data.Add(
                id,
                new GachaItem(
                    id,
                    2,
                    "tesw",
                    2.3f,
                    5
                )
            );

            // TODO 値参照例外処理
            ItemNumber = 1;
            ItemName = "trsssafvasfe";
            ItemWeight = 4.4f;
            Rare = 2;
        }
    }
}