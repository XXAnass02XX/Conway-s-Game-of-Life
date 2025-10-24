using System.Runtime.ExceptionServices;

namespace GameOfLife.Core;

public class Grid
{
    private int _width;
    private int _height;
    private Cell[] _cells;
    private int[] _previousStates;

    public Grid()
    {
        _width = 9;
        _height = 9;
        _cells = new Cell[81];
        _previousStates = new int[81];
        for (int i = 0; i < 81; i++)
        {
            _cells[i] = new Cell(0, 2);
            _previousStates[i] = 0;
        }
    }
    public Grid(int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();
        _width = width;
        _height = height;
        _cells = new Cell[width * height];
        _previousStates = new int[width * height];
        for (int i = 0; i < width * height; i++)
        {
            _cells[i] = new Cell(0, 2);
            _previousStates[i] = 0;
        }
    }
    public void CloneState()
    {
        for (int i = 0; i < _width * _height; i++)
        {
            _previousStates[i] = _cells[i].GetState();
        }
    }
    private int Index(int x, int y) => y * _width + x;
    public void ChangeCellToNextState(int x, int y)
    {
        _cells[Index(x, y)].MoveToNextState();
    }

    public bool ShouldCellChangeState(int x, int y)
    {
        int idx = Index(x, y);
        int current = _cells[idx].GetState();
        int n = CountLiveNeighbors(x, y);
        if (current == 1)
        {
            if (n < 2 || n > 3)
            {
                return true;
            }
        }
        else
        {
            if (n == 3) { return true; }
        }
        return false;
    }

    private int CountLiveNeighbors(int x, int y)
    {
        int count = 0;
        for (int dy = -1; dy <= 1; dy++)
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0) continue;
                int nx = x + dx, ny = y + dy;
                if (nx >= _width || ny >= _height || nx < 0 || ny < 0) continue;
                if (_previousStates[Index(nx, ny)] == 1) count++;
            }
        return count;
    }

    public void NextOneGeneration()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {

                if (ShouldCellChangeState(x, y))
                {
                    ChangeCellToNextState(x, y);
                }
            }
        }
        CloneState();
    }
    public int GetCellState(int x, int y) => _cells[Index(x,y)].GetState();

}