using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSolidController : MonoBehaviour
{
    bool hide = false;
    float spritesAlpha = 1;
   
    public List<Collider> colliders;
    public List<SpriteRenderer> sprites;

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            HideSolid();
        }

        if (hide)
        {
            if (spritesAlpha > 0)
            {
                spritesAlpha -= Time.deltaTime;

                foreach (SpriteRenderer spr in sprites)
                {
                    spr.color = new Color(spr.color.r, spr.color.b, spr.color.g, spritesAlpha);
                }
            }
        }
    }

    public void HideSolid()
    {
        foreach(Collider coll in colliders)
            coll.enabled = false;

        foreach (SpriteRenderer spr in sprites)
        {
            hide = true;
        }
    }
}
