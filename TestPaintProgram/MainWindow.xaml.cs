using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestPaintProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            inkPanel.Strokes.Clear();
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG Files |*.jpg";

            if (openFileDialog.ShowDialog().Value)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                inkPanel.Children.Add(img);
            }
        }

        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG files|*.jpg";
            if (saveFileDialog.ShowDialog().Value)
            {
                FileStream fs = (FileStream)saveFileDialog.OpenFile();
                RenderTargetBitmap visualObjectRenderer = new RenderTargetBitmap((int)inkPanel.ActualWidth, (int)inkPanel.ActualHeight, 96d, 96d, PixelFormats.Default);
                visualObjectRenderer.Render(inkPanel);
                JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                jpegEncoder.Frames.Add(BitmapFrame.Create(visualObjectRenderer));
                jpegEncoder.Save(fs);
                fs.Close();
            }
        }

        private void sldBrushSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (inkPanel != null)
            {
                inkPanel.DefaultDrawingAttributes.Width =
                inkPanel.DefaultDrawingAttributes.Height = sldBrushSize.Value;
                ellipseBrushPresenter.Width = ellipseBrushPresenter.Height = sldBrushSize.Value;
            }
            
        }

        private void btnGenerateRandomColor_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            SolidColorBrush brush = 
                new SolidColorBrush(Color.FromRgb((byte)rnd.Next(), (byte)rnd.Next(), (byte)rnd.Next()));
            ellipseBrushPresenter.Fill = brush;
            borderColorPresenter.Background = brush;
            inkPanel.DefaultDrawingAttributes.Color = brush.Color;

        }

        private void btnEraser_Click(object sender, RoutedEventArgs e)
        {
            if (btnEraser.IsChecked.Value)
            {
                inkPanel.EditingMode = InkCanvasEditingMode.EraseByPoint;
                inkPanel.EraserShape = new EllipseStylusShape(sldBrushSize.Value, sldBrushSize.Value);
            }
            else
            {
                inkPanel.EditingMode = InkCanvasEditingMode.Ink;
            }
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
