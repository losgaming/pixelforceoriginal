IMPORTANT: You need to install Standard Assets_Characters for the demo scene to work!
Hi, thank you for buying this asset.

Lean/Peek

Tutorial: If you are using the fps controller, you have to set up some things. First you have to make a new layer for your weapon. (Select your weapon, go to layer -> Add Layer)
Next up you need to change te first person character's camera, so it can't see the weapon layer. (Culling mask) 

After you've done that, you need to set up 2 camera's inside the first person character. Let's call the first one WeaponCamera, and te second one DefaultCamera. 
On the WeaponCamera you need to set the culling mask to the weapon layer only, and on DefaultCamera to everything except the weapon layer. 

WeaponCamera settings:
Clear Flags : Depth Only
Min clipping planes : 0.01
Depth : 3

DefaultCamera settings:
Clear Flags: Skybox
Min clipping planes : 0.01
Depth : 2

You have to apply the Lean and Peek script to your First Person Character. Apply the DefaultCamera to the Peek script, and the WeaponCamera to the Lean script.
In the inspector you can change all the settings for both scripts. If you have any questions, contact me at: mijndertt@hotmail.com

ADS/Sway

The weapon sway and ADS system are pretty self explanatory. Add the script to your weapon, ajust all values how you want them, and drag and drop everything that's needed. 
You can check the example scene if you have problems, or contact me at: mijndertt@hotmail.com