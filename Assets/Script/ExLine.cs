using UnityEngine;

public class ExLine : MonoBehaviour
{
    [SerializeField]
    [Range(3, 100)]
    private int polygonPoints = 3;
    [SerializeField]
    [Min(0.1f)]
    float radius = 3;
    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.loop = true;
    }

    private void Update()
    {
        Play1();
    }

    void Play()
    {
        lineRenderer.positionCount = polygonPoints;

        float anglePerStep = 2 * Mathf.PI * ((float)1 / polygonPoints);

        for (int i = 0; i < polygonPoints; ++i)
        {
            Vector2 point = Vector2.zero;
            float angle = anglePerStep * i;

            point.x = Mathf.Cos(angle) * radius;
            point.y = Mathf.Sin(angle) * radius / 2;

            lineRenderer.SetPosition(i, point);
        }
    }

    void Play1()
    {
        lineRenderer.positionCount = polygonPoints;

        float anglePerStep = 2 * Mathf.PI * ((float)1 / polygonPoints);
        float rotationAngle = 20f * Mathf.Deg2Rad;

        for (int i = 0; i < polygonPoints; ++i)
        {
            float angle = anglePerStep * i;

            // 타원 좌표 계산 (수직 타원)
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * (radius / 2f);

            // 45도 회전 변환 적용
            float rotatedX = x * Mathf.Cos(rotationAngle) - y * Mathf.Sin(rotationAngle);
            float rotatedY = x * Mathf.Sin(rotationAngle) + y * Mathf.Cos(rotationAngle);

            // 최종 위치 설정
            Vector3 point = new Vector3(rotatedX, rotatedY, 0f);
            lineRenderer.SetPosition(i, point);
        }
    }
}
