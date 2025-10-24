namespace GameOfLife.Core;

public class Cell
{
    private static int _maxNbrOfStates;
    private  int _state;

    public Cell()
    {
        _state = 0;
        _maxNbrOfStates = 2;
    }
    public Cell(int state, int maxNbrOfStates)
    {
        _state = state;
        _maxNbrOfStates = maxNbrOfStates;
    }
    public void MoveToNextState()
    {
        _state = (_state + 1) % _maxNbrOfStates;
    }
    public int GetState()
    {
        return _state;
    }
}
