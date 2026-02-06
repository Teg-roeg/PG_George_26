using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshTrail : MonoBehaviour
{
    [Header("Trail Timing")]
    public float meshRefreshRate = 0.1f;

    [Header("Mesh")]
    public Transform positionToSpawn;

    [Header("Shader")]
    public Material mat;
    public string shaderVarRef = "_Alpha";
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private GameObject[] trailPool;
    private int poolIndex;

    private Coroutine trailCoroutine;

    void Start()
    {
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        CreateTrailPool();
    }

    void Update()
    {
        bool isMoving =
            Input.GetKey(KeyCode.LeftShift);

        if (isMoving && trailCoroutine == null)
        {
            trailCoroutine = StartCoroutine(TrailLoop());
        }
        else if (!isMoving && trailCoroutine != null)
        {
            StopCoroutine(trailCoroutine);
            trailCoroutine = null;
        }
    }

    void CreateTrailPool()
    {
        int poolSize = skinnedMeshRenderers.Length * 10;
        trailPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject gObj = new GameObject("TrailMesh");
            gObj.SetActive(false);

            gObj.AddComponent<MeshFilter>();
            gObj.AddComponent<MeshRenderer>();

            trailPool[i] = gObj;
        }
    }

    IEnumerator TrailLoop()
    {
        while (true)
        {
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = trailPool[poolIndex];
                poolIndex = (poolIndex + 1) % trailPool.Length;

                gObj.SetActive(true);
                gObj.transform.SetPositionAndRotation(
                    positionToSpawn.position,
                    positionToSpawn.rotation
                );

                MeshFilter mf = gObj.GetComponent<MeshFilter>();
                MeshRenderer mr = gObj.GetComponent<MeshRenderer>();

                // 🔹 Disable shadows
                mr.shadowCastingMode = ShadowCastingMode.Off;
                mr.receiveShadows = false;

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;

                Material trailMat = new Material(mat);
                mr.material = trailMat;

                StartCoroutine(AnimateMaterialFloat(trailMat));
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }
    }

    IEnumerator AnimateMaterialFloat(Material material)
    {
        float value = material.GetFloat(shaderVarRef);

        while (value > 0)
        {
            value -= shaderVarRate;
            material.SetFloat(shaderVarRef, value);
            yield return new WaitForSeconds(shaderVarRefreshRate);
        }
    }
}
