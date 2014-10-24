using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _2048WP.CustomControls
{
    public class ValueColors
    {
        public ValueColors()
        {
            _innerCollection.Add(0, new ValueColor(0, Color.FromArgb(100, 255, 255, 255), Color.FromArgb(0, 255, 255, 255)));
            _innerCollection.Add(2, new ValueColor(2, Color.FromArgb(0xFF, 238, 228, 218), Color.FromArgb(0xFF, 119, 110, 101)));
            _innerCollection.Add(4, new ValueColor(4, Color.FromArgb(0xFF, 237, 224, 200), Color.FromArgb(0xFF, 119, 110, 101)));
            _innerCollection.Add(8, new ValueColor(8, Color.FromArgb(0xFF, 242, 177, 121), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(16, new ValueColor(16, Color.FromArgb(0xFF, 245, 149, 99), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(32, new ValueColor(32, Color.FromArgb(0xFF, 246, 124, 95), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(64, new ValueColor(64, Color.FromArgb(0xFF, 246, 94, 59), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(128, new ValueColor(128, Color.FromArgb(0xFF, 237, 207, 114), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(256, new ValueColor(256, Color.FromArgb(0xFF, 237, 204, 97), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(512, new ValueColor(512, Color.FromArgb(0xFF, 237, 200, 80), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(1024, new ValueColor(1024, Color.FromArgb(0xFF, 237, 197, 63), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(2048, new ValueColor(2048, Color.FromArgb(0xFF, 237, 194, 46), Color.FromArgb(0xFF, 249, 246, 242)));
            _innerCollection.Add(2049, new ValueColor(2049, Color.FromArgb(0xFF, 60, 179, 113), Color.FromArgb(0xFF, 249, 246, 242)));

        }
        private Dictionary<int, ValueColor> _innerCollection = new Dictionary<int, ValueColor>();

        public int Value
        { get; set; }

        public ValueColor GetValueColor(int value)
        {
            if (value > 2048)
                return _innerCollection[2049];
            return _innerCollection[value];
        }

        public Brush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(GetValueColor(Value).BackColor);
            }
        }

        public Brush ForegroundColor
        {
            get
            {
                return new SolidColorBrush(GetValueColor(Value).TextColor);
            }
        }
    }

    public class ValueColor
    {
        public ValueColor(int value, Color backColor, Color textColor)
        {
            Value = value;
            BackColor = backColor;
            TextColor = textColor;
        }
        public readonly int Value;
        public readonly Color BackColor;
        public readonly Color TextColor;
    }
}
