# ExpressionHelper

The `ExpressionHelper` class provides methods which I've found useful for dealing with Expressions. It supports logging, if you provide an instance of `ILoggerFactory` in the constructor call.

## TryGetPropertyInfo

This method attempts to get the `PropertyInfo` instance associated with an `Expression`.

|Argument|Type|Comments|
|--------|----|--------|
|propExpr|`Expression<Func<TRoot, TProp>>`|the `Expression` to be evaluated|
|propertyInfo|`out PropertyInfo?`|the `PropertyInfo` instance associated with `propExpr`, if one could be found|

The method returns `true` if the associated `PropertyInfo` object can be found. It returns false if `propExpr.Body` is not a `MemberExpression` or a `UnaryExpression` whose `Operand` is not a `MemberExpression`.

[return to readme](../readme.md)

## GetPropertyInfoPath

This method attempts to return a `List<PropertyInfo>` instance which records all of the properties along the path from the supplied `Expression` back to the root of the `Expression`.

|Argument|Type|Comments|
|--------|----|--------|
|propExpr|`Expression<Func<TRoot, TProp>>`|the `Expression` to be evaluated|

For example, consider this object structure:

```c#
public class RootObject
{
    public ChildObject ChildObjectc { get; set; }
}

public class ChildObject
{
    public GrandChildObject GrandChildObject { get; set; }
}

public class GrandChildObject
{
    public string Target { get; set; }
}
```

GetPropertyInfoPath<RootOjbect, GrandChildObject>( x => x.ChildObject.GrandChildObject.Target ) would return the following list:

- `PropertyInfo` object for `Target`
- `PropertyInfo` object for `GrandChildObject`
- `PropertyInfo` object for `ChildObject`
- `PropertyInfo` object for `RootObject`

The list is in leaf-order, i.e., the first entry is the *leaf* of the branch extending from the root object to the leaf property.

List generation will terminate as soon as the recursive process encounters an `Expression` that is neither a `MemberExpression` or a `UnaryExpression` whose `Operand` property is not a `MemberExpression`.

[return to readme](../readme.md)

## CreatePropertySetter

This method creates a property setter (`Action<TRoot, TProp>`) for the property defined by the provided `Expression`.

|Argument|Type|Comments|
|--------|----|--------|
|propExpr|`Expression<Func<TRoot, TProp>>`|the `Expression` to be evaluated|

It uses `GetPropertyInfoPath()` internally, so the path between the root object and the leaf property must involve only `MemberExpression` or a `UnaryExpression` whose `Operand` property is not a `MemberExpression`.

[return to readme](../readme.md)

## CreatePropertyGetter

This method creates a property getter (`Func<TRoot, TProp>`) for the property defined by the provided `Expression`.

|Argument|Type|Comments|
|--------|----|--------|
|propExpr|`Expression<Func<TRoot, TProp>>`|the `Expression` to be evaluated|

It uses `GetPropertyInfoPath()` internally, so the path between the root object and the leaf property must involve only `MemberExpression` or a `UnaryExpression` whose `Operand` property is not a `MemberExpression`.

[return to readme](../readme.md)
