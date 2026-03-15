using RustRcon.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RustRcon.EventArgs;

public abstract class ResultEventArgs<TEntity> : System.EventArgs
    where TEntity : EntityBase
{
    public TEntity Result {get; set; }
    public ResultEventArgs(TEntity result)
    {
        Result = result;
    }
}
