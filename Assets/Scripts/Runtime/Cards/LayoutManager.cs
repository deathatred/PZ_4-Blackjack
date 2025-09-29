using System.Collections.Generic;
using UnityEngine;

public static class LayoutManager
{
    public static List<Vector3> GetLinearLayout(int cardCount, float spacing)
    {
        List<Vector3> layout = new List<Vector3>();
        float newSpacing = 0;
        if (cardCount > 6)
        {
            newSpacing = spacing / 1.5f;
            if (cardCount > 8)
            {
                newSpacing = spacing / 2f;
            }
        }
        else
        {
            newSpacing = spacing;
        }
        float startX = -(cardCount - 1) * newSpacing / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            Vector3 pos = new Vector3(startX + i * newSpacing, 0, 0);

            layout.Add(pos);
        }

        return layout;
    }
}
