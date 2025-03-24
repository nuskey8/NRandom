using RandomExtensions;
using RandomExtensions.Unity;
using UnityEngine;

public class Sandbox : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0; i < 2500; i++)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = RandomEx.Shared.NextVector3InsideSphere() * 10;
            obj.transform.rotation = RandomEx.Shared.NextQuaternionRotation();

            obj.GetComponent<MeshRenderer>().material.color = RandomEx.Shared.NextColor(new Color(0f, 0f, 0f), new Color(1f, 1f, 1f));
        }
    }
}
