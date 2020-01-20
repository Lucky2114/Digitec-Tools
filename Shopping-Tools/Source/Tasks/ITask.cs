using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shopping_Tools.Source.Tasks
{
    public interface ITask
    {
        public Task Task { get; set; }
        public void StartTask();
        public void Abort();
    }
}
