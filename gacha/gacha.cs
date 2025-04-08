using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Localization;
using System.Collections.Generic;

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
            this.GACHA_ITEM_LIST = new Dictionary<(int, int), GachaItem>();
        }

        public void Add(int gachaId, int itemNumber, GachaItem gachaItem)
        {
            this.GACHA_ITEM_LIST.Add((gachaId, itemNumber), gachaItem);
        }

        public GachaItem Get()
        {
            return this.GACHA_ITEM_LIST[(0, 0)];
        }

        public Dictionary<(int, int), GachaItem> GACHA_ITEM_LIST { get; set; }
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
        public string GachaItemList;

        public int counter = 0; // でバック用

        protected override void OnCreate()
        {
            base.OnCreate();
            Watch(nameof(GachaItemList), SetupGachaDatabase);
            SetupGachaDatabase();
        }

        public void SetupGachaDatabase()
        {
            // TODO DataInputからの値を格納
            GachaDatabaseManage.data.Add(0, 0, new GachaItem(0, 0, "testname", 0.5f, 2));
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
            ItemNumber = 2;
            ItemName = "test";
            ItemWeight = 3.5f;
            Rare = 4;
        }
    }
}