using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the game called Compulsion.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The number of tasks required to win the game.
    /// </summary>
    [SerializeField]
    [Range(1, 10)]
    [Tooltip("Number of normal tasks to win the game.")]
    private int totalTaskCount;

    /// <summary>
    /// The max amount of influence(stress) per increase in stress level.
    /// </summary>          
    [SerializeField]
    [Range(1, 10)]
    [Tooltip("How strong the OCD pressure is before we see a new level of side effects")]
    private int stressPerOCDEffectLevel;

    /// <summary>
    /// Length of game in seconds.
    /// </summary>
    [SerializeField]
    [Range(1f, 3600f)]
    [Tooltip("In seconds")]
    private float gameLength;

    /// <summary>
    /// Minimum time between OCD Random Attacks.
    /// </summary>
    [SerializeField]
    [Range(1f, 60f)]
    [Tooltip("In seconds")]
    private float randomOCDTimerMin;

    /// <summary>
    /// Maximum time between OCD Random Attacks.
    /// </summary>
    [SerializeField]
    [Range(1f, 300f)]
    [Tooltip("In seconds")]
    private float randomOCDTimerMax;

    /// <summary>
    /// OCD tasks to be started at random.
    /// </summary>
    [SerializeField]
    [Tooltip("OCD Attacks that occur at random.")]
    private GameObject[] randomOCDEvents;

    /// <summary>
    /// List of AudioClips to play when discussing amount of time left.
    /// </summary>
    [SerializeField]
    [Tooltip("List of AudioClips to play when discussing amount of time left.")]
    private AudioClip[] timeLeftAudioClips;

    private int taskCompleteCount;          // The current completed tasks (Win if equal to totalTaskCount).
    private int ocdCurrentCount;            // Number of current active ocd events (Zero for win?).
    private int totalInfluence;             // The influence that the OCD tasks are having on player.
    private int stressLevel;
    private float startTime;                // To account for menus and stuff.
    private bool isSoundPlaying;            // Lets the script know if the sound coroutine is playing.

    private GameObject player;                          // The Player
    private AudioSource playerSoundSource;              // The Player's mouth
    private Queue<AudioClip> queuedSounds;              // A queue of sounds in case multiple sound requests are made to this script
    private Dictionary<GameObject, OCDTask> activeOCD;  // A dictionary of currently running OCD effects and relevent data about each one.

    private static readonly Object ocdLock = new Object();   // Lock for OCD related class members
    private static readonly Object soundLock = new Object(); // Lock for sound related class memeber

    /// <summary>
    /// Initializes and starts the beginning of game sequence.
    /// </summary>
    private void Start()
    {
        // initialize class members
        taskCompleteCount = 0;
        ocdCurrentCount = 0;
        startTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");
        playerSoundSource = player.GetComponent<AudioSource>();
        queuedSounds = new Queue<AudioClip>();
        activeOCD = new Dictionary<GameObject, OCDTask>();
        totalInfluence = 0;
        isSoundPlaying = false;
        randomOCDTimerMax = randomOCDTimerMax > randomOCDTimerMin ? randomOCDTimerMax : randomOCDTimerMin;
        stressLevel = 0;

        // start game
        StartCoroutine(StartGame());

    }

    /// <summary>
    /// Checks if the conditions for a win have been met.
    /// </summary>
    private void CheckWinState()
    {
        if (taskCompleteCount == totalTaskCount && ocdCurrentCount == 0)
        {
            lock (soundLock) { queuedSounds.Clear(); }
            GameOver(true);
        }
    }

    /// <summary>
    /// Will inform the GameManager that a normal task has been completed.
    /// </summary>
    public void NormalTaskCompleted()
    {
        taskCompleteCount++;
        lock (ocdLock) { CheckWinState(); }
    }

    /// <summary>
    /// Will register task as an OCD and begin tracking it's influence on the player.
    /// </summary>
    /// <param name="task">The GameObject that will be identified with as the OCD task</param>
    public void StartOCD(GameObject task)
    {
        lock (ocdLock)
        {
            if (!activeOCD.ContainsKey(task))
            {
                activeOCD.Add(task, new OCDTask(task));
                totalInfluence += activeOCD[task].Influence;
                ocdCurrentCount++;
                CheckStress();
            }
        }
    }

    /// <summary>
    /// Will remove the OCD as a current OCD and will reduce the influence (severity) that
    /// it was causing.
    /// </summary>
    /// <param name="task">The OCD attached to task will be ended.</param>
    public void EndOCD(GameObject task)
    {
        lock (ocdLock)
        {
            if (activeOCD.ContainsKey(task))
            {
                totalInfluence -= activeOCD[task].Influence;
                activeOCD.Remove(task);
                ocdCurrentCount--;
                CheckWinState();
                CheckStress();
            }
        }
    }

    /// <summary>
    /// Will increase the influence (severity) of the OCD attached to task (passed in arg).
    /// Must call StartOCD(GameObject) to register task before using this method.
    /// </summary>
    /// <param name="task">GameManager uses task to keep track of which OCD is calling the method.</param>
    public void IncreaseInfluence(GameObject task)
    {
        lock (ocdLock)
        {
            if (activeOCD.ContainsKey(task))
            {
                activeOCD[task].Influence++;
                totalInfluence++;
                CheckStress();
            }
        }
    }

    /// <summary>
    /// Compares the current stressLevel to the totalInfluence and informs
    /// OCDEffectManager of any changes that need to be made.
    /// </summary>
    private void CheckStress()
    {
        int stress = totalInfluence / stressPerOCDEffectLevel;
        if (stress != stressLevel)
        {
            stressLevel = stress;
            if (stressLevel == 5)
            {
                GameOver(false);
            }

            Debug.Log("StressLevel = " + stressLevel);
            // UPDATE OCDEffectManager about new level
        }
        Debug.Log("Influence = " + totalInfluence);

    }

    /// <summary>
    /// Will add the provided AudioClip to the queuedSounds then play
    /// all the AudioClips in the queue if not already running.
    /// </summary>
    /// <param name="clip">Will be added to queuedSounds.</param>
    public void QueuePlayerSpeech(AudioClip clip)
    {
        lock (soundLock)
        {
            queuedSounds.Enqueue(clip);
            if (!isSoundPlaying)
            {
                isSoundPlaying = true;
                StartCoroutine(PlaySounds());
            }
        }
    }

    /// <summary>
    /// Will play all of the AudioClips stored in queuedSounds.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlaySounds()
    {
        bool shouldContinue = true;
        AudioClip sound;

        while (shouldContinue)
        {
            lock (soundLock) { sound = queuedSounds.Dequeue(); }

            playerSoundSource.PlayOneShot(sound);
            yield return new WaitForSeconds(sound.length);

            lock (soundLock) { shouldContinue = queuedSounds.Count != 0; }
        }

        lock (soundLock) { isSoundPlaying = false; }

    }

    /// <summary>
    /// Will choose an event from randomOCDEvents at random then go to sleep for 
    /// a random amount of time (based on randomOCDTimerMin and randomOCDTimerMax).
    /// </summary>
    private IEnumerator RandomOCDAttack()
    {
        yield return null;
        if (randomOCDEvents.Length != 0)
        {
            float wait = 0;
            int randomInt = 0;
            bool keyThatIsContaind = false;
            while (true)
            {
                wait = Random.Range(randomOCDTimerMin, randomOCDTimerMax);
                randomInt = Random.Range(0, randomOCDEvents.Length);
                lock (ocdLock) { keyThatIsContaind = activeOCD.ContainsKey(randomOCDEvents[randomInt]); }
                while (keyThatIsContaind)
                {
                    yield return new WaitForSeconds(1f);
                    randomInt = Random.Range(0, randomOCDEvents.Length);
                }

                lock (ocdLock)
                {
                    activeOCD.Add(randomOCDEvents[randomInt], new OCDTask(randomOCDEvents[randomInt]));
                    ocdCurrentCount++;
                    totalInfluence += activeOCD[randomOCDEvents[randomInt]].Influence;
                }
					
                randomOCDEvents[randomInt].SendMessage("Activate");
                yield return new WaitForSeconds(wait);
            }
        }
    }

    /// <summary>
    /// Coroutine that will create a gameover if the player takes too long (based on gameLength).  
    /// Will occasionally play some player dialogue based on a set timer (based on length of timeLeftAudioClips).
    /// </summary>
    /// <returns></returns>
    private IEnumerator MasterGameTimer()
    {
        int timeClipCounter = 0;
        float timeBetweenSounds = timeLeftAudioClips.Length > 0 ?
            gameLength / timeLeftAudioClips.Length :
            gameLength;
        while (Time.time - startTime < gameLength)
        {
            if (timeClipCounter < timeLeftAudioClips.Length)
                QueuePlayerSpeech(timeLeftAudioClips[timeClipCounter]);
            timeClipCounter++;
            yield return new WaitForSeconds(timeBetweenSounds);
        }

        GameOver(false);
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    private IEnumerator StartGame()
    {
        // PLAY STARTING SEQUENCE
        yield return new WaitForSeconds(1f);
        if (randomOCDEvents.Length != 0)
            StartCoroutine(RandomOCDAttack());

        StartCoroutine(MasterGameTimer());
    }

    /// <summary>
    /// Stops and executes last sequence of the game based on whether the player won or lost.
    /// </summary>
    /// <param name="win">Did player win?</param>
    private void GameOver(bool win)
    {
        lock (soundLock)
        {
            // STOP STUFF
            StopAllCoroutines();

            if (win)
            {
                // Start Win sequence
            }
            else
            {
                // Start Loss sequence
            }
        }
    }
}
