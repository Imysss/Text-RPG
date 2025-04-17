using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Store
    {
        public List<Equipment> Equipments = new List<Equipment>();

        public Store()
        {
            InitStore();
        }

        public void InitStore()
        {
            for (int i = 0; i < EquipmentDatabase.DataArray.Length; i++)
            {
                AddEquipment(new Equipment(i));
            }
        }

        public void AddEquipment(Equipment equip)
        {
            //중복 방지
            if (!Equipments.Any(e => e.DataId == equip.DataId))
            {
                equip.OnEquippedChanged += Manager.Instance.Character.ApplyEquipmentStat;
                Equipments.Add(equip);
            }
        }

        public void ShowStore()
        {
            foreach (Equipment equip in Equipments)
            {
                equip.DisplayInfo(EquipmentDisplayMode.Shop, false);
            }
        }

        public void ShowPurchase()
        {
            int idx = 1;
            foreach (Equipment equip in Equipments)
            {
                equip.DisplayInfo(EquipmentDisplayMode.Shop, true, idx++);
            }
        }
    }
}
