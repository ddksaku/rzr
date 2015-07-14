using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;

namespace Rzr.Core.Calculator
{
    [XmlRoot("Conditions")]
    public class ConditionMap
    {
        public static ulong HANDVALUE_HIGHCARD_MASK = 1ul;
        public static ulong HANDVALUE_PAIR_MASK = 1ul << 1;
        public static ulong HANDVALUE_TWOPAIR_MASK = 1ul << 2;
        public static ulong HANDVALUE_TRIPS_MASK = 1ul << 3;
        public static ulong HANDVALUE_STRAIGHT_MASK = 1ul << 4;
        public static ulong HANDVALUE_FLUSH_MASK = 1ul << 5;
        public static ulong HANDVALUE_FULLHOUSE_MASK = 1ul << 6;
        public static ulong HANDVALUE_QUADS_MASK = 1ul << 7;
        public static ulong HANDVALUE_STRAIGHTFLUSH_MASK = 1ul << 8;

        public static ulong SUBVALUE_ACETOP = 1ul << 10;
        public static ulong SUBVALUE_KINGTOP = 1ul << 11;
        public static ulong SUBVALUE_QUEENTOP = 1ul << 12;
        public static ulong SUBVALUE_JACKTOP = 1ul << 13;
        public static ulong SUBVALUE_TENLOWERTOP = 1ul << 14;

        public static ulong SUBVALUE_TWOOVERCARDS = 1ul << 15;
        public static ulong SUBVALUE_ONEOVERCARD = 1ul << 16;
        public static ulong SUBVALUE_NOOVERCARDS = 1ul << 17;

        public static ulong SUBVALUE_POCKETPAIR = 1ul << 10;
        public static ulong SUBVALUE_NOPOCKETPAIR = 1ul << 11;

        public static ulong SUBVALUE_PAIRONBOARD = 1ul << 12;
        public static ulong SUBVALUE_NOPAIRONBOARD = 1ul << 13;

        public static ulong SUBVALUE_PAIROVERPAIR = 1ul << 14;
        public static ulong SUBVALUE_PAIRTOPPAIR = 1ul << 15;
        public static ulong SUBVALUE_PAIRSECONDPAIR = 1ul << 16;
        public static ulong SUBVALUE_PAIRLOWERPAIR = 1ul << 17;

        public static ulong SUBVALUE_SECONDPAIRTOPPAIR = 1ul << 18;
        public static ulong SUBVALUE_SECONDPAIRSECONDPAIR = 1ul << 19;
        public static ulong SUBVALUE_SECONDPAIRLOWERPAIR = 1ul << 49;

        public static ulong SUBVALUE_THREESET = 1ul << 15;
        public static ulong SUBVALUE_THREETRIPS = 1ul << 16;
        public static ulong SUBVALUE_THREEBOARD = 1ul << 17;

        public static ulong SUBVALUE_TOP = 1ul << 10;
        public static ulong SUBVALUE_SECOND = 1ul << 11;
        public static ulong SUBVALUE_LOWER = 1ul << 12;

        public static ulong SUBVALUE_HOLECARDSBOTH = 1ul << 13;
        public static ulong SUBVALUE_HOLECARDSONE = 1ul << 14;
        public static ulong SUBVALUE_HOLECARDSNONE = 1ul << 15;

        public static ulong DRAWTYPE = 1ul << 20;
        
        public static ulong SUBDRAWVALUE_OPENENDEDSTRAIGHT = 1ul << 21;
        public static ulong SUBDRAWVALUE_DOUBLEGUTSHOT = 1ul << 21;
        public static ulong SUBDRAWVALUE_OPENENDEDSTRAIGHTORDOUBLEGUTSHOT = 1ul << 23;
        public static ulong SUBDRAWVALUE_SINGLEENDEDSTRAIGHT = 1ul << 24;
        public static ulong SUBDRAWVALUE_GUTSHOT = 1ul << 25;
        public static ulong SUBDRAWVALUE_SINGLEENDEDSTRAIGHTORGUTSHOT = 1ul << 26;
        public static ulong SUBDRAWVALUE_NOSTRAIGHTDRAW = 1ul << 27;

