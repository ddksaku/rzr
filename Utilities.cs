using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rzr.Core
{
    public class Utilities
    {
        public const string PARAMETER_DELIMITER = "||";
        public const string KEYVALUEPAIR_DELIMITER = "==";

        public static BitmapSource LoadBitmap(Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        /// <summary>
        /// Parses the content string into a dictionary of key/value pairs
        /// </summary>
        public static Dictionary<string, string> ParseParameterString(string content)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            string[] items = content.Split(new string[] { PARAMETER_DELIMITER }, StringSplitOptions.None);
            foreach (string item in items)
            {
                string[] pair = item.Split(new string[] { KEYVALUEPAIR_DELIMITER }, StringSplitOptions.None);
                if (pair.Length == 2)
                    parms[pair[0]] = pair[1];
            }

            return parms;
        }

        public static Control GetEditorControl(object obj)
        {
            object[] atts = obj.GetType().GetCustomAttributes(typeof(Editor), true);
            if (atts.Length > 0)
            {
                object editor = Activator.CreateInstance(((Editor)atts[0]).EditorType);
                return (Control)editor;
            }
            return null;
        }

        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child) as DependencyObject;

            if (parent != null)
            {
                if (parent is T) return parent as T;
                else return FindParent<T>(parent);
            }

            return null;
        }

        public static T FindChild<T>(DependencyObject item) where T : DependencyObject
        {
            int children = VisualTreeHelper.GetChildrenCount(item);

            for (int i = 1; i < 10; i++)
            {
                T ret = FindChild<T>(item, 1, i);
                if (ret != null) return ret;
            }

            return null;
        }

        protected static T FindChild<T>(DependencyObject item, int level, int maxLevel) where T : DependencyObject
        {
            int children = VisualTreeHelper.GetChildrenCount(item);

            for (int i = 0; i < children; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(item, i) as DependencyObject;

                if (level < maxLevel)
                {
                    T ret = FindChild<T>(child, level + 1, maxLevel);
                    if (ret != null) return ret;
                }
                else
                {
                    if (child != null && child is T) return child as T;                    
                }
            }

            return null;
        }

        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        public static Vector GetOffset(DependencyObject start, DependencyObject end)
        {
            return GetOffset(start, end, new Vector());
        }

        protected static Vector GetOffset(DependencyObject start, DependencyObject end, Vector startVector)
        {
            Vector ret = VisualTreeHelper.GetOffset((Visual)start);
            ret.X += startVector.X;
            ret.Y += startVector.Y;

            DependencyObject parent = VisualTreeHelper.GetParent(start) as DependencyObject;
            if (start == end)
                return ret;
            else if (parent != null)
                return GetOffset(parent, end, ret);
            else
                return ret;
        }
    }
}
