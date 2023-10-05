using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace UwUBotcord
{
    /// <summary>
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:UwUBotcord"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:UwUBotcord;assembly=UwUBotcord"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:Message/>
    ///
    /// </summary>

    //[TemplatePart(Name = "Embeds", Type = typeof(Grid))]
    public class Message : Control
    {
        static Message()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Message), new FrameworkPropertyMetadata(typeof(Message)));
        }

        internal string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        internal string AuthorAvatarUrl
        {
            get { return (string)GetValue(AuthorAvatarUrlProperty); }
            set { SetValue(AuthorAvatarUrlProperty, value); }
        }
        internal string AuthorName
        {
            get { return (string)GetValue(AuthorNameProperty); }
            set { SetValue(AuthorNameProperty, value); }
        }
        internal Brush AuthorNameColor
        {
            get { return (Brush)GetValue(AuthorNameColorProperty); }
            set { SetValue(AuthorNameColorProperty, value); }
        }

        internal List<ClassJSON.Embed> Embeds
        {
            get { return (List<ClassJSON.Embed>)GetValue(EmbedsProperty); }
            set { SetValue(EmbedsProperty, value); }
        }
        
        private int index_embed { get; set; }

        public void Funt()
        {
            var myAimedGrid = this.Template.FindName("embeds", this) as Grid;
            Console.WriteLine($"test : {myAimedGrid.Visibility}");
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(string), typeof(Message), new PropertyMetadata(null));
        public static readonly DependencyProperty AuthorAvatarUrlProperty = DependencyProperty.Register("AuthorAvatarUrl", typeof(string), typeof(Message), new PropertyMetadata(null));
        public static readonly DependencyProperty AuthorNameProperty = DependencyProperty.Register("AuthorName", typeof(string), typeof(Message), new PropertyMetadata(null));
        public static readonly DependencyProperty AuthorNameColorProperty = DependencyProperty.Register("AuthorNameColor", typeof(Brush), typeof(Message), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty EmbedsProperty = DependencyProperty.Register("Embeds", typeof(List<ClassJSON.Embed>), typeof(Message), new PropertyMetadata(new List<ClassJSON.Embed>()));

        public static RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Message));

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

        private static void OnMessageChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        {
            //ListIconButton tmp = (ListIconButton)Source;
            //tmp.Radius = (int)e.NewValue;
            Console.WriteLine("!!!");
        }
    }
}
