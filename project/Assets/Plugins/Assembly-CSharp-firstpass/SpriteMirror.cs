using UnityEngine;

public class SpriteMirror : SpriteRootMirror
{
	public Vector2 lowerLeftPixel;

	public Vector2 pixelDimensions;

	public override void Mirror(SpriteRoot s)
	{
		base.Mirror(s);
		lowerLeftPixel = ((Sprite)s).lowerLeftPixel;
		pixelDimensions = ((Sprite)s).pixelDimensions;
	}

	public override bool DidChange(SpriteRoot s)
	{
		if (base.DidChange(s))
		{
			return true;
		}
		if (((Sprite)s).lowerLeftPixel != lowerLeftPixel)
		{
			return true;
		}
		if (((Sprite)s).pixelDimensions != pixelDimensions)
		{
			return true;
		}
		return false;
	}
}
