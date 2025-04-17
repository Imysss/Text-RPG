using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TextRPG.Program;

namespace TextRPG
{
    internal class Character
    {
        private string _name;
        private CharacterClass _class;
        private int _level;
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                LevelUpStat();
            }
        }
              

        private float _baseAttack;
        private int _bonusAttack;

        private float _baseDefense;
        private int _bonusDefense;

        public int HP { get; set; }
        public int Gold { get; set; }

        public float Attack => _baseAttack + _bonusAttack;
        public float Defense => _baseDefense + _bonusDefense;

        public void Init()
        {
            _name = "Chad";
            _class = CharacterClass.Warrior;
            Level = 1;

            _baseAttack = 10;
            _bonusAttack = 0;

            _baseDefense = 5;
            _bonusDefense = 0;

            HP = 100;
            Gold = 5000;
        }

        public void SetCharacterInfo(string name, int classInput)
        {
            _name = name;

            switch(classInput)
            {
                case 1:
                    _class = CharacterClass.Warrior;
                    break;
                case 2:
                    _class = CharacterClass.Magician;
                    break;
                case 3:
                    _class = CharacterClass.Archer;
                    break;
                default:
                    _class = CharacterClass.Warrior;
                    break;
            }
        }

        public void ShowCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{_name} ({GetClassName(_class)})");
            Console.WriteLine($"공격력: {Attack} " + (_bonusAttack > 0 ? $"(+{_bonusAttack})" : ""));
            Console.WriteLine($"방어력: {Defense} " + (_bonusDefense > 0 ? $"(+{_bonusDefense})" : ""));
            Console.WriteLine($"체력: {HP}");
            Console.WriteLine($"Gold: {Gold}G");
        }

        //장착 장비 능력치 적용
        public void ApplyEquipmentStat(Equipment equip)
        {
            int sign = equip.IsEquipped ? 1 : -1;
            switch(equip.Data.Type)
            {
                case EquipmentType.Armor:
                    _bonusDefense += sign * equip.Data.AddStatus;
                    break;
                case EquipmentType.Weapon:
                    _bonusAttack += sign * equip.Data.AddStatus;
                    break;
            }
        }

        //레벨 업 시 스탯 상승
        public void LevelUpStat()
        {
            _baseAttack += 0.5f;
            _baseDefense += 1f;
        }

        //직업 이름 받아오기
        private string GetClassName(CharacterClass type)
        {
            return type switch
            {
                CharacterClass.Warrior => "전사",
                CharacterClass.Magician => "마법사",
                CharacterClass.Archer => "궁수",
                _ => "무직",
            };
        }

        //데이터 저장하기
        public SaveData CreateSaveData(List<int> inventoryIds, int? weaponIdx, int? armorIdx)
        {
            return new SaveData
            {
                Name = _name,
                Class = _class,
                Level = _level,
                BaseAttack = _baseAttack,
                BaseDefense = _baseDefense,
                HP = HP,
                Gold = Gold,
                InventoryItemIds = inventoryIds,
                EquippedWeaponIdx = weaponIdx,
                EquippedArmorIdx = armorIdx,
            };
        }

        public void LoadSaveData(SaveData data)
        {
            _name = data.Name;
            _class = data.Class;
            _level = data.Level;
            _baseAttack = data.BaseAttack;
            _baseDefense = data.BaseDefense;
            HP = data.HP;
            Gold = data.Gold;
        }
    }

}
