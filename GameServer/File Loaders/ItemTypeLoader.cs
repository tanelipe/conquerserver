using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace GameServer
{
    public struct ItemDetail
    {
        public string Name;
        public string Description;

        public uint ID;
        public uint Price;

        public ushort Class;
        public ushort Profiency;
        public ushort Level;
        public ushort Strength;
        public ushort Dexterity;
        public ushort Vitality;
        public ushort Spirit;
        public ushort MaxDamage;
        public ushort MinDamage;
        public ushort Defence;
        public ushort DexterityBonus;
        public ushort DodgeBonus;
        public ushort HitPointBonus;
        public ushort ManaPointBonus;
        public ushort MagicAttack;
        public ushort MagicDefenceBonus;
        public ushort Durability;
        public ushort MaxDurability;
        public ushort Frequency;

        public byte Range;
        public byte TradeType;
    }
    public class ItemTypeLoader
    {
        private Dictionary<uint, ItemDetail> ItemData;
        private const string ITEMTYPE_LOCATION = "..\\..\\..\\Data Files\\itemtype.dat";

        public void LoadItems()
        {
            using (FileStream Stream = new FileStream(ITEMTYPE_LOCATION, FileMode.Open))
            {
                using (BinaryReader Reader = new BinaryReader(Stream))
                {
                    int Amount = Reader.ReadInt32();
                    ItemData = new Dictionary<uint, ItemDetail>(Amount);

                    Reader.BaseStream.Position += (Amount * 4);
                    for (int i = 0; i < Amount; i++)
                    {
                        ItemDetail Detail = new ItemDetail();
                        Detail.ID = Reader.ReadUInt32();
                        for (int j = 0; j < 16; j++)
                        {
                            Detail.Name += (char)Reader.ReadByte();
                        }
                        Detail.Name = Detail.Name.Trim('\x00');
                        Detail.Class = Reader.ReadByte();
                        Detail.Profiency = Reader.ReadByte();
                        Detail.Level = Reader.ReadUInt16();
                        Detail.Vitality = Reader.ReadUInt16();
                        Detail.Strength = Reader.ReadUInt16();
                        Detail.Dexterity = Reader.ReadUInt16();
                        Detail.Spirit = Reader.ReadUInt16();
                        Detail.TradeType = (byte)Reader.ReadUInt32();
                        Detail.Price = Reader.ReadUInt32();


                        Reader.BaseStream.Position += 4;

                        Detail.MaxDamage = Reader.ReadUInt16();
                        Detail.MinDamage = Reader.ReadUInt16();
                        Detail.Defence = Reader.ReadUInt16();
                        Detail.DexterityBonus = Reader.ReadUInt16();
                        Detail.DodgeBonus = Reader.ReadUInt16();
                        Detail.HitPointBonus = Reader.ReadUInt16();
                        Detail.ManaPointBonus = Reader.ReadUInt16(); ;
                        Detail.Durability = Reader.ReadUInt16();
                        Detail.MaxDurability = Reader.ReadUInt16();
                        Detail.MagicAttack = Reader.ReadUInt16();
                        Detail.MagicDefenceBonus = Reader.ReadUInt16();

                        Reader.BaseStream.Position += 6;

                        Detail.Range = (byte)Reader.ReadUInt16();
                        Detail.Frequency = Reader.ReadUInt16();

                        for (int x = 0; x < 16; x++)
                        {
                            Detail.Description += (char)Reader.ReadByte();
                        }

                        Detail.Description = Detail.Description.Trim('\x00');

                        if (!ItemData.ContainsKey(Detail.ID))
                        {
                            ItemData.Add(Detail.ID, Detail);

                        }
                        Reader.BaseStream.Position += 112;
                    }
                }
            }
        }
        public ItemDetail[] ItemDetails
        {
            get { return ItemData.Values.ToArray(); }
        }
    }
}
