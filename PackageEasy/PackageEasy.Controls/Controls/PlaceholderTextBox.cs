using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using PackageEasy.Common.Data;

namespace PackageEasy.Controls.Controls
{
    [TemplatePart(Name = "PART_BottomBorder", Type = typeof(Border))]
    public class PlaceholderTextBox : TextBox
    {

        VisualBrush visualBrush = null;
        Border border = null;
        public PlaceholderTextBox()
        {
            visualBrush = new VisualBrush();
            visualBrush.TileMode = TileMode.None;
            visualBrush.AutoLayoutContent = true;
            visualBrush.AlignmentX = AlignmentX.Left;
            visualBrush.AlignmentY = AlignmentY.Top;
            visualBrush.Stretch = Stretch.None;
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = 16;
            translateTransform.Y = 8;
            visualBrush.Transform = translateTransform;
            this.IsKeyboardFocusedChanged += PlaceholderTextBox_IsKeyboardFocusedChanged;
            this.TextChanged += PlaceholderTextBox_TextChanged;
            this.Loaded += PlaceholderTextBox_Loaded;
        }

        private void PlaceholderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (border == null) return;
            if (string.IsNullOrWhiteSpace(Text))
            {
                border.Background = visualBrush;
            }
            else
            {
                border.Background = Brushes.Transparent;
            }
        }

        private void PlaceholderTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (border == null) return;
            if (string.IsNullOrWhiteSpace(Text))
            {
                border.Background = visualBrush;
            }
            else
            {
                border.Background = Brushes.Transparent;
            }
        }

        private void PlaceholderTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (border == null) return;
            if (string.IsNullOrWhiteSpace(Text))
            {
                border.Background = visualBrush;
            }
            else
            {
                border.Background = Brushes.Transparent;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            border = GetTemplateChild("PART_DropButton") as Border;
            if (border == null) return;
            if (string.IsNullOrWhiteSpace(Text))
            {
                border.Background = visualBrush;
            }
            else
            {
                border.Background = Brushes.Transparent;
            }
        }
        private void PlaceholderTextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (border == null) return;
            if ((bool)e.NewValue == false && string.IsNullOrWhiteSpace(Text))
            {
                border.Background = visualBrush;
            }
            else
            {
                border.Background = Brushes.Transparent;
            }
        }


        public string PlaceHolderText
        {
            get
            {
                var result = (string)GetValue(PlaceHolderTextProperty);
                return result;
            }
            set
            {
                SetValue(PlaceHolderTextProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for PlaceHolderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceHolderTextProperty =
            DependencyProperty.Register("PlaceHolderText", typeof(string), typeof(PlaceholderTextBox), new PropertyMetadata("", new PropertyChangedCallback(PlaceHolderTextChangedCallBack)));

        private static void PlaceHolderTextChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(8, 0, 8, 0);
            textBlock.Visibility = Visibility.Visible;
            textBlock.FontSize = 12;
            var text = e.NewValue.ToString();
            PlaceholderTextBox placeholderTextBox = (PlaceholderTextBox)d;
            textBlock.Text = text.GetLangText();
            textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C6C6C6"));
            placeholderTextBox.visualBrush.Visual = textBlock;
        }


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PlaceholderTextBox), new PropertyMetadata(new CornerRadius(4)));


        public bool IsShowError
        {
            get { return (bool)GetValue(IsShowErrorProperty); }
            set { SetValue(IsShowErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowErrorProperty =
            DependencyProperty.Register("IsShowError", typeof(bool), typeof(PlaceholderTextBox), new PropertyMetadata(false));



    }
}
