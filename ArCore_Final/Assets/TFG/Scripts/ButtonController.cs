using UnityEngine.UI;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	public string [] Texts = new string[2];
	private bool activated = false;
	public Text buttonText;
	public Color activatedColor;

    public void OnClick()
	{
		activated = !activated;

		if (buttonText)
		{
			buttonText.text = (activated ? Texts[1] : Texts[0]);
			if(buttonText.gameObject.GetComponentInParent<Image>())
				buttonText.gameObject.GetComponentInParent<Image>().color = (activated ? activatedColor : Color.white);
		}
	}
}
