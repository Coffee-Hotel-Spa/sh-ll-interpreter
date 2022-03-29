
namespace ShaellLang.StdLibrary;

public class Math
{
    private Number _num;

    public Math(Number num)
    {
        _num = num;
    }
    
    public double Sqrt() => System.Math.Sqrt(_num.ToFloating());
    public double Floor() => System.Math.Floor(_num.ToFloating());
    public double Ceil() => System.Math.Ceiling(_num.ToFloating());
    public double Log2() => System.Math.Log2(_num.ToFloating());
}