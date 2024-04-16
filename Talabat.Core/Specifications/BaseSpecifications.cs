﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public BaseSpecifications()
        {
            //Includes = new List<Expression<Func<T, object>>> ();
        }

        public BaseSpecifications(Expression<Func<T,bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
            //Includes = new List<Expression<Func <T, object>>> ();
        }
    }
}
