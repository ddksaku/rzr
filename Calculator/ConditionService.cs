using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Rzr.Core.Data;
using Rzr.Core.Game;
using Rzr.Core.Xml;

namespace Rzr.Core.Calculator
{
    /// <summary>
    /// The condition service manages a set of conditions available for evaluating 
    /// holdem hands against the board cards
    /// </summary>
    public class ConditionService
    {
        #region static

        public const string RZR_CONDITION_SAVE_SUFFIX = ".conditions.rzr";
        public const string RZR_DEFAULT_CONDITION_NAME = "Default";
        public const string RZR_LIST_CONDITION_NAME = "List";
        public const string RZR_TABLEX_CONDITION_NAME = "TableX";
        public const string RZR_TABLEY_CONDITION_NAME = "TableY";

        /// <summary>
        /// The default service - initialised at runtime - contains the default conditions set
        /// </summary>
        public static ConditionService DefaultService { get; private set; }

        /// <summary>
        /// The master condition map - list of possible conditions
        /// </summary>
        public static ConditionMap ConditionMap { get; private set; }

        /// <summary>
        /// A list of saved condition sets available 
        /// </summary>
        public static List<string> AvailableConditionSets { get; private set; }

        /// <summary>
        /// Initialise the default service withthe default condition set
        /// </summary>
        public static void Initialise()
        {
            string mapPath = Path.Combine(RzrConfiguration.DataDirectory, RzrConfiguration.ConditionsMap);
            ConditionMap = LoadMap(mapPath);

            string conditionsPath = Path.Combine(RzrConfiguration.DataDirectory, RzrConfiguration.DefaultConditions);
            DefaultService = new ConditionService(conditionsPath, false);
            DefaultService.Name = RZR_DEFAULT_CONDITION_NAME;

            DirectoryInfo info = new DirectoryInfo(RzrConfiguration.SaveDirectory);
            FileInfo[] files = info.GetFiles("*" + RZR_CONDITION_SAVE_SUFFIX);
            AvailableConditionSets = files.Select(x => x.Name.Replace(RZR_CONDITION_SAVE_SUFFIX, "")).ToList();
        }

        /// <summary>
        /// Load the condition map
        /// </summary>
        /// <returns></returns>
        private static ConditionMap LoadMap(string mapFile)
        {
            using (StreamReader reader = new StreamReader(mapFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConditionMap));
                ConditionMap map =  serializer.Deserialize(reader) as ConditionMap;
                foreach (PrimaryCondition condition in map.Conditions)
                    condition.SetValue();
                return map;
            }   
        }

        #endregion

        public string Name { get; set; }

        public string FileName { get; private set; }

        public ConditionsDefinition Definition { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="conditionsFile">The file containing the conditions definitions</param>
        public ConditionService(string conditionsFile, bool setDirectory) 
        {
            Name = conditionsFile;
            if(setDirectory)
                conditionsFile = Path.Combine(RzrConfiguration.SaveDirectory, conditionsFile + RZR_CONDITION_SAVE_SUFFIX);
            DoLoadDefinition(conditionsFile);
        }

        public ConditionService(string conditionsFile, ConditionsDefinition definition)
        {
            Name = conditionsFile;
            FileName = conditionsFile;
            Definition = definition;
        }

        public void LoadDefinition(string conditionsFile)
        {
            string path = Path.Combine(RzrConfiguration.SaveDirectory, conditionsFile + RZR_CONDITION_SAVE_SUFFIX);
            DoLoadDefinition(path);
            if (Load != null) Load();
        }

        public void SaveDefinition()
        {
            string path = Path.Combine(RzrConfiguration.SaveDirectory, FileName + RZR_CONDITION_SAVE_SUFFIX);
            DoSaveDefinition(path);
            if (Save != null) Save();
        }

        public void SaveDefinitionAs(string conditionsFile)
        {
            string path = Path.Combine(RzrConfiguration.SaveDirectory, conditionsFile + RZR_CONDITION_SAVE_SUFFIX);
            DoSaveDefinition(path);
            if (Save != null) Save();
        }

        public ConditionService SaveNewDefinitionAs(string conditionsFile)
        {
            ConditionService service = new ConditionService(conditionsFile, Definition);
            service.SaveDefinition();
            return service;
        }

        private void DoLoadDefinition(string conditionsFile)
        {            
            FileName = conditionsFile;
            using (StreamReader reader = new StreamReader(conditionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConditionsDefinition));
                Definition = serializer.Deserialize(reader) as ConditionsDefinition;
            }
        }

        private void DoSaveDefinition(string conditionsFile)
        {
            FileName = conditionsFile;
            using (StreamWriter writer = new StreamWriter(conditionsFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConditionsDefinition));
                serializer.Serialize(writer, Definition);
            }
        }        

        public void Distribute(List<CompiledCondition> conditions, HandRangeModel range, int numIterations)
        {
            RangeCalculatorService service = new RangeCalculatorService();
            HandMask[][][] masks = service.Calculate(new HandRange[] { range.Range }, new Card[] { null, null, null, null, null }, 
                numIterations, new bool[] { true, false, false });

            int[] hits = new int[conditions.Count];
            for (int i = 0; i < numIterations; i++)
            {
                HandMask mask = masks[i][0][0];
                int index = 0;
                foreach (CompiledCondition container in conditions)
                {
                    if (container.Matches(mask))
                    {
                        hits[index]++;
                        break;
                    }
                    index++;
                }
            }

            for (int i = 0; i < this.Definition.ConfiguredConditions.Count; i++)
            {
                this.Definition.ConfiguredConditions[i].ExpectedProbability = 0;
            }

            for (int i = 0; i < hits.Length; i++)
            {
                conditions[i].Container.ExpectedProbability = (float)hits[i] / numIterations;
            }
        }

        public List<CompiledCondition> GetCompiledConditions(bool includeInactive)
        {
            if (includeInactive)
            {
                return GetCompiledConditions(Definition.ConfiguredConditions);
            }
            else
            {
                return GetCompiledConditions(Definition.ConfiguredConditions.FindAll(x => 
                    this.Definition.ActiveItems.Exists(y => y.Name == x.Name)));
            }
        }

        public List<CompiledCondition> GetCompiledConditions(string key)
        {
            List<Parameter> parms = RzrInit.InitFile.Parms.FindAll(x => x.Key == key);
            List<ConditionContainer> containers = parms.Select(x =>
                ConditionService.DefaultService.Definition.ConfiguredConditions.Find(y =>
                    y.Name == x.Value)).ToList();
            return ConditionService.DefaultService.GetCompiledConditions(containers);
        }

        public List<CompiledCondition> GetCompiledConditions(List<ConditionContainer> containers)
        {
            return containers.Where(x => x != null).Select(x => new CompiledCondition(x, this)).ToList();
        }

        public event EmptyEventHandler Load;
        public event EmptyEventHandler Save;
    }
}
