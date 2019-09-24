using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class GeographicCoords
{
    public static (float lat, float lon) ToGeographic(Vector3 vec)
    {
        vec = vec.normalized;
        return (Mathf.Asin(vec.z / 1), Mathf.Atan2(vec.y, vec.x));
    }

    public static (float lat, float lon) ToGeographicDegrees(Vector3 vec)
    {
        var (lat, lon) = ToGeographic(vec);
        return (lat * Mathf.Rad2Deg, lon * Mathf.Rad2Deg);
    }

    public static Vector3 ToVector3(float lat, float lon)
    {
        lat = Mathf.Clamp(lat, -90, 90);

        lon %= 360;
        if (lon > 180) lon = 360 - lon;
        else if (lon < -180) lon = 360 + lon;

        var x = Mathf.Cos(lat) * Mathf.Cos(lon);
        var y = Mathf.Cos(lat) * Mathf.Sin(lon);
        var z = Mathf.Sin(lat);
        return new Vector3(x, y, z);
    }

    public static Vector3 ToVector3Degrees(float latDegrees, float lonDegrees)
    {
        latDegrees *= Mathf.Deg2Rad;
        lonDegrees *= Mathf.Deg2Rad;
        return ToVector3(latDegrees, lonDegrees);
    }
}
