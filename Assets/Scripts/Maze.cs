using System.Collections.Generic;
using Random = System.Random;

namespace InfiniteMaze
{
	public struct Position
	{
		public int X { get; }
		public int Y { get; }

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Position operator +(Position a, Position b) => new Position(a.X + b.X, a.Y + b.Y);

		public override string ToString()
		{
			return $"{X}, {Y}";
		}
	}

	public class Maze
	{
		public int CellWidth { get; }
		public int CellHeight { get; }

		public int Width { get; }
		public int Height { get; }

		public bool this[Position pos] => InMaze(pos) ? cells[pos.X, pos.Y] : false;
		public bool this[int x, int y] => this[new Position(x, y)];

		private bool[,] cells;

		public Maze(int cellWidth, int cellHeight)
		{
			CellWidth = cellWidth;
			CellHeight = cellHeight;

			Width = cellWidth * 2 + 1;
			Height = cellHeight * 2 + 1;

			cells = new bool[Width, Height];
		}

		bool InMaze(Position pos)
		{
			return pos.X >= 0 && pos.X < Width &&
			       pos.Y >= 0 && pos.Y < Height;
		}

		bool CanMoveDirection(Position pos, Position dir)
		{
			var next = pos;
			for (int i = 0; i < 2; i++)
			{
				next += dir;
				if (!InMaze(next) || cells[next.X, next.Y])
					return false;
			}

			return true;
		}

		// Regenerates this maze using recursive backtracking
		public void Generate(System.Random rnd)
		{
			var up = new Position(0, 1);
			var right = new Position(1, 0);
			var down = new Position(0, -1);
			var left = new Position(-1, 0);

			// Convert to maze coordinates
			var startX = rnd.Next(0, CellWidth) * 2 + 1;
			var startY = rnd.Next(0, CellHeight) * 2 + 1;

			// Reset all cells in the maze
			for (int x = 0; x < Width; x++)
			for (int y = 0; y < Height; y++)
				cells[x, y] = false;

			// Path, used for backtracking
			var path = new List<Position>(Width);
			cells[startX, startY] = true;
			path.Add(new Position(startX, startY));

			var possibleDirections = new List<Position>();

			// Continue until there are no more open cells
			while (path.Count > 0)
			{
				var pos = path[path.Count - 1];

				// Get possible directions
				possibleDirections.Clear();
				if (CanMoveDirection(pos, up)) possibleDirections.Add(up);
				if (CanMoveDirection(pos, right)) possibleDirections.Add(right);
				if (CanMoveDirection(pos, down)) possibleDirections.Add(down);
				if (CanMoveDirection(pos, left)) possibleDirections.Add(left);

				// Move in a random direction
				if (possibleDirections.Count > 0)
				{
					var dir = possibleDirections[rnd.Next(0, possibleDirections.Count)];

					// Carve Path
					for (int i = 0; i < 2; i++)
					{
						pos += dir;
						cells[pos.X, pos.Y] = true;
					}

					path.Add(pos);
				}
				else
				{
					// No other places to move, backtrack
					path.RemoveAt(path.Count - 1);
				}
			}
		}

		// Text representation of the cells
		public override string ToString()
		{
			var str = "";
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
					str += cells[x, y] ? 'O' : '_';
				str += '\n';
			}

			return str.TrimEnd('\n');
		}
	}
}