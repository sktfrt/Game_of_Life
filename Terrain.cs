using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Game_of_Life;

public class Terrain
{
    public GameField Field { get; private set; }
    public event Action TurnFinished;

    public Terrain(int w, int h)
    {
        Field = new GameField(w + 2, h + 2);
        Field.Randomize();
    }

    public int ContainsAliveNeighbours(GameField field, int x, int y)
    {
        int aliveCells = 0;
        var directions = new (int, int)[]
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        };

        foreach (var (dx, dy) in directions)
        {
            if (field[dx + x, dy + y] == GameField.CellState.Alive)
                aliveCells += 1;
        }

        return aliveCells;
    }

    public void NextTurn()
    {
        GameField newField = new GameField(Field.Width, Field.Height);
        for (int x = 1; x < Field.Width - 1; x++)
            for (int y = 1; y < Field.Height - 1; y++)
            {
                int alive = ContainsAliveNeighbours(Field, x, y);

                if (Field[x, y] == GameField.CellState.Alive)
                {
                    newField[x, y] = alive == 2 || alive == 3 ?
                        GameField.CellState.Alive :
                        GameField.CellState.Dead;
                }
                else
                {
                    newField[x, y] = alive == 3 ?
                        GameField.CellState.Alive :
                        GameField.CellState.Dead;
                }
            }

        Field = newField;
        TurnFinished?.Invoke();
    }

    public async Task RunAsync()
    {
        while (true)
        {
            NextTurn();
            await Task.Delay(300);
        }
    }
}

public class LifeForm : Window
{
    public LifeForm(Terrain terrain)
    {
        var renderer = new DrawField
        {
            Field = terrain.Field,
            Width = 600,
            Height = 600,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Margin = new Avalonia.Thickness(20)
        };

        Content = renderer;

        terrain.TurnFinished += () =>
        {
            renderer.Field = terrain.Field;
            renderer.InvalidateVisual();
        };

        Loaded += async (_, _) => await terrain.RunAsync();
    }
}