        public static ulong SUBDRAWVALUE_FLUSHDRAW = 1ul << 28;
        public static ulong SUBDRAWVALUE_NUTFLUSHDRAW = 1ul << 29;
        public static ulong SUBDRAWVALUE_BACKDOORFLUSHDRAW = 1ul << 30;
        public static ulong SUBDRAWVALUE_NOFLUSHDRAW = 1ul << 31;

        public static ulong SUBDRAWVALUE_FLUSHANDOPENENDEDSTRAIGHT = 1ul << 32;
        public static ulong SUBDRAWVALUE_FLUSHANDGUTSHOT = 1ul << 33;
        public static ulong SUBDRAWVALUE_BACKDOORFLUSHANDOPENENDEDSTRAIGHT = 1ul << 34;
        public static ulong SUBDRAWVALUE_BACKDOORFLUSHANDGUTSHOT = 1ul << 35;

        public static ulong SUBDRAWVALUE_TWOOVERCARDS = 1ul << 36;
        public static ulong SUBDRAWVALUE_ONEOVERCARDS = 1ul << 37;
        public static ulong SUBDRAWVALUE_NOOVERCARDS = 1ul << 38;

        public static ulong SUBDRAWVALUE_BOTHHOLECARDS = 1ul << 39;
        public static ulong SUBDRAWVALUE_ONEHOLECARD = 1ul << 40;
        public static ulong SUBDRAWVALUE_NOHOLECARDS = 1ul << 41;

        public static ulong SUBVALUE_KICKER_A = 1ul << 50;
        public static ulong SUBVALUE_KICKER_K = 1ul << 51;
        public static ulong SUBVALUE_KICKER_Q = 1ul << 52;
        public static ulong SUBVALUE_KICKER_J = 1ul << 53;

        [XmlElement("Condition")]
        public PrimaryCondition[] Conditions { get; set; }
    }

    [XmlType("Condition")]
    public class PrimaryCondition
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("MaskName")]
        public string MaskName { get; set; }

        [XmlAttribute("MaskValue")]
        public ulong MaskValue { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Group")]
        public string GroupName { get; set; }

        [XmlArray("SubConditions")]
        public List<SecondaryCondition> SubConditions { get; set; }

        public void SetValue()
        {
            if (!String.IsNullOrEmpty(MaskName))
            {
                try
                {
                    Type t = typeof(ConditionMap);
                    FieldInfo info = t.GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.Name == this.MaskName);
                    object value = info.GetValue(null);
                    MaskValue = Convert.ToUInt64(value);
                }
                catch (Exception exc)
                {
                    throw new Exception("Field: " + this.MaskName, exc);
                }
            }

            if (SubConditions != null)
            {
                foreach (SecondaryCondition condition in SubConditions)
                {
                    condition.SetValue();
                }
            }
        }
    }

    [XmlType("Component")]
    public class SecondaryCondition
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlArray("Values")]
        public List<SecondaryConditionValue> Values { get; set; }

        public void SetValue()
        {
            if (Values != null)
            {
                foreach (SecondaryConditionValue condition in Values)
                {
                    condition.SetValue();
                }
            }

        }
    }

    [XmlType("Value")]
    public class SecondaryConditionValue
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("MaskName")]
        public string MaskName { get; set; }

        [XmlAttribute("MaskValue")]
        public ulong MaskValue { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        public void SetValue()
        {
            if (!String.IsNullOrEmpty(MaskName))
            {
                Type t = typeof(ConditionMap);
                FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.Static);
                FieldInfo info = fields.FirstOrDefault(x => x.Name == this.MaskName);
                object value = info.GetValue(null);
                MaskValue = Convert.ToUInt64(value);
            }
        }
    }
}
