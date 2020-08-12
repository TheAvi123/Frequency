using Tutorial;

using UnityEngine;

namespace UserInterface.Buttons
{
	public class ClosePopUpButton : MonoBehaviour
	{
		private void OnMouseUp() {
			FindObjectOfType<TutorialPopUp>().ClosePopUp();
		}
	}
}