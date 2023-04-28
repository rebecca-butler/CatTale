using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Every component that saves data must implement this interface

public interface ISavable
{
    object CaptureState();
    void RestoreState(object state);
}
