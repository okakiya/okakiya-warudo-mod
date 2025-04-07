using UnityEngine;
using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Localization;
using System.Collections.Generic;

namespace Warudo.Plugins.Core.Nodes {

    // ガチャアイテム構造体
    public readonly struct GachaItem {
        public GachaItem(int gachaId, int itemNumber, string itemName, float weight, int rare) {
            GACHA_ID = gachaId;
            ITEM_NUMBER = itemNumber;
            ITEM_NAME = itemName;
            WEIGHT = weight;
            RARE = rare;
        }

        public int GACHA_ID { get; }
        public int ITEM_NUMBER  { get; }
        public string ITEM_NAME { get; }
        public float WEIGHT { get; }
        public int RARE { get; }
    }

    // ガチャデータベース
    public class GachaDatabase {
        public GachaDatabase() {
            this.GACHA_ITEM_LIST = new Dictionary<(int, int), GachaItem>();
        }

        public void Add(int gachaId, int itemNumber, GachaItem gachaItem) {
            this.GACHA_ITEM_LIST.Add((gachaId, itemNumber), gachaItem);
        }

        public GachaItem Get() {
            return this.GACHA_ITEM_LIST[(0,0)];
        }

        public Dictionary<(int, int), GachaItem> GACHA_ITEM_LIST { get; set; }
    }

    public static class GachaDatabaseManage {
        public static GachaDatabase data = new GachaDatabase();
    }

    // TODO カテゴリーをなんとかする
    [NodeType(
        Id = "684E705F-3C14-45F8-8A5C-E6666873BF93",
        Title = "setGachaItemList",
        Category = "Gacha"
    )]
    public class SetGachaItemList : Node {

        [DataInput]
        [Label("GACHA_ITEM_LIST")]
        public string GachaItemList;

        public int counter = 0; // でバック用

        protected override void OnCreate() {
            base.OnCreate();
            Watch(nameof(GachaItemList), SetupGachaDatabase);
            SetupGachaDatabase();
        }
        
        public void SetupGachaDatabase() {
            GachaDatabaseManage.data.Add(0, 0, new GachaItem(0, 0, "testname", 0.5f, 2));
        }
    }
}