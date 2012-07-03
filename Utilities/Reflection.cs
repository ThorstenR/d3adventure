using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utilities
{
    public static class Reflection
    {
        // Not really reflection
        //  http://stackoverflow.com/questions/2566101/how-to-get-variable-name-using-reflection
        /// <summary>
        /// Use: 
        /// var someVar = 3;
        ///    Console.Write(GetVariableName(() => someVar));
        /// </summary>
        /// <returns>Returns the given variable's name from src</returns>
        public static string GetVariableName<T>(Expression<Func<T>> expr)
        {
            var body = (MemberExpression)expr.Body;

            return body.Member.Name;
        }
    }
}
