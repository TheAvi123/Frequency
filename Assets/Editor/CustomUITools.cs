using UnityEditor;

using UnityEngine;

namespace Editor {
	public class CustomUITools : MonoBehaviour {

		[MenuItem("Custom Tools/UI/Set Anchors To Corners")]
		static void AnchorsToCorners(){
			foreach(Transform transform in Selection.transforms){
				RectTransform t = transform as RectTransform;
				RectTransform pt = Selection.activeTransform.parent as RectTransform;

				if(t == null || pt == null) return;
			
				Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
					t.anchorMin.y + t.offsetMin.y / pt.rect.height);
				Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
					t.anchorMax.y + t.offsetMax.y / pt.rect.height);

				t.anchorMin = newAnchorsMin;
				t.anchorMax = newAnchorsMax;
				t.offsetMin = t.offsetMax = new Vector2(0, 0);
			}
		}

		[MenuItem("Custom Tools/UI/Set Corners To Anchors")]
		static void CornersToAnchors(){
			foreach(Transform transform in Selection.transforms){
				RectTransform t = transform as RectTransform;

				if(t == null) return;

				t.offsetMin = t.offsetMax = new Vector2(0, 0);
			}
		}

		[MenuItem("Custom Tools/UI/Mirror Horizontally Around Anchors")]
		static void MirrorHorizontallyAnchors(){
			MirrorHorizontally(false);
		}

		[MenuItem("Custom Tools/UI/Mirror Vertically Around Anchors")]
		static void MirrorVerticallyAnchors() {
			MirrorVertically(false);
		}

		[MenuItem("Custom Tools/UI/Mirror Horizontally Around Parent")]
		static void MirrorHorizontallyParent(){
			MirrorHorizontally(true);
		}

		[MenuItem("Custom Tools/UI/Mirror Vertically Around Parent")]
		static void MirrorVerticallyParent() {
			MirrorVertically(true);
		}

		//Helper Methods
		static void MirrorHorizontally(bool mirrorAnchors){
			foreach(Transform transform in Selection.transforms){
				RectTransform t = transform as RectTransform;
				RectTransform pt = Selection.activeTransform.parent as RectTransform;
			
				if(t == null || pt == null) return;

				if(mirrorAnchors){
					Vector2 oldAnchorMin = t.anchorMin;
					t.anchorMin = new Vector2(1 - t.anchorMax.x, t.anchorMin.y);
					t.anchorMax = new Vector2(1 - oldAnchorMin.x, t.anchorMax.y);
				}

				Vector2 oldOffsetMin = t.offsetMin;
				t.offsetMin = new Vector2(-t.offsetMax.x, t.offsetMin.y);
				t.offsetMax = new Vector2(-oldOffsetMin.x, t.offsetMax.y);

				t.localScale = new Vector3(-t.localScale.x, t.localScale.y, t.localScale.z);
			}
		}
	
		static void MirrorVertically(bool mirrorAnchors){
			foreach(Transform transform in Selection.transforms){
				RectTransform t = transform as RectTransform;
				RectTransform pt = Selection.activeTransform.parent as RectTransform;
			
				if(t == null || pt == null) return;
			
				if(mirrorAnchors){
					Vector2 oldAnchorMin = t.anchorMin;
					t.anchorMin = new Vector2(t.anchorMin.x, 1 - t.anchorMax.y);
					t.anchorMax = new Vector2(t.anchorMax.x, 1 - oldAnchorMin.y);
				}
			
				Vector2 oldOffsetMin = t.offsetMin;
				t.offsetMin = new Vector2(t.offsetMin.x, -t.offsetMax.y);
				t.offsetMax = new Vector2(t.offsetMax.x, -oldOffsetMin.y);
			
				t.localScale = new Vector3(t.localScale.x, -t.localScale.y, t.localScale.z);
			}
		}
	}
}
