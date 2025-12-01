using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Game_of_Life;

public class GameField
{
    public enum CellState { Dead, Alive };
    public int Width { get; }
    public int Height { get; }
    private CellState[,] _field;

    public GameField(int width, int height)
    {
        Width = width;
        Height = height;
        _field = new CellState[Width, Height];
    }

    public CellState this[int w, int h]
    {
        get => _field[w, h];
        set
        {
            if (w < 0 || w >= Width || h < 0 || h >= Height)
                throw new ArgumentOutOfRangeException(w < 0 || w >= Width ? nameof(w) : nameof(h));
            _field[w, h] = value;
        }
    }

    public void Randomize(double aliveProbability = 0.3)
    {
        var rand = new Random();

        for (int x = 0; x < Width; x++)
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || x == Height + 1 || y == 0 || y == Width + 1)
                    _field[x, y] = CellState.Dead;
                else
                {
                    _field[x, y] = rand.NextDouble() < aliveProbability ?
                            CellState.Alive : CellState.Dead;
                }
            }
    }
}

public class DrawField : Control
{
    public GameField? Field { get; set; }
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (Field == null) return;

        context.FillRectangle(Brushes.LightSteelBlue, new Rect(0, 0, Bounds.Width, Bounds.Height));

        double cellWidth = Bounds.Width / Field.Width;
        double cellHeight = Bounds.Height / Field.Height;
        double cellSize = Math.Min(cellHeight, cellWidth);

        for (int x = 0; x < Field.Width; x++)
            for (int y = 0; y < Field.Height; y++)
            {
                var state = Field[x, y];
                var brush = (state == GameField.CellState.Alive ? Brushes.OliveDrab : Brushes.WhiteSmoke);
                var rect = new Rect(x * cellSize, y * cellSize, cellSize, cellSize);

                context.FillRectangle(brush, rect, (float)cellSize / 2);
                context.DrawRectangle(null, new Pen(Brushes.DarkOliveGreen, 1.7), rect, cellSize / 2, cellSize / 2);
            }
    }
}
