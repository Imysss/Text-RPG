using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Newtonsoft.Json;

namespace TextRPG
{
    public class SaveData
    {
        public string Name;
        public CharacterClass Class;
        public int Level;
        public float BaseAttack;
        public float BaseDefense;
        public int HP;
        public int Gold;
        public List<int> InventoryItemIds;         // 인벤토리에 담긴 장비 ID
        public int? EquippedWeaponIdx;             // 장착된 무기 ID
        public int? EquippedArmorIdx;              // 장착된 방어구 ID
    }

    internal class SaveManager
    {
        private static readonly string SavePath = "C:\\Users\\user\\source\\repos\\TextRPG\\textRpgSaveFile.json";

        //게임 데이터를 SaveData 객체로 받아 JSON 문자열로 변환하고 지정한 경로에 저장
        public static void Save(SaveData data)
        {
            //data 객체를 JSON 문자열로 변환
            string json = JsonConvert.SerializeObject(data);
            //변환한 문자열을 지정한 경로의 파일로 저장
            File.WriteAllText(SavePath, json);
        }

        public static SaveData Load()
        {
            //파일이 없으면 null 반환
            if (!File.Exists(SavePath))
            {
                return null;
            }

            //파일에서 문자열을 읽고 JSON을 SaveData로 역직렬화하여 반환
            string json = File.ReadAllText(SavePath);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
}
