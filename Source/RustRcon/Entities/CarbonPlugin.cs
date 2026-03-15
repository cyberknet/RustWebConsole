using System;
using System.Collections.Generic;
using System.Text;

namespace RustRcon.Entities;

public class CarbonPlugin : EntityBase 
{
    public string Number { get; set; }
    public string Package { get; set; }
    public string Author { get; set; }
    public string Version { get; set; }
    public string HookTime { get; set; }
    public string HookFires { get; set; }
    public string HookMemory { get; set; }
    public string HookLag { get; set; }
    public string HookExceptions { get; set; }
    public string CompileTime { get; set; }
    public string Uptime { get; set; }
}
