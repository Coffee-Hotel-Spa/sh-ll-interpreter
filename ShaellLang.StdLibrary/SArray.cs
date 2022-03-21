namespace ShaellLang.StdLibrary;

public class SArray<T>
{
    public List<T> Arr;

    public SArray(List<T> array)
    {
        Arr = array;
    }

    int Len => Arr.Count;


    public  T Append(T item)
    {
        Arr.Add(item);
        return item;
    }

    public T Pop(int index)
    {
        T x = Arr[index];
        Arr.RemoveAt(index);
        return x;
    }

    public T Get(int index)
    {
        return Arr[index];
    }

    public void Concat(SArray<T> arr)
    {
        Arr.Concat(arr.Arr);
    }
}