using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public static class MotionUtil {
    private static float pointToPointSmoothlyThresfold = 0.1f;

    //=============================================================
    /// <summary>
    /// 点から点に移動する
    /// </summary>
    public static IEnumerator PointToPoint (Vector3 from,Vector3 to,float timeLength) {
        Vector3 currentPos = Vector3.zero;

        float time = 0;
        //一定の時間が経過したら
        while(time < 1) {
            time += Time.fixedDeltaTime / timeLength;
            currentPos = Vector3.Lerp(from,to,time);

            yield return currentPos;
        }

        yield return to;
    }

    //=============================================================
    /// <summary>
    /// 点から点に移動する(滑らかに)
    /// </summary>
    public static IEnumerator PointToPointSmoothly (Vector3 from,Vector3 to,float speed) {
        Vector3 currentPos = from;

        //定めたゴールに近づいたら
        while(Vector3.SqrMagnitude(currentPos - to) > pointToPointSmoothlyThresfold) {
            currentPos = Vector3.Lerp(currentPos,to,speed);

            yield return currentPos;
        }

        yield return to;
    }
}