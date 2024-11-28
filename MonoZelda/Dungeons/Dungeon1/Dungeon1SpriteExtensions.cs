using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Dungeon1;

internal static class Dungeon1SpriteExtensions
{
    public static Dungeon1Sprite ToOpened(this Dungeon1Sprite sprite)
    {
        return sprite switch
        {
            Dungeon1Sprite.bombable_wall_north => Dungeon1Sprite.wall_bombed_north,
            Dungeon1Sprite.bombable_wall_east => Dungeon1Sprite.wall_bombed_east,
            Dungeon1Sprite.bombable_wall_south => Dungeon1Sprite.wall_bombed_south,
            Dungeon1Sprite.bombable_wall_west => Dungeon1Sprite.wall_bombed_west,
            Dungeon1Sprite.door_closed_north => Dungeon1Sprite.door_open_north,
            Dungeon1Sprite.door_closed_east => Dungeon1Sprite.door_open_east,
            Dungeon1Sprite.door_closed_south => Dungeon1Sprite.door_open_south,
            Dungeon1Sprite.door_closed_west => Dungeon1Sprite.door_open_west,
            Dungeon1Sprite.diamond_door_north => Dungeon1Sprite.door_open_north,
            Dungeon1Sprite.diamond_door_east => Dungeon1Sprite.door_open_east,
            Dungeon1Sprite.diamond_door_south => Dungeon1Sprite.door_open_south,
            Dungeon1Sprite.diamond_door_west => Dungeon1Sprite.door_open_west,
            Dungeon1Sprite.door_locked_north => Dungeon1Sprite.door_open_north,
            Dungeon1Sprite.door_locked_east => Dungeon1Sprite.door_open_east,
            Dungeon1Sprite.door_locked_south => Dungeon1Sprite.door_open_south,
            Dungeon1Sprite.door_locked_west => Dungeon1Sprite.door_open_west,
            _ => sprite
        };
    }
}
