using UnityEngine;

public class Star : MonoBehaviour {

    public new SpriteRenderer renderer;

	public void SetColor(Color color)
    {
        renderer.color = color;
    }
}
