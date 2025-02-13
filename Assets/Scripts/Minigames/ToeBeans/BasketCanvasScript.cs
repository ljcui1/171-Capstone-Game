using UnityEngine;

public class BasketCanvasScript : MonoBehaviour
{
    public float speed = 20f; // UI-based movement requires high speed
    public bool movementOn = true;
    public Minigame2Manager manager;

    private RectTransform basketRect;
    private RectTransform canvasRect;
    private float minX, maxX;

    void Start()
    {
        basketRect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        // Ensure correct boundary calculation
        float halfBasketWidth = basketRect.rect.width * 0.5f;
        float halfCanvasWidth = canvasRect.rect.width * 0.5f;

        Debug.Log(halfBasketWidth);
        Debug.Log(halfCanvasWidth);

        // Corrected minX and maxX calculation
        minX = -halfCanvasWidth + halfBasketWidth;  // Left boundary
        maxX = halfCanvasWidth - halfBasketWidth;   // Right boundary

        Debug.Log($"Canvas Width: {canvasRect.rect.width}, MinX: {minX}, MaxX: {maxX}");
    }



    void Update()
    {
        if (movementOn)
        {
            MoveBasket();
        }
        movementOn = !manager.gameOver;
    }

    void MoveBasket()
    {
        float moveX = Input.GetAxis("Horizontal"); // Use GetAxisRaw for instant response
        float currentX = basketRect.anchoredPosition.x;
        float moveStep = moveX * speed * Time.deltaTime;


        if (moveStep != 0) // Only update if there's movement
        {
            Debug.Log($"MoveStep: {moveStep}");
            float newX = Mathf.Clamp(currentX + moveStep, maxX, minX);
            basketRect.anchoredPosition = new Vector2(newX, basketRect.anchoredPosition.y);
            Debug.Log($"MoveX: {moveX}, CurrentX: {currentX}, NewX: {newX}");
        }

    }

}
