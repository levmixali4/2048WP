using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace _2048WP.CustomControls
{
    public partial class Tile : UserControl
    {
        ValueColors ValueColors = new ValueColors();
        public Tile(int value)
        {
            InitializeComponent();
            Value = value;
            this.DataContext = ValueColors;
        }
        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                ValueColors.Value = value;

                Border.Background = ValueColors.BackgroundColor;
                TextValue.Foreground = ValueColors.ForegroundColor;
                if (_value == 0)
                {
                    TextValue.Text = string.Empty;
                }
                else
                {
                    TextValue.Text = _value.ToString();
                    TextValue.FontSize = 30;
                }
            }
        }
    }
}
