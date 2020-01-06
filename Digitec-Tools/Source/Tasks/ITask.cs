using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Digitec_Tools.Source.Tasks
{
    public interface ITask
    {
        public void StartTask();
        public void Abort();
    }
}
