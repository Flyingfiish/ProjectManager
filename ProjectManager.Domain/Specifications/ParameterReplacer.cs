using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications
{
    internal class ParameterReplacer : ExpressionVisitor
    {
        readonly ParameterExpression _parameter;
        readonly ParameterExpression _replacement;

        public ParameterReplacer(ParameterExpression parameter, ParameterExpression replacement)
        {
            _parameter = parameter;
            _replacement = replacement;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(_parameter == node ? _replacement : node);
        }
    }
}
