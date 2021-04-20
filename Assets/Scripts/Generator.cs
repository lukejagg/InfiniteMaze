using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfiniteMaze
{
	public class Generator : MonoBehaviour
	{
		public Transform mazePrefab;

		public int cellWidth, cellHeight;

		public Dictionary<Position, Maze> Mazes = new Dictionary<Position, Maze>();

		void CreateMaze(Position pos)
		{
			if (Mazes.ContainsKey(pos)) return;

			var rnd = new System.Random(pos.X * 100320 + pos.Y);

			var obj = new GameObject(pos.ToString()).transform;
			obj.localPosition = new Vector3Int(pos.X * (cellWidth * 2), 0, pos.Y * (cellHeight * 2));

			var maze = new Maze(cellWidth, cellHeight);
			maze.Generate(rnd);


			int badX = rnd.Next(0, cellWidth) * 2 + 1;
			int badY = rnd.Next(0, cellHeight) * 2 + 1;

			for (int i = 0; i < maze.Width - 1; i++)
			{
				for (int j = 0; j < maze.Height - 1; j++)
				{
					if (i == 0 && j == badY || i == badX && j == 0)
						continue;

					if (!maze[i, j])
					{
						var t = Instantiate(mazePrefab, obj);
						t.localPosition = new Vector3(i, 0, j);
					}
				}
			}

			Mazes.Add(pos, maze);
		}


		// Start is called before the first frame update
		void Start()
		{
			NextPosition.Add(new Position(0, 0));
		}

		void Add(Position p)
		{
			if (!Generated.Contains(p))
				NextPosition.Add(p);
		}

		HashSet<Position> Generated = new HashSet<Position>();
		List<Position> NextPosition = new List<Position>();
		void Update()
		{
			var p = NextPosition[0];
			NextPosition.RemoveAt(0);

			if (!Generated.Contains(p))
			{
				Generated.Add(p);
				CreateMaze(p);

				Add(p + new Position(0, 1));
				Add(p + new Position(0, -1));
				Add(p + new Position(1, 0));
				Add(p + new Position(-1, 0));
			}
		}
	}
}