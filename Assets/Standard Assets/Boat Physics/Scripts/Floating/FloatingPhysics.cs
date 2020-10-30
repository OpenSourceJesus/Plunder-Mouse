using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

//Simples possible script to make a mesh float
public class FloatingPhysics : MonoBehaviour, IUpdatable
{
    public GameObject floatingObj;
    public GameObject underWaterObj;
    private ModifyFloatingMesh modifyFloatingMesh;
    private Mesh underWaterMesh;
    public Rigidbody rigid;

    void OnEnable ()
    {
        //Init the script that will modify the mesh
        modifyFloatingMesh = new ModifyFloatingMesh(floatingObj);
        GameManager.updatables = GameManager.updatables.Add(this);
    }

    void OnDisable ()
    {
        GameManager.updatables = GameManager.updatables.Remove(this);
    }

    public void DoUpdate ()
    {
        modifyFloatingMesh.GenerateUnderwaterMesh();
        if (modifyFloatingMesh.underWaterTriangleData.Count > 0)
        {
            AddUnderWaterForces();
        }
        //Display the under water mesh - will take some computer power so remove in final version
        // if (underWaterMesh != null)
        // {
            //modifyFloatingMesh.DisplayMesh(underWaterMesh, "UnderWater Mesh", modifyFloatingMesh.underWaterTriangleData);
        // }
    }

    //Add all forces that act on the triangles below the water
    void AddUnderWaterForces()
    {
        //Get all triangles
        List<FloatingTriangleData> underWaterTriangleData = modifyFloatingMesh.underWaterTriangleData;

        //Add forces to all triangles
        for (int i = 0; i < underWaterTriangleData.Count; i++)
        {
            //This triangle
            FloatingTriangleData triangleData = underWaterTriangleData[i];

            //Calculate the buoyancy force
            Vector3 buoyancyForce = BuoyancyForce(triangleData);

            //Add the force to the boat
            rigid.AddForceAtPosition(buoyancyForce, triangleData.center);


            //Debug

            //Normal
            //Debug.DrawRay(triangleData.center, triangleData.normal * 3f, Color.white);

            //Buoyancy
            //Debug.DrawRay(triangleData.center, buoyancyForce.normalized * -3f, Color.blue);
        }
    }

    Vector3 BuoyancyForce (FloatingTriangleData triangleData)
    {
        //Buoyancy is a hydrostatic force - it's there even if the water isn't flowing or if the boat stays still

        // F_buoyancy = rho * g * V
        // rho - density of the mediaum you are in
        // g - gravity
        // V - volume of fluid directly above the curved surface 

        //The density of the water
        float rhoWater = 1027f;
        float gravity = Physics.gravity.y;

        // V = z * S * n 
        // z - distance to surface
        // S - surface area
        // n - normal to the surface
        Vector3 buoyancyForce = rhoWater * gravity * triangleData.distanceToSurface * triangleData.area * triangleData.normal;

        //The vertical component of the hydrostatic forces don't cancel out but the horizontal do
        buoyancyForce.x = 0f;
        buoyancyForce.z = 0f;

        return buoyancyForce;
    }
}
