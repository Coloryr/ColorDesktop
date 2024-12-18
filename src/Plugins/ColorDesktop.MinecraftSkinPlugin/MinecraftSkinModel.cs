﻿using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ColorDesktop.MinecraftSkinPlugin;

/// <summary>
/// 移动方式
/// </summary>
public enum MoveType
{
    LeftUp, Up, RightUp,
    Left, Right,
    LeftDown, Down, RightDown
}

public partial class MinecraftSkinModel : ObservableObject
{
    public const string RotateName = "Rotate";
    public const string ScollName = "Scoll";
    public const string PosName = "Pos";
    public const string RotName = "Rot";
    public const string LoadName = "Load";
    public const string ResetName = "Reset";

    [ObservableProperty]
    private int _rotateType;

    [ObservableProperty]
    private string text;
    [ObservableProperty]
    private string _nowFps;

    [ObservableProperty]
    private bool _haveSkin;
    [ObservableProperty]
    private bool _enableCape = true;
    [ObservableProperty]
    private bool _enableTop = true;
    [ObservableProperty]
    private bool _enableZ;
    [ObservableProperty]
    private bool _enableMSAA = true;
    [ObservableProperty]
    private bool _displayButton;

    [ObservableProperty]
    private float _rotateX;
    [ObservableProperty]
    private float _rotateY;
    [ObservableProperty]
    private float _rotateZ;

    [ObservableProperty]
    private float _minX;
    [ObservableProperty]
    private float _minY = -360;
    [ObservableProperty]
    private float _minZ;

    public Vector3 ArmRotate;
    public Vector3 LegRotate;
    public Vector3 HeadRotate;

    public float X;
    public float Y;

    private bool _load;

    public int Fps
    {
        set
        {
            NowFps = $"{value}Fps";
        }
    }

    partial void OnRotateXChanged(float value)
    {
        switch (RotateType)
        {
            case 0:
                ArmRotate.X = value;
                break;
            case 1:
                LegRotate.X = value;
                break;
            case 2:
                HeadRotate.X = value;
                break;
            default:
                return;
        }

        OnPropertyChanged(RotateName);
    }

    partial void OnRotateYChanged(float value)
    {
        switch (RotateType)
        {
            case 0:
                ArmRotate.Y = value;
                break;
            case 1:
                LegRotate.Y = value;
                break;
            case 2:
                HeadRotate.Y = value;
                break;
            default:
                return;
        }

        OnPropertyChanged(RotateName);
    }

    partial void OnRotateZChanged(float value)
    {
        switch (RotateType)
        {
            case 0:
                ArmRotate.Z = value;
                break;
            case 1:
                LegRotate.Z = value;
                break;
            case 2:
                HeadRotate.Z = value;
                break;
            default:
                return;
        }

        OnPropertyChanged(RotateName);
    }

    partial void OnRotateTypeChanged(int value)
    {
        Vector3 rotate;
        switch (RotateType)
        {
            case 0:
                rotate = ArmRotate;
                MinX = 0;
                MinY = -360;
                MinZ = 0;
                EnableZ = false;
                break;
            case 1:
                rotate = LegRotate;
                MinX = 0;
                MinY = -360;
                MinZ = 0;
                EnableZ = false;
                break;
            case 2:
                rotate = HeadRotate;
                MinX = -360;
                MinY = -360;
                MinZ = -360;
                EnableZ = true;
                break;
            default:
                return;
        }

        RotateX = rotate.X;
        RotateY = rotate.Y;
        RotateZ = rotate.Z;
    }

    [RelayCommand]
    public void Move(object comm)
    {
        switch (comm)
        {
            case MoveType.LeftUp:
                X = -0.05f;
                Y = 0.05f;
                break;
            case MoveType.Up:
                X = 0;
                Y = 0.05f;
                break;
            case MoveType.RightUp:
                X = 0.05f;
                Y = 0.05f;
                break;
            case MoveType.Left:
                X = -0.05f;
                Y = 0;
                break;
            case MoveType.Right:
                X = 0.05f;
                Y = 0;
                break;
            case MoveType.LeftDown:
                X = -0.05f;
                Y = -0.05f;
                break;
            case MoveType.Down:
                X = 0;
                Y = -0.05f;
                break;
            case MoveType.RightDown:
                X = 0.05f;
                Y = -0.05f;
                break;
        }
        OnPropertyChanged(PosName);
    }

    [RelayCommand]
    public void Rot(object comm)
    {
        switch (comm)
        {
            case MoveType.LeftUp:
                X = -10;
                Y = -10;
                break;
            case MoveType.Up:
                X = -10;
                Y = 0;
                break;
            case MoveType.RightUp:
                X = -10;
                Y = 10;
                break;
            case MoveType.Left:
                X = 0;
                Y = -10;
                break;
            case MoveType.Right:
                X = 0;
                Y = 10;
                break;
            case MoveType.LeftDown:
                X = 10;
                Y = -10;
                break;
            case MoveType.Down:
                X = 10;
                Y = 0;
                break;
            case MoveType.RightDown:
                X = 10;
                Y = 10;
                break;
        }
        OnPropertyChanged("Rot");
    }

    [RelayCommand]
    public void Scoll(object comm)
    {
        switch (comm)
        {
            case MoveType.Up:
                X = 0.05f;
                break;
            case MoveType.Down:
                X = -0.05f;
                break;
        }

        OnPropertyChanged(ScollName);
    }

    [RelayCommand]
    public void Reset()
    {
        switch (RotateType)
        {
            case 0:
                ArmRotate.X = 0;
                ArmRotate.Y = 0;
                ArmRotate.Z = 0;
                break;
            case 1:
                LegRotate.X = 0;
                LegRotate.Y = 0;
                LegRotate.Z = 0;
                break;
            case 2:
                HeadRotate.X = 0;
                HeadRotate.Y = 0;
                HeadRotate.Z = 0;
                break;
            default:
                return;
        }
        RotateX = 0;
        RotateY = 0;
        RotateZ = 0;

        OnPropertyChanged(RotateName);
    }

    [RelayCommand]
    public void ResetPos()
    {
        OnPropertyChanged(ResetName);
    }

    [RelayCommand]
    public void Reload()
    {
        OnPropertyChanged(LoadName);
    }
}
