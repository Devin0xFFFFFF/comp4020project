Permission Manual ('AndroidManifest.xml')

https://developer.android.com/guide/topics/security/permissions.html
List of functions requiring permission in manifest file (AndroidManifest.xml).

(*) Depending on the function you want to use, you need to manually add permissions in 'AndroidManifest.xml'. Since all permissions have been added to the demonstration manifest file 'AndroidManifest_demo.xml', if necessary, please copy and paste it in the same way (It is better to delete the permission of functions not used).

·Recording audio permission is required for Speech Recognizer.
<uses-permission android:name="android.permission.RECORD_AUDIO />

·To write (save) to External Storage, access permission (read/write) to media storage is required.
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />

·To read (load) to External Storage, access permission (read) to media storage is required (* However, it is not necessary if there is "WRITE_EXTERNAL_STORAGE").
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />

·Bluetooth permission is required for Bluetooth connection request.
<uses-permission android:name="android.permission.BLUETOOTH" />

·Vibrate permission is required for using vibrator.
<uses-permission android:name="android.permission.VIBRATE"/>


----------------------------------------------------------------------------
https://developer.android.com/guide/topics/security/permissions.html?hl=ja
マニフェストファイル「AndroidManifest.xml」にパーミッションが必要な機能一覧

※使いたい機能によってはパーミッションを「AndroidManifest.xml」に手動で追加する必要があります。デモのマニフェストファイル「AndroidManifest_demo.xml」には全てのパーミッションが追加してあるので、必要であれば同じようにコピペなどして追加して下さい（利用しない機能のパーミッションは削除する方が好ましいです）。

・音声認識には録音パーミッションが必要です。
<uses-permission android:name="android.permission.RECORD_AUDIO />

・ExternalStorageに書き込み（保存）を行うにはメディアストレージへのアクセス（読み書き）パーミッションが必要です。
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />

・ExternalStorageから読み込みを行うにはメディアストレージへのアクセスパーミッション（読み取り）が必要です（※ただし「WRITE_EXTERNAL_STORAGE」がある場合は必要ありません）。
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />

・Bluetoothの接続要求を行うにはBluetoothパーミッションが必要です。
<uses-permission android:name="android.permission.BLUETOOTH" />

・バイブレーターを利用する場合には以下のパーミッションが必要です。
<uses-permission android:name="android.permission.VIBRATE"/>


------------------------------------------------
By Fantom

[Blog] http://fantom1x.blog130.fc2.com/
[Twitter] https://twitter.com/fantom_1x
[Monappy] https://monappy.jp/u/Fantom

