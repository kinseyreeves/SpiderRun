Spider Run
![SpiderRun.jpg](https://bitbucket.org/repo/yEoayr/images/3935786392-SpiderRun.jpg)



# What the application does #
Spider run is a game where you try and survive oncoming waves of spiders.
There are items you can place via buttons and you may change the terrain to 
your advantage.

# How to use it #
Click where you want the player to go. Two finger tap to create fire.
Click on the buttons to create items which kill spiders.

# Notes #

This is a demo build, so, for example, no cost is applied for building or controlling terrain. 
This would be changed in the full game.

# Running #

Build development build in unity for Windows Store (10) and run.

# Controls #

Most controls self explanatory, press start to start the game.

Push items to create those items at the player's current location.

## Items ##

Rolling Ball - Rotate tablet to influence where ball goes.

Poison Bush - Releases poison in area, this is dangerous for you as well.

Fire Tree - Releases damaging projectiles in surrounding directions periodically.

Turret - Targets and fires at nearby enemies.

## Other Controls ##

To switch between player and terrain controls, click the game mode button (the button which says player or terrain).

Walk over stamina (green) or hearts (red) to get stamina or health

Instructions are also on main menu.

### Player Controls ###

Touch the direction you wish to go in to move towards that direction.

Touch in two locations to produce a flame between those locations.
Note, this will cost form points, so you may not be able to create these
if you have used all your form points.

### Terrain Controls ###

Touch a location on the map to select that area and drag to raise or lower terrain there.
Where terrain is non-zero height, a ring will appear nearby to show which area is being raised.
As a note, partial rings may be shown on nearby raised terrain as well to show clearly where is being
raised. I thought this was nice personally, though this may be switched to a cooler radial selection mark
with light colour animation when no terrain modification is present in future.

Note: The rolling ball can also be controlled during the terrain mode.

# Winning the Game #

As this is a demo, the number of waves is limited, after completing all waves, the player is free
to roam.

# Shaders Used #

The following were used

- Lighting Shader: Phong with multi-pass shadow mapping (on Terrain Shader).

- Advanced Lighting/shading: Selection projection shader (on Terrain Shader). Fog of war.

- Geometry Shader: Selection projection shader (on Terrain Shader).

- Particle System Shader: Unity's inbuilt particle system (Flames).

- Shadow Volumes: on Terrain Shader, using Unity's inbuilt shadow system. This was significantly different to the 
tutorial version's code originally, however due to another part of the code which relied more heavily on having
specific names being present, non-Pro Unity not processing extra shadows and a standard pragma that was somehow
missed, the shadow shader has become basically the tutorial code mentioned in the shader comments.

# App Certification #

ValidationResult.html in base directory. All passed.

# Youtube Link #

[Youtube](https://youtu.be/Qho5DAr75gI)

# Extra Shader Explanation #

Projection shader - projects out a certain distance from the surface and draws a hollow square around the edges of the surface. Its addition into the terrain shader has additional logic to limit the area it is drawn for, however some extra is included in case the location selected itself is difficult to see.

# Assets Used #

All models free from Unity Asset store.