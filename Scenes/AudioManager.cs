using Godot;
using System;

public partial class AudioManager : Node
{
    public static AudioManager Instance { get; set; }
    [Export] private AudioStreamPlayer _pluck;
    [Export] private AudioStreamPlayer _start;
    [Export] private AudioStreamPlayer _end;

    public override void _Ready()
    {
        Instance = this;
    }

    public void PlayPluck()
    {
        if (_pluck.IsPlaying()) return;
        _pluck.Play();
    }

    public void PlayStart()
    {
        
        _start.Play();
    }

    public void PlayEnd()
    {
        _end.Play();
    }
    
    
}
