using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UwUBotcord
{

    enum TypeListIconButton
    {
        DirectMessages,
        Server
    }
    public class ListIconButton : Control
    {
        static ListIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListIconButton), new FrameworkPropertyMetadata(typeof(ListIconButton)));
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public int Radius
        {
            get { return (int)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Visibility Block
        {
            get { return (Visibility)GetValue(BlockProperty); }
            set { SetValue(BlockProperty, value); }
        }

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        internal string Id { get; set; }

        internal int MaxRadius { get; set; }
        internal int MinRadius { get; set; }
        internal TypeListIconButton Type { get; set; }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(int), typeof(ListIconButton), new FrameworkPropertyMetadata(ListIconButton.OnRadiusChanged));
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ListIconButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(ListIconButton), new PropertyMetadata(null)/*new FrameworkPropertyMetadata(ListIconButton.OnContentChanged)*/);
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(TypeListIconButton), typeof(ListIconButton), new PropertyMetadata(null));
        public static readonly DependencyProperty BlockProperty = DependencyProperty.Register("Block", typeof(Visibility), typeof(ListIconButton), new PropertyMetadata(null));

        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListIconButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected virtual void OnClick()
        {
            RoutedEventArgs args = new RoutedEventArgs(ClickEvent, this);

            RaiseEvent(args);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            OnClick();
        }

        private static void OnRadiusChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        {
            ListIconButton tmp = (ListIconButton)Source;
            tmp.Radius = (int)e.NewValue;
        }
        //private static void OnContentChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        //{
        //    ListIconButton tmp = (ListIconButton)Source;
        //    Console.WriteLine(tmp.Content);
        //}
    }
}
