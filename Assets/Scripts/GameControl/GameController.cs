using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class GameController : MonoBehaviour
{
    [SerializeField] private Transform parentObject; // The object to search under
    [SerializeField] private DiceManager diceManager; // The object to search under
    [SerializeField] private SphereCollector sphereCollector; // The object to search under
    [SerializeField] private BlockCollector blockCollector; // The object to search under
    [SerializeField] private Camera mainCamera; // The object to search under
    private List<Transform> perkList;
    public List<GameObject> ballList = new List<GameObject>();
    public List<GameObject> blockList = new List<GameObject>();
    public float waitTime = 1f;
    private int totalDiceValue;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    private bool gameIsRunning = false;
public void StartGame(){
    StartCoroutine(StartGameFunction());
}
    private IEnumerator StartGameFunction()
    {
        sphereCollector.InitializeSphere();
        blockCollector.SpawnBlocks();
        yield return new WaitForEndOfFrame();
        diceManager.Reset();
        CreatePerkList();
        FindAllBlocks();
        FindAllBalls();
        ApplyPerkEffects();
        yield return new WaitForEndOfFrame();

    }
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
        gameIsRunning=!gameIsRunning;
        if (gameIsRunning){
            StartCoroutine(ApplyPerkEffectsNumerator());
        }
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
                mainCamera.GetComponent<CameraManager>().MoveToPosition2();
                perkTransform.GetComponent<ButtonHighlighter>().StartHighlight();
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(ApplyEffectSquare(squarePerk.perkData));
            }
            else if (perkTransform.TryGetComponent(out TrianglePerkButton trianglePerk))
            {
                perkTransform.GetComponent<ButtonHighlighter>().StartHighlight();
                // Wait for 0.5 seconds before applying the effect
                yield return new WaitForSeconds(waitTime);
                mainCamera.GetComponent<CameraManager>().MoveToPosition1();
                yield return StartCoroutine(ApplyEffectTriangle(trianglePerk.perkData));
            }
            else if (perkTransform.TryGetComponent(out CirclePerkButton circlePerk))
            {
                perkTransform.GetComponent<ButtonHighlighter>().StartHighlight();
                // Wait for 0.5 seconds before applying the effect
                yield return new WaitForSeconds(waitTime);
                mainCamera.GetComponent<CameraManager>().MoveToPosition1();
                yield return StartCoroutine(ApplyEffectCircle(circlePerk.perkData));
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
            Debug.Log("Activating Never2RollDice1To6");
            diceManager.Prevent(2);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never3RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never3RollDice1To6");
            diceManager.Prevent(3);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never4RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never4RollDice1To6");
            diceManager.Prevent(4);
            yield return StartCoroutine(diceManager.Roll(1, 6));
            break;
         case "Never5RollDice1To6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Never5RollDice1To6");
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
         case "Force1":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Force1");
            yield return StartCoroutine(diceManager.ForceRoll(1, 1));
            break;
         case "Force2":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Force2");
            yield return StartCoroutine(diceManager.ForceRoll(2, 2));
            break;
         case "Force3":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Force3");
            yield return StartCoroutine(diceManager.ForceRoll(3, 3));
            break;
         case "Force4":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Force4");
            yield return StartCoroutine(diceManager.ForceRoll(4, 4));
            break;
         case "Force5":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Force5");
            yield return StartCoroutine(diceManager.ForceRoll(5, 5));
            break;
         case "Force6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating Force6");
            yield return StartCoroutine(diceManager.ForceRoll(6, 6));
            break;
         case "10For1":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating 10For1");
            yield return StartCoroutine(diceManager.AcknowledgeRoll(10, 1));
            break;   
         case "10For2":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating 10For2");
            yield return StartCoroutine(diceManager.AcknowledgeRoll(10, 2));
            break;   
          case "10For3":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating 10For3");
            yield return StartCoroutine(diceManager.AcknowledgeRoll(10, 3));
            break;   
          case "10For4":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating 10For4");
            yield return StartCoroutine(diceManager.AcknowledgeRoll(10, 4));
            break;   
          case "10For5":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating 10For5");
            yield return StartCoroutine(diceManager.AcknowledgeRoll(10, 5));
            break;   
          case "10For6":
            // Roll the dice and wait for it to finish
            Debug.Log("Activating 10For6");
            yield return StartCoroutine(diceManager.AcknowledgeRoll(10, 6));
            break;   
            
        default:
            Debug.LogWarning($"No effect defined for key: {perkData.effectKey}");
            break;
    }
}
    private IEnumerator ApplyEffectCircle(PerkData perkData)
    {
        if (perkData == null)
        {
            Debug.LogWarning("PerkData is null.");
            yield break;
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
                    originalPosition = ball.transform.position;
                    originalRotation = ball.transform.rotation;
                    originalParent = ball.transform.parent;
                    // Instantiate a clone at the same position and rotation
                    GameObject ballClone = Instantiate(ball, originalPosition, originalRotation);
                    ballClone.transform.SetParent(originalParent);
                    ballClone.name = ball.name + "_Clone";
                }

                break;
            case "Duplicate1XTimes":
                Debug.Log("Activating Duplicate1XTimes");
                GameObject randomBall = ballList[Random.Range(0, ballList.Count)];
                originalPosition = randomBall.transform.position;
                originalRotation = randomBall.transform.rotation;
                originalParent = randomBall.transform.parent; 
                // Clone the selected ball X times
                for (int i = 0; i < totalDiceValue; i++)
                {
                    // Instantiate a clone at the same position and rotation
                    GameObject ballClone = Instantiate(randomBall, originalPosition, originalRotation);
                    ballClone.transform.SetParent(originalParent);
                    ballClone.name = randomBall.name + "_Clone_" + (i + 1); // Optional: differentiate clones by index
                }
                break;
            case "SpeedBurst":
                Debug.Log("Activating SpeedBurst");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().IncreaseSpeed(1+ (float)totalDiceValue/100, true);
                     
                }
                break;
            case "SlowMotion":
                Debug.Log("Activating SlowMotion");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().DecreaseSpeed(1+ (float)totalDiceValue/100);
                }
                break;
            case "SpeedLust":
                Debug.Log("Activating SpeedLust");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().speedLust = true;
                    ball.GetComponent<SphereController>().speedLustFactor *= 1 + (float)totalDiceValue/1000;
                }
                break;
            case "TemporarySpeed":
                Debug.Log("Activating TemporarySpeed");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().temporarySpeed = true;
                    ball.GetComponent<SphereController>().temporarySpeedFactor *= 3;
                    ball.GetComponent<SphereController>().temporarySpeedDuration = (float)totalDiceValue/10;
                }
                break;
            case "TemporaryFreeze":
                Debug.Log("Activating TemporaryFreeze");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().temporarySpeed = true;
                    ball.GetComponent<SphereController>().temporarySpeedFactor *= 0;
                    ball.GetComponent<SphereController>().temporarySpeedDuration = (float)totalDiceValue/10;
                }
                break;
            case "SizeMatters":
                Debug.Log("Activating SizeMatters");
                foreach (GameObject ball in ballList)
                {
                    float sizeFactor = (1+(float)totalDiceValue/100)*ball.transform.localScale.x;
                    if (sizeFactor > 50){
                        sizeFactor = 50;
                    }
                    ball.transform.localScale = new Vector3(sizeFactor, sizeFactor, sizeFactor);                }
                break;
            case "ShrinkRay":
                Debug.Log("Activating ShrinkRay");
                foreach (GameObject ball in ballList)
                {
                    float sizeFactor = ball.transform.localScale.x/(1+ (float)totalDiceValue/100);
                    if (sizeFactor < 5){
                        sizeFactor = 5;
                    }
                    ball.transform.localScale = new Vector3(sizeFactor, sizeFactor, sizeFactor);                }
                break;
            case "WildMovement":
                Debug.Log("Activating WildMovement MIGHT BE BUGGED");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().wildMovement=true;
                    ball.GetComponent<SphereController>().wildMovementInterval=0.2f;
                    StartCoroutine(ball.GetComponent<SphereController>().ChangeDirectionRoutine());
                }
                break;
            case "PowerOfChaos":
                Debug.Log("Activating PowerOfChaos");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().powerOfChaos=true;
                }
                break;
            case "PowerOfSpeed":
                Debug.Log("Activating PowerOfSpeed");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().powerOfSpeed=true;
                }
                break;
            case "SplashDamage":
                Debug.Log("Activating SplashDamage");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().splashDamage=true;
                    ball.GetComponent<SphereController>().splashDamageFactor+=(float)totalDiceValue/100;
                    StartCoroutine(ball.GetComponent<BallVisuals>().SplashEffect());
                }
                break;
            case "Unbreakable":
                Debug.Log("Activating Unbreakable");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().unbreakable=true;
                    ball.GetComponent<SphereController>().unbreakableDuration+=(float)totalDiceValue/10;
                }
                break;
            case "ExplosiveDeath":
                Debug.Log("Activating ExplosiveDeath");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().explosiveDeath=true;
                }
                break;                
            case "BallSplitting":
                Debug.Log("Activating BallSplitting");
                foreach (GameObject ball in ballList)
                {
                    ball.GetComponent<SphereController>().ballSplitting=true;
                }
                break;

                default:
                Debug.LogWarning($"No effect defined for key: {perkData.effectKey}");
                break;
        }
    }
    private IEnumerator ApplyEffectSquare(PerkData perkData)
    {
        if (perkData == null)
        {
            Debug.LogWarning("PerkData is null.");
            yield break;
        }

        switch (perkData.effectKey)
        {
            case "Example":
                // Implement speed boost effect
                Debug.Log("Activating Example");
                break;
            case "Lifelink":
                // Implement speed boost effect
                Debug.Log("Activating Lifelink");
                foreach (GameObject block in blockList)
                {
                    block.GetComponent<BlockScript>().lifeLink = true;
                }
                blockList[0].GetComponent<BlockScript>().CreateLifeLinkVisuals(new List<BlockScript>(FindObjectsOfType<BlockScript>()));
                
                break;
            // Add more cases as needed

            default:
                Debug.LogWarning($"No effect defined for key: {perkData.effectKey}");
                break;
        }
    }
public void FindAllBalls()
{
    // Clear the existing list of balls
    ballList.Clear();

    // Find all GameObjects with the tag "Ball"
    GameObject[] ballArray = GameObject.FindGameObjectsWithTag("Ball");

        foreach (GameObject ball in ballArray)
    {
        // Add only if the ball is active in the hierarchy
        if (ball != null && ball.activeInHierarchy)
        {
            ballList.Add(ball);
        }
    }
}
public void FindAllBlocks()
{
    // Clear the existing list of balls
    blockList.Clear();

    // Find all GameObjects with the tag "Ball"
    GameObject[] blockArray = GameObject.FindGameObjectsWithTag("Block");

        foreach (GameObject block in blockArray)
    {
        // Add only if the ball is active in the hierarchy
        if (block != null && block.activeInHierarchy)
        {
            blockList.Add(block);
        }
    }
}
}
