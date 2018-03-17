using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrow : MonoBehaviour {

    //public variables
    public float radius;
    float resetradius;
    //private variables
    [SerializeField] GameObject sphereObject;
    CharacterController controller;
    PlayerData data;
    bool growing = false;
    GameObject radiusSphere;

    private void Start()
    {
        resetradius = radius;
        data = GetComponent<PlayerData>();
    }

    void Update()
    {
        if (growing)
        {
            grow(transform.position, radius);
            if (radiusSphere == null)
            {
                radiusSphere = GameObject.Instantiate(sphereObject);
                radiusSphere.transform.SetParent(transform);
                radiusSphere.transform.localPosition = Vector3.up;
                radiusSphere.transform.localScale = new Vector3(1, 1, 1) * resetradius * 2;
            }
            else
            {
                radiusSphere.transform.localScale = new Vector3(1, 1, 1) * radius * 2;
            }
        }

        else
        {
            if (radiusSphere != null)
            {
                Destroy(radiusSphere);
            }
        }
    }

    void grow(Vector3 pos, float r)
    {
        if (data.currentPower > 0)
        {
            Collider[] blocks = Physics.OverlapSphere(transform.position + Vector3.up, r);
            for (int i = 0; i < blocks.Length; i++)
            {
                BuildingBlock b = blocks[i].GetComponent<BuildingBlock>();
                if (b != null)
                {
                    if (!b.hasGrown && growing)
                    {
                        b.isGrowing = true;
                    }
                    else if (!growing)
                        b.isGrowing = false;
                }
            }
            data.currentPower -= Time.deltaTime;
        }
    }

    public void startGrowing()
    {
        growing = true;
        radius += Time.deltaTime;
    }

    public void endGrowing()
    {
        growing = false;
        radius = resetradius;
    }
}
