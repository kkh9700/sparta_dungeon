using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace SpartaDungeon
{
    class Program
    {
        private static Character player;
        private static List<Item> shop;
        private static List<Dungeon> dungeons;
        private static BinaryFormatter binaryFormatter = new BinaryFormatter();
        private static Random random = new Random();

        static void Main(string[] args)
        {
            GameDataSetting();
            DisplayGameIntro();
            GameDataSave();
        }

        static void GameDataSetting()
        {
            // 파일 읽어오기
            FileInfo fi = new FileInfo("player.dat");

            if (fi.Exists)  // 파일이 존재하면
            {
                GameDataLoad();
            }
            else    // 파일이 존재하지 않으면
            {
                // 캐릭터 정보 세팅
                player = new Character("Rtan", "전사", 1, 10, 5, 100, 1500);

                // 아이템 정보 세팅
                player.AddItem(new Item("낡은 갑옷", "쉽게 볼 수 있는 낡은 갑옷 입니다.", Equipment.Armor, 3, 0, false));
                player.AddItem(new Item("무딘 검", "쉽게 볼 수 있는 무딘 검 입니다.", Equipment.Weapon, 1, 0, false));

                // 상점 정보 세팅
                shop = new List<Item>();
                shop.Add(new Item("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", Equipment.Armor, 5, 1000, true));
                shop.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", Equipment.Armor, 9, 1800, true));
                shop.Add(new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", Equipment.Armor, 15, 3500, true));
                shop.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", Equipment.Weapon, 2, 600, true));
                shop.Add(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", Equipment.Weapon, 5, 1500, true));
                shop.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", Equipment.Weapon, 7, 2700, true));

                // 던전 정보 세팅
                dungeons = new List<Dungeon>();
                dungeons.Add(new Dungeon(5, "쉬운 던전", 20, 35, 1000));
                dungeons.Add(new Dungeon(11, "일반 던전", 20, 35, 1700));
                dungeons.Add(new Dungeon(17, "어려운 던전", 20, 35, 2500));
            }
        }

        static void GameDataLoad()
        {
            if (File.Exists("player.dat"))
            {
                using (Stream stream = File.Open("player.dat", FileMode.Open))
                {
                    player = (Character)binaryFormatter.Deserialize(stream);
                }
            }

            if (File.Exists("shop.dat"))
            {
                using (Stream stream = File.Open("shop.dat", FileMode.Open))
                {
                    shop = (List<Item>)binaryFormatter.Deserialize(stream);
                }
            }

            if (File.Exists("dungeons.dat"))
            {
                using (Stream stream = File.Open("dungeons.dat", FileMode.Open))
                {
                    dungeons = (List<Dungeon>)binaryFormatter.Deserialize(stream);
                }
            }
        }

        static void GameDataSave()
        {
            using (Stream stream = File.Open("player.dat", FileMode.OpenOrCreate))
                binaryFormatter.Serialize(stream, player);

            using (Stream stream = File.Open("dungeons.dat", FileMode.OpenOrCreate))
                binaryFormatter.Serialize(stream, dungeons);

            using (Stream stream = File.Open("shop.dat", FileMode.OpenOrCreate))
                binaryFormatter.Serialize(stream, shop);
        }

        static void DisplayGameIntro()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 전전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("6. 종료");
                Console.WriteLine();

                int input = CheckValidInput(1, 6);
                switch (input)
                {
                    case 1:
                        DisplayMyInfo();
                        break;
                    case 2:
                        // 작업해보기
                        DisplayInventory();
                        break;
                    case 3:
                        DisplayShop();
                        break;
                    case 4:
                        DisplayDungeon();
                        break;
                    case 5:
                        DisplayRest();
                        break;
                    case 6:
                        return;
                }
            }
        }

        static void DisplayMyInfo()
        {
            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            Console.WriteLine($"공격력 :{player.Atk}");
            Console.WriteLine($"방어력 : {player.Def}");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    return;
            }
        }

        static void DisplayInventory()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine();
                foreach (Item item in player.Inventory)
                {
                    Console.WriteLine($"- {item.ToString()}");
                }
                Console.WriteLine();
                Console.WriteLine("1. 장착관리");
                Console.WriteLine("2. 아이템 정렬");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = CheckValidInput(0, 2);
                switch (input)
                {
                    case 0:
                        return;
                    case 1:
                        DisplayEquipManagement();
                        break;
                    case 2:
                        player.inventorySort();
                        break;
                }

            }
        }

        static void DisplayEquipManagement()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                Console.WriteLine();

                int index = player.Inventory.Count;
                for (int i = 0; i < index; i++)
                {
                    Console.Write("- " + (i + 1));
                    Console.WriteLine(player.Inventory[i].ToString());
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = CheckValidInput(0, index + 1);
                switch (input)
                {
                    case 0:
                        return;
                    default:
                        player.EquipItem(input - 1);
                        break;
                }
            }
        }

        static void DisplayShop()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                foreach (Item item in shop)
                {
                    Console.WriteLine(item.ToStringBuy());
                }

                Console.WriteLine();
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = CheckValidInput(0, 2);
                switch (input)
                {
                    case 0:
                        return;
                    case 1:
                        DisplayShopBuy();
                        break;
                    case 2:
                        DisplayShopSale();
                        break;
                }
            }
        }

        static void DisplayShopBuy()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                int index = shop.Count;
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < index; i++)
                {
                    Console.Write("- " + (i + 1));
                    Console.WriteLine(shop[i].ToStringBuy());
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();


                int input = CheckValidInput(0, index);
                switch (input)
                {
                    case 0:
                        return;
                    default:
                        if (shop[input - 1].IsSale)
                        {
                            if (shop[input - 1].BuyItem(player))
                            {
                                Console.WriteLine("구매를 완료했습니다.");
                            }
                            else
                            {
                                Console.WriteLine("Gold 가 부족합니다.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("이미 구매한 아이템입니다.");
                        }
                        break;
                }
            }
        }

        static void DisplayShopSale()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{player.Gold} G");
                Console.WriteLine();
                int index = player.Inventory.Count;
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < index; i++)
                {
                    Console.Write("- " + (i + 1));
                    Console.WriteLine(player.Inventory[i].ToStringSale());
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                int input = CheckValidInput(0, index);
                switch (input)
                {
                    case 0:
                        return;
                    default:
                        player.Inventory[input - 1].SaleItem(player);
                        break;
                }
            }
        }

        static void DisplayDungeon()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();

                int index = dungeons.Count;

                for (int i = 1; i <= index; i++)
                {
                    Console.WriteLine($"{i}. {dungeons[i - 1].ToString()}");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();

                int input = CheckValidInput(0, index);
                switch (input)
                {
                    case 0:
                        return;
                    default:
                        DungeonResult(input - 1);
                        break;
                }
            }
        }

        static void DungeonResult(int index)
        {
            Dungeon dungeon = dungeons[index];
            int HP = player.Hp;
            int Gold = player.Gold;

            Console.Clear();

            if (dungeon.DungeonEntry(player, random))
            {
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!!");
                Console.WriteLine($"{dungeon.DungeonName}을 클리어 하였습니다.");
                Console.WriteLine("");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {HP} -> {player.Hp}");
                Console.WriteLine($"Gold {Gold} G -> {player.Gold} G");

                if (player.Level == player.Experience)
                {
                    Console.WriteLine();
                    Console.WriteLine("[레벨업]");
                    Console.WriteLine($"Lv {player.Level} -> {player.Level + 1}");
                    Console.WriteLine($"Atk {player.Atk} -> {player.Atk + .5}");
                    Console.WriteLine($"Def {player.Def} -> {player.Def + 1}");
                    Console.WriteLine($"Hp {player.Hp} -> {100}");

                    player.LevelUp();
                }
            }
            else
            {
                Console.WriteLine("던전 클리어 실패");
                Console.WriteLine($"{dungeon.DungeonName}을 클리어 하지못하였습니다..");
                Console.WriteLine("체력과 방어력을 더 높여주세요.");
                Console.WriteLine("");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {HP} -> {player.Hp}");
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    return;
            }

        }

        static void DisplayRest()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("휴식하기");
                Console.WriteLine($"500 G 를 내면 체력을 회복 할 수 있습니다. (보유골드 ; {player.Gold} G)");
                Console.WriteLine("");
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                int input = CheckValidInput(0, 1);
                switch (input)
                {
                    case 0:
                        return;
                    case 1:
                        if (player.Gold < 500)
                        {
                            Console.WriteLine("Gold 가 부족합니다.");
                        }
                        else
                        {
                            Console.WriteLine("휴식을 완료했습니다.");
                            player.Rest();
                        }
                        break;
                }
            }
        }

        static int CheckValidInput(int min, int max)
        {
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            while (true)
            {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }
    }


    [Serializable]
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public double Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }
        public List<Item> Inventory { get; set; }

        public Item Weapon;
        public Item Armor;

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            Weapon = null;
            Armor = null;
            Experience = 0;
            Inventory = new List<Item>();
        }

        public void inventorySort()
        {
            Inventory.Sort((x, y) =>
            {
                return x.Name.CompareTo(y.Name);
            });
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        public void EquipItem(int index)
        {
            Item item = Inventory[index];

            item.IsEquip = true;

            switch (item.Type)
            {
                case Equipment.Weapon:
                    if (!(Weapon is null))
                        UnEquipItem(Equipment.Weapon);

                    Weapon = item;
                    Atk += item.StatsValue;
                    break;
                case Equipment.Armor:
                    if (!(Armor is null))
                        UnEquipItem(Equipment.Armor);

                    Armor = item;
                    Def += item.StatsValue;
                    break;
            }
        }

        public void UnEquipItem(Equipment equipment)
        {
            switch (equipment)
            {
                case Equipment.Weapon:
                    Weapon.IsEquip = false;
                    Atk -= Weapon.StatsValue;
                    Weapon = null;
                    break;
                case Equipment.Armor:
                    Armor.IsEquip = false;
                    Def -= Armor.StatsValue;
                    Armor = null;
                    break;
            }
        }

        public void Rest()
        {
            Gold -= 500;
            Hp += 50;

            if (Hp > 100)
                Hp = 100;
        }

        public void LevelUp()
        {
            Level++;
            Experience = 0;
            Atk += 0.5;
            Def += 1;
            Hp = 100;
        }
    }

    [Serializable]
    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public Stats StatsType { get; }
        public string StatsName { get; }
        public Equipment Type { get; set; }
        public int StatsValue { get; set; }
        public bool IsEquip { get; set; }
        public bool IsSale { get; set; }
        public int Price { get; set; }

        public Item(String name, String description, Equipment type, int statsValue, int price, bool isSale)
        {
            Name = name;
            Description = description;
            Type = type;
            StatsValue = statsValue;
            Price = price;
            IsEquip = false;
            IsSale = isSale;

            switch (Type)
            {
                case Equipment.Weapon:
                    StatsName = "공격력";
                    StatsType = Stats.Att;
                    break;
                case Equipment.Armor:
                    StatsName = "방어력";
                    StatsType = Stats.Def;
                    break;
                default:
                    StatsName = "체력";
                    break;
            }
        }

        public bool BuyItem(Character player)
        {
            if (Price > player.Gold)
                return false;

            IsSale = false;
            player.Gold -= Price;
            player.AddItem(this);

            return true;
        }

        public void SaleItem(Character player)
        {
            IsSale = true;

            switch (Type)
            {
                case Equipment.Weapon:
                    if (this.Equals(player.Weapon))
                    {
                        player.UnEquipItem(Equipment.Weapon);
                    }
                    break;
                case Equipment.Armor:
                    if (this.Equals(player.Armor))
                    {
                        player.UnEquipItem(Equipment.Armor);
                    }
                    break;
            }

            player.Gold += (int)(Price * 0.85);
            player.RemoveItem(this);
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(" ");
            if (IsEquip)
                sb.Append("[E]");
            sb.Append(Name.PadRight(10, ' '));
            sb.Append(" | ");
            sb.Append($"{StatsName} +{StatsValue}".PadRight(10, ' '));
            sb.Append(" | ");

            sb.Append(Description.PadRight(30, ' '));

            return sb.ToString();
        }

        public string ToStringBuy()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ToString());
            sb.Append(" | ");

            if (IsSale)
            {
                sb.Append($"{Price} G");
            }
            else
            {
                sb.Append("구매완료");
            }
            return sb.ToString();
        }

        public String ToStringSale()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ToString());
            sb.Append(" | ");
            sb.Append($"{Price} G");

            return sb.ToString();
        }
    }

    [Serializable]
    public class Dungeon
    {
        int RecommentedDef { get; set; }
        public string DungeonName { get; set; }
        int MinConsumeHp { get; set; }
        int MaxConsumeHp { get; set; }
        int ClearGold { get; set; }


        public Dungeon(int recommentedDef, string dungeonName, int minConsumeHp, int maxConsumeHp, int clearGold)
        {
            RecommentedDef = recommentedDef;
            DungeonName = dungeonName;
            MinConsumeHp = minConsumeHp;
            MaxConsumeHp = maxConsumeHp;
            ClearGold = clearGold;
        }

        public int ConsumeHp(int def, Random random)
        {
            int bonus = def - RecommentedDef;
            return random.Next(MinConsumeHp - bonus, MaxConsumeHp - bonus + 1);
        }

        public int ClearReward(double atk, Random random)
        {
            return (int)(ClearGold * (100 + atk + random.NextDouble() * atk) / 100);
        }

        public bool DungeonEntry(Character player, Random random)
        {
            int reduceHp = ConsumeHp(player.Def, random);
            if (player.Def < RecommentedDef)
            {
                int dice = random.Next(10);

                if (dice < 4)
                {
                    player.Hp -= reduceHp / 2;
                    return false;
                }
            }


            if (player.Hp < reduceHp)
            {
                player.Hp -= reduceHp / 2;

                if (player.Hp < 0)
                    player.Hp = 0;

                return false;
            }

            player.Hp -= reduceHp;
            player.Gold += ClearReward(player.Atk, random);
            player.Experience++;

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(DungeonName.PadRight(10, ' '));
            sb.Append(" | ");
            sb.Append($"방어력 {RecommentedDef} 이상 권장");

            return sb.ToString();
        }
    }

    public enum Equipment
    {
        Weapon, Armor
    }

    public enum Stats
    {
        Att, Def, Hp
    }
}
