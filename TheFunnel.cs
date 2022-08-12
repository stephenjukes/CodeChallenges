using System;
using System.Collections.Generic;
using System.Linq;

namespace TheFunnel
{
    public class TheFunnel
    {
        public static void Run()
        {
            var funnel = new Funnel(5);
            funnel.Fill(1, 2);
            Console.WriteLine(funnel.ToString());

            funnel.Drip(); Console.WriteLine(funnel.ToString());
            //funnel.Drip(); Console.WriteLine(funnel.ToString());
            //funnel.Drip(); Console.WriteLine(funnel.ToString());
            //funnel.Drip(); Console.WriteLine(funnel.ToString());
            //funnel.Drip(); Console.WriteLine(funnel.ToString());
            //funnel.Drip(); Console.WriteLine(funnel.ToString());
            //funnel.Drip(); Console.WriteLine(funnel.ToString());
            funnel.Fill(1, 2, 3); Console.WriteLine(funnel.ToString());
        }
    }

    public class Funnel
    {
        private List<Cell> _flatFunnel = new List<Cell>();
        private int? _maxCapacity = null;

        public Funnel()
        {
        }

        public Funnel(int height)
        {
            _maxCapacity = MathHelpers.ResolveTriangularNumber(height);
            _flatFunnel = new Cell[(int)_maxCapacity]
                .Select((e, i) => new Cell(null, ResolvePosition(i)))
                .ToList();
        }

        public void Fill(params int[] values)
        {
            var indexToAdd = 0;
            var fillIndex = 0;
            var isWithinCapacity = (_maxCapacity == null || _flatFunnel.Count(cell => cell.Value != null) < _maxCapacity);

            while (indexToAdd < values.Length && isWithinCapacity)
            {
                var indexOutOfRange = _flatFunnel.ElementAtOrDefault(fillIndex) == null;
                if (indexOutOfRange)
                {
                    _flatFunnel.Add(new Cell(values[indexToAdd], ResolvePosition(fillIndex)));
                    indexToAdd++;
                }

                var cellHasNullValue = _flatFunnel[fillIndex].Value == null;
                if (cellHasNullValue)
                {
                    _flatFunnel[fillIndex].Value = values[indexToAdd];
                    indexToAdd++;
                };

                fillIndex++;
            }
        }

        public int? Drip()
        {
            var dripped = _flatFunnel.FirstOrDefault();
            var dripValue = dripped.Value;

            ResolveNewFunnel(dripped);

            return dripValue;
        }

        private void ResolveNewFunnel(Cell dripped)
        {
            if (dripped == null) return;

            var replacement = _flatFunnel
                .Where(cell => IsReplacementCandidate(cell, dripped))
                .Select(cell => IncludeSupportedWeight(cell))
                .OrderByDescending(cell => cell.SupportedWeight)
                .ThenBy(cell => cell.Position.Column)
                .FirstOrDefault();

            dripped.Value = replacement?.Value;

            ResolveNewFunnel(replacement);
        }

        private bool IsReplacementCandidate(Cell cell, Cell dripped)
        {
            var isReplacementCandidate = cell.Value != null
                && cell.Position.Height == dripped.Position.Height + 1
                && (cell.Position.Column == dripped.Position.Column || cell.Position.Column == dripped.Position.Column + 1);

            return isReplacementCandidate;
        }

        private Cell IncludeSupportedWeight(Cell supportingCell)
        {
            supportingCell.SupportedWeight = _flatFunnel.Count(cell =>
                cell.Value != null
                && cell.Position.Height > supportingCell.Position.Height
                && cell.Position.Column >= supportingCell.Position.Column
                && (cell.Position.Column - cell.Position.Height) <= (supportingCell.Position.Column - supportingCell.Position.Height));

            return supportingCell;
        }

        private Position ResolvePosition(int index)
        {
            var triangularRoot = MathHelpers.ResolveTriangularRoot(index);
            var height = (int)Math.Floor(triangularRoot);
            var column = index - MathHelpers.ResolveTriangularNumber(height);

            return new Position
            {
                Height = height,
                Column = column
            };
        }

