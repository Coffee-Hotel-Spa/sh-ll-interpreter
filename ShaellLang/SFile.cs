using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ShaellLang;

public class SFile : BaseValue
{
    private string _path;
    private NativeTable _table;
    private string _cwd;
    private string _absoluteFilePath;
    public SFile(string path, string cwd) : base("file")
    {
        _path = path;
        _cwd = cwd;
        GenerateFileValues();
        string oldCwd = Directory.GetCurrentDirectory();
        Environment.CurrentDirectory = _cwd;
        _absoluteFilePath = Path.GetFullPath(_path);
        Environment.CurrentDirectory = oldCwd;
    }
    
    private void GenerateFileValues()
    {
        _table = new NativeTable();
        _table.SetValue("read", new NativeFunc( ReadFunc, 2));
        _table.SetValue("delete", new NativeFunc( DeleteFunc, 0));
        _table.SetValue("readToEnd", new NativeFunc( ReadToEndFunc, 0));
        _table.SetValue("append", new NativeFunc( AppendFunc, 1));
        _table.SetValue("size", new NativeFunc( SizeFunc, 2));
        _table.SetValue("exists", new NativeFunc( ExistsFunc, 0));
    }

    private string RealPath => _absoluteFilePath;

    private IValue ExistsFunc(IEnumerable<IValue> argCollection)
    {
        return new SBool(File.Exists(RealPath));
    }

    private IValue SizeFunc(IEnumerable<IValue> argCollection)
    {
        return new Number(new FileInfo(RealPath).Length);
    }

    private IValue AppendFunc(IEnumerable<IValue> argCollection)
    {
        StreamWriter f = new FileInfo(RealPath).AppendText();
        f.Write(argCollection.ToArray()[0].ToSString().Val);
        f.Flush();
        return new SNull();
    }

    private IValue ReadToEndFunc(IEnumerable<IValue> argCollection)
    {
        if (!File.Exists(RealPath)) throw new Exception("No file");
        return new BString(File.ReadAllBytes(RealPath));
    }

    private IValue DeleteFunc(IEnumerable<IValue> argCollection)
    {
        File.Delete(RealPath);
        return new SNull();
    }

    private IValue ReadFunc(IEnumerable<IValue> argCollection)
    {
        long[] args = argCollection.Select(y => y.ToNumber().ToInteger()).ToArray();
        FileStream fs = new FileInfo(RealPath).OpenRead();
        fs.Seek(args[1], SeekOrigin.Begin);

        byte[] buffer = new byte[args[0]];
        fs.Read(buffer, 0, (int) args[0]);

        return new BString(buffer);
    }

    public override bool ToBool() => true;

    public string GetProgramPath()
    {
        IPathFinder pathFinder;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            pathFinder = new WindowsPathFinder();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            pathFinder = new UnixPathFinder();
        else
            throw new Exception("Unsupported platform");

        var oldCwd = Environment.CurrentDirectory;
        Environment.CurrentDirectory = _cwd;
        var path = pathFinder.GetAbsolutePath(_path);
        Environment.CurrentDirectory = oldCwd;
        
        return path;
    }
    
    public override SString ToSString() => new SString($"(FilePath \"{_path}\" relative to \"{_cwd}\")");

    public override ITable ToTable() => _table;

    public override SFile ToSFile() => this;
    
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }
}
