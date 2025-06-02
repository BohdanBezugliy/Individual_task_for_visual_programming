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

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Random rdn = new Random();
    int total;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        total = int.Parse(comboBox.SelectedItem.ToString());
        panel1.Children.Clear();
        panel2.Children.Clear();
        panel3.Children.Clear();
        var w = (panel1.ActualWidth-20)/(2*total);
        for(int i = 0; i < total; i++)
        {
            Rectangle r = new Rectangle();
            r.Width = panel1.ActualWidth - (20+i*2*w);
            r.Height = (panel1.ActualHeight - 10)/total;
            r.Stroke = Brushes.Black;
            r.StrokeThickness = 1;
            DockPanel.SetDock(r,Dock.Bottom);
            LinearGradientBrush b = new LinearGradientBrush();
            b.StartPoint = new Point(0.5, 0);
            b.EndPoint = new Point(0.5, 0.5);
            Color c1 = Color.FromRgb((byte)rdn.Next(256), (byte)rdn.Next(256), (byte)rdn.Next(256));
            b.GradientStops.Add(new GradientStop(Colors.White, 0));
            b.GradientStops.Add(new GradientStop(c1, 2));
            r.Fill = b;
            r.RadiusX = r.Height / 8;
            r.RadiusY = r.Height / 8;
            panel1.Children.Add(r);
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        comboBox.ItemsSource = Enumerable.Range(2, 9).Select(i => i.ToString());
        comboBox.SelectedIndex = 8;
        comboBox.Focus();
    }
}