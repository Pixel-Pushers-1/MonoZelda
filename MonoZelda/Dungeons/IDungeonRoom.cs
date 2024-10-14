using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons
{
    public interface IDungeonRoom
    {
        public List<IDoor> GetDoors();
    }
}
