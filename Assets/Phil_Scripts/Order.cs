public class Order
{
    public enum Type
    {
        Speed
       , Rotation
       , Repeat
       , Pass
    }

    public Type type;
    public float value;

    public Order(Type _type, float _value = 0)
    {
        type = _type;
        value = _value;
    }
}
