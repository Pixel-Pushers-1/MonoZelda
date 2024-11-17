using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Save
{
    public interface ISaveable
    {
        void Save(SaveState save);
        void Load(SaveState save);
    }
}
