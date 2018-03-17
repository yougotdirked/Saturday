using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop_Generator : Generation {

    public float radius;

    public override void newChild(Vector3 inputLoc, int c)
    {
        if (c > 0)
        {
            //create child
            GameObject iteration = GameObject.Instantiate(child);

            //move child
            iteration.transform.Translate(Vector3.up * radius);
            iteration.transform.Rotate(new Vector3(0, 0, 20));
            iteration.transform.Translate(Vector3.down * radius);
            iteration.transform.Translate(Vector3.forward * .8f);

            //put child at offset position, reduce its complexity and make sure it generates
            iteration.GetComponent<BuildingBlock>().isGenerating = false;

            //Make weird rotations for the hell of it
            //iteration.GetComponent<Transform>().Rotate(new Vector3(0, 10, 10));

            //Make sure the current generating block stops generating.
            GetComponent<BuildingBlock>().isGenerating = false;

            //stop generating under certain depth
            if (iteration.transform.position.y < -5)
                Destroy(iteration);
        }
    }
}
