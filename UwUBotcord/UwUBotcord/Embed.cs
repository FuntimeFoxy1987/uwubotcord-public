using System;
using System.Collections.Generic;
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
    ///     <MyNamespace:Embed/>
    ///
    /// </summary>
    public class Embed : Control
    {
        static Embed()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Embed), new FrameworkPropertyMetadata(typeof(Embed)));
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public ImageSource ImageUrl
        {
            get { return (ImageSource)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }
        public ImageSource ThumbnailUrl
        {
            get { return (ImageSource)GetValue(ThumbnailUrlProperty); }
            set { SetValue(ThumbnailUrlProperty, value); }
        }
        public string AuthorName
        {
            get { return (string)GetValue(AuthorNameProperty); }
            set { SetValue(AuthorNameProperty, value); }
        }
        public ImageSource AuthorIconUrl
        {
            get { return (ImageSource)GetValue(AuthorIconUrlProperty); }
            set { SetValue(AuthorIconUrlProperty, value); }
        }
        public string FooterText
        {
            get { return (string)GetValue(FooterTextProperty); }
            set { SetValue(FooterTextProperty, value); }
        }
        public ImageSource FooterIсonUrl
        {
            get { return (ImageSource)GetValue(FooterIconUrlProperty); }
            set { SetValue(FooterIconUrlProperty, value); }
        }

        public double SizeTitle {
            get { return (double)GetValue(SizeTitleProperty); }
            set { SetValue(SizeTitleProperty, value); }
        }
        public double SizeDescription
        {
            get { return (double)GetValue(SizeDescriptionProperty); }
            set { SetValue(SizeDescriptionProperty, value); }
        }
        public double SizeAuthor
        {
            get { return (double)GetValue(SizeAuthorProperty); }
            set { SetValue(SizeAuthorProperty, value); }
        }
        public double SizeAuthorIcon
        {
            get { return (double)GetValue(SizeAuthorIconProperty); }
            set { SetValue(SizeAuthorIconProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(Embed), new FrameworkPropertyMetadata(OnDescriptionChanged));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Embed), new FrameworkPropertyMetadata(OnTitleChanged));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Embed), new PropertyMetadata(null));
        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl", typeof(ImageSource), typeof(Embed), new PropertyMetadata(null));
        public static readonly DependencyProperty ThumbnailUrlProperty = DependencyProperty.Register("ThumbnailUrl", typeof(ImageSource), typeof(Embed), new PropertyMetadata(null));
        public static readonly DependencyProperty AuthorNameProperty = DependencyProperty.Register("AuthorName", typeof(string), typeof(Embed), new FrameworkPropertyMetadata(OnAuthorNameChanged));
        public static readonly DependencyProperty AuthorIconUrlProperty = DependencyProperty.Register("AuthorIconUrl", typeof(ImageSource), typeof(Embed), new FrameworkPropertyMetadata(OnAuthorUrlChanged));
        public static readonly DependencyProperty FooterTextProperty = DependencyProperty.Register("FooterText", typeof(string), typeof(Embed), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterIconUrlProperty = DependencyProperty.Register("FooterIсonUrl", typeof(ImageSource), typeof(Embed), new PropertyMetadata(null));

        public static readonly DependencyProperty SizeTitleProperty = DependencyProperty.Register("SizeTitle", typeof(double), typeof(Embed), new PropertyMetadata(0.0));
        public static readonly DependencyProperty SizeDescriptionProperty = DependencyProperty.Register("SizeDescription", typeof(double), typeof(Embed), new PropertyMetadata(0.0));
        public static readonly DependencyProperty SizeAuthorProperty = DependencyProperty.Register("SizeAuthor", typeof(double), typeof(Embed), new PropertyMetadata(0.0));
        public static readonly DependencyProperty SizeAuthorIconProperty = DependencyProperty.Register("SizeAuthorIcon", typeof(double), typeof(Embed), new PropertyMetadata(0.0));

        private void FF()
        {
            Console.WriteLine(this.Description);
        }

        private static void OnTitleChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        {
            Embed tmp = (Embed)Source;
            if (e.NewValue.ToString() != "")
            {
                tmp.SizeTitle = 30;
            }
            else
            {
                tmp.SizeTitle = 0;
            }
        }
        private static void OnDescriptionChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        {
            Embed tmp = (Embed)Source;
            if (e.NewValue.ToString() != "")
            {
                tmp.SizeDescription = 26;
                //tmp.FF();
            }
            else
            {
                tmp.SizeDescription = 0;
            }
        }
        private static void OnAuthorUrlChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        {
            Embed tmp = (Embed)Source;
            if (e.NewValue.ToString() != "")
            {
                if(tmp.AuthorName != "")
                {
                    tmp.SizeAuthor = 32;
                }
                else
                {
                    tmp.SizeAuthor = 0;
                }

                tmp.SizeAuthorIcon = 32;
            }
            else
            {
                tmp.SizeAuthor = 0;
                tmp.SizeAuthorIcon = 0;
            }
        }
        private static void OnAuthorNameChanged(DependencyObject Source, DependencyPropertyChangedEventArgs e)
        {
            Embed tmp = (Embed)Source;
            if (e.NewValue.ToString() != "")
            {
                if (tmp.SizeAuthor < 30)
                {
                    tmp.SizeAuthor = 30;
                }
            }
            else
            {
                tmp.SizeAuthor = 0;
            }
        }
    }
}
