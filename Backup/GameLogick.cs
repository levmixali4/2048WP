using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace _2048.Core
{
    public class GameLogick
    {
        Dictionary<Coordinate, int> _tilesState;
        int _soccer = 0;
        public GameLogick(int side)
        {
            Side = side;
        }

        public int Soccer
        {
            get
            {
                return _soccer;
            }
        }

        public Dictionary<Coordinate, int> TilesState
        {
            get
            {
                return _tilesState;
            }
        }

        public int Side
        {
            get;
            private set;
        }

        public event EventHandler GameOver;
        public event StateChangedEventHandler StateChanged;

        public void NewGame()
        {
            if (_tilesState == null)
            {
                _tilesState = new Dictionary<Coordinate, int>(new CoordinateEqualityComparer());
                int x = 1, y = 1;
                Coordinate c;
                for (int i = 1; i <= Side * Side; i++)
                {
                    c = new Coordinate(x, y);
                    
                    _tilesState.Add(c, 0);
                    if (i % Side == 0)
                    {
                        x++;
                        y = 1;
                    }
                    else
                    {
                        y++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _tilesState.Count; i++)
                {
                    _tilesState[_tilesState.ElementAt(i).Key] = 0;
                }
            }
            Coordinate[] newValues = new Coordinate[2];
            newValues[0] = AddRandomValue();
            newValues[1] = AddRandomValue();
            //newValues[0] = new Coordinate(1, 1);
            //_tilesState[newValues[0]] = 2;
            //newValues[1] = new Coordinate(1, 2);
            //_tilesState[newValues[1]] = 4;
            //newValues[2] = new Coordinate(1, 3);
            //_tilesState[newValues[2]] = 4;
            OnStateChanged(newValues);
        }

        public void ExecuteMove(Direction direction)
        {
            int x=1, y=1;
            GetFirstCoordinates(direction, ref x, ref y);
            Coordinate c, cNext, cEmpty;            
            bool[] wasMoved = new bool[Side];
            List<Coordinate> newValues = null;

            for (int i = 1; i <= Side; i++)
            {
                if (direction == Direction.Down || direction == Direction.Up)
                {
                    x = i;
                }
                else
                {
                    y = i;
                }
                c = new Coordinate(x, y);
                TryGetNextCoordinate(direction, c, out cNext);
                cEmpty = null;
                wasMoved[i-1] = MoveRowCell(c, cNext, cEmpty, direction, newValues);
            }
            if (wasMoved.Any(m=>m==true))
            {
                if (newValues == null)
                    newValues = new List<Coordinate>();
                newValues.Add(AddRandomValue());
                OnStateChanged(newValues.ToArray());
            }
        }

        private bool MoveRowCell(Coordinate c, Coordinate cNext, Coordinate cEmpty, Direction direction, List<Coordinate> newValues)
        {
            bool wasMoved=false;
            while (true)
            {
                if (_tilesState[c] == 0 && _tilesState[cNext] == 0)
                {
                    if (!TryGetNextCoordinate(direction, cNext, out cNext))
                    {
                        break;
                    }
                    continue;
                }
                else if (_tilesState[c] != 0 && _tilesState[cNext] == 0)
                {
                    if (!TryGetNextCoordinate(direction, cNext, out cNext))
                    {
                        break;
                    }
                    continue;
                }
                else if (_tilesState[c] == 0 && _tilesState[cNext] != 0)
                {
                    _tilesState[c] = _tilesState[cNext];
                    _tilesState[cNext] = 0;
                    wasMoved = true;
                    if (cEmpty == null)
                        cEmpty = cNext;
                    if (!TryGetNextCoordinate(direction, cNext, out cNext))
                    {
                        break;
                    }
                    continue;
                }
                else if (_tilesState[c] != 0 && _tilesState[cNext] != 0)
                {
                    if (_tilesState[c] == _tilesState[cNext])
                    {
                        _tilesState[c] = _tilesState[c] + _tilesState[cNext];
                        if (newValues == null)
                            newValues = new List<Coordinate>();
                        newValues.Add(new Coordinate(c.X, c.Y));
                        _soccer += _tilesState[c];
                        _tilesState[cNext] = 0;
                        wasMoved = true;
                        if (!TryGetNextCoordinate(direction, c, out c) || !TryGetNextCoordinate(direction, cNext, out cNext))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (!TryGetNextCoordinate(direction, c, out c) || !TryGetNextCoordinate(direction, c, out cNext))
                        {
                            break;
                        }
                    }
                }                
            }
            return wasMoved;
        }

        private void GetFirstCoordinates(Direction direction, ref int x, ref int y)
        {
            switch (direction)
            {
                case Direction.Left:
                case Direction.Up:
                    x = 1;
                    y = 1;
                    break;
                case Direction.Right:
                    x = Side;
                    y = 1;
                    break;
                case Direction.Down:
                    x = 1;
                    y = Side;
                    break;
            }
        }

        private void OnGameOver()
        {
            if (GameOver != null)
                GameOver.Invoke(null, EventArgs.Empty);
        }

        private void OnStateChanged(Coordinate[] newCoordinates)
        {
            if (StateChanged != null)
            {
                StateChanged.Invoke(null, new StateChangedEventArgs(newCoordinates));
            }
        }

        private Coordinate AddRandomValue()
        {
            int value = -1;
            Coordinate c = new Coordinate();
            while (value == -1 || value != 0)
            {
                c.X = Random.Next(1, Side + 1);
                c.Y = Random.Next(1, Side + 1);
                value = _tilesState[c];
            }
            if (Random.Next(-5, 10) >= 0)
                value = 2;
            else
                value = 4;
            _tilesState[c] = value;

            if (IsGameOver())
            {
                OnGameOver();
            }
            return c;
        }

        private bool IsGameOver()
        {
            if (_tilesState.Any(t => t.Value == 0))
                return false;

            return !AnyMoves(Direction.Left) && !AnyMoves(Direction.Up);
        }

        private bool AnyMoves(Direction direction)
        {
            int x = 1, y = 1;
            Coordinate c = new Coordinate();
            Coordinate cNext = new Coordinate();
            for (int i = 1; i <= Side; i++)
            {
                if (direction == Direction.Up)
                {
                    x = i;
                }
                else
                {
                    y = i;
                }
                c = new Coordinate(x, y);
                TryGetNextCoordinate(direction, c, out cNext);
                while (true)
                {
                    if (_tilesState[c] == _tilesState[cNext])
                    {
                        return true;
                    }
                    if (!TryGetNextCoordinate(direction, c, out c) || !TryGetNextCoordinate(direction, cNext, out cNext))
                    {
                        break;
                    }
                }
            }
            return false;
        }

        private bool TryGetNextCoordinate(Direction direction, Coordinate c, out Coordinate cNext)
        {
            switch (direction)
            {
                case Direction.Down:
                    if (c.Y == 1)
                    {
                        cNext = c;
                        return false;
                    }
                    cNext = new Coordinate(c.X, c.Y - 1);
                    return true;
                case Direction.Left:
                    if (c.X == Side)
                    {
                        cNext = c;
                        return false;
                    }
                    cNext = new Coordinate(c.X + 1, c.Y);
                    return true;
                case Direction.Right:
                    if (c.X == 1)
                    {
                        cNext = c;
                        return false;
                    }
                    cNext = new Coordinate(c.X - 1, c.Y);
                    return true;
                case Direction.Up:
                    if (c.Y == Side)
                    {
                        cNext = c;
                        return false;
                    }
                    cNext = new Coordinate(c.X, c.Y + 1);
                    return true;
            }
            cNext = c;
            return false;
        }

        Random _random;
        Random Random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }
    }
}
