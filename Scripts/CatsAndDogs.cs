﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main;

public partial class Cats : Target
{
    public Cats() 
    {
        List<string> allCats = DirAccess.GetFilesAt("res://Media/Cats/").ToList();
        for (int i = 0; i < allCats.Count; i++)
        {
            if (!allCats[i].EndsWith(".import"))
            {
                allCats.RemoveAt(i);
                i--;
            }
            else
            {
                allCats[i] = allCats[i].Replace(".import", string.Empty);
            }
        }

        Texture = GD.Load<Texture2D>("res://Media/Cats/" + allCats[RndGen.Next(allCats.Count)]);
    }
}

public partial class Dogs : Target
{
    public bool IsSelfDistract;
    public Dogs()
    {
        clickedSound = "res://Media/Audio/SFX/ClickedDog.mp3";

        List<string> allDogs = DirAccess.GetFilesAt("res://Media/Dogs/").ToList();
        for (int i = 0; i < allDogs.Count; i++)
        {
            if (!allDogs[i].EndsWith(".import"))
            {
                allDogs.RemoveAt(i);
                i--;
            }
            else
            {
                allDogs[i] = allDogs[i].Replace(".import", string.Empty);
            }
        }

        Texture = GD.Load<Texture2D>("res://Media/Dogs/" + allDogs[RndGen.Next(allDogs.Count)]);
    }
    public override void _Ready()
    {
        base._Ready();
        Timer temp = new()
        {
            WaitTime = RndGen.NextSingle() * 20,
            Autostart = true
        };
        temp.Timeout += TimerOut;
        AddChild(temp);
    }

    private void TimerOut()
    {
        if (IsFalling) return;
        IsSelfDistract = true;
        Clicked();
    }
}
