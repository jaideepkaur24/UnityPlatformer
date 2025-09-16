using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;// sound that we'll play when picking up a new checkpoint
    private Transform currentCheckpoint; // we'll store our last checkpoint here
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        //check if checkpoint available
        if (currentCheckpoint == null)
        {
            //  Show game over screen
            uiManager.GameOver();
        
            return; // Don't execute the rest of this function 
        }

        playerHealth.Respawn();// Restore player health and reset animation
        transform.position = currentCheckpoint.position; // moveplayer to checkpoint position

        // move camera to checkpoint's room(for this to work the checkpoint objects has to placed as a child of the room object)
        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    }

    // Activate Checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; // store the checkpoint that we activate as the current one
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; // Deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear"); // Trigger checkpoint animation
        }
    }
}
