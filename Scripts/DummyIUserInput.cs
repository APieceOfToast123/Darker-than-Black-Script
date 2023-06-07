using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyIUserInput : IUserInput{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            // Dup = 1.0f;
            // Dright = 0;
            // Jup = 0;
            // Jright = 1.0f;
            // run = true;
            // yield return new WaitForSeconds(1.0f);
            mlA = true;
            yield return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDmagDvec(Dup, Dright);
    }

    // public IEnumerator SetRun(){
    //     while (true)
    //     {
    //         run = true;
    //         yield return 0;
    //     }
    // }

    // public IEnumerator SetAttack(){
    //     while (true)
    //     {
    //         // run = true;
    //         mlA = true;
    //         yield return 0;
    //     }
    // }
}
