using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ShaellLang;

public class SProcess : BaseValue, IFunction
{
    private Process _process = new Process();
    public SProcess(string file) 
        : base("process")
    {
        IPathFinder pathFinder;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            pathFinder = new WindowsPathFinder();
            _process.StartInfo.FileName = pathFinder.GetAbsolutePath(file);
        } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            pathFinder = new UnixPathFinder();
            _process.StartInfo.FileName = pathFinder.GetAbsolutePath(file);
        }
    }

    private void AddArguments(IEnumerable<IValue> args)
    {
        foreach (var arg in args)
        {
            AddArg(arg.ToSString().Val);
        }
    }

    private void AddArg(string str) => _process.StartInfo.ArgumentList.Add(str);
    public void Dispose() => _process.Dispose();

    private JobObject Run(Process process)
    {
        return JobObject.Factory.StartProcess(process);
    }

    public IValue Call(IEnumerable<IValue> args)
    {
        AddArguments(args);
        return Run(_process);
    }
    
    public override IFunction ToFunction() => this;
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }

    public uint ArgumentCount { get; }
}