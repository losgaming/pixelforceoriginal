using UnityEngine;

public class WeaponSway : MonoBehaviour
{


    public float amount = 0.055f;
    public float maxAmount = 0.09f;
    public float smooth = 3f;
    private Vector3 def;
    private Vector2 defAth;
    private Vector3 euler;
    private readonly GameObject ath;
    public FixedTouchField fixedTouchField;

    // Use this for initialization
    private void Start()
    {


        def = transform.localPosition;
        euler = transform.localEulerAngles;

    }




    float _smooth;

    // Update is called once per frame
    private void Update()
    {




        _smooth = smooth;


        float factorX = -fixedTouchField.TouchDist.x * amount;
        float factorY = -fixedTouchField.TouchDist.y * amount;



        if (factorX > maxAmount)
            factorX = maxAmount;

        if (factorX < -maxAmount)
            factorX = -maxAmount;


        if (factorY > maxAmount)
            factorY = maxAmount;

        if (factorY < -maxAmount)
            factorY = -maxAmount;



        Vector3 final = new Vector3(def.x + factorX, def.y + factorY, def.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, final, Time.deltaTime * _smooth);


    }
}
