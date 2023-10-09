using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AP
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // IF THE DAMAGE IS CAUSED BY ANOTHER CHARACTERS ATTACK IT WILL BE STORED HERE

        [Header("Damage")]
        public float physicalDamage = 0; // (IN THE FUTURE WILL BE SPLIT INTO SUB TYPES "STANDARD", "STRIKE", "SLASH", and "PIERCE")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0; // THE DAMAGE THE CHARACTER TAKES AFTER ALL CALCULATIONS HAVE BEEN MADE

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false; // IF A CHARACTERS POISE IS BROKEN, THEY WILL BE "STUNNED" AND PLAY A DAMAGE ANIMATION


        // BUILD UPS
        // build up effect amounts

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // USED ON TOP OF REGULAR SFX IF THERE US ELEMENTAL DAMAGE PRESENT (Magic/Fire/Lightning/Holy)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom; // USED TO DETERMINE WHAT DAMAGE ANIMATION TO PLAY
        public Vector3 contactPoint; // USED TO DETERMINE WHERE BLOOD FX INSTANTIATE

        

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            // IF CHARACTER IS DEAD, NO ADDITIONAL EFFECTS SHOULD BE PROCESSED
            if (character.isDead.Value)
                return;

            // CHECK FOR "INVULNERABILITY"

            // CALCULATE DAMAGE
            CalculateDamage(character);
            // CHECK WHICH DIRECTION DAMAGE CAME FROM
            // PLAY DAMAGE ANIMATION
            // CHECK FOR BUILD UPS (POISON, BLEED, ETC)
            // PLAY DAMAGE SOUND FX
            PlayDamageSFX(character);
            // PLAY DAMAGE VFX (BLOOD)
            PlayDamageVFX(character);

            // IF CHARACTER IS A.I, CHECK FOR NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (characterCausingDamage != null)
            {
                // CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE (PHYSICAL/ELEMENTAL DAMAGE BUFF)
            }

            // CHECK CHARACTER FOR FLAT DAMAGE REDUCTION AND SUBTRACT THEM FROM THE DAMAGE

            // CHECK CHARACTER FOR ARMOUR ABSORBTION, AND SUBTRACT THE PERCENTAGE FROM THE DAMAGE

            // ADD ALL DAMAGE TYPES TOGETHER, AND APPLY FINAL DAMAGE

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            character.characterNetworkManager.currentHealth -= finalDamageDealt;
            Debug.Log("finalDamageDealt: " + finalDamageDealt);

            // CALCULATE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            // IF WE HAVE FIRE DAMAGE, PLAY FIRE PARTICLES
            // LIGHTNING DAMAGE, LIGHTNING PARTICLES

            character.characterEffectsManager.PlayDamageVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);

            // IF FIRE DAMAGE IS GREATER THAN 0, PLAY BURN SFX
            // IF LIGHTNING DAMAGE IS GREATER THAN 0, PLAY ZAP SFX
        }
    }
}
