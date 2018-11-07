using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float amount = 0.02f;
    public float maxamount = 0.03f;
    public float smooth = 3;
    private Quaternion def;
    public FixedTouchField fixedTouchField;


    void Start()
    {
        def = transform.localRotation;
    }

    void Update()
    {

        float factorZ = -(fixedTouchField.TouchDist.x) * amount;
        //float factorY = -(Input.GetAxis("Jump")) * amount;
        //float factorZ = -Input.GetAxis("Vertical") * amount;

        //if (factorX > maxamount)
        //factorX = maxamount;

        //if (factorX < -maxamount)
        //factorX = -maxamount;

        //if (factorY > maxamount)
        //factorY = maxamount;

        //if (factorY < -maxamount)
        //factorY = -maxamount;

        if (factorZ > maxamount)
            factorZ = maxamount;

        if (factorZ < -maxamount)
            factorZ = -maxamount;

        Quaternion Final = Quaternion.Euler(0, 0, def.z + factorZ);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Final, (Time.deltaTime * amount) * smooth);
    }
}