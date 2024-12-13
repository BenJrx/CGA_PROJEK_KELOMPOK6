using UnityEngine;
using UnityEngine.AI;

public class MinionAI : MonoBehaviour
{
    public Transform statue; // Referensi ke patung
    public Transform player; // Referensi ke player
    public float playerDetectionRange = 5f; // Jarak deteksi minion terhadap player
    public float stopRange = 2f; // Jarak dimana minion berhenti mengejar player dan diam

    private Transform target;
    private bool attackingPlayer = false;
    private NavMeshAgent agent; // Referensi ke NavMeshAgent

    void Start()
    {
        // Mendapatkan komponen NavMeshAgent dari objek ini
        agent = GetComponent<NavMeshAgent>();
        target = statue; // Target awal adalah statue

        // Pastikan stopping distance lebih besar dari stopRange
        agent.stoppingDistance = stopRange + 0.1f; // Agar minion benar-benar berhenti tepat setelah stopRange
        agent.updateRotation = false; // Disable automatic rotation update from NavMeshAgent
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Debug log untuk status agent
        Debug.Log("Minion Position: " + transform.position);
        Debug.Log("Player Position: " + player.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);
        Debug.Log("Player Detection Range: " + playerDetectionRange);
        Debug.Log("Stop Range: " + stopRange);
        Debug.Log("NavMeshAgent Is Stopped: " + agent.isStopped); // Log status agent

        // Menentukan apakah minion mendekati player
        if (distanceToPlayer <= playerDetectionRange)
        {
            target = player;
            attackingPlayer = true;

            // Jika player sangat dekat, minion berhenti mengejar
            if (distanceToPlayer <= stopRange)
            {
                if (!agent.isStopped)
                {
                    Debug.Log("Stopping minion, player too close.");
                    agent.isStopped = true;  // Minion berhenti
                    agent.velocity = Vector3.zero; // Pastikan tidak bergerak
                }
                // Rotasi minion menghadap ke player
                RotateTowards(player.position);
            }
            else
            {
                if (agent.isStopped)
                {
                    Debug.Log("Resuming movement.");
                    agent.isStopped = false; // Minion mulai bergerak
                }
                agent.SetDestination(player.position); // Minion mengejar player
                RotateTowards(player.position); // Rotasi minion menghadap ke player
            }
        }
        else
        {
            target = statue;
            attackingPlayer = false;

            if (agent.isStopped)
            {
                Debug.Log("Resuming movement towards statue.");
                agent.isStopped = false; // Minion kembali bergerak ke statue
            }
            agent.SetDestination(statue.position); // Minion menuju statue
            RotateTowards(statue.position); // Rotasi minion menghadap ke statue
        }

        // Minion berhenti jika sudah cukup dekat dengan target
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            if (!agent.isStopped)
            {
                Debug.Log("Minion stopped at target.");
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }
        }
    }

    // Fungsi untuk merotasi minion agar menghadap ke tujuan
    private void RotateTowards(Vector3 targetPosition)
    {
        // Hitung arah menuju target
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0; // Pastikan rotasi hanya terjadi di sumbu Y (yaitu horisontal)

        // Menghitung rotasi yang diinginkan
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Gunakan Quaternion.RotateTowards untuk rotasi yang halus
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
        
        // Pastikan agent bergerak ke depan (tanpa ke samping)
        agent.velocity = transform.forward * agent.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Menangani tabrakan dengan patung atau player
        if (other.CompareTag("Statue") && !attackingPlayer)
        {
            // Tambahkan logika untuk saat minion mencapai statue
            Debug.Log("Minion reached the statue and will be destroyed.");
            Destroy(gameObject); // Hancurkan minion setelah menyerang
        }
        else if (other.CompareTag("Player") && attackingPlayer)
        {
            // Logika damage ke player bisa ditambahkan di sini
            Debug.Log("Minion is attacking the player!");
        }
    }
}
