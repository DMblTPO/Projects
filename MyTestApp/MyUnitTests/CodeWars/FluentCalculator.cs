using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUnitTests.CodeWars
{
    public class FluentCalculator
    {
        private enum ExpressionType
        {
            Value, Operation
        }

        private abstract class Expression
        {
            protected Expression(ExpressionType type) => Type = type;
            public ExpressionType Type { get; }
        }

        private enum OperatorType
        {
            Plus, Minus, Times, DevidedBy
        }

        private enum PriorityType
        {
            High, Low
        }

        private class Operator: Expression
        {
            public Operator(OperatorType name, PriorityType priority) : base(ExpressionType.Operation)
            {
                Name = name;
                Priority = priority;
            }
            public OperatorType Name { get; }
            public PriorityType Priority { get; }
        }

        private class Operand : Expression
        {
            private readonly List<Operand> _expression = new List<Operand>();
            public Operand(double value) : base(ExpressionType.Value)
            {
                Value = value;
            }
            public double Value { get; }
            public double Res
            {
                get
                {
                    var res = Value;
                    _expression.ForEach(x => { res *= x.Value; });
                    return res;
                }
            }
            public void Add(Operand ex) => _expression.Add(ex);
        }

        private readonly List<Operand> _expression = new List<Operand>();
        private Expression _lastExpr;
        private Operand _lastOperand;
        private Operator _lastOperator;

        private void Init()
        {
            _lastOperand = new Operand(0);
            _lastOperator = new Operator(OperatorType.Plus, PriorityType.Low);
            _lastExpr = _lastOperator;

            _expression.Clear();
            _expression.Add(_lastOperand);
        }

        public FluentCalculator()
        {
            Init();
        }

        private static Exception WrongExpression => new Exception("Wrong expression");

        private void Validate(Expression expr)
        {
            switch (expr)
            {
                case Operand operand when _lastExpr.Type == ExpressionType.Operation:
                    if (_lastOperator.Priority == PriorityType.High)
                    {
                        _lastOperand.Add(
                            _lastOperator.Name == OperatorType.Times
                                ? operand
                                : new Operand(1 / operand.Value));
                    }
                    else
                    {
                        if (_lastOperator.Name == OperatorType.Minus)
                        {
                            operand = new Operand(-operand.Value);
                        }
                        _lastOperand = operand;
                        _expression.Add(operand);
                    }
                    _lastExpr = operand;
                    return;
                case Operator @operator when _lastExpr.Type == ExpressionType.Value:
                    _lastOperator = @operator;
                    _lastExpr = @operator;
                    return;
            }

            throw WrongExpression;
        }

        private FluentCalculator AddOperand(double i)
        {
            Validate(new Operand(i));
            return this;
        }

        private FluentCalculator AddOperator(OperatorType oper, PriorityType priority)
        {
            Validate(new Operator(oper, priority));
            return this;
        }

        public FluentCalculator Zero => AddOperand(0);
        public FluentCalculator One => AddOperand(1);
        public FluentCalculator Two  => AddOperand(2);
        public FluentCalculator Three  => AddOperand(3);
        public FluentCalculator Four  => AddOperand(4);
        public FluentCalculator Five => AddOperand(5);
        public FluentCalculator Six => AddOperand(6);
        public FluentCalculator Seven => AddOperand(7);
        public FluentCalculator Eight => AddOperand(8);
        public FluentCalculator Nine => AddOperand(9);
        public FluentCalculator Ten => AddOperand(10);

        public FluentCalculator Plus => AddOperator(OperatorType.Plus, PriorityType.Low);
        public FluentCalculator Minus => AddOperator(OperatorType.Minus, PriorityType.Low);
        public FluentCalculator Times => AddOperator(OperatorType.Times, PriorityType.High);
        public FluentCalculator DividedBy => AddOperator(OperatorType.DevidedBy, PriorityType.High);

        public double Result()
        {
            if (_lastExpr is Operator)
            {
                throw WrongExpression;
            }

            var result = _expression.Sum(x => x.Res);
            Init();

            return result;
        }

        public static implicit operator double(FluentCalculator fc)
        {
            try
            {
                return fc.Result();
            }
            catch
            {
                return 0;
            }
        }
    }
}

namespace MyUnitTests.CodeWars
{
    using NUnit.Framework;

    [TestFixture]
    public class FluentCalculatorTests
    {
        [Test]
        public static void BasicAddition()
        {
            var calculator = new FluentCalculator();

            //Test Result Call
            Assert.AreEqual(3, calculator.One.Plus.Two.Result());
        }

        [Test]
        public static void MultipleInstances()
        {
            var calculatorOne = new FluentCalculator();
            var calculatorTwo = new FluentCalculator();

            Assert.AreNotEqual((double)calculatorOne.Five.Plus.Five, (double)calculatorTwo.Seven.Times.Three);
        }

        [Test]
        public static void MultipleCalls()
        {
            //Testing that the expression or reference clears between calls
            var calculator = new FluentCalculator();
            Assert.AreEqual(4, calculator.One.Plus.One.Result() + calculator.One.Plus.One.Result());
        }

        [Test]
        public static void Bedmas()
        {
            //Testing Order of Operations
            var calculator = new FluentCalculator();
            Assert.AreEqual(58, (double)calculator.Six.Times.Six.Plus.Eight.DividedBy.Two.Times.Two.Plus.Ten.Times.Four.DividedBy.Two.Minus.Six);
            Assert.AreEqual(-11.972, calculator.Zero.Minus.Four.Times.Three.Plus.Two.DividedBy.Eight.Times.One.DividedBy.Nine, 0.01);
        }

        [Test]
        public static void StaticCombinationCalls()
        {
            //Testing Implicit Conversions
            var calculator = new FluentCalculator();
            Assert.AreEqual(177.5, 10 * calculator.Six.Plus.Four.Times.Three.Minus.Two.DividedBy.Eight.Times.One.Minus.Five.Times.Zero);
        }
    }
}