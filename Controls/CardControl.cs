using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Rzr.Core.Controls
{
    /// <summary>
    /// Control that presents a visual representation of a card (the bitmap image of the card
    /// </summary>
    public class CardControl : Image
    {
        #region property definitions

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int?), typeof(CardControl), new PropertyMetadata(null, OnCardBackChanged));

        #endregion

        #region properties

        /// <summary>
        /// The style of the card back
        /// </summary>
        public string BackStyle { get; set; }

        /// <summary>
        /// The current value of the card
        /// </summary>
        public int? Value
        {
            get { return (int?)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        #endregion

        protected static void OnCardBackChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CardControl control = sender as CardControl;
            control.UpdateImage();
        }

        #region initialise

        /// <summary>
        /// Static initialiser, to declare the default style for the control
        /// </summary>
        static CardControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CardControl), new FrameworkPropertyMetadata(typeof(CardControl)));
        }

        /// <summary>
        /// Constructor for an unknown card
        /// </summary>
        public CardControl()
        {
            this.Value = null;
        }

        /// <summary>
        /// Constructor for a known card
        /// </summary>
        /// <param name="card"></param>
        public CardControl(int card)
        {
            this.Value = card;
        }

        #endregion

        /// <summary>
        /// Updates the card image to match the current card value
        /// </summary>
        public void UpdateImage()
        {
            if (Value != null)
            {
                this.Source = Utilities.LoadBitmap(RzrConfiguration.Cards[(int)Value]);
            }
            else
            {
                if (BackStyle == "Empty")
                {
                    this.Source = null;
                }
                else
                {
                    this.Source = Utilities.LoadBitmap(Properties.Resources.CARD_BACK_3);
                }
            }
            this.UpdateLayout();
        }
    }
}
