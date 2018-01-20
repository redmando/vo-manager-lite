# VO Manager - Lite
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
To set up our VOManager you have two of the following options. You can either:

1. Create a new empty GameObject in the Hierarchy and drop the VOBank script or the VOManager script onto it or;
2. You can go into the Project panel and into the Prefabs folder and drag and drop the VOManager prefab into the Hierarchy.

By default when adding the VOBank script or the VOManager script onto a new object an Audio Source component will be added automatically along with one or the other script since they are dependent on each other. You should see a similar interface below when you’ve successfully setup the initial VOManager.

![VOManager GameObject](https://raw.githubusercontent.com/tvledesignLLC/vo-manager/master/documentation/src/img/v1.0/ss-1.png)

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

In order to use our VOManager, we must make sure that our VOManager is always present in the scene at all times. We can make calls to our VOManager with the following lines:

```csharp
VOManager.Instance.Play(int _id);
VOManager.Instance.Play(string _name)
```
VOManager.Instance.Play() takes in two different parameters. You can pass in either an integer ID or a string name that the audio clip has been assigned in the VOBank. Calling this function will play an audio clip normally. Running it again, however, would only work if there is currently no audio clips being played via the VOManager.

```csharp
VOManager.Instance.Play(AudioSource _audSrc, int _id);
VOManager.Instance.Play(AudioSource _audSrc, string _name)
```
Alongside being able to pass in either an integer ID or a string name, you also have the ability to pass in an external audio source. Calling the function with an external audio source will play the current audio clip at that target source. 

```csharp
VOManager.Instance.ForcePlay(int _id);
VOManager.Instance.ForcePlay(string _name)
```
Like the normal play method, you can pass in either an integer ID or a string name that the audio clip has been assigned in the VOBank. Calling this function will immediately cut off any current audio clip that is being played.

```csharp
VOManager.Instance.ForcePlay(AudioSource _audSrc, int _id);
VOManager.Instance.ForcePlay(AudioSource _audSrc, string _name)
```
Similar to the play method, calling the VOManager.Instance.ForcePlay() function with an external audio source will play the current audio clip at the target source. However, calling this while an audio clip is playing from the VOManager will immediately cut it off.

```csharp
VOManager.Instance.IsPlaying()
```
Calling VOManager.Instance.IsPlaying() will return a boolean either true or false. This method checks to see if an audio clip is being played from the VOManager.

```csharp
VOManager.Instance.Stop()
```
Calling the VOManager.Instance.Stop() method will immediately stop any audio clip being played from the VOManager.

VOManager by default handles all subtitle text that appears on the screen so you don’t have to worry about scripting any kind of functionality.

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
Special thanks to these wonderful people who were on my team at the Global Game Jam 2016 which inspired me to create VO Manager:

* Cameron Cintron at [http://ccgamedesign.com/](http://ccgamedesign.com/)
* Jessica Borlovan at [http://jmborlovan.com/](http://jmborlovan.com/)
* Joe Song
* Neal Shaw at [http://nealryanshaw.com/](http://nealryanshaw.com/)
    
Last but not least these wonderful organizations that made this idea possible:

* Global Game Jam at [http://globalgamejam.org/](http://globalgamejam.org/)
* Petal et Al at [http://petaletal.org/](http://petaletal.org/)
