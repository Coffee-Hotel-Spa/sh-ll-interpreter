namespace ShaellLang.StdLibrary;


public static class SFile
{
    public static void Append(string path, string str)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, str);
        }
        else
        {
            File.AppendAllText(path, str);
        }
    }

    public static void Write(string path, string str)
    {
        File.WriteAllText(path, str);
    }
}
