using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityGoogleDrive;

public class AppLogger
{
    private static StringBuilder _log = new StringBuilder();

    public static string Log(string screen, string element, string message) {
        string log = string.Format("[{0}] ({1}, {2}) {3}", DateTime.Now.Ticks, screen, element, message);

        _log.AppendLine(log);

        return log;
    }

    public static void Export() {
        var file = new UnityGoogleDrive.Data.File() {
            Name = string.Format("{0}_log.txt", DateTime.Now.ToLongTimeString()),
            Content = Encoding.Default.GetBytes(_log.ToString()) };

        GoogleDriveFiles.Create(file).Send();
    }
}
