Imports System.IO
Imports SkyEditor_Extensions.Helpers

Public Module ExtensionsMonitorSetup
    Private WithEvents watcher As IO.FileSystemWatcher
    Sub StartExtensionMonitor()
        Dim path = IO.Path.Combine(ConfigurationManager.AppSettings("ExtensionsPath"), "In")
        If Not IO.Directory.Exists(path) Then
            IO.Directory.CreateDirectory(path)
        End If

        watcher = New IO.FileSystemWatcher(path)
        watcher.EnableRaisingEvents = True
    End Sub

    Private Async Sub watcher_Created(sender As Object, e As FileSystemEventArgs) Handles watcher.Created
        Await ExtensionsHelper.InstallExtension(e.FullPath)
        File.Delete(e.FullPath)
    End Sub
End Module
