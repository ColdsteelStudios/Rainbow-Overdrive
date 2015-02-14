If you wish to instantiate a prefab over the network using the Photon Networking library, it is a requirement that they be placed in this folder so that they can be loaded at run time.

When running the game through the web player, all prefabs which are placed within this folder will be streamed at the very first scene per default.  
Under the webplayer settings you can specify the first level that uses assets from this Resources folder by using the "FIrst streamed level".  
If you set this to your first game scene, your preloader and main menu will not be slowed down if they don't use the Resources folder assets.