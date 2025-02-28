namespace Test.Expressions;

public class X( bool yIsNull, bool zIsNull )
{
    public Y? YEntity { get; set; } = yIsNull ? null : new Y( zIsNull );
}
