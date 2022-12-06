using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace Player
{
    public class CaptureImage : MonoBehaviour
    {
        public CaptureImageManager imageManager;
        public Image mainImage;
        public Sprite[] blueSprites;
        public Sprite[] redSprites;
        public Sprite[] deadSprites;

        private int spriteIndex = 0;
        public bool playerOne = false;
        public bool playerTwo = false;
        public bool dead = true;

        private void Awake()
        {
            imageManager = GetComponentInParent<CaptureImageManager>();
            playerOne = true;

            dead = true;
            spriteIndex = 0;
        }
        private void Update()
        {
;          spriteIndex = imageManager.spriteIndex;
           if(playerOne)
                mainImage.sprite = blueSprites[spriteIndex];
            else if (playerTwo)
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
