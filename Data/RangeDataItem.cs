using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rzr.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface RangeDataItem
    {
        /// <summary>
        /// A unique identifier for this item
        /// </summary>
        string ID { get; set; }       
 
        /// <summary>
        /// The higher the rank for this item, the better it is and the more likely to be included in the returned
        /// results. 
        /// 
        /// E.g. If there are a 100 items of Weight 100 and Probability 1% and a range of 20% is required, then the
        /// items with the top 20 Rank values are returned.
        /// </summary>
        float Rank { get; set; }

        /// <summary>
        /// The higher the weight the more times this item will be included relative to the probability of inclusion.
        /// 
        /// E.g. If the item has a probability of occuring of 1% and a weight of 80%, then it will account for 0.8% of 
        /// a range if it's rank falls within the range boundaries
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// The probability that this particular item occurs naturally. 
        /// 
        /// For example, the probability that two Aces are dealt from a pack of 52 cards is around 0.45%. 
        /// </summary>
        double Probability { get; set; }        
    }

    /// <summary>
    /// Extension of range data item that includes co-ordinates for plotting on a rectangular grid
    /// </summary>
    public interface RangeDisplayItem : RangeDataItem
    {
        /// <summary>
        /// The X co-ordinate of the item on the display grid
        /// </summary>
        int XCord { get; set; }

        /// <summary>
        /// The Y co-ordinate of the item on the display grid
        /// </summary>
        int YCord { get; set; }

        /// <summary>
        /// The description of the item, to be displayed on the grid
        /// </summary>
        string Description { get; set; }
    }
}
