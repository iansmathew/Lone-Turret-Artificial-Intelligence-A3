using UnityEngine;
[System.Serializable]

public static class CustomFunc {


    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < 90f || angle > 270f)
        {
            if (angle > 180)
                angle -= 360f;
            if (max > 180)
                max -= 360f;
            if (min > 180)
                min -= 360f;
        }

        angle = Mathf.Clamp(angle, min, max);

        if (angle < 0)
            angle += 360f;

        return angle;
    }


}
