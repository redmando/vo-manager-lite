# VO Manager Lite
VO Manager is a Unity Scripting Package which enables developers to rapidly create and prototype games around the usage of audio and subtitles. It streamlines the process of playing audio and displaying subtitles without the hassle of creating a complex system.

_Copyright (c) 2016 - 2018 tvledesign LLC. All rights reserved._

## Importing the Package

There are two ways to import the VO Manager package. You can choose to either:

1. Visit the Unity Asset Store to download and directly import the package into Unity at [https://assetstore.unity.com/packages/tools/audio/vo-manager-56002](https://assetstore.unity.com/packages/tools/audio/vo-manager-56002) or;
2. You can go to the GitHub repository at [https://github.com/tvledesign/vo-manager-lite](https://github.com/tvledesign/vo-manager-lite) and download one of the packages inside the packages folder and import it into Unity.

If you've chosen to import the package from Unity or from the GitHub, depending on which package on GitHub you've used, it comes with a demo scene as well as the core VO Manager files which contain:

* VOManager
   * Editor
      * EditorClasses.cs
      * VOBankEditor.cs
      * VOManagerEditor.cs
   * Prefabs
      * VOManager.prefab
   * Scripts
      * VOBank.cs
      * VOManager.cs
   * ReadMe.pdf
        
## Setup
In order to use our VO Manager, we must first make sure that our manager is present on the scene at all time. To set up a new manager you can either:

1. Create a new empty GameObject in the Hierarchy and drag and drop the VO Bank or VO Manager script onto the newly made gameobject or;
2. You can go into the prefabs folder in the project panel and simply drag and drop the VO Manager prefab into the Hierarchy.

![VOManager GameObject](https://raw.githubusercontent.com/tvledesignLLC/vo-manager/master/documentation/src/img/ss-1.png)

Now before we can begin using VOManager we must first start adding VO (voice-over) clips to our VOBank and any subtitle dialogue that we may want to come with it. To do so, click on VOManager in the Hierarchy and press the “Add Clip” button under VOBank. You should see a similar interface below once you click on the add clip button.



![VOBank Add Clip](https://raw.githubusercontent.com/tvledesignLLC/vo-manager/master/documentation/src/img/v1.0/ss-2.png)

Go ahead and click on the expand icon by “[ID 0] Clip 0” to expand the panel.

![Clip Properties](https://raw.githubusercontent.com/tvledesignLLC/vo-manager/master/documentation/src/img/v1.0/ss-3.png)

Once expanded you should see a few more options. Here is a list of what they are and do:

* **ID** - An integer value that auto-increments (non-changeable) based on the position the current clip is in. This is used as a reference to call your clip’s properties.
* **Clip Name** - A string for self-reference. This is mainly use to keep track of all the different clips you may have in the bank for quick reference.
* **Subtitle Text** - A multi-line text area (string format) that contains the subtitle that you wish to have displayed on the UI when an audio clip is playing (should usually match the dialogue of the clip).
* **Dialogue Clip** - The audio clip.
    
You can also remove a clip by simply pressing the “Remove Clip” button. All ID values will reassign itself if you do so. There is however one last step we need to perform before our setup is complete. In order to use our subtitle system we must first have to go to the Hierarchy and create a new Text object. To do this go to Create -> UI -> Text. By default (if you have no Canvas elements in the scene) it will automatically create a Canvas and EventSystem object for you like so:

![Clip Properties](https://raw.githubusercontent.com/tvledesignLLC/vo-manager/master/documentation/src/img/v1.0/ss-4.png)

Setup the Text component properties and values the way you wish to (by default you can keep the text field of the text component empty). Once you are done click back on the VOManager object and drag and drop the Text component we just created (highlighted in blue above) into the Subtitle Object field in our VOManager. Once you have at least one sound clip added into your VOBank it should look like the following:

![Clip Properties](https://raw.githubusercontent.com/tvledesignLLC/vo-manager/master/documentation/src/img/v1.0/ss-5.png)

Once you have finished setting up your VOManager, save your scene and do one of the following:

1. If you used the VOManager prefab you can either Apply the current prefab or;
2. Create a new prefab by dragging your VOManager object into the Project panel (this should be done if you made your VOManager by creating a new GameObject)
    
## Usage

There are currently two ways to trigger an audio through the VO Manager. Those are:

1. Play Mode
2. Force Play Mode

### Play Mode

Play mode can be called in 4 different ways and triggers audio calls normally. This means that once you make a call to the play function, you can not make another one until the function is done playing.

```csharp
VOManager.Instance.Play(int _id);
VOManager.Instance.Play(string _name)
```
The example above is a set of the same except the major difference is that they both take in a different type of parameter. One takes in an integer while the other takes in a string. You can call an audio to play by passing through its ID or its assign Name.

```csharp
VOManager.Instance.Play(AudioSource _audSrc, int _id);
VOManager.Instance.Play(AudioSource _audSrc, string _name)
```
The second set is very identical to the ones above except it takes in a second parameter which is an audio source. By passing through an audio source you are able to play an audio from an external source instead of the one attached to the VO Manager. This allows for more control over the 3D sound.

### Force Play Mode

Just like our regular play mode, force play mode takes in the same type of parameters to trigger an audio. This parameter takes in either an integer or a string. The biggest difference between Play and Force Play mode is that calling the force play function immediately cuts off the current audio that is playing and plays the new on that is called.

```csharp
VOManager.Instance.ForcePlay(int _id);
VOManager.Instance.ForcePlay(string _name)
```
Much like the functions above, force play mode also takes in an external audio source.

```csharp
VOManager.Instance.ForcePlay(AudioSource _audSrc, int _id);
VOManager.Instance.ForcePlay(AudioSource _audSrc, string _name)
```

### Checks

If at any given time you need to check if an audio is playing, you can call the is playing function which return a boolean value of true or false.

```csharp
VOManager.Instance.IsPlaying()
```

### Stop

Calling the stop functionality simply just stops all current audio sources that are playing and subtitles drawn on-screen.

```csharp
VOManager.Instance.Stop()
```

## Useful Links
If you have any questions, feedback, or issues, please feel free to contact me via:

* Email at [tvledesign@gmail.com](mailto:tvledesign@gmail.com) or;
* Go to the GitHub Repo at [https://github.com/tvledesign/vo-manager-lite](https://github.com/tvledesign/vo-manager-lite)

Additionally to learn more or find tutorials you can go to:

* Website at [https://www.tvledesign.com](https://www.tvledesign.com)
* Blog at [https://www.tonyvle.com](https://www.tonyvle.com)
* YouTube Channel at [https://www.youtube.com/channel/UCeto2S7J0vwAeNAdonRH80w](https://www.youtube.com/channel/UCeto2S7J0vwAeNAdonRH80w)
    
## Credits

* Tony V. Le - Developer
* Gareth Lynch - Artist
* Joe Song - Audio

## Special Thanks
Special thanks to these wonderful people who were on my team at the Global Game Jam 2016 which inspired me to create the VO Manager:

* Cameron Cintron 
* Jessica Borlovan
* Joe Song
* Neal Shaw 
    
Last but not least these wonderful organizations that made this idea possible:

* Global Game Jam 
* Petal et Al 
