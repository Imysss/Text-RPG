using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static TextRPG.Program;

namespace TextRPG
{
    internal class SceneManager
    {
        private SceneType _currentScene;

        private Character _character;
        private Inventory _inventory;
        private Store _store;

        private Difficulty _difficulty;
        private float _healthLoss;
        private float _reward;

        private readonly Dictionary<SceneType, Action> _sceneMap;

        private bool IsValidIndex(int input, int count) => input > 0 && input <= count;

        public SceneManager()
        {
            _currentScene = SceneType.StartScene;

            //씬 종류와 씬 처리 함수 매핑
            _sceneMap = new Dictionary<SceneType, Action>
            {
                { SceneType.NewGameScene, LoadNewGameScene },
                { SceneType.SaveGameScene, LoadSaveGameScene },
                { SceneType.StartScene, LoadStartScene },
                { SceneType.ChacterInfoScene, LoadCharacterInfoScene },
                { SceneType.InventoryScene, LoadInventoryScene },
                { SceneType.EquipmentScene, LoadEquipmentScene },
                { SceneType.StoreScene, LoadStoreScene },
                { SceneType.PurchaseScene, LoadPurchaseScene },
                { SceneType.SaleScene, LoadSaleScene },
                { SceneType.DungeonScene, LoadDungeonScene },
                { SceneType.RestScene, LoadRestScene },
                { SceneType.DungeonClearScene, LoadDungeonClearScene },
            };
        }

        public void Init()
        {
            _character = Manager.Instance.Character;
            _inventory = Manager.Instance.Inventory;
            _store = Manager.Instance.Store;
        }

        //씬 전환 함수
        public void LoadScene(SceneType sceneType)
        {
            Console.Clear();

            //씬을 찾고 매핑된 Action을 실행
            if(_sceneMap.TryGetValue(sceneType, out var action))
            {
                action.Invoke();
            }
        }

        private int GetMenuInput()
        {
            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            Console.Write(">> ");
            return int.TryParse(Console.ReadLine(), out int input) ? input : -1;
        }

        public void LoadNewGameScene()
        {
            Console.WriteLine("새로운 게임 시작\n");

            Console.Write("이름을 입력하세요: ");
            string name = Console.ReadLine();

            Console.WriteLine("\n직업을 선택하세요.");
            Console.WriteLine("1. 전사  |  2. 마법사  |  3. 궁수");
            Console.Write(">> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;

            _character.SetCharacterInfo(name, input);

            LoadScene(SceneType.StartScene);
        }

        public void LoadSaveGameScene()
        {
            Console.WriteLine("저장된 게임 시작\n");

            Console.WriteLine("정보를 확인하세요\n");

            _character.ShowCharacterInfo();

            Console.WriteLine("\n이 정보로 저장된 게임을 시작하시겠습니까?");
            Console.WriteLine("1. 예  |  2. 아니오");
            Console.Write(">> ");
            int input = int.TryParse(Console.ReadLine(), out int result) ? result : -1;
            if (input == 1)
            {
                LoadScene(SceneType.StartScene);
            }
            else
            {
                return;
            }
        }

        public void LoadStartScene()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 저장하기\n");


            //1~3 이외 입력 시 잘못된 입력입니다 출력하기
            while (true)
            {
                int input = GetMenuInput();

                switch (input)
                {
                    case 1: LoadScene(SceneType.ChacterInfoScene); break;
                    case 2: LoadScene(SceneType.InventoryScene); break;
                    case 3: LoadScene(SceneType.StoreScene); break;
                    case 4: LoadScene(SceneType.DungeonScene); break;
                    case 5: LoadScene(SceneType.RestScene); break;
                    case 6: Manager.Instance.SaveGame(); break;
                    default: Console.WriteLine("잘못된 입력입니다.\n"); break;
                }
            }
        }

