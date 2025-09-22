using System;
using UnityEngine;

public static class MathUtils
{
    // Based on math from: https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/
    public static float PowDamp(float t, float dt)
    {
        return 1.0f - Mathf.Pow(1.0f - t, dt);
    }

    public static float ExpDamp(float lambda, float dt)
    {
        return 1.0f - Mathf.Exp(-lambda * dt);
    }

    // Works with negative numbers
    public static int Mod(int value, int modulo)
    {
        int remainder = value % modulo;
        return remainder < 0 ? remainder + modulo : remainder;
    }
}