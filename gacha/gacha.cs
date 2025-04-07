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
    public struct GachaDatabaseStruct {
        public GachaDatabaseStruct() {
            GACHA_ITEM_LIST = new Dictionary<(int, int), GachaItem[]>();
        }

        public Dictionary<(int, int), GachaItem[]> GACHA_ITEM_LIST { get; set; }
    }

    public static class GachaDatabase {
        public static GachaDatabaseStruct data { get; set; };

        public static add(GachaItem gachaItem) {
            gachaId = gachaItem.GACHA_ID;
            gachaItemNum = gachaItem.ITEM_NUMBER;
            if (data.GACHA_ITEM_LIST.Contains(gachaId, gachaItemNum)) {
                self.Remove(gachaId, gachaItemNum);
            }
            data.GACHA_ITEM_LIST.Add((gachaId, gachaItemNum), gachaItem);
        }

        public static remove(int gachaId, int gachaItemNum) {
            data.GACHA_ITEM_LIST.Remove(gachaId, gachaItemNum);
        }

        public static get(int gachaId, int gachaItemNum) {
            // TODO バリデーション
            return data.GACHA_ITEM_LIST[gachaId][gachaItemNum];
        }

        public static destroy() {
            // TODO 明示的にディクショナリは捨てた方がいいか？
            data.GACHA_ITEM_LIST = new Dictionary<(int, int), GachaItem[]>();
        }
    }

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

        [DataOutput]
        [Label("DEBUG_GACHA")]
        public string[] DebugGachaList; // でバック用

        public int counter = 0; // でバック用

        protected override void OnCreate() {
            base.OnCreate();
            Watch(nameof(GachaItemList), SetupGachaDatabase);
            SetupGachaDatabase();
        }
        
        public void SetupGachaDatabase() {
            GachaDatabase.destroy();
            foreach (string gachaItemStr in GachaItemList) {
                string[] gachaItemPropList = gachaItemStr.Split(',');
                // TODO バリデーション, ガチャID-景品Noユニーク判定
                Int32.TryParse(gachaItemPropList[0], out int id);
                Int32.TryParse(gachaItemPropList[1], out int num);
                string itemName = gachaItemPropList[2];
                float.TryParse(gachaItemPropList[3], out float weight);
                Int32.TryParse(gachaItemPropList[4], out int rare);
                GachaDatabase.add(
                    GachaItem(id, num, itemName, weight, rare);
                );
            }
            GachaDatabase.get(0,0); // TODO
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