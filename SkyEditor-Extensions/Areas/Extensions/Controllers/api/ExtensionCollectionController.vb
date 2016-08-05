Imports System.Data.Entity
Imports System.Net
Imports System.Threading.Tasks
Imports System.Web.Http
Imports SkyEditor.Core.Windows.Extensions.Online
Imports SkyEditor_Extensions.Helpers
Imports SkyEditor_Extensions.Models.Extensions

Namespace Areas.Extensions.Controllers.api
    Public Class ExtensionCollectionController
        Inherits ApiController

        ' GET: api/ExtensionCollection
        Public Async Function GetValueAsync() As Task(Of RootCollectionResponse)
            Using context As New ExtensionsContext
                Dim collection = Await ExtensionsHelper.GetDefaultExtensionCollection(context)

                Return Await GetRootExtensionCollectionResponse(collection, context)
            End Using
        End Function

        ' GET: api/ExtensionCollection/5
        Public Async Function GetValueAsync(ByVal id As Guid) As Task(Of RootCollectionResponse)
            Using context As New ExtensionsContext
                Dim collection = Await (From c In context.ExtensionCollections
                                        Where c.ID = id).FirstAsync

                Return Await GetRootExtensionCollectionResponse(collection, context)
            End Using
        End Function

        Private Async Function GetRootExtensionCollectionResponse(collection As ExtensionCollection, context As ExtensionsContext) As Task(Of RootCollectionResponse)
            Dim out As New RootCollectionResponse
            out.Name = collection.Name

            out.ChildCollections = Await (From c In context.ExtensionCollections
                                          Where c.ParentCollectionId = collection.ID
                                          Select New ExtensionCollectionModel With {.ID = collection.ID.ToString, .Name = c.Name}).ToListAsync()
            out.DownloadExtensionEndpoint = ConfigurationManager.AppSettings("APIRoot") & "Extensions"
            out.GetExtensionListEndpoint = ConfigurationManager.AppSettings("APIRoot") & "Extensions/" & collection.ID.ToString
            out.ExtensionCount = Await (From e In context.Extensions
                                        Where e.ExtensionCollectionId = collection.ID).CountAsync
            Return out
        End Function

        '' POST: api/ExtensionCollection
        'Public Sub PostValue(<FromBody()> ByVal value As String)

        'End Sub

        '' PUT: api/ExtensionCollection/5
        'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        'End Sub

        '' DELETE: api/ExtensionCollection/5
        'Public Sub DeleteValue(ByVal id As Integer)

        'End Sub
    End Class
End Namespace