using System;
using System.Collections.Generic;
using System.Text;

namespace BackupsExtra.Lib.Serializable
{
    [Serializable]
    public class RestorePointModel
    {
        public int Number { get; set; }
        public DateTime DateTime { get; set; }
        public StorageModel Storage { get; set; }
    }
}
