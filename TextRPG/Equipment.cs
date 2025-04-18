using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TextRPG.Program;

namespace TextRPG
{
    public static class EquipmentDatabase
    {
        public static readonly EquipmentData[] DataArray = new EquipmentData[]
        {
            new EquipmentData("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", EquipmentType.Armor, 5, 1000),                   //0
            new EquipmentData("무쇠 갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", EquipmentType.Armor, 9, 1500),                 //1
            new EquipmentData("스파르타 갑옷", "스파르타 전사들이 사용했던 전설의 갑옷입니다.", EquipmentType.Armor, 15, 3500),  //2
            new EquipmentData("낡은 검", "쉽게 볼 수 있는 낡은 검입니다.", EquipmentType.Weapon, 2, 600),                      //3
            new EquipmentData("청동 도끼", "어디선가 사용됐던 것 같은 도끼입니다.", EquipmentType.Weapon, 5, 1500),             //4
            new EquipmentData("스파르타 창", "스파르타 전사들이 사용했던 전설의 창입니다.", EquipmentType.Weapon, 7, 3500),      //5
            new EquipmentData("첫 번째 검", "내가 추가한 첫 번째 검입니다.", EquipmentType.Weapon, 10, 4000),      //6
            new EquipmentData("두 번째 검", "내가 추가한 두 번째 검입니다.", EquipmentType.Weapon, 15, 5000),      //7
            new EquipmentData("첫 번째 갑옷", "내가 추가한 첫 번째 갑옷입니다.", EquipmentType.Armor, 10, 4000),      //8
            new EquipmentData("두 번째 갑옷", "내가 추가한 두 번째 갑옷입니다.", EquipmentType.Armor, 15, 5000)       //9
        };
    }

    public class EquipmentData
    {
        public string Name;
        public string Description;
        public EquipmentType Type;
        public int AddStatus;
        public int Price;

        public EquipmentData(string name, string description, EquipmentType type, int addStatus, int price)
        {
            Name = name;
            Description = description;
            Type = type;
            AddStatus = addStatus;
            Price = price;
        }
    }

    public class Equipment
    {
        //배열 인덱스
        private int _id;
        private bool _isEquipped;

        public EquipmentData Data => EquipmentDatabase.DataArray[_id];

        public int DataId => _id;

        public bool IsEquipped => _isEquipped;
        public bool IsPurchased
        {
            get { return Manager.Instance.Inventory.HasEquipment(_id); }
        }

        public event Action<Equipment> OnEquippedChanged;
        public event Action<Equipment> OnPurchasedChanged;

        public Equipment(int id)
        {
            _id = id;
        }

        public void SetEquipped(bool value)
        {
            _isEquipped = value;
            OnEquippedChanged?.Invoke(this);
        }

        public void PurchaseEquipment()
        {
            Manager.Instance.Inventory.AddEquipment(this);
        }

        public void SaleEquipment()
        {
            Manager.Instance.Inventory.RemoveEquipment(_id);
        }

        public void DisplayInfo(EquipmentDisplayMode mode, bool showIdx = false, int index = 0)
        {
            string type = (Data.Type == EquipmentType.Armor ? "방어력" : "공격력");
            string idx = (showIdx ? $"{index}" : "");
            string name = $"{(IsEquipped ? "(E)" : "")}{Data.Name,-15}";
            string status = mode switch
            {
                EquipmentDisplayMode.Inventory => $"{type} +{Data.AddStatus}\t| {Data.Description}",
                EquipmentDisplayMode.Shop => $"{type} +{Data.AddStatus}\t| {Data.Description,-20}\t| {(IsPurchased ? "구매 완료" : $"{Data.Price}G")}",
                EquipmentDisplayMode.Sale => $"{type} +{Data.AddStatus}\t| {Data.Description,-20}\t| {Data.Price}G",
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };
            Console.WriteLine($"- {idx} {name}\t| {status}");

        }
    }
}
