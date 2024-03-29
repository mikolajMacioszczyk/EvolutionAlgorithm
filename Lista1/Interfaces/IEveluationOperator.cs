﻿using Lista1.Models;

namespace Lista1.Interfaces
{
    public interface IEveluationOperator : IComparer<Member>
    {
        public string Name { get; }
        public double Evaluate(Member member);
    }
}
