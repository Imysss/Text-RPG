using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Manager
    {
        private static Manager _instance;
        public static Manager Instance => _instance ??= new Manager();

        public SceneManager Scene { get; private set; }
        public Character Character { get; private set; }
        public Inventory Inventory { get; private set; }
        public Store Store { get; private set; }
        public SaveManager SaveManager { get; private set; }

        public void Init()
        {
            Character = new Character();
            Character.Init();

            Inventory = new Inventory();
            Store = new Store();
            SaveManager = new SaveManager();

            Scene = new SceneManager();
            Scene.Init();
        }

        public void SaveGame()
        {
            SaveData data = Character.CreateSaveData(Inventory.GetInventoryItemIds(), Inventory.GetEquippedWeaponId(), Inventory.GetEquippedArmorId());
            SaveManager.Save(data);
        }

        public void LoadGame()
        {
            SaveData data = SaveManager.Load();
            if (data == null)
            {
                Console.WriteLine("데이터 엄슴");
                return;
            }

            Console.WriteLine("데이터 받아오는 중");
            Character.LoadSaveData(data);

            Inventory.Clear();
            foreach (var id in data.InventoryItemIds)
            {
                Equipment equip = new Equipment(id);
                Inventory.AddEquipment(equip);
            }

            if (data.EquippedWeaponIdx != null)
            {
                int dataId = data.EquippedWeaponIdx.Value;
                int invenIdx = Inventory.Equipments.FindIndex(e => e.Data == EquipmentDatabase.DataArray[dataId]);
                if (invenIdx != -1)
                {
                    Inventory.Equip(Inventory.Equipments[invenIdx], invenIdx);
                }
            }

            if (data.EquippedArmorIdx != null)
            {
                int dataId = data.EquippedArmorIdx.Value;
                int invenIdx = Inventory.Equipments.FindIndex(e => e.Data == EquipmentDatabase.DataArray[dataId]);
                if (invenIdx != -1)
                {
                    Inventory.Equip(Inventory.Equipments[invenIdx], invenIdx);
                }
            }
        }
    }
}
