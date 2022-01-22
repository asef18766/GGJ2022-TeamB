using UnityEngine;
using UnityEngine.SceneManagement;

namespace asef18766.Scripts
{
    public class SceneController : MonoBehaviour
    {
        public void ChangeScene(int idx)
        {
            SceneManager.LoadScene(idx);
        }
    }
}