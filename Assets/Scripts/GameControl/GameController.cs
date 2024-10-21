using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
public class GameController : MonoBehaviour
{
    [SerializeField] private Transform parentObject; // The object to search under
    [SerializeField] private DiceManager diceManager; // The object to search under
    private List<Transform> perkList;
    private List<GameObject> ballList;
    public float waitTime = 1f;
    private int totalDiceValue;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    public void CreatePerkList()
    {
        List<Transform> perkTypeChildren = FindAllPerkTypeChildren();
        
        foreach (Transform perk in perkTypeChildren)
        {
            Debug.Log($"Found PerkType: {perk.name}");
        }
        perkList = perkTypeChildren;
    }
    // Function to find all child objects with the PerkType component
    public List<Transform> FindAllPerkTypeChildren()
    {
        List<Transform> perkTypeChildren = new List<Transform>();
        SearchForPerkTypeChildren(parentObject, perkTypeChildren);
        return perkTypeChildren;
    }

    // Recursive function to search for PerkType components
    private void SearchForPerkTypeChildren(Transform currentTransform, List<Transform> result)
    {
        // Check if the current transform has the PerkType component
        if (currentTransform.GetComponent<TrianglePerkButton>() != null)
        {
            result.Add(currentTransform);
        }
        if (currentTransform.GetComponent<CirclePerkButton>() != null)
        {
            result.Add(currentTransform);
        }
        if (currentTransform.GetComponent<SquarePerkButton>() != null)
        {
            result.Add(currentTransform);
        }
        // Iterate through all children of the current transform
        foreach (Transform child in currentTransform)
        {
            SearchForPerkTypeChildren(child, result); // Recursive call for each child
        }
    }
    public void ApplyPerkEffects(){
        StartCoroutine(ApplyPerkEffectsNumerator());
    }
   public IEnumerator ApplyPerkEffectsNumerator()
{
    foreach (Transform perkTransform in perkList)
    {
        FindAllBalls(); // Presumably, this doesn't require waiting
        totalDiceValue=diceManager.totalRolls;
        if (perkTransform != null)
        {
            // Check which type of perk button the Transform has
            if (perkTransform.TryGetComponent(out SquarePerkButton squarePerk))
            {
                // Wait for 0.5 seconds before applying the effect
                yield return new WaitForSeconds(waitTime);
                ApplyEffectSquare(squarePerk.perkData);
            }
            else if (perkTransform.TryGetComponent(out TrianglePerkButton trianglePerk))
            {
                // Wait for 0.5 seconds before applying the effect
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(ApplyEffectTriangle(trianglePerk.perkData));
            }
            else if (perkTransform.TryGetComponent(out CirclePerkButton circlePerk))
            {
                // Wait for 0.5 seconds before applying the effect
                yield return new WaitForSeconds(waitTime);
                ApplyEffectCircle(circlePerk.perkData);
            }
            else
            {
                Debug.LogWarning("Unknown perk type on " + perkTransform.name);
            }
        }
        else
        {
            Debug.LogWarning("Perk Transform is null in perkList.");
        }
    }
}


    // Helper function to apply effects based on the PerkData's effectKey
private IEnumerator ApplyEffectTriangle(PerkData perkData)
{
    if (perkData == null)
    {
        Debug.LogWarning("PerkData is null.");
        yield break; // Use yield break to exit the coroutine
    }

    switch (perkData.effectKey)
    {
        case "Example":
            // Implement speed boost effect
            Debug.Log("Example");
            break;

        case "RollTwoDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating RollTwoDice1To6");
            yield return StartCoroutine(diceManager.Roll(1, 6));
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
        case "DoubleRolls":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating DoubleRolls");
            diceManager.rollCount *=2;
            break;
         case "Never1RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never1RollDice1To6");
            diceManager.Prevent(1);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never2RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never1RollDice2To6");
            diceManager.Prevent(2);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never3RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never1RollDice3To6");
            diceManager.Prevent(3);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never4RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never1RollDice4To6");
            diceManager.Prevent(4);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never5RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never1RollDice5To6");
            diceManager.Prevent(5);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;

        case "PreventAllBut6":
            Debug.Log("Activating PreventAllBut6");
            diceManager.Prevent(1);
            diceManager.Prevent(2);
            diceManager.Prevent(3);
            diceManager.Prevent(4);
            diceManager.Prevent(5);
            break;
        // Add more cases as needed

        default:
            Debug.LogWarning($"No effect defined for key: {perkData.effectKey}");
            break;
    }
}
    private void ApplyEffectCircle(PerkData perkData)
    {
        if (perkData == null)
        {
            Debug.LogWarning("PerkData is null.");
            return;
        }

        switch (perkData.effectKey)
        {
            case "Example":
                // Implement speed boost effect
                Debug.Log("Example");
                break;
            case "Duplicate":
                Debug.Log("Activating Duplicate");
                foreach (GameObject ball in ballList)
                {
                    // Get the original position, rotation, and parent of the ball
                    originalPosition = ball.transform.position;
                    originalRotation = ball.transform.rotation;
                    originalParent = ball.transform.parent; // Get the parent of the original ball

                    // Instantiate a clone at the same position and rotation
                    GameObject ballClone = Instantiate(ball, originalPosition, originalRotation);

                    // Set the clone's parent to the original ball's parent
                    ballClone.transform.SetParent(originalParent);

                    // Optionally, give the clone a new name or modify its properties
                    ballClone.name = ball.name + "_Clone";
                }

                break;
            case "Duplicate1XTimes":
                Debug.Log("Activating Duplicate1XTimes");
                GameObject randomBall = ballList[Random.Range(0, ballList.Count)];

                // Get the original position, rotation, and parent of the randomly selected ball
                originalPosition = randomBall.transform.position;
                originalRotation = randomBall.transform.rotation;
                originalParent = randomBall.transform.parent; // Get the parent of the original ball

                // Clone the selected ball X times
                for (int i = 0; i < totalDiceValue; i++)
                {
                    // Instantiate a clone at the same position and rotation
                    GameObject ballClone = Instantiate(randomBall, originalPosition, originalRotation);

                    // Set the clone's parent to the original ball's parent
                    ballClone.transform.SetParent(originalParent);

                    // Optionally, give the clone a new name or modify its properties
                    ballClone.name = randomBall.name + "_Clone_" + (i + 1); // Optional: differentiate clones by index
                }
                break;

            default:
                Debug.LogWarning($"No effect defined for key: {perkData.effectKey}");
                break;
        }
    }
    private void ApplyEffectSquare(PerkData perkData)
    {
        if (perkData == null)
        {
            Debug.LogWarning("PerkData is null.");
            return;
        }

        switch (perkData.effectKey)
        {
            case "Example":
                // Implement speed boost effect
                Debug.Log("Example");
                break;

            // Add more cases as needed

            default:
                Debug.LogWarning($"No effect defined for key: {perkData.effectKey}");
                break;
        }
    }
        public void FindAllBalls()
    {
        // Find all GameObjects with the tag "Ball"
        GameObject[] ballArray = GameObject.FindGameObjectsWithTag("Ball");

        // Convert the array to a list
        List<GameObject> currentBallList = new List<GameObject>(ballArray);

        // Return the list
        ballList = currentBallList;
    }

}
