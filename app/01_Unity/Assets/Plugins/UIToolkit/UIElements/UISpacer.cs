using UnityEngine;
using System.Collections;



public class UISpacer : UISprite
{
    public override bool hidden
    {
        get { return ___hidden; }
        set {}
    }
	
	
	public override void updateTransform()
	{}
	
	
	public UISpacer( int width, int height )
	{
		manager = UI.firstToolkit;
		
		_width = width;
		_height = height;
	}
	
	// HACK: Gabo edit, overriding the destroy method of the spacer since it was 
	// deleting sprites. Problem with the original method is that it looks for the sprite.index
	// and removes it from the internal arrays. Spacers don't have one so that's why it's deleting
	// things.
	public override void destroy()
	{
		manager.removeElement( this );
	}
}
