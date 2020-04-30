
using UnityEngine;
using UnityEngine.UI;
public class DistanceScore : MonoBehaviour
{
    Transform player;
    [SerializeField] Slider DistanceSlider;

    float initDistance;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Ball").transform;

        initDistance = transform.position.z - player.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = transform.position.z - player.position.z;
        DistanceSlider.value = (initDistance - currentDistance) / initDistance;
    }
}