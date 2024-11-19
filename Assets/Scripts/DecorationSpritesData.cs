using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationScriptable")]
public class DecorationSpritesData : ScriptableObject
{
    public List<Sprite> Sprites = new List<Sprite>();
}
