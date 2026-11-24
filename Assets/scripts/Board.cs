using UnityEngine;
public class Board : Rotatable
{
    SpriteRenderer board;
    Color defaultColor = Color.white;
    public Color selectColor = Color.yellow;
    protected override void Awake()
    {
        base.Awake();

        // Set board renderer and color
        board = GetComponent<SpriteRenderer>();
        if (board != null)
        {
            defaultColor = board.color;
        }
    }

    // select or deselect board
    protected override void SetSelected(bool value)
    {
        base.SetSelected(value);

        // set board to yellow to indicate it's being used
        if (board != null)
        {
            board.color = selected ? selectColor : defaultColor;
        }
    }
}
