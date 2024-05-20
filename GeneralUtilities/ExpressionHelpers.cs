#region copyright
// Copyright (c) 2021, 2022, 2023 Mark A. Olbert 
// https://www.JumpForJoySoftware.com
// ExpressionExtensions.cs
//
// This file is part of JumpForJoy Software's MiscellaneousUtilities.
// 
// MiscellaneousUtilities is free software: you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or 
// (at your option) any later version.
// 
// MiscellaneousUtilities is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
// 
// You should have received a copy of the GNU General Public License along 
// with MiscellaneousUtilities. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace J4JSoftware.Utilities;

public class ExpressionHelpers( ILoggerFactory? loggerFactory )
{
    private readonly ILogger? _logger = loggerFactory?.CreateLogger<ExpressionHelpers>();

    public bool TryGetPropertyInfo<TRoot, TProp>(
        Expression<Func<TRoot, TProp>> propExpr,
        out PropertyInfo? propInfo,
        [ CallerMemberName ] string caller = ""
    )
    {
        propInfo = propExpr.Body is MemberExpression memExpr ? (PropertyInfo) memExpr.Member : null;

        if( propInfo == null )
            _logger?.InvalidExpression( propExpr.Body.NodeType, caller );

        return propInfo != null;
    }

    public List<PropertyInfo> GetPropertyInfoPath<TRoot, TProp>(
        Expression<Func<TRoot, TProp>> propExpr,
        [ CallerMemberName ] string caller = ""
    )
    {
        var retVal = new List<PropertyInfo>();

        var curExpr = propExpr.Body;

        while( curExpr != null )
        {
            switch( curExpr )
            {
                case MemberExpression memExpr:
                    retVal.Add( (PropertyInfo) memExpr.Member );
                    curExpr = memExpr.Expression;

                    break;

                default:
                    curExpr = null;
                    break;
            }
        }

        if( retVal.Count == 0 )
            _logger?.EmptyPropertyInfoPath( propExpr.ToString() );

        // the list is leaf first (i.e., the root is the last element in the list)
        return retVal;
    }

    public Action<TRoot, TProp>? CreatePropertySetter<TRoot, TProp>(
        Expression<Func<TRoot, TProp>> property,
        [ CallerMemberName ] string caller = ""
    )
    {
        var propInfoPath = GetPropertyInfoPath( property );

        // make the list root first
        propInfoPath.Reverse();

        if( propInfoPath.Count == 0 )
            return null;

        var rootArg = Expression.Parameter(typeof(TRoot), "root");
        var newValueArg = Expression.Parameter(typeof(TProp), "newValue");

        Expression? prevExpr = null;

        foreach( var propInfo in propInfoPath )
        {
            // first/root element
            var curExpr = propInfo == propInfoPath.First() ? rootArg : prevExpr;

            prevExpr = Expression.Property( curExpr, propInfo );

            if( propInfo == propInfoPath.Last() )
                prevExpr = Expression.Assign( prevExpr, newValueArg );
        }

        var lambdaExpr = Expression.Lambda<Action<TRoot, TProp>>( prevExpr!, [rootArg, newValueArg] );

        return lambdaExpr.Compile();
    }

    public Action<TRoot, object?>? CreateObjectPropertySetter<TRoot, TProp>(
        Expression<Func<TRoot, TProp?>> property,
        [CallerMemberName] string caller = ""
    )
    {
        var propInfoPath = GetPropertyInfoPath(property);

        // make the list root first
        propInfoPath.Reverse();

        if (propInfoPath.Count == 0)
            return null;

        var rootArg = Expression.Parameter(typeof(TRoot), "root");
        var newValueArg = Expression.Parameter(typeof(object), "newValue");

        Expression? prevExpr = null;

        foreach (var propInfo in propInfoPath)
        {
            // first/root element
            var curExpr = propInfo == propInfoPath.First() ? rootArg : prevExpr;

            prevExpr = Expression.Property(curExpr, propInfo);

            if( propInfo != propInfoPath.Last() )
                continue;

            var objNewValue = Expression.Convert( newValueArg, typeof( TProp? ) );
            prevExpr = Expression.Assign( prevExpr, objNewValue );
        }

        var lambdaExpr = Expression.Lambda<Action<TRoot, object?>>(prevExpr!, [rootArg, newValueArg]);

        return lambdaExpr.Compile();
    }
}
