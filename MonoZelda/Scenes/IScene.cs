﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoZelda.Scenes;

public interface IScene
{
    void StopSound();
    void Update(GameTime gameTime);
    void LoadContent(ContentManager contentManager);
}
