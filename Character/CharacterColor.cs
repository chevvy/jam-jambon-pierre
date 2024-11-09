using Godot;
using System.Collections.Generic;

public static class CharacterColor
{
    public static Dictionary<int, Color> CharColor = new Dictionary<int, Color>
    {
        { 0, Color.Color8(255, 0, 0, 255) },
        { 1, Color.Color8(50, 200, 0, 255) },
        { 2, Color.Color8(0, 0, 255, 255) },
        { 3, Color.Color8(255, 255, 0, 255) }
    };
}
