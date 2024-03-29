﻿using System.Collections.Generic;

namespace Akces.Unity.DataAccess
{
    public class NexoOperationResult
    {
        public string ObjectName { get; set; }
        public bool IsSuccess { get; set; }
        public bool NoChangesMade { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Warrnings { get; set; }
        public List<string> Infos { get; set; }

        public NexoOperationResult()
        {
            Errors = new List<string>();
            Warrnings = new List<string>();
            Infos = new List<string>();
        }
    }
}
