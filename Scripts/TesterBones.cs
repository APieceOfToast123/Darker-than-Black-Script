using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterBones : MonoBehaviour
{
    public SkinnedMeshRenderer srcMeshRenderer;
    public SkinnedMeshRenderer tgtMeshRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        print(srcMeshRenderer.bones.Length);
        print(tgtMeshRenderer.bones.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
