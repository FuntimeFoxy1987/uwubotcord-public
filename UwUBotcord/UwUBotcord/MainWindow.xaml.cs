namespace UwUBotcord
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using System.Windows.Controls;
    using UwUBotcord.styles;

    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Media.Animation;
    using System.Text.Json;
    using System.Text;
    using System.Windows.Markup;
    using System.Collections.Specialized;
    using System.Windows.Documents;
    using Emoji.Wpf;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    public class DoubleToEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object result = (double)values[0];
            for (int i = 1; i < values.Length; i++)
            {
                result = (double)result + (double)values[i];
            }
            return result;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainWindow : System.Windows.Window
    {
        //private RenderWindow _renderWindow;
        //private readonly CircleShape _circle;
        private readonly DispatcherTimer _timer;

        private int Number;

        private Dictionary<string, ListIconButton> listIconButton = new Dictionary<string, ListIconButton>();

        private string listIconButton_selected_id = "";

        public static bool ShowGridLinesBool { get; set; } = true;

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void Callback_Str(string value);
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public delegate void Callback_int32(Int32 value);
        [STAThread]
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetNotification(Callback_Str notification);
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void mainDLL();
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool onReady();
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Get_list_guilds(Callback_Str data);
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Get_list_DMS(Callback_Str data);
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Send_message_DM(string data, string id_user);
        [DllImport("Core.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Get_list_messages_DM(Callback_Str data, string id_user, Int64 before);
        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();

            Thread th = new Thread(TreadCore);
            th.Name = "CoreDLL";
            th.Start();

            while (!onReady())
            {
                Color color = HsLtoRgb(169, 0.8, 0.8);
                //Console.WriteLine(color);
            };

            string str_msgs = "";

            Get_list_messages_DM(data => str_msgs = data, "609654736731242516", 0);

            str_msgs = Format_Default_to_UTF8(str_msgs);

            Console.WriteLine(str_msgs);

            var msgs = JsonSerializer.Deserialize<List<ClassJSON.Message>>(str_msgs);

            //List<ClassJSON.Embed> emds = new List<ClassJSON.Embed>()
            //{
            //    new ClassJSON.Embed(){author_name = "you", description = "Всех девушек хочу поздравить с 8 марта.\r\nДля меня, 8 марта, один из самых мистических праздников. Часто, именно в этот день погода на улице особенно солнечный. Но, после этого праздника, на следующий день, мир снова уходит в суровый климат. И, кто бы что не говорил, но в нашем мире женщины тоже имеют особенную роль: Хранительница домашнего очага и уюта. Ведь именно они помогают нам, мужчинам, преодолевать то, что ранее нам было не под силу.\r\nДорогие девочки, девушки и женщины. От всей души, я поздравляю вас с 8 марта, и желаю вам, чтобы все ваши мечты, желания, и цели исполнялись ровно так, как должны были. И чтобы ваша красота никогда не угасала.", color = "#731353"}
            //};
            //msgs.Add(new ClassJSON.Message() { author_name = "???", content = "text 1", author_avatar_url = "https://cdn.discordapp.com/emojis/720285299200229386.webp?size=96" , embeds = emds});
            Messages.ItemsSource = msgs;

            //string thisPathData = "M12233 M222333 M3443";// fake
            //c.Clip = Geometry.Parse(thisPathData);

            //TabsWindows.SelectedIndex = 1;

            //public const string 

            //SolidColorBrush

            //this._circle = new CircleShape(20) { FillColor = Color.Magenta };
            //this.CreateRenderWindow();
            TabsWindows.SelectedIndex = 0;
            TabsServer.SelectedIndex = 1;
            TabsServerChannels.SelectedIndex = 0;

            var refreshRate = new TimeSpan(0, 0, 0, 1, 0);
            this._timer = new DispatcherTimer { Interval = refreshRate };
            _timer.Tick += Timer_Tick;
            this._timer.Start();

            string str = "";

            Get_list_guilds(data => str = data);

            ClassJSON.List_guilds list_Guilds = JsonSerializer.Deserialize<ClassJSON.List_guilds>(str);

            for (int i = 0; i < list_Guilds.array.Length; i++)
            {
                ClassJSON.Guilds guild = list_Guilds.guilds[list_Guilds.array[i]];

                ListIconButton button = new ListIconButton();

                if (guild.icon_url != "")
                {
                    button.Image = new BitmapImage(new Uri(guild.icon_url, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    button.Image = new BitmapImage(new Uri("./logoUwUBotcord.ico", UriKind.RelativeOrAbsolute));
                }

                TranslateTransform translate = new TranslateTransform();

                translate.Y = 66 + (i * 50);

                button.RenderTransform = translate;

                button.Width = 47;
                button.Height = 47;

                button.Radius = 47;
                button.MaxRadius = 47;
                button.MinRadius = 10;

                button.Click += new RoutedEventHandler(ListIconButton_Click);
                button.MouseEnter += new MouseEventHandler(ListIconButton_MouseEnter);
                button.MouseLeave += new MouseEventHandler(ListIconButton_MouseLeave);

                button.Type = TypeListIconButton.Server;

                button.Content = Format_Default_to_UTF8(guild.name);
                button.Id = list_Guilds.array[i];

                button.VerticalAlignment = VerticalAlignment.Top;

                ListIcons.Children.Add(button);

                if (button != null)
                {
                    listIconButton.Add($"{list_Guilds.array[i]}", button);
                }
            }

            //Test.Text = Format_Default_to_UTF8(list_Guilds.guilds["681410931808403513"].name);
        }

        private string Format_Default_to_UTF8 (string str)
        {
            byte[] Buffer_defaultFormatStr = Encoding.Default.GetBytes(str);
            string utf8FormatStr = Encoding.UTF8.GetString(Buffer_defaultFormatStr);
            return utf8FormatStr;
        }
        static internal void TreadCore()
        {
            int hash = Thread.CurrentThread.GetHashCode();
            string name = Thread.CurrentThread.Name;
            Console.WriteLine("hash: {0} name: {1}", hash, name);
            mainDLL();
        }
        public static Color HsLtoRgb(double h, double s, double l, int a = 255)
        {
            h = Math.Max(0D, Math.Min(360D, h));
            s = Math.Max(0D, Math.Min(1D, s));
            l = Math.Max(0D, Math.Min(1D, l));
            a = Math.Max(0, Math.Min(255, a));

            // achromatic argb (gray scale)
            if (Math.Abs(s) < 0.000000000000001)
            {
                return Color.FromArgb(
                        (byte)a,
                        (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{l * 255D:0.00}")))),
                        (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{l * 255D:0.00}")))),
                        (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{l * 255D:0.00}")))));
            }

            double q = l < .5D
                    ? l * (1D + s)
                    : (l + s) - (l * s);
            double p = (2D * l) - q;

            double hk = h / 360D;
            double[] T = new double[3];
            T[0] = hk + (1D / 3D); // Tr
            T[1] = hk; // Tb
            T[2] = hk - (1D / 3D); // Tg

            for (int i = 0; i < 3; i++)
            {
                if (T[i] < 0D)
                    T[i] += 1D;
                if (T[i] > 1D)
                    T[i] -= 1D;

                if ((T[i] * 6D) < 1D)
                    T[i] = p + ((q - p) * 6D * T[i]);
                else if ((T[i] * 2D) < 1)
                    T[i] = q;
                else if ((T[i] * 3D) < 2)
                    T[i] = p + ((q - p) * ((2D / 3D) - T[i]) * 6D);
                else
                    T[i] = p;
            }

            return Color.FromArgb(
                    (byte)a,
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{T[0] * 255D:0.00}")))),
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{T[1] * 255D:0.00}")))),
                    (byte)Math.Max(0, Math.Min(255, Convert.ToInt32(double.Parse($"{T[2] * 255D:0.00}")))));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Number++;
            //Button newButton = new Button() { Content = "hey" };
            //newButton.Style = UwUBotcord.styles.Styl;
            //ResourceDictionary 

            //GridF.Children.Add(newButton);

            //newButton.Text = "Created Button";
            //newButton.Location = new Point(70, 70);
            //newButton.Size = new Size(50, 100);
            //Text.Text = Convert.ToString(Number);
            //if (Number == 10)
            //{
            //    TabsWindows.SelectedIndex = 1;
            //}

            if (Number == null)
            {
                //GridF.Visibility = Visibility.Visible;
                //BackGraundImage.Source = new BitmapImage(new Uri("./2.png", UriKind.RelativeOrAbsolute));
                System.Windows.Controls.Image image = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri("./2.png", UriKind.RelativeOrAbsolute))
                };
                GridF.Children.Add(image);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void TabsWindows_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {



        }

        private void ListIconButton_Click(object sender, RoutedEventArgs e)
        {
            if (listIconButton_selected_id != "")
            {
                ListIconButton Unselected = listIconButton[listIconButton_selected_id];

                Int32Animation Animation_Unselected = new Int32Animation();
                Animation_Unselected.From = Unselected.MinRadius;
                Animation_Unselected.To = Unselected.MaxRadius;
                Animation_Unselected.Duration = TimeSpan.FromMilliseconds(500);

                Unselected.BeginAnimation(ListIconButton.RadiusProperty, Animation_Unselected);
            }

            ListIconButton Seleced = (ListIconButton)e.Source;

            Int32Animation Animation = new Int32Animation();
            Animation.From = Seleced.Radius;
            Animation.To = Seleced.MinRadius;
            Animation.Duration = TimeSpan.FromMilliseconds(500);

            Seleced.BeginAnimation(ListIconButton.RadiusProperty, Animation);

            if (Seleced.Type == TypeListIconButton.Server)
            {
                listIconButton_selected_id = Seleced.Id;
            }
        }

        private void ListIconButton_MouseEnter(object sender, RoutedEventArgs e)
        {
            ListIconButton Seleced = (ListIconButton)e.Source;

            Int32Animation Animation = new Int32Animation();
            Animation.From = Seleced.Radius;
            Animation.To = Seleced.MinRadius;
            Animation.Duration = TimeSpan.FromMilliseconds(500);

            Seleced.BeginAnimation(ListIconButton.RadiusProperty, Animation);
        }

        private void ListIconButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            ListIconButton Seleced = (ListIconButton)e.Source;

            if (Seleced.Id != listIconButton_selected_id)
            {
                Int32Animation Animation = new Int32Animation();
                Animation.From = Seleced.Radius;
                Animation.To = Seleced.MaxRadius;
                Animation.Duration = TimeSpan.FromMilliseconds(500);

                Seleced.BeginAnimation(ListIconButton.RadiusProperty, Animation);
            }
        }


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var rand = new Random();
        //    var color = new Color((byte)rand.Next(), (byte)rand.Next(), (byte)rand.Next());
        //    this._circle.FillColor = color;
        //}

        //private void CreateRenderWindow()
        //{
        //    if (this._renderWindow != null)
        //    {
        //        this._renderWindow.SetActive(false);
        //        this._renderWindow.Dispose();
        //    }

        //    var context = new ContextSettings { DepthBits = 24 };
        //    this._renderWindow = new RenderWindow(this.DrawSurface.Handle, context);
        //    this._renderWindow.MouseButtonPressed += RenderWindow_MouseButtonPressed;
        //    this._renderWindow.SetActive(true);
        //}

        //private void DrawSurface_SizeChanged(object sender, EventArgs e)
        //{
        //    this.CreateRenderWindow();
        //}

        //private void RenderWindow_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        //{
        //    this._circle.Position = new Vector2f(e.X, e.Y);
        //}

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    this._renderWindow.DispatchEvents();

        //    this._renderWindow.Clear(new Color((byte)11, (byte)12, (byte)20));
        //    this._renderWindow.Draw(this._circle);
        //    this._renderWindow.Display();
        //}

    }
}