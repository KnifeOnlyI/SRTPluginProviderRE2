﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SRTPluginProviderRE2.Structs.GameStructs
{
    [DebuggerDisplay("{_DebuggerDisplay,nq}")]
    public struct InventoryEntry
    {
        private int slotNo;
        private ItemID itemId;
        private WeaponType weaponId;
        private WeaponParts weaponParts;
        private ItemID bulletId;
        private int count;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string _DebuggerDisplay
        {
            get
            {
                if (IsItem)
                    return string.Format("Item {0} Quantity {1}", ItemName, Count);
                else if (IsWeapon)
                    return string.Format("Weapon {0} Quantity {1} Attachments {2}", WeaponName, Count, WeaponPartFlags);
                else
                    return string.Format("Empty Slot");
            }
        }

        public int SlotNo { get => slotNo; set => slotNo = value; }
        public ItemID ItemId { get => itemId; set => itemId = value; }
        public WeaponType WeaponId { get => weaponId; set => weaponId = value; }
        public WeaponParts WeaponParts { get => weaponParts; set => weaponParts = value; }
        public ItemID BulletId { get => bulletId; set => bulletId = value; }
        public int Count { get => count; set => count = value; }
        public bool IsItem => ItemId != ItemID.None && WeaponId == WeaponType.Invalid || ItemId != ItemID.None && WeaponId == WeaponType.BareHand;
        public bool IsWeapon => ItemId == ItemID.None && WeaponId != WeaponType.Invalid && WeaponId != WeaponType.BareHand;
        public bool IsEmptySlot => !IsItem && !IsWeapon;
        public string ItemName => ItemId.ToString();
        public string WeaponName => WeaponId.ToString();
        public string WeaponPartFlags => GetFlagsString(WeaponParts);
        public string ItemDebug => IsItem ? ItemName : WeaponName + GetFlagsString(WeaponParts);
        public bool IsFatSlot { get => Utils.Slot2Items.Contains(ItemDebug); }
        public string BulletName => BulletId.ToString();

        private string GetFlagsString(WeaponParts _weaponParts)
        {
            List<string> includedParts = new List<string>();
            if (_weaponParts.HasFlag(WeaponParts.First))
                includedParts.Add(nameof(WeaponParts.First));
            if (_weaponParts.HasFlag(WeaponParts.Second))
                includedParts.Add(nameof(WeaponParts.Second));
            if (_weaponParts.HasFlag(WeaponParts.Third))
                includedParts.Add(nameof(WeaponParts.Third));
            string result = string.Join("_", includedParts);
            return result;
        }

        public void SetValues(int index, PrimitiveItem item)
        {
            SlotNo = index;
            ItemId = item.ItemId;
            WeaponId = item.WeaponId;
            WeaponParts = item.WeaponParts;
            BulletId = item.BulletId;
            Count = item.Count;
        }
    }

    public class Utils
    {
        public static List<string> Slot2Items = new List<string>()
        {
            "AntiTankRocketLauncher",
            "ATM4_Infinite",
            "ChemicalFlamethrower",
            "ChemicalFlamethrowerSecond",
            "GearLarge",
            "GrenadeLauncher_GM79First",
            "Handgun_LightningHawkFirst",
            "Handgun_LightningHawkFirst_Second",
            "Handgun_MatildaFirst",
            "Handgun_MatildaFirst_Second",
            "Handgun_MatildaFirst_Third",
            "Handgun_MatildaFirst_Second_Third",
            "Minigun",
            "Shotgun_W870First",
            "Shotgun_W870First_Second",
            "SMG_LE5_Infinite",
            "SMG_MQ11First",
            "SMG_MQ11First_Second",
            "SparkShot",
            "SparkShotFirst",
            "JointPlug"
        };
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    public struct InventoryManager
    {
        [FieldOffset(0x10)] private nint inventory;
        [FieldOffset(0x18)] private int mSize;
        public IntPtr Inventory => IntPtr.Add(inventory, 0x0);
        public int Count => mSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x40)]
    public struct ShortcutManager
    {
        [FieldOffset(0x18)] private nint entries;
        [FieldOffset(0x20)] private int mSize;
        public IntPtr Entries => IntPtr.Add(entries, 0x0);
        public int Count => mSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]

    public struct ListInventory
    {
        [FieldOffset(0x20)] private nint listInventory;
        public IntPtr _ListInventory => IntPtr.Add(listInventory, 0x0);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    public struct Inventory
    {
        [FieldOffset(0x90)] private int currentSlotSize;
        [FieldOffset(0x98)] private nint listSlots;
        public int CurrentSlotSize => currentSlotSize;
        public IntPtr ListSlots => IntPtr.Add(listSlots, 0x0);
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x20)]
    public struct Slots
    {
        [FieldOffset(0x10)] private nint slots;
        [FieldOffset(0x18)] private int mSize;
        public IntPtr _Slots => IntPtr.Add(slots, 0x0);
        public int Count => mSize;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x28)]
    public struct Slot
    {
        [FieldOffset(0x18)] private nint slot;
        [FieldOffset(0x28)] private int index;
        public IntPtr _Slot => IntPtr.Add(slot, 0x0);
        public int Index => index;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x24)]
    public struct PrimitiveItem
    {
        [FieldOffset(0x10)] private int itemId;
        [FieldOffset(0x14)] private int weaponId;
        [FieldOffset(0x18)] private int weaponParts;
        [FieldOffset(0x1C)] private int bulletId;
        [FieldOffset(0x20)] private int count;

        public ItemID ItemId => (ItemID)itemId;
        public WeaponType WeaponId => (WeaponType)weaponId;
        public WeaponParts WeaponParts => (WeaponParts)weaponParts;
        public ItemID BulletId => (ItemID)bulletId;
        public int Count => count;
    }

    public enum ItemID : int
    {
        None = 0,
        FirstAidSpray = 0x01,
        Herb_Green1 = 0x02,
        Herb_Red1 = 0x03,
        Herb_Blue1 = 0x04,
        Herb_Mixed_GG = 0x05,
        Herb_Mixed_GR = 0x06,
        Herb_Mixed_GB = 0x07,
        Herb_Mixed_GGB = 0x08,
        Herb_Mixed_GGG = 0x09,
        Herb_Mixed_GRB = 0x0A,
        Herb_Mixed_RB = 0x0B,
        Herb_Green2 = 0x0C,
        Herb_Red2 = 0x0D,
        Herb_Blue2 = 0x0E,
        HandgunBullets = 0x0F,
        ShotgunShells = 0x10,
        SubmachineGunAmmo = 0x11,
        MAGAmmo = 0x12,
        GrenadeAcidRounds = 0x16,
        GrenadeFlameRounds = 0x17,
        NeedleCartridges = 0x18,
        Fuel = 0x19,
        HandgunLargeCaliberAmmo = 0x1A,
        SLS60HighPoweredRounds = 0x1B,
        Detonator = 0x1F,
        InkRibbon = 0x20,
        WoodenBoard = 0x21,
        ElectronicGadget = 0x22,
        Battery9Volt = 0x23,
        Gunpowder = 0x24,
        GunpowderLarge = 0x25,
        GunpowderHighGradeYellow = 0x26,
        GunpowderHighGradeWhite = 0x27,
        MatildaHighCapacityMagazine = 0x30,
        MatildaMuzzleBrake = 0x31,
        MatildaGunStock = 0x32,
        SLS60SpeedLoader = 0x33,
        JMBHp3LaserSight = 0x34,
        SLS60ReinforcedFrame = 0x35,
        JMBHp3HighCapacityMagazine = 0x36,
        W870ShotgunStock = 0x37,
        W870LongBarrel = 0x38,
        MQ11HighCapacityMagazine = 0x3A,
        MQ11Suppressor = 0x3C,
        LightningHawkRedDotSight = 0x3D,
        LightningHawkLongBarrel = 0x3E,
        GM79ShoulderStock = 0x40,
        FlamethrowerRegulator = 0x41,
        SparkShotHighVoltageCondenser = 0x42,
        Film_HidingPlace = 0x48,
        Film_RisingRookie = 0x49,
        Film_Commemorative = 0x4A,
        Film_3FLocker = 0x4B,
        Film_LionStatue = 0x4C,
        KeyStorageRoom = 0x4D,
        JackHandle = 0x4F,
        SquareCrank = 0x50,
        MedallionUnicorn = 0x51,
        KeySpade = 0x52,
        KeyCardParkingGarage = 0x53,
        KeyCardWeaponsLocker = 0x54,
        ValveHandle = 0x56,
        STARSBadge = 0x57,
        Scepter = 0x58,
        RedJewel = 0x5A,
        BejeweledBox = 0x5B,
        PlugBishop = 0x5D,
        PlugRook = 0x5E,
        PlugKing = 0x5F,
        PictureBlock = 0x62,
        USBDongleKey = 0x66,
        SpareKey = 0x70,
        RedBook = 0x72,
        StatuesLeftArm = 0x73,
        StatuesLeftArmWithRedBook = 0x74,
        MedallionLion = 0x76,
        KeyDiamond = 0x77,
        KeyCar = 0x78,
        MedallionMaiden = 0x7C,
        PowerPanelPart1 = 0x7E,
        PowerPanelPart2 = 0x7F,
        LoversRelief = 0x80,
        GearSmall = 0x81,
        GearLarge = 0x82,
        KeyCourtyard = 0x83,
        PlugKnight = 0x84,
        PlugPawn = 0x85,
        PlugQueen = 0x86,
        BoxedElectronicPart1 = 0x87,
        BoxedElectronicPart2 = 0x88,
        KeyOrphanage = 0x9F,
        KeyClub = 0xA0,
        KeyHeart = 0xA9,
        USSDigitalVideoCassette = 0xAA,
        TBarValveHandle = 0xB0,
        DispersalCartridgeEmpty = 0xB3,
        DispersalCartridgeSolution = 0xB4,
        DispersalCartridgeHerbicide = 0xB5,
        JointPlug = 0xB7,
        UpgradeChipAdministrator = 0xBA,
        IDWristbandAdministrator = 0xBB,
        ElectronicChip = 0xBC,
        SignalModulator = 0xBD,
        Trophy1 = 0xBE,
        Trophy2 = 0xBF,
        KeySewers = 0xC2,
        IDWristbandVisitor1 = 0xC3,
        IDWristbandGeneralStaff1 = 0xC4,
        IDWristbandSeniorStaff1 = 0xC5,
        UpgradeChipGeneralStaff = 0xC6,
        UpgradeChipSeniorStaff = 0xC7,
        IDWristbandVisitor2 = 0xC8,
        IDWristbandGeneralStaff2 = 0xC9,
        IDWristbandSeniorStaff2 = 0xCA,
        LabDigitalVideoCassette = 0xCB,
        Briefcase = 0xE6,
        FuseMainHall = 0xF0,
        FuseBreakRoom = 0xF1,
        Scissors = 0xF3,
        BoltCutter = 0xF4,
        StuffedDoll = 0xF5,
        HipPouch = 0x0106,
        OldKey = 0x011E,
        PortableSafe = 0x0123,
        TinStorageBox1 = 0x0125,
        WoodenBox1 = 0x0126,
        WoodenBox2 = 0x0127,
        TinStorageBox2 = 0x0128,
        MAX = 297,
        JointPlug2 = 0x7F000001,
        GearLarge2 = 0x7F000002,
    };

    public enum WeaponType : int
    {
        Invalid = -1,
        BareHand = 0,
        Handgun_Matilda = 0x01,
        Handgun_M19 = 0x02,
        Handgun_JMB_Hp3 = 0x03,
        Handgun_Quickdraw_Army = 0x04, // Colt SAA
        WP0400 = 5,
        WP0500 = 6,
        Handgun_MUP = 0x07,
        Handgun_BroomHc = 0x08,
        Handgun_SLS60 = 0x09,
        WP0900 = 10,
        Shotgun_W870 = 0x0B,
        WP1100 = 12,
        WP1200 = 13,
        WP1300 = 14,
        WP1400 = 15,
        WP1500 = 16,
        WP1600 = 17,
        WP1700 = 18,
        WP1800 = 19,
        WP1900 = 20,
        SMG_MQ11 = 0x15,
        WP2100 = 22,
        SMG_LE5_Infinite = 0x17,
        WP2300 = 24,
        WP2400 = 25,
        WP2500 = 26,
        WP2600 = 27,
        WP2700 = 28,
        WP2800 = 29,
        WP2900 = 30,
        Handgun_LightningHawk = 0x1F,
        WP3100 = 32,
        WP3200 = 33,
        WP3300 = 34,
        WP3400 = 35,
        WP3500 = 36,
        WP3600 = 37,
        WP3700 = 38,
        WP3800 = 39,
        WP3900 = 40,
        EMF_Visualizer = 41,
        GrenadeLauncher_GM79 = 0x2A,
        ChemicalFlamethrower = 0x2B,
        SparkShot = 0x2C,
        ATM4 = 0x2D,
        CombatKnife = 0x2E,
        CombatKnife_Infinite = 0x2F,
        WP4520 = 48,
        AntiTankRocketLauncher = 0x31,
        Minigun = 0x32,
        WP4800 = 51,
        WP4900 = 52,
        WP5000 = 53,
        WP5100 = 54,
        WP5200 = 55,
        WP5300 = 56,
        WP5400 = 57,
        WP5500 = 58,
        WP5600 = 59,
        WP5700 = 60,
        WP5800 = 61,
        WP5900 = 62,
        WP6000 = 63,
        WP6100 = 64,
        HandGrenade = 0x41,
        FlashGrenade = 0x42,
        WP6400 = 67,
        WP6410 = 68,
        WP6420 = 69,
        WP6430 = 70,
        WP6440 = 71,
        WP6450 = 72,
        WP6460 = 73,
        WP6470 = 74,
        WP6480 = 75,
        WP6490 = 76,
        WP6500 = 77,
        WP6600 = 78,
        WP6700 = 79,
        WP6800 = 80,
        WP6900 = 81,
        Handgun_SamuraiEdge_Infinite = 0x52,
        Handgun_SamuraiEdge_ChrisRedfield = 0x53,
        Handgun_SamuraiEdge_JillValentine = 0x54,
        Handgun_SamuraiEdge_AlbertWesker = 0x55,
        WP7040 = 86,
        WP7050 = 87,
        WP7060 = 88,
        WP7070 = 89,
        WP7080 = 90,
        WP7090 = 91,
        WP7100 = 92,
        WP7110 = 93,
        WP7120 = 94,
        WP7130 = 95,
        WP7140 = 96,
        WP7150 = 97,
        WP7160 = 98,
        WP7170 = 99,
        WP7180 = 100,
        WP7190 = 101,
        WP7200 = 102,
        WP7210 = 103,
        WP7220 = 104,
        WP7230 = 105,
        WP7240 = 106,
        WP7250 = 107,
        WP7260 = 108,
        WP7270 = 109,
        WP7280 = 110,
        WP7290 = 111,
        WP7300 = 112,
        WP7310 = 113,
        WP7320 = 114,
        WP7330 = 115,
        WP7340 = 116,
        WP7350 = 117,
        WP7360 = 118,
        WP7370 = 119,
        WP7380 = 120,
        WP7390 = 121,
        WP7400 = 122,
        WP7410 = 123,
        WP7420 = 124,
        WP7430 = 125,
        WP7440 = 126,
        WP7450 = 127,
        WP7460 = 128,
        WP7470 = 129,
        WP7480 = 130,
        WP7490 = 131,
        WP7500 = 132,
        WP7510 = 133,
        WP7520 = 134,
        WP7530 = 135,
        WP7540 = 136,
        WP7550 = 137,
        WP7560 = 138,
        WP7570 = 139,
        WP7580 = 140,
        WP7590 = 141,
        WP7600 = 142,
        WP7610 = 143,
        WP7620 = 144,
        WP7630 = 145,
        WP7640 = 146,
        WP7650 = 147,
        WP7660 = 148,
        WP7670 = 149,
        WP7680 = 150,
        WP7690 = 151,
        WP7700 = 152,
        WP7710 = 153,
        WP7720 = 154,
        WP7730 = 155,
        WP7740 = 156,
        WP7750 = 157,
        WP7760 = 158,
        WP7770 = 159,
        WP7780 = 160,
        WP7790 = 161,
        WP7800 = 162,
        WP7810 = 163,
        WP7820 = 164,
        WP7830 = 165,
        WP7840 = 166,
        WP7850 = 167,
        WP7860 = 168,
        WP7870 = 169,
        WP7880 = 170,
        WP7890 = 171,
        WP7900 = 172,
        WP7910 = 173,
        WP7920 = 174,
        WP7930 = 175,
        WP7940 = 176,
        WP7950 = 177,
        WP7960 = 178,
        WP7970 = 179,
        WP7980 = 180,
        WP7990 = 181,
        WP8000 = 182,
        WP8010 = 183,
        WP8020 = 184,
        WP8030 = 185,
        WP8040 = 186,
        WP8050 = 187,
        WP8060 = 188,
        WP8070 = 189,
        WP8080 = 190,
        WP8090 = 191,
        WP8100 = 192,
        WP8110 = 193,
        WP8120 = 194,
        WP8130 = 195,
        WP8140 = 196,
        WP8150 = 197,
        WP8160 = 198,
        WP8170 = 199,
        WP8180 = 200,
        WP8190 = 201,
        WP8200 = 202,
        WP8210 = 203,
        WP8220 = 204,
        WP8230 = 205,
        WP8240 = 206,
        WP8250 = 207,
        WP8260 = 208,
        WP8270 = 209,
        WP8280 = 210,
        WP8290 = 211,
        WP8300 = 212,
        WP8310 = 213,
        WP8320 = 214,
        WP8330 = 215,
        WP8340 = 216,
        WP8350 = 217,
        WP8360 = 218,
        WP8370 = 219,
        WP8380 = 220,
        WP8390 = 221,
        ATM4_Infinite = 0xDE,
        WP8410 = 223,
        WP8420 = 224,
        WP8430 = 225,
        WP8440 = 226,
        WP8450 = 227,
        WP8460 = 228,
        WP8470 = 229,
        WP8480 = 230,
        WP8490 = 231,
        WP8500 = 232,
        WP8510 = 233,
        WP8520 = 234,
        WP8530 = 235,
        WP8540 = 236,
        WP8550 = 237,
        WP8560 = 238,
        WP8570 = 239,
        WP8580 = 240,
        WP8590 = 241,
        AntiTankRocketLauncher_Infinite = 0xF2,
        WP8610 = 243,
        WP8620 = 244,
        WP8630 = 245,
        WP8640 = 246,
        WP8650 = 247,
        WP8660 = 248,
        WP8670 = 249,
        WP8680 = 250,
        WP8690 = 251,
        Minigun_Infinite = 0xFC,
        WP8710 = 253,
        WP8720 = 254,
        WP8730 = 255,
        WP8740 = 256,
        WP8750 = 257,
        WP8760 = 258,
        WP8770 = 259,
        WP8780 = 260,
        WP8790 = 261,
        WP8800 = 262,
        WP8810 = 263,
        WP8820 = 264,
        WP8830 = 265,
        WP8840 = 266,
        WP8850 = 267,
        WP8860 = 268,
        WP8870 = 269,
        WP8880 = 270,
        WP8890 = 271,
        WP8900 = 272,
        WP8910 = 273,
        WP8920 = 274,
        WP8930 = 275,
        WP8940 = 276,
        WP8950 = 277,
        WP8960 = 278,
        WP8970 = 279,
        WP8980 = 280,
        WP8990 = 281,
        WP9000 = 282,
        WP9010 = 283,
        WP9020 = 284,
        WP9030 = 285,
        WP9040 = 286,
        WP9050 = 287,
        WP9060 = 288,
        WP9070 = 289,
        WP9080 = 290,
        WP9090 = 291,
        WP9100 = 292,
        WP9110 = 293,
        WP9120 = 294,
        WP9130 = 295,
        WP9140 = 296,
        WP9150 = 297,
        WP9160 = 298,
        WP9170 = 299,
        WP9180 = 300,
        WP9190 = 301,
        WP9200 = 302,
        WP9210 = 303,
        WP9220 = 304,
        WP9230 = 305,
        WP9240 = 306,
        WP9250 = 307,
        WP9260 = 308,
        WP9270 = 309,
        WP9280 = 310,
        WP9290 = 311,
        WP9300 = 312,
        WP9310 = 313,
        WP9320 = 314,
        WP9330 = 315,
        WP9340 = 316,
        WP9350 = 317,
        WP9360 = 318,
        WP9370 = 319,
        WP9380 = 320,
        WP9390 = 321,
        WP9400 = 322,
        WP9410 = 323,
        WP9420 = 324,
        WP9430 = 325,
        WP9440 = 326,
        WP9450 = 327,
        WP9460 = 328,
        WP9470 = 329,
        WP9480 = 330,
        WP9490 = 331,
        WP9500 = 332,
        WP9510 = 333,
        WP9520 = 334,
        WP9530 = 335,
        WP9540 = 336,
        WP9550 = 337,
        WP9560 = 338,
        WP9570 = 339,
        WP9580 = 340,
        WP9590 = 341,
        WP9600 = 342,
        WP9610 = 343,
        WP9620 = 344,
        WP9630 = 345,
        WP9640 = 346,
        WP9650 = 347,
        WP9660 = 348,
        WP9670 = 349,
        WP9680 = 350,
        WP9690 = 351,
        WP9700 = 352,
        WP9710 = 353,
        WP9720 = 354,
        WP9730 = 355,
        WP9740 = 356,
        WP9750 = 357,
        WP9760 = 358,
        WP9770 = 359,
        WP9780 = 360,
        WP9790 = 361,
        WP9800 = 362,
        WP9810 = 363,
        WP9820 = 364,
        WP9830 = 365,
        WP9840 = 366,
        WP9850 = 367,
        WP9860 = 368,
        WP9870 = 369,
        WP9880 = 370,
        WP9890 = 371,
        WP9900 = 372,
        WP9910 = 373,
        WP9920 = 374,
        WP9930 = 375,
        WP9940 = 376,
        WP9950 = 377,
        WP9960 = 378,
        WP9970 = 379,
        WP9980 = 380,
        WP9990 = 381,
        // Added because these images exist but the item ids do not?
        ChemicalFlamethrower2 = 0x7F000001,
        SparkShot2 = 0x7F000002,
        ATM42 = 0x7F000003,
        ATM43 = 0x7F000004,
        ATM44 = 0x7F000005,
        AntiTankRocketLauncher2 = 0x7F000006,
        AntiTankRocketLauncher3 = 0x7F000007,
        AntiTankRocketLauncher4 = 0x7F000008,
        Minigun2 = 0x7F000009,
        Minigun3 = 0x7F00000A,
        Minigun4 = 0x7F00000B,
        SMG_LE5_Infinite2 = 0x7F00000C,
    }

    [Flags]
    public enum WeaponParts : int
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 4,
    }
}