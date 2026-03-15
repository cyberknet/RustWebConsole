using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RustRcon.Entities
{
    public class ModFrameworkVersion : EntityBase
    {
        // Carbon 2.0.213.0/linux/2025.10.02.0 [production] [production_build] on Rust 935/2602.272.1 (10/07/2025 09:21:09)
        private static string CarbonPattern 
                            = @"^Carbon\s(?<carbonVersion>[\d\.]+)\/" 
                            + @"(?<os>[^\/]+)\/"
                            + @"(?<buildDate>[\d\.]+)\s\"
                            + @"[(?<branch>[^\]]+)\]\s"
                            + @"\[(?<buildType>[^\]]+)\]"
                            +@"\son\sRust\s"
                            +@"(?<rustVersion>[\d\.]+)\/"
                            +@"(?<rustBuild>[\d\.]+)\s"
                            +@"\((?<rustBuildDate>[^\)]+)\)$";
        public string ModFramework { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string BuildDate { get; set; } = string.Empty;

        public string Branch { get; set; } = string.Empty;
        public string BuildType { get; set; } = string.Empty;
        public string RustVersion { get; set; } = string.Empty;
        public string RustBuild { get; set; } = string.Empty;
        public string RustBuildDate { get; set; } = string.Empty;
    }
}
