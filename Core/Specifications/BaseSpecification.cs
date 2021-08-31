using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{

    // use to implement the include function 
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
             
        }

        public Expression<Func<T, bool>> Criteria  {get; }

        //initalize it with an empty list so that we can use it to add things into the list 
        //
        public List<Expression<Func<T, object>>> Includes {get;} = new List<Expression<Func<T, object>>>();


        //use to add includes  
        protected void AddInclude(Expression<Func<T, object>> includeExpression){
            Includes.Add(includeExpression);
        }   
    }
}