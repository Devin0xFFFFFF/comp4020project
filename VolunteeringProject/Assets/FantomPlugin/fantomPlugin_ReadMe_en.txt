http://fantom1x.blog130.fc2.com/blog-entry-273.html
Android Native Dialogs and Functions Plugin
Setup & Build Manual

･Native Plugin "fantomPlugin.aar" is required 'Minimum API Level：Android 4.2 (API 17)' or later.

(*) It is necessary to set it to 'Android 4.4 (API 19)' or later in order to use 'StorageLoadTextController' and 'StorageSaveTextController' to read/write text file of storage.

･Move the "Assets/FantomPlugin/Plugins/" folder just under "Assets/" like "Assets/Plugins/". This "Plugins" folder is a special folder for running the plugin at runtime.
(see) https://docs.unity3d.com/Manual/ScriptCompileOrderFolders.html

･Rename "AndroidManifest-FullPlugin~.xml" to "AndroidManifest.xml" when receive events of Hardware Volume buttons, Speech Recognizer with dialog, open Wifi Settings, request Bluetooth enable, read/write text file on External Storage, open Gallery, register MediaScanner.

·Depending on the function you use, Android permission is required (https://developer.android.com/guide/topics/security/permissions.html). Permission is summarized in "Assets/Plugins/Android/Permission_ReadMe.txt". Please copy the necessary permission to "AndroidManifest.xml" (It is better to delete the permission of functions not used).

･Text To Speech is required the reading engine and voice data must be installed on the smartphone.
(see) http://fantom1x.blog130.fc2.com/blog-entry-275.html#fantomPlugin_TextToSpeech_install
(Voice data: Google Play)
https://play.google.com/store/apps/details?id=com.google.android.tts
https://play.google.com/store/apps/details?id=jp.kddilabs.n2tts

･Select "_Landscape" or "_Portrait" of "AndroidManifest~.xml" according to the screen rotation attribute (screenOrientation) of the application.
(see) https://developer.android.com/guide/topics/manifest/activity-element.html#screen

(*) Warning "Unable to find unity activity in manifest. You need to make sure orientation attribute is set to sensorPortrait manually." can be ignored if you use anything other than Unity standard Activity (UnityPlayerActivity).
(see) https://docs.unity3d.com/ja/current/Manual/AndroidUnityPlayerActivity.html

------------------------------------------------
■About demo

･Rename "AndroidManifest_demo.xml" to "AndroidManifest.xml" when building the Demo. Also add the scene in "Assets/FantomPlugin/Demo/Scenes/" to 'Build Settings...' and switch to 'Android' with 'Switch Platform'.

(*) The demo of 'GalleryPickTest' does not include the mesh of the whole sphere (360 degrees). If necessary, download 'Sphere100.fbx' from the following URL and set it to 'Sphere' of hierarchy 'GalleryPickTest (Script)'. Also, please set 'TextureMat' to the material of 'Sphere 100'. Because the whole sphere look from the inside, if you give a negative value to the scale X, you can invert the image (size as you like, 10 in the demo video).
(Mesh of whole sphere: Sphere100.fbx)
http://warapuri.com/post/131599525953/
(Demo video: Vimeo)
https://vimeo.com/255712215


Let's enjoy creative life!

------------------------------------------------
■Update history

(ver.1.1)
·Added PinchInput, SwipeInput, LongClickInput/LongClickEventTrigger and its demo scene (PinchSwipeTest).
·Added SmoothFollow3 (originally StandardAssets SmoothFollow) with right/left rotation angle, height and distance, and added a corresponding to pinch (PinchInput) and swipe (SwipeInput) (demo scene: used with PinchSwipeTest).
·Changed the color format conversion of 'XColor' from ColorUtility to calculation formulas(Mathf.RoundToInt()).
･Changed 'XDebug' option of lines limit.
(ver.1.2)
･Added prefab and '-Controller' script of all functions.
･Added value change callbacks to SingleChoiceDialog, MultiChoiceDialog, SwitchDialog and CustomDialog items.
･Fixed bug that XDebug's automatic newline flag (newline) was ignored. Also, cleared the text buffer (Queue) with OnDestory() when using line limit.
(ver.1.3)
･Added function to open WIFI system settings (WifiSettingController).
･Added function to make Bluetooth connection request (dialog display) (BluetoothSettingController).
･Added function to send text using Chooser application (simple text sharing function) (SendTextController).
･Added functions to read/write text files (StorageLoadTextController/StorageSaveTextController) using the Storage Access Framework (API 19 or later).
･Added function to open the gallery application and get the path of the image file (GalleryPickController) (also load texture and save screenshot).
･Added function to register (scan) file path to MediaScanner (MediaScannerController).
(ver.1.4)
·Added function to vibrate the Vibrator (VibratorController).
·Added vibrator function to notification (NotificationController).
·Changed all extended editor scripts with 'SerializedProperty' (as the setting was sometimes not saved in the editor).


(*)The latest version can be downloaded from GoogleDrive on blog (Japanese version only).
http://fantom1x.blog130.fc2.com/blog-entry-273.html

------------------------------------------------
■News!

The music library including sample song is on sale at the Asset Store!

Seamless Loop and Short Music (FREE)
https://www.assetstore.unity3d.com/#!/content/107732

------------------------------------------------
By Fantom

[Blog] http://fantom1x.blog130.fc2.com/
[Twitter] https://twitter.com/fantom_1x
[SoundCloud] https://soundcloud.com/user-751508071
[Picotune] http://picotune.me/?@Fantom
[Monappy] https://monappy.jp/u/Fantom