        public void LoadCharacterInfoScene()
        {
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            //캐릭터 스테이터스 보여 주기
            _character.ShowCharacterInfo();

            Console.WriteLine("\n0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();
                switch (input)
                {
                    case 0: LoadScene(SceneType.StartScene); break;
                    default: Console.WriteLine("잘못된 입력입니다.\n"); break;
                }
            }
        }

        public void LoadInventoryScene()
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            //인벤토리 목록 보여 주기
            Console.WriteLine("[아이템 목록]");
            _inventory.ShowInventory();

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();
                switch (input)
                {
                    case 0: LoadScene(SceneType.StartScene); break;
                    case 1: LoadScene(SceneType.EquipmentScene); break;
                    default: Console.WriteLine("잘못된 입력입니다.\n");break;
                }
            }
        }

        public void LoadEquipmentScene()
        {
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("아이템 번호를 입력하면 장착/해제할 수 있습니다.\n");

            //인벤토리 목록 및 장착 상태 보여 주기
            Console.WriteLine("[아이템 목록]");
            _inventory.ShowEquipmentState();

            Console.WriteLine("\n0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();

                if (input == 0)
                {
                    LoadScene(SceneType.StartScene);
                    return;
                }

                if (IsValidIndex(input, _inventory.Equipments.Count))
                {
                    //장착 설정/해제
                    _inventory.Equip(_inventory.Equipments[input - 1], input - 1);
                    LoadScene(SceneType.EquipmentScene);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                }
            }
        }

        public void LoadStoreScene()
        {
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{_character.Gold} G\n");

            //상점에 있는 아이템 목록 보여 주기
            Console.WriteLine("[아이템 목록]");
            _store.ShowStore();

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();
                switch (input)
                {
                    case 0: LoadScene(SceneType.StartScene); break;
                    case 1: LoadScene(SceneType.PurchaseScene); break;
                    case 2: LoadScene(SceneType.SaleScene); break;
                    default: Console.WriteLine("잘못된 입력입니다.\n"); break;
                }
            }
        }

        public void LoadPurchaseScene()
        {
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("원하는 아이템 번호를 입력하면 구매할 수 있습니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{_character.Gold} G\n");

            //상점에서 구매 목록 보여 주기 (선택 숫자도 함께 보여 줘야 함)
            Console.WriteLine("[아이템 목록]");
            _store.ShowPurchase();

            Console.WriteLine("\n0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();

                if (input == 0)
                {
                    LoadScene(SceneType.StartScene);
                    return;
                }

                if (IsValidIndex(input, _store.Equipments.Count))
                {
                    if (_store.Equipments[input - 1].IsPurchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.\n");
                    }
                    else
                    {
                        //보유 금액이 충분하다면 구매
                        if (_store.Equipments[input - 1].Data.Price <= _character.Gold)
                        {
                            //구매 및 인벤토리에 추가되는 이벤트 호출
                            _store.Equipments[input - 1].PurchaseEquipment();
                            //보유 금액 차감
                            _character.Gold -= _store.Equipments[input - 1].Data.Price;
                            LoadScene(SceneType.PurchaseScene);
                        }
                        //보유 금액이 부족
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                }
            }
        }

        public void LoadSaleScene()
        {
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("원하는 아이템 번호를 입력하면 판매할 수 있습니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{_character.Gold} G\n");

            //상점에서 판매 목록 보여 주기 (인벤토리에 있는 것 불러와야겠지)
            Console.WriteLine("[아이템 목록]");
            _inventory.ShowEquipmentSale();

            Console.WriteLine("\n0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();

                if (input == 0)
                {
                    LoadScene(SceneType.StartScene);
                    return;
                }
                if (IsValidIndex(input, _inventory.Equipments.Count))
                {
                    //가격만큼 골드 추가
                    _character.Gold += (int)(_inventory.Equipments[input - 1].Data.Price*0.85f);

                    //장비 판매
                    _inventory.RemoveEquipment(_inventory.Equipments[input - 1].DataId);

                    //SaleScene 다시 불러오기
                    LoadScene(SceneType.SaleScene);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                }
            }
        }

        public void LoadDungeonScene()
        {
            Console.WriteLine("던전 입장");
            Console.WriteLine("이곳에서 던전의 난이도를 선택할 수 있습니다.\n");

            Console.WriteLine("1. 쉬운 던전\t| 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전\t| 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전\t| 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기\n");

            while (true)
            {
                int input = GetMenuInput();
                _difficulty = input switch
                {
                    0 => Difficulty.None,
                    1 => Difficulty.Easy,
                    2 => Difficulty.Normal,
                    3 => Difficulty.Hard,
                    _ => Difficulty.Error
                };

                if (_difficulty == Difficulty.None)
                {
                    LoadScene(SceneType.StartScene);
                    return;
                }
                else if (_difficulty == Difficulty.Error)
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                }
                else
                {
                    CalculateDungeonResult();
                }
            }
        }

        public void LoadRestScene()
        {
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500G를 내면 체력을 100까지 회복할 수 있습니다. (보유 골드: {_character.Gold}G)\n");

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기\n");

            while(true)
            {
                Console.WriteLine("원하시는 행동을 입력해 주세요.");
                Console.Write(">> ");

                int input = int.Parse(Console.ReadLine());
                switch (input)
                {
                    case 0:
                        LoadScene(SceneType.StartScene);
                        break;
                    case 1:
                        if (_character.Gold >= 500)
                        {
                            Console.WriteLine("휴식을 완료했습니다.\n");
                            _character.Gold -= 500;
                            _character.HP = 100;
                        }    
                        else
                        {
                            Console.WriteLine("Gold가 부족합니다.\n");
                        }
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.\n");
                        break;
                }
            }
        }

        public void LoadDungeonClearScene()
        {
            //던전 난이도 string
            string difficultyStr = _difficulty switch
            {
                Difficulty.Easy => "쉬운",
                Difficulty.Normal => "일반",
                Difficulty.Hard => "어려운",
                _ => "",
            };

            //클리어 또는 실패 확인
            if (_reward == 0)
            {
                Console.WriteLine("던전 실패");
                Console.WriteLine($"{difficultyStr} 던전에서 패배하여 보상을 얻지 못했습니다.\n");
            }
            else
            {
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{difficultyStr} 던전을 클리어 하였습니다.\n");

                //던전 클리어 시 레벨 업
                _character.Level++;
            }

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력: {_character.HP} -> {_character.HP - _healthLoss}");
            Console.WriteLine($"Gold: {_character.Gold} -> {_character.Gold + _reward}\n");

            Console.WriteLine("0. 나가기");

            //체력, 골드 계산하여 저장
            _character.HP -= (int)_healthLoss;
            _character.Gold += (int)_reward;

            while (true)
            {
                int input = GetMenuInput();
                switch (input)
                {
                    case 0: LoadScene(SceneType.StartScene); break;
                    default: Console.WriteLine("잘못된 입력입니다.\n"); break;
                }
            }
        }

        public void CalculateDungeonResult()
        {
            Random random = new Random();

            (float recommendDfs, float baseReward) = _difficulty switch
            {
                Difficulty.Easy => (5, 1000),
                Difficulty.Normal => (11, 1700),
                Difficulty.Hard => (17, 2500),
                _ => (0, 0),
            };

            //권장 방어력보다 낮으면 40프로 확률로 실패
            bool failed = _character.Defense < recommendDfs && random.NextDouble() < 0.4;

            if (failed)
            {
                _healthLoss = _character.HP / 2f;
                _reward = 0;
            }
            else
            {
                //기본 체력 감소량
                _healthLoss = random.Next(20, 36) + (_character.Defense - recommendDfs);
                _healthLoss = Math.Clamp(_healthLoss, 0, _character.HP);

                //던전 클리어 보상
                int extraReward = random.Next((int)_character.Attack, (int)_character.Attack * 2 + 1);
                _reward = baseReward * (1 + extraReward / 100f);
            }

            LoadScene(SceneType.DungeonClearScene);
        }
    }
}
