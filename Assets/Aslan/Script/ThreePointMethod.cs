using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePointMethod : MonoBehaviour
{
    public static Point Main(double D1, double D2, double D3)
    {

        Point p1 = new Point() { X = 0, Y = 2, Distance = D1 };
        Point p2 = new Point() { X = 2, Y = 2, Distance = D2 };
        Point p3 = new Point() { X = 1, Y = 0, Distance = D3 };
        var p = GetPiontByThree(p1, p2, p3);
        Debug.Log("Point x:{0} " + p.X);
        Debug.Log("Point y:{0} " + p.Y);

        return GetPiontByThree(p1, p2, p3);
    }

    /// <summary>
    /// 三点绝对定位
    /// </summary>
    private static Point GetPiontByThree(Point p1, Point p2, Point p3)
    {
        /* Mathf.Pow(y1-Y)+Mathf.Pow(X-x1)=Mathf.Pow(D1)
         * Mathf.Pow(y2-Y)+Mathf.Pow(X-x2)=Mathf.Pow(D2)
         * Mathf.Pow(y3-Y)+Mathf.Pow(X-x3)=Mathf.Pow(D3)
         * 1-3.2-3解得：
         * 2 * (p1.X - p3.X)x + 2 * (p1.Y - p3.Y)y = Mathf.Pow(p1.X, 2) - Mathf.Pow(p3.X, 2) + Mathf.Pow(p1.Y, 2) - Mathf.Pow(p3.Y, 2) + Mathf.Pow(p3.Distance, 2) - Mathf.Pow(p1.Distance, 2);
         * 2 * (p2.X - p3.X)x + 2 * (p2.Y - p3.Y)y = Mathf.Pow(p2.X, 2) - Mathf.Pow(p3.X, 2) + Mathf.Pow(p2.Y, 2) - Mathf.Pow(p3.Y, 2) + Mathf.Pow(p3.Distance, 2) - Mathf.Pow(p2.Distance, 2);
         * 简化：
         * 2Ax+2By=C
         * 2Dx+2Ey=F
         * 简化：
         * x=(BF-EC)/(2BD-2AE)
         * y=(AF-DC)/(2AE-2BD)
         */
        var A = p1.X - p3.X;
        var B = p1.Y - p3.Y;
        var C = Mathf.Pow(p1.X, 2) - Mathf.Pow(p3.X, 2) + Mathf.Pow(p1.Y, 2) - Mathf.Pow(p3.Y, 2) + Mathf.Pow((float)p3.Distance, 2) - Mathf.Pow((float)p1.Distance, 2);
        var D = p2.X - p3.X;
        var E = p2.Y - p3.Y;
        var F = Mathf.Pow(p2.X, 2) - Mathf.Pow(p3.X, 2) + Mathf.Pow(p2.Y, 2) - Mathf.Pow(p3.Y, 2) + Mathf.Pow((float)p3.Distance, 2) - Mathf.Pow((float)p2.Distance, 2);

        var x = (B * F - E * C) / (2 * B * D - 2 * A * E);
        var y = (A * F - D * C) / (2 * A * E - 2 * B * D);

        Point P = new Point() { X = x, Y = y, Distance = 0 };
        return P;
    }
}

public class Point
{
    public float X { get; set; }
    public float Y { get; set; }
    //表示指定点，据此点的距离
    public double Distance { get; set; }
}

