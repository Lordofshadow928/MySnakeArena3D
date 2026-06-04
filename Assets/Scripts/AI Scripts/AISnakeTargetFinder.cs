//using UnityEngine;

//public class AISnakeTargetFinder : MonoBehaviour
//{
//    [SerializeField] private float refreshRate = 0.5f;

//    private AISnakeBrain brain;

//    private void Awake()
//    {
//        brain = GetComponent<AISnakeBrain>();
//    }

//    private void Start()
//    {
//        InvokeRepeating(nameof(UpdateTarget), 0f, refreshRate);
//    }

//    private void UpdateTarget()
//    {
//        Transform nearestFood = FoodManager.Instance.GetNearestFood(transform.position);

//        if (nearestFood != null)
//        {
//            brain.SetTarget(nearestFood);
//        }
//    }
//}