using Warudo.Core.Attributes;
using Warudo.Core.Data;
using Warudo.Core.Graphs;
using Warudo.Core.Localization;

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

        public int GACHA_ID { get; init; }
        public int ITEM_NUMBER  { get; init; }
        public string ITEM_NAME { get; init; }
        public float WEIGHT { get; init; }
        public int RARE { get; init; }

        public override string ToString() => $"{GACHA_ID},{ITEM_NUMBER},{ITEM_NAME},{WEIGHT},{RARE}"
    }

    // ガチャデータベース
    public struct GachaDatabase {
        public GachaDatabase() {
            GACHA_ITEM_LIST = new Dictionary<int, GachaItem[]>();
        }

        public Add(int gachaId, GachaItem[] gachaItemList) {
            if (GACHA_ITEM_LIST.Contains(gachaId)) {
                self.Remove(gachaId);
            }
            GACHA_ITEM_LIST.Add(gachaId, gachaItemList);
        }

        public Remove(int gachaId) {
            GACHA_ITEM_LIST.Remove(gachaId);
        }

        public Get(int gachaId) {
            return GACHA_ITEM_LIST[gachaId];
        }

        public Dictionary<int, GachaItem[]> GACHA_ITEM_LIST { get; set; }
    }

    public static GachaDatabase gachaDatabase;

    // TODO カテゴリーをなんとかする
    [NodeType(
        Id = "684E705F-3C14-45F8-8A5C-E6666873BF93",
        Title = "setGachaItemList",
        Category = "Gacha"
    )]
    public class SetGachaItemList : Nodes {

        [DataInput]
        [Label("GACHA_ITEM_LIST")]
        public string[] GachaItemList;

        public int counter = 0;

        protected override void OnCreate() {
            base.OnCreate();
            Watch(nameof(GachaData), SetupGachaDatabase);
            SetupGachaDatabase();
        }
        
        public void SetupGachaDatabase() {
            // ここ
            foreach (string gachaItemStr in GachaItemList) {
                string[] gachaItemPropList = gachaItemStr.Split(',');
                // TODO バリデーション, ガチャID-景品Noユニーク判定
                Int32.TryParse(gachaItemPropList[0], out int gachaId);
                gachaItemPropList[1];
                gachaItemPropList[2];
                gachaItemPropList[3];
                gachaItemPropList[4];
            }
            gachaDatabase.Add();
        }
    }

    // TODO カテゴリーをなんとかする
    [NodeType(
        Id = "c8f02de5-169c-44f1-9466-598e01e68389",
        Title = "doGacha",
        Category = "Gacha"
    )]
    public class DoGacha : Node {

        [DataOutput]
        [Label("GACHA_DATA")]
        public string[] GachaData;

        protected override void OnCreate() {
            base.OnCreate();
        }
    }
}