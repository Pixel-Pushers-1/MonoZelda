using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons
{
    internal class ExampleDungeonRoom : IDungeonRoom
    {
        private List<IDoor> doors;

        public ExampleDungeonRoom(List<IDoor> doors)
        {
            this.doors = doors;
        }

        public List<IDoor> GetDoors()
        {
            return doors;
        }
    }
}
