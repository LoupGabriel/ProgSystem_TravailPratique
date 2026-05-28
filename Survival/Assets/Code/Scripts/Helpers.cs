
using UnityEngine;
using UnityEngine.Windows;

public static class Helpers 
{
    private static Matrix4x4 isometricMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    //transform the local input to isometric matrix 
    public static Vector3 ToIso(this Vector3 input) => isometricMatrix.MultiplyPoint3x4(input);
}
