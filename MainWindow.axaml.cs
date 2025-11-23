using Avalonia.Controls;

namespace Game_of_Life;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var field = new GameField(10, 10);
        field.Randomize();

        var renderer = new DrawField
        {
            Field = field,
            Width = 500,
            Height = 500
        };

        this.Content = renderer;
    }
}