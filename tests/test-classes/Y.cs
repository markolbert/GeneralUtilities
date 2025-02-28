namespace Test.Expressions;

public class Y( bool zIsNull )
{
    public Z? ZEntity { get; set; } = zIsNull ? null: new Z();
}
