using FluentAssertions;
using J4JSoftware.Utilities;
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace Test.Expressions;

public class ExpressionTests
{
    [ Fact ]
    public void TestPropertyInfo()
    {
        var helpers = new ExpressionHelpers( null );

        helpers.TryGetPropertyInfo<X, int>( x => x.YEntity.ZEntity.IntValue, out var propInfo ).Should().BeTrue();
        propInfo!.Name.Should().Be( nameof( Z.IntValue ) );

        var propPath = helpers.GetPropertyInfoPath<X, int>( x => x.YEntity.ZEntity.IntValue );
        propPath.Count.Should().Be( 3 );
        propPath[ 0 ].Name.Should().Be( nameof( Z.IntValue ) );
        propPath[ 1 ].Name.Should().Be( nameof( Y.ZEntity ) );
        propPath[ 2 ].Name.Should().Be( nameof( X.YEntity ) );

        helpers.TryGetPropertyInfo<X, int?>( x => x.YEntity.ZEntity.IntNullValue, out var nullPropInfo )
               .Should()
               .BeTrue();
        nullPropInfo!.Name.Should().Be( nameof( Z.IntNullValue ) );

        propPath = helpers.GetPropertyInfoPath<X, int?>( x => x.YEntity.ZEntity.IntNullValue );
        propPath.Count.Should().Be( 3 );
        propPath[ 0 ].Name.Should().Be( nameof( Z.IntNullValue ) );
        propPath[ 1 ].Name.Should().Be( nameof( Y.ZEntity ) );
        propPath[ 2 ].Name.Should().Be( nameof( X.YEntity ) );
    }

    [ Theory ]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void TestIntSetter( bool yIsNull, bool zIsNull )
    {
        var helpers = new ExpressionHelpers(null);

        var setter = helpers.CreatePropertySetter<X, int>( x => x.YEntity.ZEntity.IntValue );
        setter.Should().NotBeNull();

        var testObj = new X( yIsNull, zIsNull );

        if( !yIsNull && !zIsNull )
            setter!( testObj, 37 );

        TestSetter( testObj, yIsNull, zIsNull, 37, null );

        var nullSetter = helpers.CreatePropertySetter<X, int?>(x => x.YEntity.ZEntity.IntNullValue);
        nullSetter.Should().NotBeNull();

        var nullObj = new X(yIsNull, zIsNull);

        if (!yIsNull && !zIsNull)
            nullSetter!(nullObj, 37);
    
        TestSetter(nullObj, yIsNull, zIsNull, null, 37);

        if (!yIsNull && !zIsNull)
            nullSetter!(nullObj, null);

        TestSetter(nullObj, yIsNull, zIsNull, null, null);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void TestObjectSetter(bool yIsNull, bool zIsNull)
    {
        var helpers = new ExpressionHelpers(null);

        var setter = helpers.CreateObjectPropertySetter<X, int>(x => x.YEntity.ZEntity.IntValue);
        setter.Should().NotBeNull();

        var testObj = new X(yIsNull, zIsNull);

        if (!yIsNull && !zIsNull)
            setter!(testObj, 37);

        TestSetter(testObj, yIsNull, zIsNull, 37, null);

        var nullSetter = helpers.CreateObjectPropertySetter<X, int?>(x => x.YEntity.ZEntity.IntNullValue);
        nullSetter.Should().NotBeNull();

        var nullObj = new X(yIsNull, zIsNull);

        if (!yIsNull && !zIsNull)
            nullSetter!(nullObj, 37);

        TestSetter(nullObj, yIsNull, zIsNull, null, 37);

        if (!yIsNull && !zIsNull)
            nullSetter!(nullObj, null);

        TestSetter(nullObj, yIsNull, zIsNull, null, null);
    }

    private void TestSetter( X testObj, bool yIsNull, bool zIsNull, int? intValue, int? intNullValue )
    {
        if( yIsNull )
        {
            testObj.YEntity.Should().BeNull();
            return;
        }

        testObj.YEntity.Should().NotBeNull();

        if( zIsNull )
        {
            testObj.YEntity.ZEntity.Should().BeNull();
            return;
        }

        testObj.YEntity.ZEntity.Should().NotBeNull();

        if( intValue.HasValue )
            testObj.YEntity.ZEntity.IntValue.Should().Be( intValue.Value );

        if( intNullValue.HasValue )
        {
            testObj.YEntity.ZEntity.IntNullValue.Should().NotBeNull();
            testObj.YEntity.ZEntity.IntNullValue?.Should().Be( intNullValue.Value );
        }
        else testObj.YEntity.ZEntity.IntNullValue.Should().BeNull();
    }
}