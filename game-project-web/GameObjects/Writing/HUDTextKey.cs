﻿using game_project.ECS.Components;
using Microsoft.Xna.Framework;

namespace game_project.GameObjects.Writing
{
    public class HUDTextKey : TextEntity
    {
        public HUDTextKey(Vector2 pos, string stringToWrite, Color? color = null, float scale = 1f, bool animate = false)
        {
            GetComponent<Transform>().position = pos;
            Constants.SetLayerDepth(this, Constants.LayerDepth.Text);

            Text text = new Text(stringToWrite.ToUpper(), animate);
            text.scale = scale;
            if (color == null)
            {
                text.color = Color.White;
            }
            else
            {
                text.color = (Color)color;
            }
            this.AddComponent(text);

            BehaviorScript script = new HUDKeyManager();
            this.AddComponent(script);
        }

    }
}
