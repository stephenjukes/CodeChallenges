﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public class Day_02
    {
        public static void Run()
        {
			var moves = Day2_Input.Test;

			// Part 1
			var part1Calculator = new CourseCalculator("part1Calculator",
				summaryFunc: state => state.HorizontalTraversal * state.Depth,
				new MoveAction(Direction.Forward, (state, distance) => state.HorizontalTraversal += distance),
				new MoveAction(Direction.Up, (state, distance) => state.Depth -= distance),
				new MoveAction(Direction.Down, (state, distance) => state.Depth += distance));

			var part1CourseSummary = part1Calculator.GetCourseSummary(moves);
            Console.WriteLine($"Part 1 course summary: { part1CourseSummary }");

			// Part 2
			var part2Calculator = new CourseCalculator("part2Calculator",
				summaryFunc: state => state.HorizontalTraversal * state.Depth,
				new MoveAction(Direction.Up, (state, distance) => state.Aim -= distance),
				new MoveAction(Direction.Down, (state, distance) => state.Aim += distance),
				new MoveAction(Direction.Forward, (state, distance) =>
					{
						state.HorizontalTraversal += distance;
						state.Depth += (state.Aim * distance);
					}));

			var part2CourseSummary = part2Calculator.GetCourseSummary(moves);
            Console.WriteLine($"Part 2 course summary: { part2CourseSummary }");
		}
	}

	public class CourseCalculator
    {
		private State _state;
		private Dictionary<string, Action<State, int>> _actions;
		private Func<State, int> _summaryFunc;

        public CourseCalculator(string name, Func<State, int> summaryFunc, params MoveAction[] actions)
        {
			_state = new State();
			_summaryFunc = summaryFunc;
			_actions = actions.ToDictionary(a => a.Direction, a => a.Implementation);

			Name = name;
        }

        public string Name { get; set; }

		public int GetCourseSummary(IEnumerable<Move> moves)
        {
			foreach (var move in moves)
			{
				_actions[move.Direction](_state, move.Distance);
			}

			return _summaryFunc(_state);
		}
	}

	public class State
    {
        public int Depth { get; set; }
        public int HorizontalTraversal { get; set; }
        public int Aim { get; set; }
    }

	public class MoveAction
    {
        public MoveAction(string direction, Action<State, int> implementation)
        {
			Direction = direction;
			Implementation = implementation;
        }
	
        public string Direction { get; set; }
        public Action<State, int> Implementation { get; set; }
    }

    public class Move
    {
        public Move(string direction, int distance)
        {
            Direction = direction;
            Distance = distance;
        }

        public string Direction { get; set; }
        public int Distance { get; set; }
    }

	public static class Direction
    {
		public static string Forward = "forward";
		public static string Up = "up";
		public static string Down = "down";
	}

    public static class Day2_Input
    {
		public static IEnumerable<Move> Test = new Move[]
		{
			new Move("forward", 5),
			new Move("down", 5),
			new Move("forward", 8),
			new Move("up", 3),
			new Move("down", 8),
			new Move("forward", 2)
		};

		public static IEnumerable<Move> Moves = new Move[]
		{
			new Move("forward", 2),
			new Move("down", 9),
			new Move("up", 6),
			new Move("forward", 1),
			new Move("down", 5),
			new Move("down", 7),
			new Move("down", 9),
			new Move("forward", 9),
			new Move("down", 8),
			new Move("up", 7),
			new Move("forward", 2),
			new Move("up", 6),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("down", 9),
			new Move("up", 1),
			new Move("down", 9),
			new Move("forward", 8),
			new Move("forward", 6),
			new Move("forward", 6),
			new Move("forward", 5),
			new Move("forward", 9),
			new Move("up", 3),
			new Move("up", 5),
			new Move("forward", 1),
			new Move("down", 4),
			new Move("down", 7),
			new Move("forward", 2),
			new Move("up", 3),
			new Move("down", 8),
			new Move("forward", 1),
			new Move("down", 2),
			new Move("forward", 3),
			new Move("up", 1),
			new Move("up", 1),
			new Move("up", 7),
			new Move("forward", 5),
			new Move("up", 8),
			new Move("forward", 8),
			new Move("forward", 8),
			new Move("down", 6),
			new Move("forward", 1),
			new Move("forward", 5),
			new Move("forward", 4),
			new Move("forward", 6),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("down", 9),
			new Move("forward", 9),
			new Move("down", 8),
			new Move("forward", 6),
			new Move("down", 5),
			new Move("forward", 9),
			new Move("up", 3),
			new Move("up", 1),
			new Move("down", 8),
			new Move("down", 7),
			new Move("down", 9),
			new Move("forward", 7),
			new Move("down", 8),
			new Move("down", 9),
			new Move("down", 5),
			new Move("down", 3),
			new Move("forward", 1),
			new Move("forward", 6),
			new Move("down", 1),
			new Move("forward", 9),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("up", 2),
			new Move("down", 8),
			new Move("forward", 1),
			new Move("down", 4),
			new Move("down", 9),
			new Move("down", 4),
			new Move("up", 5),
			new Move("forward", 4),
			new Move("forward", 6),
			new Move("forward", 1),
			new Move("down", 3),
			new Move("forward", 1),
			new Move("down", 6),
			new Move("up", 5),
			new Move("up", 4),
			new Move("forward", 6),
			new Move("forward", 1),
			new Move("forward", 1),
			new Move("down", 2),
			new Move("up", 4),
			new Move("up", 3),
			new Move("up", 2),
			new Move("up", 6),
			new Move("down", 6),
			new Move("forward", 1),
			new Move("down", 8),
			new Move("forward", 1),
			new Move("up", 6),
			new Move("forward", 7),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("forward", 6),
			new Move("down", 4),
			new Move("forward", 4),
			new Move("down", 4),
			new Move("down", 4),
			new Move("forward", 2),
			new Move("forward", 8),
			new Move("down", 5),
			new Move("down", 1),
			new Move("down", 8),
			new Move("up", 5),
			new Move("up", 8),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("down", 6),
			new Move("up", 7),
			new Move("forward", 2),
			new Move("down", 3),
			new Move("forward", 2),
			new Move("forward", 2),
			new Move("down", 9),
			new Move("down", 3),
			new Move("up", 6),
			new Move("forward", 8),
			new Move("up", 2),
			new Move("up", 9),
			new Move("forward", 4),
			new Move("down", 1),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("down", 2),
			new Move("down", 3),
			new Move("forward", 5),
			new Move("down", 4),
			new Move("forward", 7),
			new Move("up", 4),
			new Move("forward", 6),
			new Move("up", 8),
			new Move("forward", 1),
			new Move("up", 9),
			new Move("down", 4),
			new Move("forward", 2),
			new Move("down", 1),
			new Move("forward", 7),
			new Move("down", 3),
			new Move("down", 2),
			new Move("forward", 5),
			new Move("down", 3),
			new Move("down", 9),
			new Move("down", 9),
			new Move("up", 5),
			new Move("forward", 2),
			new Move("down", 8),
			new Move("up", 9),
			new Move("forward", 4),
			new Move("down", 3),
			new Move("forward", 3),
			new Move("forward", 6),
			new Move("up", 2),
			new Move("forward", 3),
			new Move("down", 1),
			new Move("down", 1),
			new Move("down", 1),
			new Move("forward", 7),
			new Move("forward", 4),
			new Move("forward", 7),
			new Move("down", 5),
			new Move("down", 6),
			new Move("down", 2),
			new Move("forward", 6),
			new Move("down", 3),
			new Move("up", 6),
			new Move("forward", 4),
			new Move("down", 8),
			new Move("up", 1),
			new Move("forward", 8),
			new Move("down", 2),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("down", 9),
			new Move("forward", 2),
			new Move("forward", 2),
			new Move("down", 3),
			new Move("forward", 3),
			new Move("down", 1),
			new Move("forward", 2),
			new Move("down", 7),
			new Move("forward", 3),
			new Move("forward", 9),
			new Move("up", 9),
			new Move("forward", 6),
			new Move("forward", 2),
			new Move("down", 1),
			new Move("down", 5),
			new Move("forward", 6),
			new Move("forward", 6),
			new Move("down", 3),
			new Move("up", 3),
			new Move("forward", 9),
			new Move("down", 7),
			new Move("down", 2),
			new Move("down", 4),
			new Move("down", 7),
			new Move("forward", 5),
			new Move("up", 4),
			new Move("forward", 8),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("down", 7),
			new Move("up", 7),
			new Move("down", 8),
			new Move("forward", 9),
			new Move("up", 5),
			new Move("forward", 1),
			new Move("down", 2),
			new Move("forward", 5),
			new Move("down", 9),
			new Move("forward", 3),
			new Move("down", 5),
			new Move("forward", 8),
			new Move("forward", 3),
			new Move("up", 5),
			new Move("down", 2),
			new Move("up", 3),
			new Move("forward", 2),
			new Move("up", 1),
			new Move("up", 5),
			new Move("down", 8),
			new Move("forward", 2),
			new Move("down", 5),
			new Move("up", 4),
			new Move("up", 5),
			new Move("up", 2),
			new Move("forward", 9),
			new Move("forward", 6),
			new Move("down", 9),
			new Move("up", 9),
			new Move("forward", 6),
			new Move("forward", 4),
			new Move("forward", 3),
			new Move("forward", 7),
			new Move("up", 1),
			new Move("down", 2),
			new Move("down", 6),
			new Move("down", 1),
			new Move("forward", 8),
			new Move("down", 1),
			new Move("forward", 6),
			new Move("down", 8),
			new Move("forward", 8),
			new Move("down", 7),
			new Move("down", 6),
			new Move("down", 5),
			new Move("forward", 2),
			new Move("up", 8),
			new Move("up", 6),
			new Move("up", 5),
			new Move("down", 1),
			new Move("forward", 1),
			new Move("down", 1),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("forward", 3),
			new Move("down", 1),
			new Move("forward", 5),
			new Move("forward", 5),
			new Move("forward", 8),
			new Move("down", 1),
			new Move("up", 2),
			new Move("down", 6),
			new Move("up", 6),
			new Move("forward", 6),
			new Move("forward", 6),
			new Move("down", 3),
			new Move("forward", 9),
			new Move("up", 4),
			new Move("forward", 4),
			new Move("down", 6),
			new Move("up", 1),
			new Move("forward", 6),
			new Move("down", 2),
			new Move("down", 5),
			new Move("down", 2),
			new Move("down", 6),
			new Move("up", 5),
			new Move("down", 1),
			new Move("down", 1),
			new Move("forward", 3),
			new Move("forward", 7),
			new Move("forward", 3),
			new Move("up", 2),
			new Move("down", 8),
			new Move("down", 4),
			new Move("down", 1),
			new Move("down", 5),
			new Move("down", 1),
			new Move("down", 9),
			new Move("forward", 6),
			new Move("down", 6),
			new Move("down", 4),
			new Move("down", 6),
			new Move("down", 8),
			new Move("forward", 4),
			new Move("down", 6),
			new Move("down", 7),
			new Move("forward", 8),
			new Move("down", 4),
			new Move("up", 4),
			new Move("down", 1),
			new Move("forward", 1),
			new Move("forward", 4),
			new Move("forward", 1),
			new Move("up", 9),
			new Move("down", 7),
			new Move("forward", 7),
			new Move("down", 4),
			new Move("forward", 1),
			new Move("up", 4),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("down", 7),
			new Move("forward", 5),
			new Move("forward", 7),
			new Move("forward", 1),
			new Move("forward", 1),
			new Move("forward", 9),
			new Move("forward", 9),
			new Move("up", 3),
			new Move("forward", 4),
			new Move("down", 2),
			new Move("forward", 9),
			new Move("up", 8),
			new Move("forward", 3),
			new Move("up", 5),
			new Move("down", 3),
			new Move("down", 8),
			new Move("forward", 8),
			new Move("down", 6),
			new Move("forward", 1),
			new Move("down", 6),
			new Move("down", 6),
			new Move("up", 9),
			new Move("down", 2),
			new Move("forward", 8),
			new Move("up", 9),
			new Move("down", 7),
			new Move("up", 9),
			new Move("up", 8),
			new Move("up", 1),
			new Move("forward", 6),
			new Move("forward", 9),
			new Move("down", 2),
			new Move("forward", 8),
			new Move("down", 1),
			new Move("up", 4),
			new Move("forward", 4),
			new Move("forward", 7),
			new Move("up", 2),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("forward", 3),
			new Move("down", 2),
			new Move("down", 7),
			new Move("down", 4),
			new Move("down", 2),
			new Move("up", 5),
			new Move("down", 5),
			new Move("down", 5),
			new Move("down", 4),
			new Move("up", 1),
			new Move("forward", 7),
			new Move("down", 6),
			new Move("forward", 5),
			new Move("forward", 1),
			new Move("down", 4),
			new Move("up", 9),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("down", 3),
			new Move("down", 9),
			new Move("down", 1),
			new Move("forward", 6),
			new Move("up", 2),
			new Move("down", 7),
			new Move("down", 3),
			new Move("down", 6),
			new Move("up", 3),
			new Move("down", 4),
			new Move("down", 4),
			new Move("forward", 9),
			new Move("down", 3),
			new Move("forward", 2),
			new Move("down", 9),
			new Move("down", 8),
			new Move("up", 4),
			new Move("down", 2),
			new Move("forward", 2),
			new Move("down", 5),
			new Move("down", 4),
			new Move("down", 4),
			new Move("down", 2),
			new Move("forward", 6),
			new Move("down", 3),
			new Move("forward", 1),
			new Move("down", 4),
			new Move("forward", 7),
			new Move("down", 5),
			new Move("up", 4),
			new Move("down", 6),
			new Move("forward", 8),
			new Move("down", 6),
			new Move("forward", 2),
			new Move("forward", 4),
			new Move("forward", 5),
			new Move("forward", 7),
			new Move("forward", 4),
			new Move("forward", 5),
			new Move("down", 8),
			new Move("down", 7),
			new Move("forward", 3),
			new Move("forward", 5),
			new Move("up", 7),
			new Move("forward", 1),
			new Move("down", 4),
			new Move("forward", 5),
			new Move("forward", 4),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("down", 8),
			new Move("forward", 8),
			new Move("down", 1),
			new Move("down", 1),
			new Move("down", 5),
			new Move("up", 5),
			new Move("forward", 6),
			new Move("down", 6),
			new Move("forward", 3),
			new Move("forward", 4),
			new Move("forward", 7),
			new Move("forward", 4),
			new Move("down", 8),
			new Move("forward", 2),
			new Move("down", 4),
			new Move("forward", 4),
			new Move("down", 1),
			new Move("up", 2),
			new Move("forward", 6),
			new Move("up", 1),
			new Move("down", 7),
			new Move("down", 9),
			new Move("forward", 7),
			new Move("forward", 2),
			new Move("up", 3),
			new Move("down", 2),
			new Move("down", 9),
			new Move("down", 5),
			new Move("up", 7),
			new Move("forward", 1),
			new Move("forward", 8),
			new Move("down", 8),
			new Move("up", 3),
			new Move("down", 3),
			new Move("forward", 9),
			new Move("up", 4),
			new Move("down", 5),
			new Move("up", 5),
			new Move("down", 1),
			new Move("up", 8),
			new Move("forward", 9),
			new Move("down", 3),
			new Move("up", 6),
			new Move("forward", 6),
			new Move("forward", 1),
			new Move("down", 1),
			new Move("forward", 9),
			new Move("down", 8),
			new Move("forward", 8),
			new Move("down", 6),
			new Move("up", 9),
			new Move("down", 4),
			new Move("up", 3),
			new Move("up", 9),
			new Move("forward", 2),
			new Move("down", 2),
			new Move("down", 2),
			new Move("forward", 3),
			new Move("down", 2),
			new Move("forward", 5),
			new Move("forward", 4),
			new Move("up", 8),
			new Move("forward", 9),
			new Move("up", 7),
			new Move("forward", 2),
			new Move("down", 5),
			new Move("down", 6),
			new Move("forward", 8),
			new Move("up", 7),
			new Move("forward", 4),
			new Move("forward", 3),
			new Move("up", 5),
			new Move("down", 8),
			new Move("forward", 3),
			new Move("up", 2),
			new Move("down", 3),
			new Move("forward", 6),
			new Move("down", 9),
			new Move("down", 2),
			new Move("down", 6),
			new Move("down", 2),
			new Move("forward", 7),
			new Move("forward", 5),
			new Move("forward", 7),
			new Move("down", 8),
			new Move("forward", 2),
			new Move("down", 2),
			new Move("forward", 8),
			new Move("up", 8),
			new Move("forward", 4),
			new Move("forward", 3),
			new Move("up", 5),
			new Move("down", 3),
			new Move("forward", 3),
			new Move("up", 8),
			new Move("up", 7),
			new Move("down", 4),
			new Move("down", 1),
			new Move("forward", 2),
			new Move("down", 1),
			new Move("up", 6),
			new Move("up", 4),
			new Move("down", 3),
			new Move("up", 1),
			new Move("forward", 7),
			new Move("forward", 7),
			new Move("forward", 7),
			new Move("forward", 8),
			new Move("down", 1),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("forward", 9),
			new Move("forward", 7),
			new Move("forward", 7),
			new Move("down", 4),
			new Move("up", 4),
			new Move("down", 6),
			new Move("down", 9),
			new Move("up", 4),
			new Move("up", 2),
			new Move("up", 6),
			new Move("forward", 4),
			new Move("up", 4),
			new Move("up", 6),
			new Move("down", 2),
			new Move("forward", 4),
			new Move("down", 9),
			new Move("forward", 9),
			new Move("forward", 9),
			new Move("down", 1),
			new Move("forward", 7),
			new Move("down", 2),
			new Move("down", 7),
			new Move("down", 8),
			new Move("down", 8),
			new Move("down", 9),
			new Move("up", 9),
			new Move("down", 5),
			new Move("forward", 5),
			new Move("forward", 7),
			new Move("forward", 4),
			new Move("down", 7),
			new Move("forward", 8),
			new Move("forward", 1),
			new Move("down", 8),
			new Move("up", 9),
			new Move("down", 7),
			new Move("forward", 9),
			new Move("forward", 4),
			new Move("forward", 8),
			new Move("down", 9),
			new Move("forward", 4),
			new Move("down", 3),
			new Move("forward", 3),
			new Move("down", 1),
			new Move("down", 1),
			new Move("down", 2),
			new Move("up", 5),
			new Move("down", 2),
			new Move("down", 1),
			new Move("down", 8),
			new Move("forward", 3),
			new Move("up", 2),
			new Move("forward", 7),
			new Move("down", 3),
			new Move("down", 8),
			new Move("down", 1),
			new Move("forward", 4),
			new Move("forward", 7),
			new Move("down", 5),
			new Move("forward", 6),
			new Move("down", 6),
			new Move("down", 2),
			new Move("forward", 6),
			new Move("down", 3),
			new Move("up", 4),
			new Move("down", 7),
			new Move("forward", 7),
			new Move("up", 1),
			new Move("up", 9),
			new Move("down", 1),
			new Move("down", 2),
			new Move("down", 8),
			new Move("down", 7),
			new Move("up", 1),
			new Move("forward", 7),
			new Move("down", 2),
			new Move("forward", 4),
			new Move("forward", 6),
			new Move("forward", 9),
			new Move("down", 6),
			new Move("forward", 2),
			new Move("up", 8),
			new Move("down", 2),
			new Move("up", 2),
			new Move("up", 5),
			new Move("down", 8),
			new Move("up", 6),
			new Move("down", 9),
			new Move("forward", 6),
			new Move("down", 8),
			new Move("down", 6),
			new Move("down", 1),
			new Move("up", 7),
			new Move("up", 6),
			new Move("down", 8),
			new Move("forward", 2),
			new Move("up", 7),
			new Move("forward", 5),
			new Move("forward", 7),
			new Move("forward", 7),
			new Move("up", 5),
			new Move("forward", 2),
			new Move("down", 9),
			new Move("up", 2),
			new Move("up", 8),
			new Move("up", 2),
			new Move("down", 3),
			new Move("down", 7),
			new Move("forward", 9),
			new Move("down", 3),
			new Move("up", 9),
			new Move("forward", 8),
			new Move("up", 8),
			new Move("forward", 4),
			new Move("forward", 8),
			new Move("forward", 6),
			new Move("up", 1),
			new Move("down", 3),
			new Move("up", 1),
			new Move("down", 1),
			new Move("forward", 2),
			new Move("forward", 1),
			new Move("forward", 4),
			new Move("forward", 7),
			new Move("up", 8),
			new Move("down", 9),
			new Move("up", 2),
			new Move("down", 7),
			new Move("forward", 4),
			new Move("down", 3),
			new Move("forward", 4),
			new Move("forward", 2),
			new Move("down", 9),
			new Move("forward", 8),
			new Move("forward", 5),
			new Move("forward", 3),
			new Move("down", 6),
			new Move("forward", 4),
			new Move("forward", 4),
			new Move("forward", 9),
			new Move("forward", 4),
			new Move("up", 5),
			new Move("down", 7),
			new Move("up", 6),
			new Move("forward", 5),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("forward", 3),
			new Move("forward", 5),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("up", 4),
			new Move("down", 8),
			new Move("up", 3),
			new Move("down", 3),
			new Move("up", 5),
			new Move("forward", 4),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("forward", 6),
			new Move("forward", 1),
			new Move("forward", 8),
			new Move("down", 6),
			new Move("down", 9),
			new Move("up", 5),
			new Move("forward", 2),
			new Move("forward", 8),
			new Move("up", 6),
			new Move("down", 6),
			new Move("forward", 2),
			new Move("down", 8),
			new Move("forward", 7),
			new Move("forward", 7),
			new Move("down", 5),
			new Move("forward", 5),
			new Move("forward", 8),
			new Move("forward", 1),
			new Move("down", 4),
			new Move("down", 2),
			new Move("down", 5),
			new Move("up", 4),
			new Move("forward", 3),
			new Move("forward", 5),
			new Move("down", 4),
			new Move("down", 7),
			new Move("down", 4),
			new Move("up", 9),
			new Move("up", 6),
			new Move("forward", 1),
			new Move("down", 8),
			new Move("up", 8),
			new Move("up", 9),
			new Move("forward", 2),
			new Move("forward", 1),
			new Move("down", 6),
			new Move("forward", 6),
			new Move("down", 4),
			new Move("forward", 7),
			new Move("up", 2),
			new Move("up", 1),
			new Move("forward", 4),
			new Move("down", 1),
			new Move("forward", 8),
			new Move("forward", 3),
			new Move("up", 7),
			new Move("up", 5),
			new Move("down", 1),
			new Move("forward", 8),
			new Move("forward", 6),
			new Move("up", 6),
			new Move("forward", 9),
			new Move("down", 5),
			new Move("down", 9),
			new Move("forward", 2),
			new Move("down", 3),
			new Move("up", 1),
			new Move("up", 7),
			new Move("down", 1),
			new Move("forward", 8),
			new Move("up", 9),
			new Move("down", 1),
			new Move("down", 5),
			new Move("down", 7),
			new Move("down", 5),
			new Move("down", 5),
			new Move("down", 5),
			new Move("up", 9),
			new Move("forward", 9),
			new Move("forward", 7),
			new Move("forward", 4),
			new Move("forward", 6),
			new Move("down", 5),
			new Move("down", 3),
			new Move("forward", 9),
			new Move("forward", 1),
			new Move("down", 1),
			new Move("down", 8),
			new Move("up", 4),
			new Move("down", 9),
			new Move("forward", 9),
			new Move("up", 1),
			new Move("down", 5),
			new Move("forward", 8),
			new Move("up", 6),
			new Move("forward", 3),
			new Move("down", 6),
			new Move("up", 8),
			new Move("down", 7),
			new Move("forward", 3),
			new Move("forward", 6),
			new Move("down", 7),
			new Move("forward", 6),
			new Move("forward", 4),
			new Move("forward", 4),
			new Move("down", 4),
			new Move("forward", 6),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("forward", 6),
			new Move("down", 7),
			new Move("forward", 6),
			new Move("forward", 3),
			new Move("up", 4),
			new Move("up", 2),
			new Move("up", 6),
			new Move("down", 2),
			new Move("down", 8),
			new Move("forward", 5),
			new Move("forward", 1),
			new Move("up", 4),
			new Move("forward", 7),
			new Move("forward", 9),
			new Move("up", 6),
			new Move("down", 7),
			new Move("down", 3),
			new Move("up", 5),
			new Move("forward", 5),
			new Move("down", 8),
			new Move("up", 1),
			new Move("down", 1),
			new Move("down", 3),
			new Move("down", 2),
			new Move("down", 1),
			new Move("forward", 5),
			new Move("down", 3),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("forward", 9),
			new Move("down", 3),
			new Move("forward", 7),
			new Move("forward", 5),
			new Move("forward", 4),
			new Move("forward", 2),
			new Move("forward", 7),
			new Move("forward", 8),
			new Move("forward", 6),
			new Move("down", 8),
			new Move("forward", 5),
			new Move("forward", 6),
			new Move("forward", 6),
			new Move("down", 8),
			new Move("down", 2),
			new Move("forward", 4),
			new Move("down", 7),
			new Move("forward", 6),
			new Move("down", 7),
			new Move("down", 4),
			new Move("forward", 6),
			new Move("up", 6),
			new Move("forward", 4),
			new Move("forward", 9),
			new Move("forward", 2),
			new Move("forward", 3),
			new Move("forward", 1),
			new Move("down", 8),
			new Move("down", 3),
			new Move("forward", 4),
			new Move("up", 3),
			new Move("forward", 7),
			new Move("forward", 1),
			new Move("down", 7),
			new Move("down", 8),
			new Move("forward", 1),
			new Move("up", 8),
			new Move("forward", 8),
			new Move("up", 8),
			new Move("down", 5),
			new Move("forward", 6),
			new Move("down", 8),
			new Move("down", 4),
			new Move("down", 9),
			new Move("up", 1),
			new Move("down", 3),
			new Move("forward", 6),
			new Move("down", 6),
			new Move("forward", 7),
			new Move("forward", 3),
			new Move("down", 6),
			new Move("down", 6),
			new Move("forward", 4),
			new Move("down", 4),
			new Move("down", 1),
			new Move("down", 8),
			new Move("forward", 2),
			new Move("forward", 8),
			new Move("forward", 8),
			new Move("down", 6),
			new Move("forward", 9),
			new Move("down", 9),
			new Move("down", 5),
			new Move("down", 5),
			new Move("forward", 7),
			new Move("down", 1),
			new Move("forward", 1),
			new Move("down", 1),
			new Move("down", 6),
			new Move("down", 1),
			new Move("forward", 1),
			new Move("up", 6),
			new Move("up", 9),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("forward", 8),
			new Move("forward", 6),
			new Move("down", 7),
			new Move("forward", 1),
			new Move("forward", 4),
			new Move("forward", 9),
			new Move("forward", 2),
			new Move("forward", 4),
			new Move("down", 2),
			new Move("forward", 1),
			new Move("forward", 8),
			new Move("down", 1),
			new Move("down", 1),
			new Move("forward", 4),
			new Move("down", 5),
			new Move("down", 3),
			new Move("down", 9),
			new Move("down", 2),
			new Move("up", 8),
			new Move("down", 7),
			new Move("down", 1),
			new Move("down", 9),
			new Move("forward", 2),
			new Move("forward", 2),
			new Move("up", 3),
			new Move("forward", 3),
			new Move("down", 3),
			new Move("forward", 5),
			new Move("forward", 9),
			new Move("down", 7),
			new Move("up", 7),
			new Move("down", 9),
			new Move("forward", 3),
			new Move("forward", 7),
			new Move("down", 1),
			new Move("forward", 8),
			new Move("down", 8),
			new Move("forward", 1),
			new Move("down", 8),
			new Move("down", 6),
			new Move("forward", 2),
			new Move("down", 3),
			new Move("down", 1),
			new Move("down", 8),
			new Move("forward", 3),
			new Move("up", 5),
			new Move("down", 7),
			new Move("up", 2),
			new Move("up", 8),
			new Move("forward", 5),
			new Move("up", 7),
			new Move("down", 6),
			new Move("up", 7),
			new Move("down", 9),
			new Move("forward", 5),
			new Move("up", 4),
			new Move("forward", 9),
			new Move("down", 5),
			new Move("up", 7),
			new Move("down", 2),
			new Move("up", 2),
			new Move("up", 7),
			new Move("forward", 5),
			new Move("down", 6),
			new Move("forward", 4),
			new Move("down", 4),
			new Move("down", 3),
			new Move("forward", 2),
			new Move("up", 2),
			new Move("down", 5),
			new Move("forward", 8),
			new Move("down", 3),
			new Move("up", 7),
			new Move("down", 1),
			new Move("down", 7),
			new Move("forward", 7),
			new Move("forward", 4),
			new Move("forward", 7),
			new Move("down", 2),
			new Move("down", 9),
			new Move("down", 6),
			new Move("down", 9),
			new Move("down", 2),
			new Move("down", 9),
			new Move("down", 7),
			new Move("down", 5),
			new Move("forward", 4),
			new Move("up", 5),
			new Move("up", 7),
			new Move("forward", 2),
			new Move("forward", 7),
			new Move("down", 3),
			new Move("down", 3),
			new Move("forward", 4)
		};
	}
}
