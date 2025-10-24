using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using LifeGrid = GameOfLife.Core.Grid; // <-- alias to avoid clash with WPF Grid

namespace GameOfLife.App
{
    public partial class MainWindow : Window
    {
        private const int BoardW = 20;
        private const int BoardH = 20;
        private const int CellSize = 28;

        private readonly LifeGrid _grid;
        private readonly Border[,] _uiCells = new Border[BoardW, BoardH];
        private readonly DispatcherTimer _timer;
        private bool _running = false;
        private long _generation = 0;

        public MainWindow()
        {
            InitializeComponent();

            _grid = new LifeGrid(BoardW, BoardH);
            BuildBoard();

            // Start with R-pentomino
            SeedRpentomino();
            _grid.CloneState();
            PaintBoard();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value) };
            _timer.Tick += (_, __) => TickOnce();
        }

        private void BuildBoard()
        {
            CellGrid.Children.Clear();

            for (int y = 0; y < BoardH; y++)
            {
                for (int x = 0; x < BoardW; x++)
                {
                    var b = new Border
                    {
                        Width = CellSize,
                        Height = CellSize,
                        Margin = new Thickness(1),
                        CornerRadius = new CornerRadius(4),
                        Background = DeadBrush(),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                        BorderThickness = new Thickness(1),
                        Tag = (x, y)
                    };
                    b.MouseLeftButtonDown += Cell_Click;
                    _uiCells[x, y] = b;
                    CellGrid.Children.Add(b);
                }
            }
        }

        private static Brush AliveBrush() => new SolidColorBrush(Color.FromRgb(0x7e, 0xd3, 0x74));
        private static Brush DeadBrush()  => new SolidColorBrush(Color.FromRgb(37, 37, 38));

        private void Cell_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is not Border b || b.Tag is not ValueTuple<int, int> t) return;
            var (x, y) = t;

            _grid.ChangeCellToNextState(x, y); // toggle
            _grid.CloneState();                 // keep previous-state buffer in sync
            PaintCell(x, y);
        }

        private void PaintBoard()
        {
            for (int y = 0; y < BoardH; y++)
                for (int x = 0; x < BoardW; x++)
                    PaintCell(x, y);
        }

        private void PaintCell(int x, int y)
        {
            _uiCells[x, y].Background = (_grid.GetCellState(x, y) == 1) ? AliveBrush() : DeadBrush();
        }

        // Simulation
        private void TickOnce()
        {
            _grid.NextOneGeneration();
            _generation++;
            GenText.Text = $"Gen: {_generation}";
            PaintBoard();
        }

        // Controls
        private void PlayPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            _running = !_running;
            if (_running)
            {
                _timer.Start();
                PlayPauseBtn.Content = "⏸ Pause";
            }
            else
            {
                _timer.Stop();
                PlayPauseBtn.Content = "▶︎ Play";
            }
        }

        private void Step_Click(object sender, RoutedEventArgs e)
        {
            if (_running) return;
            TickOnce();
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_timer != null) _timer.Interval = TimeSpan.FromMilliseconds(e.NewValue);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _running = false;
            PlayPauseBtn.Content = "▶︎ Play";
            _generation = 0;
            GenText.Text = "Gen: 0";

            ClearBoard();
            _grid.CloneState();
            PaintBoard();
        }

        private void Randomize_Click(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            ClearBoard();

            for (int y = 0; y < BoardH; y++)
                for (int x = 0; x < BoardW; x++)
                    if (rnd.NextDouble() < 0.22) // ~22% alive
                        MakeAlive(x, y);

            _grid.CloneState();
            PaintBoard();
        }

        private void SeedRpentomino_Click(object sender, RoutedEventArgs e)
        {
            SeedRpentomino();
            _grid.CloneState();
            PaintBoard();
        }

        // Patterns / helpers

        private void SeedRpentomino()
        {
            ClearBoard();

            // R-pentomino centered-ish
            int cx = BoardW / 2 - 1;
            int cy = BoardH / 2 - 1;

            MakeAlive(cx + 1, cy + 0);
            MakeAlive(cx + 2, cy + 0);
            MakeAlive(cx + 0, cy + 1);
            MakeAlive(cx + 1, cy + 1);
            MakeAlive(cx + 1, cy + 2);
        }

        private void ClearBoard()
        {
            for (int y = 0; y < BoardH; y++)
                for (int x = 0; x < BoardW; x++)
                    MakeDead(x, y);
        }

        private void MakeAlive(int x, int y)
        {
            if (x < 0 || x >= BoardW || y < 0 || y >= BoardH) return;

            if (_grid.GetCellState(x, y) == 0)
            {
                _grid.ChangeCellToNextState(x, y);   // toggle to alive
                _uiCells[x, y].Background = AliveBrush();
            }
        }

        private void MakeDead(int x, int y)
        {
            if (x < 0 || x >= BoardW || y < 0 || y >= BoardH) return;

            if (_grid.GetCellState(x, y) == 1)
            {
                _grid.ChangeCellToNextState(x, y);   // toggle to dead
                _uiCells[x, y].Background = DeadBrush();
            }
        }
    }
}
