using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FeatureA
{
    [CreateAssetMenu(fileName = "SkinData", menuName = "ScriptableObjects/SkinData", order = 1)]
    public class SkinData : ScriptableObject
    {
        public Color backgroundColor;
        public Sprite backgroundSprite;

        public Color textColor;
        public Color buttonColor;

        public Font font;
    }
}
