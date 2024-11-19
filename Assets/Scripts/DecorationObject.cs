using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationObject : MonoBehaviour
{
    [SerializeField] DecorationSpritesData spritesData;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = spritesData.Sprites[Random.Range(0, spritesData.Sprites.Count)];
    }
}
