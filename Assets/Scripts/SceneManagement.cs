using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Để sử dụng Image

public class SceneManagement : MonoBehaviour
{
    public int enemyCount = 0;
    public Image victoryImage;  // Hình ảnh chiến thắng
    public AudioSource victoryAudio;  // Âm thanh chiến thắng
    public string nextSceneName = "NextScene";  // Tên scene kế tiếp

    private void Start()
    {
        // Tìm tất cả quái vật trong map hiện tại
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public void OnEnemyDefeated()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            Debug.Log("Tất cả quái đã bị tiêu diệt. Bạn có thể qua map mới!");
            if(victoryAudio != null && victoryImage != null)
            {
                StartCoroutine(HandleVictory());
            }
        }
    }

    // Xử lý sự kiện chiến thắng: hiển thị hình ảnh, phát âm thanh và chuyển scene
    private IEnumerator HandleVictory()
    {
        // Phát âm thanh chiến thắng
        victoryAudio.Play();

        // Hiển thị hình ảnh Victory và bắt đầu di chuyển hình ảnh lên
        victoryImage.gameObject.SetActive(true);

        // Đặt vị trí ban đầu của hình ảnh và vị trí mục tiêu
        Vector3 startPos = victoryImage.transform.position;
        Vector3 endPos = new Vector3(startPos.x, startPos.y + 500f, startPos.z);  // Di chuyển lên 500 đơn vị

        float elapsedTime = 0f;
        float duration = 5f;  // Thời gian di chuyển và hiển thị hình ảnh

        // Di chuyển hình ảnh từ vị trí ban đầu đến vị trí mục tiêu
        while (elapsedTime < duration)
        {
            victoryImage.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Đảm bảo hình ảnh ở vị trí cuối cùng
        victoryImage.transform.position = endPos;

        // Chuyển scene sang scene kế tiếp
        LoadLevel();
    }

    public void LoadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Chuyển sang Scene tiếp theo (nếu có)
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.LogWarning("Đây là Scene cuối cùng, không thể chuyển tiếp!");
        }
    }
}
