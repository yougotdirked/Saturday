using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGenerationv2 : Generation {

    public override void newChild(Vector3 inputLoc, int c)
    {
        if (c > 0)
        {
            //inputLoc.Normalize();


            //create child
            GameObject iteration = GameObject.Instantiate(child);

            //calculate offset
            Vector3 offset = Vector3.Normalize(transform.position);

            //put child at offset position, reduce its complexity and make sure it generates
            iteration.transform.localPosition = transform.localPosition + offset;
            iteration.GetComponent<BuildingBlock>().isGenerating = false;

            //Make weird rotations for the hell of it
            iteration.GetComponent<Transform>().Rotate(new Vector3(10, 0, 10));

            //Make sure the current generating block stops generating.
            GetComponent<BuildingBlock>().isGenerating = false;
        }
    }
}
