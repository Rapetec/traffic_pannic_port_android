using UnityEngine;

public class LoadingBarAnimation : MonoBehaviour
{
	private void Start()
	{
		UIPanel component = base.transform.parent.GetComponent<UIPanel>();
		if (component != null)
		{
			base.transform.parent.GetComponent<UIPanel>().GetTransition(UIPanelManager.SHOW_MODE.BringInForward).AddTransitionEndDelegate(PlayAnim);
		}
	}

	private void PlayAnim(EZTransition trans)
	{
		GetComponent<PackedSprite>().PlayAnim(0);
		trans.RemoveTransitionEndDelegate(PlayAnim);
	}
}
