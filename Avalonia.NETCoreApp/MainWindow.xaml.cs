using Atlas.UI;
using Avalonia.Markup.Xaml;

namespace Avalonia.NETCoreApp
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}