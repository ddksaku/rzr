﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Rzr.Core.Data;

namespace Rzr.Core.Xml
{
    [XmlRoot("Root")]
    public class HoleCardRangesFile
    {
        [XmlArray("Ranges")]
        public List<HoleCardRangeDefinition> Ranges { get; set; }
    }
}