        public override string ToString()
        {
            var funnelHeight = _flatFunnel
                .Select(cell => cell.Position.Height)
                .OrderByDescending(height => height)
                .FirstOrDefault();

            var orderedFunnel = _flatFunnel
                .GroupBy(cell => cell.Position.Height)
                .OrderByDescending(row => row.Key)
                .Select(row => row.OrderBy(cell => cell.Position.Column))
                .Select(row => FormatToArrayWithNulls(row))
                .Select(row => FormatToFunneRowString(row, funnelHeight));

            return $"\n{ string.Join("\n", orderedFunnel) }\n";
        }

        private IEnumerable<Cell> FormatToArrayWithNulls(IEnumerable<Cell> row)
        {
            return new Cell[row.First().Position.Height + 1]
                .Select((cell, index) =>
                {
                    var matching = row.FirstOrDefault(c => c.Position.Column == index);
                    return matching != null
                        ? new Cell(matching.Value, matching.Position)
                        : new Cell(null, null);
                });
        }

        private string FormatToFunneRowString(IEnumerable<Cell> row, int funnelHeight)
        {
            return new string(' ', funnelHeight - row.First().Position.Height) +
                "\\" +
                string.Join(" ", row) +
                "/";
        }
    }

    public class Cell
    {
        public Cell(int? value, Position position)
        {
            Value = value;
            Position = position;
        }

        public int? Value { get; set; }
        public Position Position { get; set; }
        public int SupportedWeight { get; set; }

        // This behaviour should really be a part of the Funnel.ToString() functionality
        public override string ToString()
            => Value != null ? Value.ToString() : " ";
    }

    public class Position
    {
        public int Height { get; set; }
        public int Column { get; set; }
    }

    public static class MathHelpers
    {
        public static double ResolveTriangularRoot(int n) => (Math.Sqrt(8 * n + 1) - 1) / 2;
        public static int ResolveTriangularNumber(int n) => n * (n + 1) / 2;
    }
}

//class Cell
//{
//    constructor(value, position)
//    {
//        this.value = value;
//        this.position = position;
//        this.supportedWeight = null;
//    }
//}

//class Position
//{
//    constructor(height, column)
//    {
//        this.height = height;
//        this.column = column;
//    }
//}

//class MathHelpers
//{
//    static resolveTriangularRoot(n)
//    {
//        return (Math.sqrt(8 * n + 1) - 1) / 2;
//    }

//    static resolveTriangularNumber(n)
//    {
//        return n * (n + 1) / 2;
//    }
//}

//class Funnel
//{
//    constructor()
//    {
//        this.flatFunnel = [];
//    }

//    fill()
//    {
//        const values = arguments;
//        let indexToAdd = 0;
//        let fillIndex = 0;

//        console.log(values);

//        while (indexToAdd < values.length)
//        {
//            console.log("checking fillIndex " + fillIndex);

//            if (fillIndex >= this.flatFunnel.length)
//            {
//                let newCell = new Cell(values[indexToAdd], this.resolvePosition(fillIndex));
//                console.log("No cell. Creating new cell with value " + newCell.position.height);

//                this.flatFunnel.push(newCell);
//                indexToAdd++;
//            }

//            if (this.flatFunnel[fillIndex].value == null)
//            {
//                console.log("Cell with null value. Populating with value " + values[indexToAdd]);

//                this.flatFunnel[fillIndex].value = values[indexToAdd];
//                indexToAdd++;
//            }

//            fillIndex++;
//            console.log();
//        }

//        //     console.log(this.flatFunnel.map(c => c.position.height));
//    }

//    drip()
//    {
//        const dripped = this.flatFunnel[0];
//        const dripValue = dripped.value;

//        this.resolveNewFunnel(dripped);
//        return dripValue;
//    }

//    resolveNewFunnel(dripped)
//    {
//        var replacement = this.flatFunnel
//          .filter(cell => this.isReplacementCandidate(cell, dripped))
//          .map(cell => this.includeSupportedWeight(cell))
//          .sort((a, b) => b.supportedWeight - a.supportedWeight || a.column - b.column)[0];

