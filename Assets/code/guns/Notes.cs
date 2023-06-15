using UnityEngine;

namespace guns
{
    public class Notes
    {
     /*
      * n is max barrels - maybe 16?
      *
      * b - is # barrels
      * 
      */
        
        /*
         * TYPES OF ACTIONS
         * 
         * 
         * Break
         * - 1-n barrels
         * - Option to fire all at once or one at a time
         * - Forces no magazine
         * 
         * Lever
         * - 1 barrel
         *
         * Rotating (Rotary [GAS] or chain)
         * - 1-n barrels
         * - pretty high weight/cost, should nearly force minigun holding
         * 
         * Revolver
         * - 1-n barrels
         * - Forces no magazine
         * 
         * Side mount RPG thingy
         * - 1-n barrels
         * - Forces no magazine
         * - Changes Ammo type
         * - blowback to anyone behind the shooter
         * 
         * Bolt
         * - 1 barrel
         *
         * Pump
         * - 1 barrel
         *
         * Semi Automatic [GAS]
         * - 1-2 Barrels
         *
         * Automatic [GAS]
         * - 1-2 Barrels
         * 
         */
        
        /*
         * Ammo delivery types:
         *
         * Magazine
         * - 1-4 barrels - above 1 the mags are side mounted
         *
         * Clip/ Autoloader
         * - 1 barrel
         * - Revolvers/Breach valid
         *
         * Whatever the Pump thingy is called
         * - 1 barrel
         *
         * Muzzle Loader
         * - Option for mortar launching in ammo
         * - 1-n barrels
         *
         * Belt Fed
         * - 1-n barrels
         *
         * 
         * 
         */
        
        /*
         * Muzzle Attachments
         *
         * Shroud
         * - 1-n barrels
         * - Reduces Muzzle flash and directs gas foreward
         *
         * Suppressor
         * - 1-n barrels
         *
         * Muzzle Breaks
         * 
         */
        
        /*
         * Barrel Attachments
         *
         * Bipod
         * - Not on rotating Barrels
         *
         * Flashlights
         *
         * Lasers
         *
         * Grips
         * - Vertical/Angled/Flat
         * 
         * UBGL/Shotgun?
         * 
         */
        
        /*
         * Grip Types
         *
         * Generic Rifle grip
         *
         * Shotgun/Long rifle Grip
         *
         * Underhand
         * - No stock
         * 
         * Shoulder
         * - Really bad for recoil
         * - No stock
         * 
         * Mounted
         *
         * Pistol Grip
         * 
         */
        
        /*
         * Stock Types
         *
         * Stockless
         *
         * Wooden stock
         *
         * Folding
         *
         * maybe some options to carry ammo/meds?
         * 
         */
        
        /*
         * Ammo Variations
         *
         * Payload/Fuse(Optional, only for some payloads)/Propellant/Casing(Depends on ammo feed)
         * Needs some sort of way to determine the balance of propellant/payload size.
         * Both to determine ammo characteristics and allow for subsonic ammo 
         * Use phys to determine recoil effects - maybe make the player use physics to move around?
         * 
         * ** PAYLOAD
         * - Explosive
         * - Buckshot (Somehow Comboed into airburst ammo)
         * - Incendiary
         * - Solid shot
         * - Sabot
         * - Pistol
         * - Rifle
         * 
         * ** FUSES
         * - Defaults to contact detonation
         * - Time/Distance
         * - Penetration depth
         * - Contact
         * - Option for some sort of guidance?
         *
         * ** Propellant
         * - Black Powder (Bad for stealth, but funny)
         * - Smokeless Powder
         * - Rocket engines
         * - Hybrid powder/rocket
         * 
         * ** Casing
         * Based pretty heavily on other stuff first
         * - Metal casing for magazines
         * - Rockets for big rounds
         * 
         * 
         */
        
    }
}