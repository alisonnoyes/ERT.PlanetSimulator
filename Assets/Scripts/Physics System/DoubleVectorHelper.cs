using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DoubleVectorHelper : MonoBehaviour
{
    public static double Magnitude(double[] vec)
    {
        return Math.Sqrt(Math.Pow(vec[0], 2) + Math.Pow(vec[1], 2) + Math.Pow(vec[2], 2));
    }

    public static double[] Normalized(double[] vec)
    {
        double mag = Magnitude(vec);
        double[] norm = { vec[0] / mag, vec[1] / mag, vec[2] / mag };

        return norm;
    }

    public static Vector3 ToVector3(double[] vec)
    {
        return new Vector3((float)vec[0], (float)vec[1], (float)vec[2]);
    }

    public static double Dot(double[] vec1, double[] vec2)
    {
        return vec1[0] * vec2[0] + vec1[1] * vec2[1] + vec1[2] * vec2[2];
    }

    public static double[] FloatToDoubleVector(Vector3 vec)
    {
        double[] array = { vec.x, vec.y, vec.z };
        return array;
    }
}
