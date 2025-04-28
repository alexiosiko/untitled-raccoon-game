using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CurtainManager : MonoBehaviour
{
	Image image;
    void Awake()
    {
		image = GetComponent<Image>();
    }
	void Start()
	{
		image.DOFade(1, 0);
		Invoke(nameof(Close), 1);
	}
	public void Close()
	{
		image.DOFade(0, 10f);
	}
	public void Open()
	{
		image.DOFade(1, 10f);
	}
}
