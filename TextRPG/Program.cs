namespace TextRPG
{
    public enum SceneType
    {
        NewGameScene,
        SaveGameScene,
        StartScene,
        ChacterInfoScene,
        InventoryScene,
        EquipmentScene,
        StoreScene,
        PurchaseScene,
        SaleScene,
        RestScene,
        DungeonScene,
        DungeonClearScene,
    };

    public enum CharacterClass
    {
        Warrior,
        Magician,
        Archer,
    }

    public enum EquipmentType
    {
        Armor,
        Weapon,
    }

    public enum Difficulty
    { 
        None,
        Easy,
        Normal,
        Hard,
        Error,
    }

    public enum EquipmentDisplayMode
    {
        Inventory,
        Shop,
        Sale,
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Manager.Instance.Init();

            Console.WriteLine("스파르타 Text RPG 게임\n");

            Console.WriteLine("1. 새로 시작");
            Console.WriteLine("2. 불러오기\n");

            Console.Write(">> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;

            if (input == 1)
            {
                Manager.Instance.Scene.LoadScene(SceneType.NewGameScene);
            }
            else if (input == 2)
            {
                Manager.Instance.LoadGame();
                Manager.Instance.Scene.LoadScene(SceneType.SaveGameScene);
            }
        }
    }
}