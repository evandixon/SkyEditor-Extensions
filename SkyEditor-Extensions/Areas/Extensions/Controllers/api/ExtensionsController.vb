Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports SkyEditor.Core.Windows.Extensions.Online
Imports SkyEditor_Extensions.Models.Extensions

Namespace Areas.Extensions.Controllers.api
    Public Class ExtensionsController
        Inherits ApiController

        ' GET: api/Extensions/5/0/10
        <Route("api/Extensions/{collectionId}/{skip}/{take}")>
        Public Function GetValue(ByVal collectionId As Guid, skip As Integer, take As Integer) As List(Of OnlineExtensionInfo)
            Dim out As New List(Of OnlineExtensionInfo)
            Using context As New ExtensionsContext
                For Each item In (From e In context.Extensions
                                  Where e.ExtensionCollectionId = collectionId
                                  Order By e.Name
                                  Select New With
                                      {
                                        .Extension = e,
                                        .Versions = e.Versions.Select(Function(x) x.Version)
                                      }).Skip(skip).Take(take)

                    out.Add(New OnlineExtensionInfo With
                            {
                                .ID = item.Extension.ExtensionID,
                                .Name = item.Extension.Name,
                                .Description = item.Extension.Description,
                                .Author = item.Extension.Author,
                                .AvailableVersions = item.Versions
                            })
                Next
            End Using
            Return out
        End Function

        ' GET: api/Extensions/extensionName/1.0.0
        <Route("api/Extensions/{extensionID}/{version}.zip")>
        Public Function GetValue(extensionID As String, version As String) As HttpResponseMessage
            Dim filename = IO.Path.Combine(ConfigurationManager.AppSettings("ExtensionsPath"), "files", extensionID, version & ".zip")
            If IO.File.Exists(filename) Then
                Dim streamContent = New StreamContent(IO.File.OpenRead(filename))
                streamContent = streamContent
                streamContent.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/sky-editor-extension")

                Dim response = Request.CreateResponse
                response.Content = streamContent
                Return response
            Else
                Return Request.CreateResponse(404)
            End If
        End Function

        '' POST: api/Extensions
        'Public Sub PostValue(<FromBody()> ByVal value As String)

        'End Sub

        '' PUT: api/Extensions/5
        'Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

        'End Sub

        '' DELETE: api/Extensions/5
        'Public Sub DeleteValue(ByVal id As Integer)

        'End Sub
    End Class
End Namespace