using System;
using UnityEngine;

public class AnimatedSprite2
{
    private Sprite[] sprites;
    private float framerate;
    private int frame;
    private Action<Sprite> setSpriteCallback;
    private float timeBetweenFrames;
    private float lastFrameTime;

    public bool isAnimating { get; set; }  // Control whether the animation is active

    public AnimatedSprite2(Sprite[] sprites, float framerate, Action<Sprite> setSpriteCallback)
    {
        this.sprites = sprites ?? throw new ArgumentNullException(nameof(sprites));
        this.framerate = framerate;
        this.setSpriteCallback = setSpriteCallback ?? throw new ArgumentNullException(nameof(setSpriteCallback));

        timeBetweenFrames = 1f / framerate;
        isAnimating = false;  // By default, animation is not playing
    }

public void Update(float deltaTime)
    {
        if (!isAnimating) return;

        lastFrameTime += deltaTime;

        if (lastFrameTime >= timeBetweenFrames)
        {
            lastFrameTime = 0f;
            frame++;

            if (frame >= sprites.Length)
            {
                frame = 0;
            }

            setSpriteCallback(sprites[frame]);
        }
    }
}
