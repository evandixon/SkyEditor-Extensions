Imports System.Data.Entity
Imports System.IO
Imports System.Threading.Tasks
Imports SkyEditor.Core.Extensions
Imports SkyEditor_Extensions.Models.Extensions

Namespace Helpers
    Public Class ExtensionsHelper
        ''' <summary>
        ''' Installs an extension to the database and filesystem.
        ''' </summary>
        ''' <param name="filename">Path of the extension zip file.</param>
        ''' <param name="collection">The collection in which to put the extension.  Must be "linked" to entity framework.</param>
        ''' <param name="context">The data current data context in which to put the extension metadata.</param>
        Public Shared Async Function InstallExtension(filename As String, collectionID As Guid, context As ExtensionsContext) As Task
            'Read the info file
            Dim infoContents As String = Nothing
            Using sourceFile As New IO.FileStream(filename, FileMode.Open)
                Dim zip As New ICSharpCode.SharpZipLib.Zip.ZipFile(sourceFile)
                Dim infoEntry = zip.FindEntry("info.skyext", True)

                Using infoFile = zip.GetInputStream(infoEntry)
                    Using reader As New StreamReader(infoFile)
                        infoContents = reader.ReadToEnd
                    End Using
                End Using
            End Using

            If Not String.IsNullOrEmpty(infoContents) Then
                Dim infoFile = ExtensionInfo.Deserialize(infoContents)

                'Create or select the extension
                Dim extension As Extension = Await context.Extensions.Where(Function(x) x.ExtensionCollectionId = collectionID AndAlso x.ExtensionID = infoFile.ID.ToString).FirstOrDefaultAsync
                If extension Is Nothing Then
                    extension = New Extension With {.ID = Guid.NewGuid, .ExtensionID = infoFile.ID.ToString, .ExtensionCollectionId = collectionID, .Name = infoFile.Name, .Description = infoFile.Description, .Author = infoFile.Author}
                    context.Extensions.Add(extension)
                End If

                'Create or select the version info
                Dim version As ExtensionVersion = Await context.ExtensionVersions.Where(Function(x) x.ID = extension.ID AndAlso x.Version = infoFile.Version).FirstOrDefaultAsync
                If version Is Nothing Then
                    version = New ExtensionVersion With {.ID = Guid.NewGuid, .ExtensionId = extension.ID, .Version = infoFile.Version}
                    context.ExtensionVersions.Add(version)
                End If

                Try
                    Await context.SaveChangesAsync()
                Catch ex As Exception
                    Debugger.Break()
                    Throw
                End Try

                'Ensure the target directory exists
                Dim path = IO.Path.Combine(ConfigurationManager.AppSettings("ExtensionsPath"), "files", extension.ExtensionID)
                If Not IO.Directory.Exists(path) Then
                    IO.Directory.CreateDirectory(path)
                End If

                'Copy the file
                IO.File.Copy(filename, IO.Path.Combine(path, version.Version & ".zip"))
            Else
                Throw New ArgumentException("Provided zip file does not contain a valid extension info file.", NameOf(filename))
            End If
        End Function

        ''' <summary>
        ''' Installs an extension to the database and filesystem.
        ''' </summary>
        ''' <param name="filename">Path of the extension zip file.</param>
        Public Shared Async Function InstallExtension(filename As String) As Task
            Using context As New ExtensionsContext
                'Create or select the default extension collection
                Dim defaultCollectionName = ConfigurationManager.AppSettings("DefaultExtensionCollectionName")
                Dim collection As ExtensionCollection = Await context.ExtensionCollections.Where(Function(x) x.Name = defaultCollectionName).FirstOrDefaultAsync
                If collection Is Nothing Then
                    collection = New ExtensionCollection With {.ID = Guid.NewGuid, .Name = defaultCollectionName}
                    context.ExtensionCollections.Add(collection)
                    Await context.SaveChangesAsync()
                End If

                Await InstallExtension(filename, collection.ID, context)
            End Using
        End Function
    End Class
End Namespace
