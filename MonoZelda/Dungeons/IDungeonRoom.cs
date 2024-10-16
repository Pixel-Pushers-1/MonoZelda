using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons
{
    public interface IDungeonRoom
    {
        string RoomName { get; }
        Dungeon1Sprite RoomSprite { get; }
        List<IDoor> GetDoors();
        List<Rectangle> GetStaticColliders();
        List<EnemySpawn> GetEnemySpawns();
        List<ItemSpawn> GetItemSpawns();
    }
}
