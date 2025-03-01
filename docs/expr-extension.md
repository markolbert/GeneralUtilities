# Expression Extension Methods (*deprecated*)

*These methods are deprecated in favor of using the `ExpressionHelpers` class.*

`GetPropertyInfo<TContainer, TProp>( this Expression<Func<TContainer, TProp>> expression )` returns a `PropertyInfo` instance from the provided Expression. It will fail with an `ArgumentException` if the provided expression lacks a Body property, or if the Expression is neither a `UnaryExpression` whose `Operand` property is not a `MemberExpression`, or a `MemberExpression`.

`Action<TEntity, TProperty> CreateSetter<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property)` creates a setter method for the provided Expression. It uses `GetPropertyInfo()` internally, so it will fail if `GetPropertyInfo()` fails. It will also fail with an `ArgumentNullException` if the associated `PropertyInfo` instance's `GetSetMethod()` returns null.

`Func<TEntity, TProperty> CreateGetter<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> property)` creates a getter method for the provided Expression.  It uses `GetPropertyInfo()` internally, so it will fail if `GetPropertyInfo()` fails. It will also fail with an `ArgumentNullException` if the associated `PropertyInfo` instance's `GetGetMethod()` returns null.
