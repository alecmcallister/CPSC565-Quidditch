using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreVisualizer : MonoBehaviour
{
	RectTransform rect;

	RectTransform t1Rect;
	RectTransform t2Rect;

	Image t1Image;
	Image t2Image;

	int t1Tween = 0;
	int t2Tween = 0;

	int prevS1;
	int prevS2;

	void Awake()
	{
		rect = GetComponent<RectTransform>();


		t1Rect = transform.Find("Team1Bar").GetComponent<RectTransform>();
		t2Rect = transform.Find("Team2Bar").GetComponent<RectTransform>();

		t1Image = transform.Find("Team1Bar").GetComponent<Image>();
		t2Image = transform.Find("Team2Bar").GetComponent<Image>();
	}

	public void Init(Color c1, Color c2, int s1, int s2)
	{
		t1Image.color = c1;
		t2Image.color = c2;
		UpdateScore(s1, s2);
	}

	public void UpdateScore(int s1, int s2)
	{
		int max = FieldManager.Instance.MaxPointsForWin;

		float interval = rect.rect.height / max;

		if (s1 != prevS1)
		{
			prevS1 = s1;

			LeanTween.cancel(t1Tween);

			t1Tween = LeanTween.value(t1Rect.sizeDelta.y, s1 * interval, 1f).setOnUpdate((float val) =>
			{
				t1Rect.sizeDelta = new Vector2(t1Rect.sizeDelta.x, val);
			}).setEase(LeanTweenType.easeInOutSine).uniqueId;
		}
		if (s2 != prevS2)
		{
			prevS2 = s2;

			LeanTween.cancel(t2Tween);

			t2Tween = LeanTween.value(t2Rect.sizeDelta.y, s2 * interval, 1f).setOnUpdate((float val) =>
			{
				t2Rect.sizeDelta = new Vector2(t2Rect.sizeDelta.x, val);
			}).setEase(LeanTweenType.easeInOutSine).uniqueId;
		}
	}
}
