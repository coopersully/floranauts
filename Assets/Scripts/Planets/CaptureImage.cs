using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CaptureSprite
{
    public class CaptureImage : MonoBehaviour
    {
        public Image mainImage;
        public Sprite[] blueSprites;
        public Sprite[] redSprites;
        public Sprite[] deadSprites;

        private int spriteIndex = 0;
        public bool bluePlayer = false;
        public bool redPlayer = false;
        public bool dead = true;

        private void Awake()
        {
            bluePlayer = true;

            dead = true;
            spriteIndex = 0;
        }
        private void Update()
        {
           if(bluePlayer)
                mainImage.sprite = blueSprites[spriteIndex];
            else if (redPlayer)
                mainImage.sprite = redSprites[spriteIndex];

        }
        public void NextImage()
        {
            spriteIndex++;
        }
        public void PreviousImage()
        {
            spriteIndex--;
        }
    }
}
