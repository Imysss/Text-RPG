using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Inventory
    {
        private const int NONE_EQUIPPED_INDEX = -1;

        public List<Equipment> Equipments { get; private set; } = new List<Equipment>();

        private int _equippedWeaponIdx = NONE_EQUIPPED_INDEX;
        private int _equippedArmorIdx = NONE_EQUIPPED_INDEX;

        public int GetEquippedWeaponId()
        {
            if (_equippedWeaponIdx == NONE_EQUIPPED_INDEX)
                return -1;
            return Equipments[_equippedWeaponIdx].DataId;
        }

        public int GetEquippedArmorId()
        {
            if (_equippedArmorIdx == NONE_EQUIPPED_INDEX) return -1;
            return Equipments[_equippedArmorIdx].DataId;
        }

        public bool HasEquipment(int dataId)
        {
            return Equipments.Any(e => e.DataId == dataId);
        }

        //?
        public List<int> GetInventoryItemIds()
        {
            return Equipments.Select(e => e.DataId).ToList();
        }

        public void AddEquipment(Equipment equip)
        {
            //중복 방지
            if (!Equipments.Any(e => e.DataId == equip.DataId))
            {
                equip.OnEquippedChanged -= Manager.Instance.Character.ApplyEquipmentStat; // 기존 이벤트 제거
                equip.OnEquippedChanged += Manager.Instance.Character.ApplyEquipmentStat;
                Equipments.Add(equip);
            }
        }

        //장비 판매 
        public void RemoveEquipment(int id)
        {
            // 해당 ID를 가진 장비를 찾아서 리스트에서 제거
            Equipment equip = Equipments.FirstOrDefault(e => e.DataId == id);

            if (equip == null)
                return;

            if (equip.IsEquipped)
            {
                equip.SetEquipped(false);
            }

            equip.OnEquippedChanged -= Manager.Instance.Character.ApplyEquipmentStat;
            Equipments.Remove(equip);
        }

        //장비 장착 함수
        public void Equip(Equipment equipment, int idx)
        {
            ref int equippedIndex = ref GetEquippedIndexRef(equipment.Data.Type);

            //현재 장착 중인 장비 선택 시 -> 장착 해제
            if(equippedIndex == idx)
            {
                equipment.SetEquipped(false);
                equippedIndex = NONE_EQUIPPED_INDEX;
                return;
            }

            //장착 중인 장비가 아니라면? 장착 중인 장비 장착 해제, 선택한 장비 장착
            if (equippedIndex != NONE_EQUIPPED_INDEX)
            {
                Equipments[equippedIndex].SetEquipped(false);
            }

            equipment.SetEquipped(true);
            equippedIndex = idx;
        }

        //값을 반환하는 것이 아니라 _equippedWeaponIdx 또는 _equippedArmorIdx라는 변수 자체에 대한 참조를 반환
        //이 값은 복사된 숫자가 아닌 원본 변수에 직접 접근하는 포인터처럼 작동
        private ref int GetEquippedIndexRef(EquipmentType type)
        {
            switch(type)
            {
                case EquipmentType.Weapon:
                    return ref _equippedWeaponIdx;
                case EquipmentType.Armor:
                    return ref _equippedArmorIdx;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Unknown equipment type");
            }
        }


        public void ShowInventory()
        {
            foreach (Equipment equip in Equipments)
            {
                equip.DisplayInfo(EquipmentDisplayMode.Inventory);
            }
        }

        public void ShowEquipmentState()
        {
            int idx = 1;
            foreach (Equipment equip in Equipments)
            {
                //idx 보여주기
                equip.DisplayInfo(EquipmentDisplayMode.Inventory, true, idx++);
            }
        }

        public void ShowEquipmentSale()
        {
            int idx = 1;
            foreach (Equipment equip in Equipments)
            {
                //idx 보여주기
                equip.DisplayInfo(EquipmentDisplayMode.Sale, true, idx++);
            }
        }

        public void Clear()
        {
            Equipments.Clear();
            _equippedWeaponIdx = NONE_EQUIPPED_INDEX;
            _equippedArmorIdx = NONE_EQUIPPED_INDEX;
        }

    }
}
