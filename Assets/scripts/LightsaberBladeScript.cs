using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightsaberBladeScript : MonoBehaviour
{

    public const float EXTEND_SPEED = 0.1f; // extend/collapse speed
    public bool state = false; // lightsaber on/off
    public const float MIN_SCALE = 0.0f; // minimum_scale val
    public float max_scale = 0.7f; // maximum_scale val
    public float interpolated_scale; // how much we need to scale per update call to make it look nice
    public float cur_scale; // current scale of the lightsaber blade
    public float init_x; // initial x scale
    public float init_z; // initial z scale

    int impactFrames = 15;
    bool impacting = false;

    public GameObject blade;
    public Material bladeMaterial;
    

    InputAction jumpAction;
    InputAction attackAction;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get local scales for x, y, z
        init_x = transform.localScale.x;
        init_z = transform.localScale.z;
        max_scale = transform.localScale.y;

        cur_scale = max_scale; // start it maximum scaled
        interpolated_scale = max_scale / EXTEND_SPEED; // calc interpolated scale using max_scale / speed
        state = true;

        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");

    }

    // Update is called once per frame
    void Update()
    {

        if (impactFrames <= 0)
        {
            bladeMaterial.SetFloat("_WobbleSpeed", 0.18f);
            impacting = false;
        }


        bool jumpValue = jumpAction.WasPressedThisFrame();
        if (jumpValue)
        {
            interpolated_scale = state ? -Mathf.Abs(interpolated_scale): Mathf.Abs(interpolated_scale);
        }

        bool attackValue = attackAction.WasPressedThisFrame();
        if (attackValue && !impacting)
        {
            bladeMaterial.SetFloat("_WobbleSpeed", 0);
            impacting = true;
            impactFrames = 15;
        }



        cur_scale += interpolated_scale * Time.deltaTime;
        cur_scale = Mathf.Clamp(cur_scale, MIN_SCALE, max_scale);
        transform.localScale = new Vector3(init_x, cur_scale, init_z);

        impactFrames -= 1;

        state = cur_scale > 0;

        blade.SetActive(state);



    }

}