//        if (replacement == null) return;

//        dripped.value = replacement
//      this.resolveNewFunnel(replacement);
//    }

//    isReplacementCandidate(cell, dripped)
//    {
//        const isReplacementCandidate = cell.value != null
//          && cell.position.height == dripped.position.height + 1
//          && (cell.position.column == dripped.position.column || cell.position.column == dripped.position.column + 1);

//        return isReplacementCandidate;
//    }

//    includeSupportedWeight(supportingCell)
//    {
//        supportingCell.supportedWeight = this.flatFunnel.filter(cell =>
//            cell.value != null
//              && cell.position.height > supportingCell.position.height
//              && cell.position.column >= supportingCell.position.column
//              && (cell.position.column - cell.position.height) <= (supportingCell.position.column - supportingCell.position.height));

//        return supportingCell;
//    }

//    resolvePosition(index)
//    {
//        //     console.log("Starting resolvePosition()");
//        //     console.log("index: " + index);

//        const triangularRoot = MathHelpers.resolveTriangularRoot(index);
//        const height = Math.floor(triangularRoot);
//        const column = index - MathHelpers.resolveTriangularNumber(height);

//        //     console.log("triangularRoot: " + triangularRoot);
//        //     console.log("height: " + height);
//        //     console.log("column: " + column);

//        //     const position = new Position(height, column);
//        //     console.log(position);

//        return new Position(height, column)
//    }

//    toString()
//    {
//        //     console.log("starting toString()");
//        //     console.log(this.flatFunnel.map(c => c.position));

//        const funnelHeight = this.flatFunnel
//          .map(cell => cell.position.height)
//          .sort((a, b) => b - a)[0];

//        const rows = this.flatFunnel
//          .reduce((groups, cell) => {
//              !groups.hasOwnProperty(cell.position.height)
//                ? groups[cell.position.height] = [cell]
//                : groups[cell.position.height].push(cell);

//              return groups;
//          }, { })
//    console.log("groups:");
//        console.log(rows);

//        const orderedFunnel = Object.values(rows);
//        console.log("orderedFunnel:")
//      console.log(orderedFunnel);

//        const sortedRows = orderedFunnel.sort((a, b) => b[0].position.height - a[0].position.height);
//        console.log("sortedRows");
//        console.log(sortedRows);

//        const sortedColumns = sortedRows.map(row => row.sort((a, b) => a.column - b.column));
//        console.log("sortedColumns");
//        console.log(sortedColumns);

//        const arrayWithNulls = sortedColumns.map(row => this.formatToArrayWithNulls(row));
//        console.log("arrayWithNulls");
//        console.log(arrayWithNulls);

//        const funnelStrings = arrayWithNulls.map(row => this.formatToFunnelRowString(row, funnelHeight));
//        console.log("funnelStrings");
//        console.log(funnelStrings);

//        console.log(funnelStrings.join("\n"));

//        return funnelStrings.join("\n");

//        // return orderedFunnel.join("\n");
//    }

//    formatToArrayWithNulls(row)
//    {
//        //     console.log("Starting formatToArrayWithNulls()");
//        //     console.log(this.flatFunnel);

//        //     console.log("row");
//        //     console.log(row);

//        let array = new Array(row[0].position.height + 1);
//        //     console.log("array");
//        //     console.log(array);

//        for (let i = 0; i < array.length; i++)
//        {
//            const matching = row.find(c => c.position.column == i);

//            array[i] = matching != null
//                ? new Cell(matching.value, matching.position)
//                : new Cell(null, null);
//        }

//        //     console.log("array");
//        //     console.log(array);

//        return array;
//    }

//    formatToFunnelRowString(row, funnelHeight)
//    {
//        console.log("Starting formatToFunnelRowString()");
//        console.log(row);

//        const values = row.map(cell => cell.value);
//        return new String(" ").repeat(funnelHeight - row[0].position.height) + "\\" + values.join(" ") + "/";
//    }
//}



