using System.Collections.Generic;
using UnityEngine;

public static class LayoutManager 
{
    public static List<Vector3> GetLinearLayout(int cardCount, float spacing)
    {
        List<Vector3> layout = new List<Vector3>();

        float startX = -(cardCount - 1) * spacing / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            Vector3 pos = new Vector3(startX + i * spacing, 0, 0);

            layout.Add(pos);
        }

        return layout;
    }
}
