using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Threading.Tasks;

namespace ShaellLang;

public class SProcess : BaseValue, IFunction
{
    public Process Process = new Process();
    public SProcess LeftProcess;
    public string Stdin = null;
    public bool Executed { get; private set; }
    public SProcess(string file) 
        : base("process")
    {
        Process.StartInfo.FileName = file;
    }

    public void AddArguments(IEnumerable<IValue> args)
    {
        foreach (var arg in args)
        {
            AddArg(arg.ToSString().Val);
        }
    }

    public void AddPipeProcess(SProcess process)
    {
        LeftProcess = process;
    }

    private void AddArg(string str) => Process.StartInfo.ArgumentList.Add(str);
    public void Dispose() => Process.Dispose();

    private JobObject Run(Process process, string stdin)
    {
        if (!Executed)
        {
            Executed = true;
            return JobObject.Factory.StartProcess(process, stdin);
        }

        return null;
    }

    public IValue Call(IEnumerable<IValue> args)
    {
        AddArguments(args);
        return Run(Process, Stdin);
    }

    public IValue Call()
    {
        return Run(Process, Stdin);
    }

    public IValue Pipe(SProcess parentProc)
    {
        JobObject jo = null;
        if (LeftProcess?.Executed != true)
        {
            jo = LeftProcess?.Pipe(this).ToJobObject();
        }

        jo = Run(Process, jo?.ToString()).ToJobObject();
        
        if(parentProc is not null)
            parentProc.Stdin = jo?.ToString();
        Console.WriteLine(jo);
        return jo;

    }
    
    public override IFunction ToFunction() => this;
    public override bool IsEqual(IValue other)
    {
        return other == this;
    }

    public uint ArgumentCount => (uint)Process.StartInfo.ArgumentList.Count;
}