using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Individual_task_for_visual_programming;

public partial class MainWindow : Window
{
    int count;
    int minCount;
    void info()
    {
        lblMsg.Text = string.Format("Число переміщень: {0}({1})",count,minCount);
    }
    Color[] colors = new Color[]
{
    (Color)ColorConverter.ConvertFromString("#8B4513"), // SaddleBrown
    (Color)ColorConverter.ConvertFromString("#CD853F"), // Peru
    (Color)ColorConverter.ConvertFromString("#DEB887"), // BurlyWood
    (Color)ColorConverter.ConvertFromString("#F5DEB3"), // Wheat
    (Color)ColorConverter.ConvertFromString("#D2B48C"), // Tan
    (Color)ColorConverter.ConvertFromString("#BC8F8F"), // RosyBrown
    (Color)ColorConverter.ConvertFromString("#FFDEAD"), // NavajoWhite
    (Color)ColorConverter.ConvertFromString("#F4A460"), // SandyBrown
    (Color)ColorConverter.ConvertFromString("#D2691E"), // Chocolate
    (Color)ColorConverter.ConvertFromString("#FFE4B5"),  // Moccasin
};

    Random rdn = new Random();
    int total;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        comboBox.ItemsSource = Enumerable.Range(2, 9).
            Select(i => i.ToString());
        comboBox.SelectedIndex = 8;
        comboBox.Focus();
    }

    private void ComboBox_SelectionChanged(object sender, 
        SelectionChangedEventArgs e)
    {
        total = int.Parse(comboBox.SelectedItem.ToString());
        panel1.Children.Clear();
        panel2.Children.Clear();
        panel3.Children.Clear();
        var w = (panel1.ActualWidth-20)/(2*total);
        count = 0;
        minCount = (int)Math.Round(Math.Pow(2, total)) - 1;
        info();
        lblMsgVictory.Visibility = Visibility.Hidden;
        for (int i = 0; i < total; i++)
        {
            Rectangle r = new Rectangle();
            r.Width = panel1.ActualWidth - (20+i*2*w);
            r.Height = (panel1.ActualHeight - 10)/total;
            r.Stroke = Brushes.Black;
            r.StrokeThickness = 1;
            DockPanel.SetDock(r,Dock.Bottom);
            LinearGradientBrush b = new LinearGradientBrush();
            b.StartPoint = new Point(0.5, 0);
            b.EndPoint = new Point(0.5, 1);
            Color c1 = colors[rdn.Next(colors.Length)];
            b.GradientStops.Add(new GradientStop(Colors.White, 0));
            b.GradientStops.Add(new GradientStop(c1, 1));
            r.Fill = b;
            r.RadiusX = r.Height / 3;
            r.RadiusY = r.Height / 3;
            panel1.Children.Add(r);
            r.MouseDown += R_MouseDown;
        }
    }

    DockPanel GetPanel(object trg)
    {
        return (trg as DockPanel) ?? (trg as Rectangle)?.Parent as DockPanel;
    }

    void MoveRect(Rectangle r, DockPanel panel)
    {
        (r.Parent as DockPanel).Children.Remove(r);
        panel.Children.Add(r);
        count++;
        info();
        if (panel2.Children.Count == total || panel3.Children.Count == total) {
            lblMsgVictory.Visibility = Visibility.Visible;
        }
    }

    private void R_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (!button1.IsEnabled) return;
        if (e.ChangedButton == MouseButton.Left)
            DragDrop.DoDragDrop(sender as Rectangle, sender, 
                DragDropEffects.Move);
    }

    private void panel1_DragEnter(object sender, DragEventArgs e)
    {
        var panel = GetPanel(e.Source);
        if (panel == null) return;
        var r=e.Data.GetData(typeof(Rectangle)) as Rectangle;
        var oldPanel = r.Parent as DockPanel;
        var c = panel.Children.Count;
        var k = c > 0 ? (panel.Children[c-1] as Rectangle).Width : double.MaxValue;
        if (lblMsgVictory.IsVisible)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            return;
        }
        else
        {
            e.Effects = oldPanel.Children.IndexOf(r) ==
                oldPanel.Children.Count - 1 && r.Width <= k ?
                DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }
    }

    private void panel1_Drop(object sender, DragEventArgs e)
    {
        var panel = GetPanel(e.Source);
        if (panel == null) return;
        var r = e.Data.GetData(typeof(Rectangle)) as Rectangle;
        if(panel == r.Parent) return;
        MoveRect(r, panel);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ComboBox_SelectionChanged(null, null);
    }

    private void button2_Click(object sender, RoutedEventArgs e)
    {
        comboBox.IsEnabled = button1.IsEnabled = !button1.IsEnabled;
        if (!button1.IsEnabled)
        {
            if (panel1.Children.Count != total)
                ComboBox_SelectionChanged(null, null);
            Step(total, panel1, panel3, panel2);
            comboBox.IsEnabled = button1.IsEnabled = true;
        }
    }
    public static void DoEvents()
    {
        Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(delegate { }));
    }
    private void Step(int k, DockPanel src, DockPanel dst, DockPanel tmp)
    {
        if (k == 0) return;
        Step(k - 1, src, tmp, dst);
        if (button1.IsEnabled) return;
        MoveRect(src.Children[src.Children.Count-1] as Rectangle,dst);
        DoEvents();
        System.Threading.Thread.Sleep(1500/(total-1));
        Step(k - 1, tmp, dst, src);
    }
}