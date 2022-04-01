﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class SString : BaseValue, ITable, IKeyable
{
    private string _val;
    private NativeTable _nativeTable;
    
    public SString(string str)
        : base("string")
    {
        _val = str;
        _nativeTable = new NativeTable();
        
        _nativeTable.SetValue("length", new NativeFunc(lengthCallHandler, 0));
        _nativeTable.SetValue("substring", new NativeFunc(SubStringFunc, 2));
    }

    private IValue SubStringFunc(IEnumerable<IValue> argCollection)
    {
        Number[] args = argCollection.ToArray().Select(x => x.ToNumber()).ToArray();
        return new SString(Val.Substring((int) args[0].ToInteger(), (int) args[1].ToInteger()));
    }

    private IValue lengthCallHandler(IEnumerable<IValue> args)
    {
        return new Number(this._val.Length);
    }

    public override bool ToBool() => true;
    public override Number ToNumber() => new Number(int.Parse(_val));
    public override SString ToSString() => this;
    public override ITable ToTable() => this;
    public override bool IsEqual(IValue other)
    {
        if (other is SString otherString)
        {
            return _val == otherString._val;
        }

        return false;
    }

    public RefValue GetValue(IKeyable key)
    {
        if (key is Number numberKey)
        {
            if (numberKey.IsInteger)
            {
                var val = numberKey.ToInteger();
                if (val >= 0 && val < _val.Length)
                {
                    //val is less than _val.Length which is an int, therefore val can safely be casted to int
                    return new RefValue(new SString(new string(_val[(int) val], 1)));
                }
            }
        }
        return _nativeTable.GetValue(key);
    }

    public void RemoveValue(IKeyable key)
    {
        return;
    }

    public override string ToString()
    {
        return _val;
    }

    public string Val => _val;
    public string KeyValue => _val;
    public string UniquePrefix => "S";
}