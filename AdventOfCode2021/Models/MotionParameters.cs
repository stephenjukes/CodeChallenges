using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbsCodeChallenges.AdventOfCode2021
{
    public class MotionDimension
    {
        public MotionDimension()
        {
        }

        public MotionDimension(
            string label, 
            double position, 
            double velocity, 
            Func<double, double> accelerationFunc)
        {
            Label = label;
            Position = position;
            Velocity = velocity;
            AccelerationFunc = accelerationFunc;
        }

        public string Label { get; set; }
        public double Position { get; set; }
        public double Velocity { get; set; }
        public Func<double, double> AccelerationFunc{ get; set; }

        public override string ToString()
        {
            return $"position: {Position}, velocity: {Velocity}";
        }
    }

    public class MotionStep
    {
        private Dictionary<string, MotionDimension> _byDimension;

        public MotionStep(IEnumerable<MotionDimension> motionDimensions)
        {
            MotionDimensions = motionDimensions;
            _byDimension = motionDimensions.ToDictionary(d => d.Label);
        }

        public IEnumerable<MotionDimension> MotionDimensions { get; set; }

        public MotionDimension ByDimension(string dimension)
        {
            return _byDimension[dimension];
        }

        public override string ToString()
        {
            return string.Join(", ", MotionDimensions);
        }
    }

    public class Trajectory
    {
        public Trajectory(IEnumerable<MotionStep> steps)
        {
            Steps = steps;
        }

        public IEnumerable<MotionStep> Steps { get; set; }
    }

    public class MotionCalculator
    {
        public Trajectory GetTrajectory(MotionStep initialStep, Func<MotionStep, bool> whileValid)
        {
            var motionSteps = new List<MotionStep>() { initialStep };

            // TODO: refactor this duplication
            for (int step = 0; whileValid(motionSteps.Last()); step++)
            {
                var currentMotionParameters = motionSteps.Last();
                var nextMotionParameters = GetNextMotionParameters(currentMotionParameters);

                motionSteps.Add(nextMotionParameters);
            }

            return new Trajectory(motionSteps);
        }

        private MotionStep GetNextMotionParameters(MotionStep motionStep)
        {
            var newMotionDimensions = motionStep.MotionDimensions.Select(d => new MotionDimension
            {
                Label = d.Label,
                Position = d.Position + d.Velocity,
                Velocity = d.AccelerationFunc(d.Velocity),
                AccelerationFunc = d.AccelerationFunc
            });

            return new MotionStep(newMotionDimensions);
        }
    }
}
